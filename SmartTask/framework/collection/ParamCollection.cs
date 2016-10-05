using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTask
{
    /// <summary>
    /// 参数键值对集合
    /// </summary>
    [Serializable]
    public class ParamCollection : List<ParamInfo>
    {
        /// <summary>
        /// 按名称索引
        /// </summary>
        /// <param name="key">参数名称</param>
        /// <returns>参数的值</returns>
        public ParamInfo this[string key]
        {
            get { return Find(x => x.Key == key); }
        }
    }
}
