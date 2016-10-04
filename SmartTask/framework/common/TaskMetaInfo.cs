using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    /// 任务属性
    /// </summary>
    [Serializable]
    public class TaskMetaInfo
    {
        /// <summary>
        /// 任务Id，必须有
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 任务是否要自动补全
        /// </summary>
        [XmlAttribute("isPatch")]
        public bool IsPatch { get; set; }

        /// <summary>
        /// 任务Hash，根据Type来
        /// </summary>
        [XmlIgnore]
        public int TaskHash { get; set; }
    }
}
