using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TulingRobot
{
    public class UnmatchedTypeException : Exception
    {
        public RobotType _type;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">机器人类型</param>
        /// <param name="message">消息提示</param>
        public UnmatchedTypeException(RobotType type, string message)
            : base(message)
        {
            _type = type;
        }

        /// <summary>
        /// 机器人类型
        /// </summary>
        public RobotType RobotType
        {
            get
            {
                return _type;
            }
        }
    }
}
