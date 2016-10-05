using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartTask
{
    /// <summary>
    /// 错误发生时后续操作枚举
    /// </summary>
    public enum ErrorWayType
    {
        /// <summary>
        /// 默认，一直执行下去，直到执行成功完成任务。
        /// </summary>
        Default = 0,

        /// <summary>
        /// 休眠后执行，直到失败次数执行完成后停止，如果失败次数为设定值后停止
        /// </summary>
        TryAndStop = 1,

        /// <summary>
        /// 停止执行
        /// </summary>
        Stop = 2,
    }
}
