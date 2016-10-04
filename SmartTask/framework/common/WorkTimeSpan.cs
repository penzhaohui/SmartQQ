using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    /// 可以序列化的MyTimeSpan
    /// TimeSpan 不能继承
    /// </summary>
    public class WorkTimeSpan
    {
        private TimeSpan _timeSpan;

        /// <summary>
        /// 默认为暂停5分钟
        /// </summary>
        public WorkTimeSpan()
            : this(new TimeSpan(0, 0, 5, 0))
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        public WorkTimeSpan(TimeSpan time)
        {
            _timeSpan = time;
        }

        /// <summary>
        /// 执行次数
        /// </summary>
        [XmlAttribute("times")]
        public int Times { get; set; }

        /// <summary>
        /// 仅供序列化使用，不要直接读取该值。
        /// </summary>
        [XmlText]
        public string XmlText
        {
            get { return _timeSpan.ToString(); }
            set { _timeSpan = TimeSpan.Parse(value); }
        }

        /// <summary>
        /// 隐式转换 WorkTimeSpan -> TimeSpan
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static implicit operator TimeSpan(WorkTimeSpan t)
        {
            return t._timeSpan;
        }

        /// <summary>
        /// 隐式转换 TimeSpan -> WorkTimeSpan
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static implicit operator WorkTimeSpan(TimeSpan t)
        {
            return new WorkTimeSpan(t);
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _timeSpan.ToString();
        }
    }
}
