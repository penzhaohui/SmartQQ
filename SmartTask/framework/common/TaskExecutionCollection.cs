using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTask
{
    /// <summary>
    /// 任务运行状态集合
    /// </summary>
    [Serializable]
    public class TaskExecutionCollection : List<TaskMetaInfo>
    {
        /// <summary>
        /// 按工作Id索引返回执行信息
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <returns>工作信息</returns>
        public TaskMetaInfo this[string taskId]
        {
            get
            {
                var task = Find(x => x.Id == taskId);
                return task;
            }
            set
            {
                var task = Find(y => y.Id == taskId);
                task = value;
            }
        }
    }
}
