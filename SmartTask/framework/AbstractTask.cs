using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask
{
    /// <summary>
    /// 抽象的单一任务
    /// </summary>
    public abstract class AbstractTask : IDisposable
    {
        /// <summary>
        /// 保存运行状态时的互斥体，共享变量，全局唯一
        /// </summary>
        protected static readonly Mutex SaveMutex = new Mutex(false);

        /// <summary>
        /// 工作线程调用间隔，毫秒
        /// </summary>
        public double WorkerInterval { get; set; }

        /// <summary>
        /// 任务信息
        /// </summary>
        public TaskInfo Task { get; set; }

        /// <summary>
        /// 正在运行的次数
        /// </summary>
        private int _runTimes;

        /// <summary>
        /// 是否正在回调中
        /// </summary>
        private bool _isCallBacking;

        /// <summary>
        /// 停止的委托
        /// </summary>
        private Action _stopTask;

        /// <summary>
        /// 任务工作
        /// 用以执行本任务的。
        /// </summary>
        protected Timer TaskWorker { get; set; }

        /// <summary>
        /// 工作委托[返回是否执行]
        /// </summary>
        protected Func<TaskResult> WorkHandler;

        /// <summary>
        /// 共公资源配置信息引用
        /// </summary>
        internal protected ResourceCollection Resources { get; set; }

        /// <summary>
        /// 系统自带的预定义扩展
        /// </summary>
        protected PreExtendInfo Extend { get; set; }

        /// <summary>
        /// 当前工作是否正在移除之中
        /// </summary>
        protected bool IsRemoving
        {
            get
            {
                return this.Task.Execution.RunStatus == TaskRunStatus.Removing;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AbstractTask()
        {
            WorkHandler = () => Work(); //附加该委托即可
            _stopTask = (delegate { }); //默认给一个空的方法
        }

        /// <summary>
        /// 工作运行
        /// Note:两次间隔有100毫秒左右的误差
        /// </summary>
        public virtual void Working()
        {
            try
            {
                // http://www.cnblogs.com/SharkBin/archive/2012/08/04/2622627.html
                TaskWorker.Change(Timeout.Infinite, Timeout.Infinite); //暂停计时。
                if (IsRemoving)
                {
                    Logger.Warn("任务被标起为移除，回调中断，且任务不会被再次调用。");
                    return;
                }

                //执行今日任务
                _runTimes++;
                var now = SystemTime.Now();
                Logger.Info("[{0}] 第{1}次执行开始。[{2}] ◇", this, _runTimes, now.ToShortTimeString());
                ChangeStatus(TaskRunStatus.Working);
                var val = new TaskResult();
                try
                {
                    val = WorkHandler(); //同步委托，任务执行[可能较耗时]
                }
                catch (Exception ex)
                {
                    Logger.Error("执行任务<{0}>时发生异常:{1}", this.Task, ex);                    
                    val.Result = TaskResultType.Error;
                    val.ExtendMessage = ex.Message;
                }
                finally
                {
                    ChangeStatus(TaskRunStatus.Worked);
                    Task.Execution.LastRun = now;
                }
               
                var runSpan = SystemTime.Now() - now;
                Logger.Info("[{0}] 第{1}次执行结果[{2} : {3}] [Execution:{4}]", this, _runTimes, val.Result, val.Message, runSpan);

                //Note:工作完成后的状态处理
                //Note:注意，这里的错误次数实际上是执行失败的次数
                if (val.Result.HasFlag(TaskResultType.Error))
                {
                    var sleepInterval = ((TimeSpan)Task.WorkSetting.SleepInterval);
                    Task.Execution.SleepTimes++;
                    Logger.Warn("[{0}] 状态更新[{1}],休眠次数++ ，准备[{2}]后再次执行", this, val.Result, sleepInterval);
                    TaskWorker.Change(sleepInterval, TimeSpan.FromMilliseconds(-1));
                    return;
                }
                else
                {
                    Task.Execution.RunTimes++;
                    var runInterval = Task.GetNextInterval();
                    if (runInterval == null)
                    {
                        ChangeStatus(TaskRunStatus.Removing);
                        Logger.Debug("[{0}] 下次运行时间为null，当前任务停止。", this);
                        return;
                    }
                    if (runInterval.Value.TotalMilliseconds > WorkerInterval * 5)
                    {
                        ChangeStatus(TaskRunStatus.Removing);
                        Logger.Debug("[{0}] 下次运行时间{1}，超过5倍工作线程间隔，暂时移除执行队列。当前任务停止。", this, runInterval);
                        return;
                    }

                    //var runInterval = runTime.Value.Subtract(now);
                    SaveExecution();
                    Logger.Debug("[{0}]第{1}次执行结束。 运行成功[Times:{2}] ，准备[{3}]后再次执行 ◆", this, _runTimes, Task.Execution.RunTimes, runInterval);
                    TaskWorker.Change(runInterval.Value, TimeSpan.FromMilliseconds(-1)); //Note:更改计时器约50多毫秒
                }

                #region 根据任务配置做出相应动作

                //本次任务已完成,Note:只有本次任务达到所设条件才算是正常完成，正常完成后才更新最后成功完成的时间。
                if ((Task.Execution.RunTimes >= Task.WorkSetting.Times && Task.WorkSetting.Times > 0) ||
                    (val.Result.HasFlag(TaskResultType.Finished)))
                {
                    ChangeStatus(TaskRunStatus.Removing);
                    Logger.Debug("■ [{0}] ({1})完成。■", this, Task.Execution.LastSucceedRun);
                    return;
                }

                //根据设定，一旦有错误发生。立即停止
                if (val.Result.HasFlag(TaskResultType.Error) && Task.WorkSetting.ErrorWay == ErrorWayType.Stop)
                {
                    ChangeStatus(TaskRunStatus.Removing);
                    Logger.Info("▲ [{0}] 根据设定Stop，发生了错误一次，等待移除。▲", this);
                    return;
                }

                //根据设定，有错误时。休眠超期后停止
                if (Task.Execution.SleepTimes >= Task.WorkSetting.SleepInterval.Times &&
                    Task.WorkSetting.SleepInterval.Times > 0 && Task.WorkSetting.ErrorWay == ErrorWayType.TryAndStop)
                {
                    ChangeStatus(TaskRunStatus.Removing);
                    Logger.Info("▲ [{0}] 根据设定Sleep，发生了错误{Task.Execution.SleepTimes}次，等待移除。▲", this);
                    return;
                }

                #endregion

            }
            catch (Exception ex)
            {
                //Note:异常发生后停止该任务，不管任何原因
                Logger.Error("[{0}] 执行异常，停止执行。{1}", this, ex);
                Stop();
            }
            finally
            {
            }
        }

        #region 任务驱动器功能

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            lock (this)
            {
                ChangeStatus(TaskRunStatus.Running);

                //校验 计算启动时间。
                var now = SystemTime.Now();
                
                var launchInterval = Task.GetNextInterval();
                if (launchInterval == null)
                {
                    Logger.Warn("[{0}] 启动时间为null，中断启动。", this);
                    return;
                }
                Task.Execution.RunTimes = 0;
                Task.Execution.SleepTimes = 0;
                TaskWorker = new Timer(TimerCallback, null, launchInterval.Value /*第一次延时调用*/, TimeSpan.FromMilliseconds(-1) /*Note:回调时会更改调用延时，此周期设为无限*/);
                Logger.Debug("[{0}] 启动成功，将于:{1}后运行", this, launchInterval);
            }
        }


        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            //任务回调结束后才可以结束
            if (_isCallBacking)
            {
                Logger.Debug("[{0}]正在回调中，停止方法附加到任务完成后的委托上。", this);
                _stopTask = StopTask; //使用委托来处理
            }
            else
            {
                StopTask();
            }
        }

        private void StopTask()
        {
            lock (this)
            {
                ChangeStatus(TaskRunStatus.Stoping);
                TaskWorker.Change(Timeout.Infinite, Timeout.Infinite); //禁止再次回调                
                Task.Execution.RunStatus = TaskRunStatus.Stoped;
                ChangeStatus(TaskRunStatus.Stoped);
                Logger.Debug("[{0}] 停止，计时器已释放。", this);
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            lock (this)
            {
                ChangeStatus(TaskRunStatus.Pausing);
                TaskWorker.Change(Timeout.Infinite, Timeout.Infinite); //禁止再次回调
                ChangeStatus(TaskRunStatus.Paused);
                Logger.Debug("[{0}]暂停，计时器已暂停。(最后一次回调的任务可能还在执行)", this);
            }
        }

        /// <summary>
        /// 恢复
        /// </summary>
        public void Resume()
        {
            lock (this)
            {
                ChangeStatus(TaskRunStatus.Running);
                var now = SystemTime.Now();
                var nextRun = Task.GetNextRunTime(now);
                if (nextRun == null)
                {
                    Logger.Warn("[{0}] 无法恢复，因为下次执行时间为null", this);
                    return;
                }
                var dueTime = nextRun.Value.Subtract(now);
                TaskWorker.Change(dueTime/*第一次延时调用*/, TimeSpan.FromMilliseconds(-1) /*Note:回调时会更改调用延时，此周期设为无限*/); //重启计时器
                Logger.Debug("[{0}] 暂停，计时器已恢复。(最后一次回调的任务可能还在执行)", this);
            }
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                Stop();
                TaskWorker.Dispose();
                TaskWorker = null; //干掉引用
            }
            catch (Exception ex)
            {
                Logger.Error("释放资源时发生异常。", ex);
            }
        }

        /// <summary>
        /// 线程回调
        /// </summary>
        private void TimerCallback(object state)
        {
            lock (this)
            {
                _isCallBacking = true;
                Working();
                _isCallBacking = false;
            }
        }

        /// <summary>
        /// 状态变更
        /// <remarks>
        /// 正在移除中的状态，不可变更
        /// </remarks>
        /// </summary>
        /// <param name="status"></param>
        public void ChangeStatus(TaskRunStatus status)
        {
            if (Task.Execution.RunStatus == TaskRunStatus.Removing) return;
            Task.Execution.RunStatus = status;
        }

        /// <summary>
        /// 运行状态保存
        /// </summary>
        public void SaveExecution()
        {
            SaveMutex.WaitOne();
            SaveMutex.ReleaseMutex();
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}", this.Task.Meta.Id, this.Task.Meta.Name);
        }

        /// <summary>
        /// 初始化自定义的
        /// <para>自定义了新的扩展配置时一定要重写该方法，并自写扩展代码</para>
        /// </summary>
        public virtual void InitExtend()
        {
            if (Extend == null)
                Extend = Task.GetExtend<PreExtendInfo>();

            Extend.InitRefResource(Resources);
        }


        /// <summary>
        /// 任务入口，继承类必须实现。
        /// </summary>
        /// <returns>
        /// 执行结果，参见：<see cref="TaskResult"/>
        /// </returns>
        protected abstract TaskResult Work();
    }
}
