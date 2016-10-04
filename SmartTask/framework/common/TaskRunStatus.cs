using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartTask
{
    /// <summary>
    /// 任务运行行状态
    /// </summary>
    public enum TaskRunStatus
    {
        /// <summary>
        /// 默认，需要计算后判断是否要执行
        /// </summary>
        Default = 0,

        /// <summary>
        /// 开始运行
        /// </summary>
        Running,

        /// <summary>
        /// 开始执行任务
        /// </summary>
        Working,

        /// <summary>
        /// 本次任务执行完成
        /// </summary>
        Worked,

        /// <summary>
        /// 正在以错误状态运行休眠中
        /// </summary>
        Sleeping,

        /// <summary>
        /// 正在暂停
        /// </summary>
        Pausing,

        /// <summary>
        /// 任务被暂停
        /// </summary>
        Paused,

        /// <summary>
        /// 正在停止
        /// </summary>
        Stoping,

        /// <summary>
        /// 任务停止了
        /// </summary>
        Stoped,

        /// <summary>
        /// 该任务需要被移除，比如：配置变更，其新的同样任务已经加入
        /// </summary>
        Removing
    }
}
