using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTask
{
    /// <summary>
    /// 该任务内的资源引用集合
    /// </summary>
    [Serializable]
    public class RefCollection : List<RefInfo>
    {
        /// <summary>
        /// 按名称索引
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <returns>资源内容</returns>
        public RefInfo this[string name]
        {
            get { return Find(x => x.Name == name); }
        }
    }

}
