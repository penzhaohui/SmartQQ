using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TulingRobot
{
    /// <summary>
    /// 申请机器人的方式
    /// </summary>
    public enum RobotType
    {
        Private = 0,    // 独占的，仅为一个客户服务
        Public = 1      // 共享的，同时为多个客户服务
    }
}
