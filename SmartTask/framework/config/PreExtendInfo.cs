using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    ///   默认的预定义扩展
    /// </summary>
    /// 
    /// <description class = "CS.WinService.ExtendInfo">
    ///   预定义的内部扩展，如果不用要重写LoadExtend方法并自已实现扩展
    /// </description>
    [Serializable]
    [XmlRoot("extend")]
    public class PreExtendInfo : TaskExtendBase
    {
        /// <summary>
        /// 初始化引用资源的信息
        /// <para>将公有资源赋值到引用上</para>
        /// </summary>
        /// <param name="resources"></param>
        public void InitRefResource(ResourceCollection resources)
        {
            foreach (var reference in Refs)
            {
                reference.Resource = resources[reference.ResName];
            }
        }

        /// <summary>
        /// 公有资源信息
        /// </summary>
        [XmlArrayItem(ElementName = "ref")]
        public RefCollection Refs { get; set; }


        /// <summary>
        /// 未关联至公有资源的参数集合
        /// </summary>
        [XmlArrayItem(ElementName = "add")]
        public ParamCollection Settings { get; set; }

    }
}
