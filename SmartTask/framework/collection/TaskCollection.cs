using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTask
{
    /// <summary>
    /// 任务配置集合，只读
    /// </summary>
    [Serializable]
    public class TaskCollection : List<TaskInfo>
    {
        /// <summary>
        /// 按工作Id的索引
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <returns></returns>
        public TaskInfo this[string taskId]
        {
            get { return Find(x => x.Meta.Id == taskId); }
        }
    }
}
