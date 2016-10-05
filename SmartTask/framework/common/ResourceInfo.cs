using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    ///  公有资源信息
    /// </summary>
    [Serializable]
    public class ResourceInfo
    {
        /// <summary>
        /// 资源名称，调用资源的key
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        [XmlAttribute(AttributeName = "type")]
        public ResourceType Type { get; set; }

        /// <summary>
        /// 资源的值，对应于Name的值的调用
        /// </summary>
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }

        /// <summary>
        /// 资源参数集合
        /// </summary>
        [XmlArrayItem(ElementName = "add")]
        public ParamCollection Params { get; set; }

    }
}
