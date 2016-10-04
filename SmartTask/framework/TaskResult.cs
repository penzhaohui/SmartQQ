using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    /// 任务执行结果
    /// </summary>
    public class TaskResult
    {
        /// <summary>
        /// 运行结果枚举
        /// </summary>
        public TaskResultType Result { get; set; }

        /// <summary>
        /// 任务返回的消息
        /// </summary>
        public string Message { get; set; }

        [XmlIgnore]
        public object ExtendMessage { get; set; }
    }
}
