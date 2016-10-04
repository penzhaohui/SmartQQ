using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    /// 该任务所有时间运行执行范围
    /// </summary>
    [Serializable]
    public class TaskWorkSettingInfo
    {
        /// <summary>
        /// 默认初始化
        /// </summary>
        public TaskWorkSettingInfo()
        {
            Times = 0;
            ErrorWay = ErrorWayType.Default;
            SleepInterval = new WorkTimeSpan();
        }

        ///// <summary>
        ///// 需要时的任务延时，秒
        ///// </summary>
        //[XmlAttribute("delaySecond")]
        //public int DelaySecond { get; set; }

        /// <summary>
        /// 任务超时时间，秒（暂未用上）
        /// </summary>
        [XmlAttribute("timeout")]
        public int Timeout { get; set; }

        /// <summary>
        /// 运行的总次数，超过多少次后不再运行
        /// <remarks>
        /// TODO:目前的计数好像有问题，当执行时间间隔小于守护线程的间隔时，计数会被重置。
        /// </remarks>
        /// </summary>
        [XmlAttribute("times")]
        public int Times { get; set; }

        /// <summary>
        /// 发生错误时的后续操作
        /// </summary>
        [XmlAttribute("whenErrorHappened")]
        public ErrorWayType ErrorWay { get; set; }

        /// <summary>
        /// 错误时的运行时休眠的间隔
        /// </summary>
        [XmlElement("sleepInterval")]
        public WorkTimeSpan SleepInterval { get; set; }
    }
}
