using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    /// 公有资源的引用信息
    /// </summary>
    [Serializable]
    public class RefInfo
    {
        /// <summary>
        /// 本任务内容的名称，调用资源的key
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 引用的公有资源名称
        /// </summary>
        [XmlAttribute(AttributeName = "resName")]
        public string ResName { get; set; }

        /// <summary>
        /// 引用的公有资源
        /// </summary>
        [XmlIgnore]
        public ResourceInfo Resource { get; set; }

        /// <summary>
        /// 针对该任务，该资源的参数的集合
        /// </summary>
        [XmlArrayItem(ElementName = "add")]
        public ParamCollection Params { get; set; }
    }
}
