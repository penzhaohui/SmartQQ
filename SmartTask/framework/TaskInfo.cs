using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    /// 单一任务配置信息
    /// </summary>
    [Serializable, XmlRoot(ElementName = "task")]
    public class TaskInfo
    {
        /// <summary>
        /// 任务是否启用
        /// </summary>
        [XmlAttribute("enable")]
        public bool Enable { get; set; }

        /// <summary>
        /// 触发时间表达式
        /// Note: 工作时间触发器:只负责启动触发，执行次数不需设在工作触发器里（即符合该表达式的时间内进行有效）：使用Quartz.net的Cron表达式
        /// </summary>
        [XmlAttribute("timeTrigger")]
        public string TimeTrigger { get; set; }

        /// <summary>
        /// 该任务反射Type
        /// </summary>
        [XmlAttribute("type")]
        public string Type { get; set; }

        /// <summary>
        /// 任务元 ，基本信息
        /// </summary>
        [XmlElement(ElementName = "meta")]
        public TaskMetaInfo Meta { get; set; }

        /// <summary>
        /// 本任务运行情况
        /// </summary>
        public TaskExecutionInfo Execution { get; set; }

        /// <summary>
        /// 工作执行时间遇错等相关设定
        /// </summary>
        [XmlElement("workSetting")]
        public TaskWorkSettingInfo WorkSetting { get; set; }

        #region 方法 GetNextLaunchTime

        /// <summary>
        /// 计算任务(单个)的启动时间[当前计算机所处时区]
        /// </summary>
        /// <returns></returns>
        public DateTime? GetNextRunTime(DateTime? lastRunTime = null)
        {
            if (Execution.RunTimes >= WorkSetting.Times && WorkSetting.Times > 0) return null;
            if (lastRunTime == null) lastRunTime = SystemTime.Now();
            var cronExp = new CronExpression(TimeTrigger);
            var runTime = (DateTime)cronExp.GetNextValidTimeAfter(lastRunTime.Value.ToUniversalTime());
            return runTime.ToLocalTime();
        }

        /// <summary>
        /// 下次启动的时间间隔
        /// </summary>
        /// <param name="lastRunTime"></param>
        /// <returns></returns>
        public TimeSpan? GetNextInterval(DateTime? lastRunTime = null)
        {
            if (Execution.RunTimes >= WorkSetting.Times && WorkSetting.Times > 0) return null;
            if (lastRunTime == null) lastRunTime = SystemTime.Now();
            var cronExp = new CronExpression(TimeTrigger);
            var runTime = (DateTime)cronExp.GetNextValidTimeAfter(lastRunTime.Value.ToUniversalTime());
            return runTime.ToLocalTime().Subtract(lastRunTime.Value.ToLocalTime());
        }

        #endregion

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}:{1}", Meta.Id, Meta.Name);
        }
    }
}
