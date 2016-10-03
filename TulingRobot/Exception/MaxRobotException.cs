using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TulingRobot
{
    public class MaxRobotException : Exception
    {
        private int _maxRobot = 0;

        public MaxRobotException(int maxRobot)
            : base("机器人数已达到" + maxRobot + "个")
        {
            this._maxRobot = maxRobot;
        }
    }
}
