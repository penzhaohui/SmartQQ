using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    /// 计时器的相关配置
    /// </summary>
    [Serializable]
    [XmlRoot("WatchTimer")]
    public class TimerConfig
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public TimerConfig()
        {
            DelayMillisecond = 500;
            WorkingInterval = new WorkTimeSpan();
        }

        /// <summary>
        /// 计时器初始化时延时[毫秒]
        /// </summary>
        [XmlAttribute("DelayMillisecond")]
        public int DelayMillisecond { get; set; }

        /// <summary>
        /// 工作间隔
        /// </summary>
        [XmlElement(ElementName = "WorkingInterval")]
        public WorkTimeSpan WorkingInterval { get; set; }
    }
}
