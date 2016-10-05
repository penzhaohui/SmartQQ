using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;

namespace SmartTask
{
    public class TaskManagementService
    {
         /// <summary>
        /// 所有正在运行的任务集合
        /// </summary>
        internal IList<AbstractTask> Tasks { get; private set; }
        
        /// <summary>
        /// 监视工人(轮询工作计时器)
        /// </summary>
        private Timer _watcher;

        /// <summary>
        /// 运行次数
        /// </summary>
        private int _workTimes;

        /// <summary>
        /// 任务配置
        /// </summary>
        internal TaskConfig TaskSetting { get; private set; }

        /// <summary>
        /// 开始执行全部
        /// </summary>
        public void Start()
        { 
            //Note:线程锁定
            lock (this)
            {
                Logger.Debug("----- 服务启动开始 -----");

                //初始化任务集合
                Tasks = new List<AbstractTask>();

                //载入所有配置
                TaskSetting = TaskConfig.GetInstance();
                TaskSetting.Changed += TaskConfigChanged; //只保留一次的事件

                //Note:初始化计时器
                _watcher = new Timer(TaskSetting.WatchTimer.DelayMillisecond); //Note:第一次运行不按间隔时间执行。
                _watcher.Elapsed += Working;
                _watcher.Start(); //启动工作回调
                Logger.Debug("监视工人第一次工作将于{0}后执行", TimeSpan.FromMilliseconds(_watcher.Interval));
                Logger.Debug("----- 服务启动完成 -----");
            }
        }

        /// <summary>
        /// 监视轮询工作
        /// 发现有可执行的任务时，启动该任务所在的驱动以使其执行。
        /// Note:Working时就要每次重读配置，释放后的任务再也回不来了。
        /// </summary>
        public void Working(object sender, ElapsedEventArgs args)
        {
            //Note:线程锁定
            lock (this)
            {
                _watcher.Stop();
                Logger.Debug("↓---- 任务监视轮询开始 ----    [下次轮询延时: {0}秒 ; 当前时间:{1}]", _watcher.Interval/1000, DateTime.Now.ToString("HH:mm:ss ffffff"));

                TryRemoveTask();

                //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss ffffff") + " Working..."); // For Test

                StartSettingTasks();
               
                _watcher.Interval = ((TimeSpan)TaskSetting.WatchTimer.WorkingInterval).TotalMilliseconds;
                _workTimes++;

                Logger.Debug("↑---- 任务监视轮询结束[Times:{0}] ----  [当前时间:{1}]", _workTimes, DateTime.Now.ToString("HH:mm:ss ffffff"));
                _watcher.Start();
            }
        }

        /// <summary>
        /// 启动设置中的任务
        /// </summary>
        private void StartSettingTasks()
        {
            var tasks = TaskSetting.Tasks;
            foreach (var task in tasks.Select(GetTask).Where(task => task != null))
            {
                Tasks.Add(task);
            }
        }
 
        /// <summary>
        /// 初始化一个任务的驱动实例
        /// <remarks>
        /// 如果任务为空，表示没有可用的任务
        /// </remarks>
        /// </summary>
        /// <param name="taskSetting">任务配置信息</param>
        private AbstractTask GetTask(TaskInfo taskSetting)
        {
            var job = Tasks.FirstOrDefault(x => x.Task.Meta.Id == taskSetting.Meta.Id);
            if (job != null) return null;
            if (!taskSetting.Enable) return null;

            var now = SystemTime.Now();
            var nextRunInterval = taskSetting.GetNextInterval();
            if (nextRunInterval == null)
            {
                Logger.Warn("○ [{0}] 下次启动时间为null ，请检查触发的表达式。或者，该任务已经完成设定次数。", taskSetting);
                return null;
            }
            var delayMilSeconds = nextRunInterval.Value.TotalMilliseconds;
            var interval = (TimeSpan) TaskSetting.WatchTimer.WorkingInterval;
            if (delayMilSeconds >  interval.TotalMilliseconds *   3)
            {
                Logger.Debug("○ [{0}] 下次启动时间间隔为:{1}，远大于监视线程间隔{2}，暂不执行。", taskSetting, nextRunInterval, interval);
                return null;
            }

            //实例化驱动实现
            try
            {
                Logger.Debug("[{0}] 开始初始化...", taskSetting);
                var typeInfo = taskSetting.Type.Split(',');
                var assembly = Assembly.Load(typeInfo[1].Trim()); //如果错误，这儿会引发异常，下面就不用执行了
                var type = assembly.GetType(typeInfo[0].Trim());
                if (type == null || !typeof (AbstractTask).IsAssignableFrom(type))
                {
                    var msg = String.Format("[{0}] 的type属性[{1}]无效，请使用实现了TaskProvider的类，服务中止。", taskSetting, taskSetting.Type);
                    throw new ConfigurationErrorsException(msg);
                }
                var task = assembly.CreateInstance(typeInfo[0]) as AbstractTask;
                if (task != null)
                {
                    task.WorkerInterval = ((TimeSpan) TaskSetting.WatchTimer.WorkingInterval).TotalMilliseconds;
                    task.Task = taskSetting;
                    task.Resources = TaskSetting.Resources; //公有资源引用
                    //task.InitPreExetend();
                    task.InitExtend(); //由于非new方式创建实现，无法在构造中获得配置
                    task.Start();
                    Logger.Debug("[{0}] 实例化成功。", taskSetting);
                }
                else
                {
                    Logger.Error("[{0}] 实例化为null。", taskSetting);
                }
                return task;
            }
            catch (Exception ex)
            {
                var msg = String.Format("[{0}] 的type属性[{1}]无效，实例化异常，该任务将被跳过。", taskSetting, taskSetting.Type);
                Logger.Error(msg, ex);
                return null;
            }
        }

        /// <summary>
        /// 开始停止全部。
        /// 并清空集合
        /// TODO:这儿的停止要把所有的任务全部停掉后才能真正的停止,,有待检测。
        /// </summary>
        public void Stop()
        {
            //Note:线程锁定
            lock (this)
            {
                if (_watcher == null) return;
                Logger.Debug("----- 服务停止开始 -----");
                _watcher.Stop();
                if (Tasks.Count > 0)
                {
                    //开始通知所有任务，让其终止。
                    foreach (var task in Tasks)
                    {
                        task.Stop();
                    }
                    Tasks = null;
                }
                //所有任务停止后 释放监工计时器
                _watcher.Dispose();
                _watcher = null; //干掉引用
                Logger.Debug("----- 服务停止完成 -----");
            }
        }

        /// <summary>
        /// 暂停所有任务。
        /// </summary>
        public void Pause()
        {
            //Note:线程锁定
            lock (this)
            {
                Logger.Debug("----- 服务暂停开始 -----");
                _watcher.Stop();
                foreach (var task in Tasks)
                {
                    task.Pause();
                }
                Logger.Debug("----- 服务暂停结束 -----");
            }
        }

        /// <summary>
        /// 从暂停处重新执行。
        /// </summary>
        public void Resume()
        {
            //Note:线程锁定
            lock (this)
            {
                Logger.Debug("----- 服务恢复开始 -----");
                foreach (var task in Tasks)
                {
                    task.Resume();
                }
                _watcher.Start();
                Logger.Debug("----- 服务恢复结束 -----");
            }
        }

        /// <summary>
        /// 发生配置变化后约30秒延迟时间才会全部重启完成。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TaskConfigChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                //Note:线程锁定
                lock (this)
                {
                    Logger.Info("■■■■■■ 文件{0}发生变化。变化类型:{1} ■■■■■■", e.Name, e.ChangeType);
                    _watcher.Stop();

                    //原任务全部移除
                    foreach (var task in Tasks)
                    {
                        task.ChangeStatus(TaskRunStatus.Removing);
                    }
                    TryRemoveTask();

                    TaskSetting = TaskConfig.GetInstance(true);
                    TaskSetting.Changed += TaskConfigChanged; //只保留一次的事件

                    //Note:初始化计时器
                    //_watcher = new Timer(cfg.WatchTimer.DelayMillisecond); //Note:第一次运行不按间隔时间执行。
                    _watcher.Interval = ((TimeSpan)TaskSetting.WatchTimer.WorkingInterval).TotalMilliseconds;
                    _watcher.Start(); //启动工作回调
                    Logger.Info("■■■■■■ 更新配置后的工作间隔为{0}后 ■■■■■■", TimeSpan.FromMilliseconds(_watcher.Interval));
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("重载任务配置发生异常", ex);
                throw;
            }
        }

        /// <summary>
        /// 尝试移除待移除的任务
        /// </summary>
        private void TryRemoveTask()
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                var task = Tasks[i];
                if (task.Task.Execution.RunStatus != TaskRunStatus.Removing) continue;
                task.Dispose();
                var val = Tasks.Remove(task);
                Logger.Debug("[{0}] 的移除结果：{1}。", task, val);
            }
        }
    }
}
