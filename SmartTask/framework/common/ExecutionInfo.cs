using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    /// 任务运行状态
    /// </summary>
    [Serializable]
    public class ExecutionInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public ExecutionInfo()
        {
        }

        /// <summary>
        /// 任务配置是否还存在
        /// </summary>
        [XmlIgnore]
        public bool IsExsit { get; set; }

        /// <summary>
        /// 最后运行时间
        /// </summary>
        public DateTime? LastRun { get; set; }

        /// <summary>
        /// 最后成功运行的时间 
        /// 新任务时为null , 只有补全功能需要时才使用。每次成功运行后更新该值。
        /// Note:主要目的是用于补全时使用的
        /// </summary>
        public DateTime? LastSucceedRun { get; set; }

        /// <summary>
        /// 正确运行的次数
        /// </summary>
        [XmlAttribute]
        public int RunTimes { get; set; }

        /// <summary>
        /// 错误运行时休眠的次数
        /// </summary>
        [XmlAttribute]
        public int SleepTimes { get; set; }

        /// <summary>
        /// 任务状态
        /// Note:状态是给人看的，不是判断任务是否执行的依据。
        /// </summary>
        [XmlAttribute]
        public TaskRunStatus RunStatus { get; set; }        
    }
}
