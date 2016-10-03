using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TulingRobot
{
    public interface IRobot
    {
        /// <summary>
        /// 向机器人发送咨询问题
        /// </summary>
        /// <param name="question">咨询问题</param>
        /// <returns>机器人的回答</returns>
        string Consult(string question);

        /// <summary>
        /// 返回闲置时间长度（单位：分钟）
        /// </summary>
        int IdledMinutes { get; }
    }
}
