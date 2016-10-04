using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTask
{
    /// <summary>
    /// 抽象的单一任务
    /// </summary>
    public abstract class AbstractTask : IDisposable
    {
        /// <summary>
        /// 任务信息
        /// </summary>
        public TaskInfo Task { get; set; }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("释放资源时发生异常。", ex);
            }
        }

        /// <summary>
        /// 任务入口，继承类必须实现。
        /// </summary>
        /// <returns>
        /// 执行结果，参见：<see cref="TaskResult"/>
        /// </returns>
        protected abstract TaskResult Work();
    }
}
