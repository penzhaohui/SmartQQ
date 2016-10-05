using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTask
{
    /// <summary>
    /// 公有资源集合
    /// </summary>
    [Serializable]
    public class ResourceCollection : List<ResourceInfo>
    {
        /// <summary>
        /// 按名称索引
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <returns>资源内容</returns>
        public ResourceInfo this[string name]
        {
            get { return Find(x => x.Name == name); }
        }
    }
}
