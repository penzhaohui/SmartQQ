using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    ///   任务扩展抽象基类
    /// <example>
    /// <code>
    /// <para>[Serializable]</para>
    /// <para>[XmlRoot("extend")] </para>
    /// <para>public class MyExtend : TaskExtendBase</para>
    /// <para>{...}</para>
    /// </code>
    /// </example>
    /// </summary>
    /// <description class = "CS.WinService.TaskExtendBase">
    ///   请在继承的类加上    [Serializable][XmlRoot("extend")] 
    /// </description>
    [Serializable]
    [XmlRoot("extend")]
    public abstract class TaskExtendBase
    {
        /// <summary>
        /// 返回扩展的Xml配置文本未例
        /// </summary>
        public virtual string ToXml()
        {
            return XmlSerializor.Serialize(this);
        }
    }
}
