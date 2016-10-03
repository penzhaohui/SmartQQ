using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TulingRobot
{
    public class MaxClientException : Exception
    {
        private int _maxClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxClient"></param>
        public MaxClientException(int maxClient)
            : base("客户数已达到" + maxClient + "个")
        {
            this._maxClient = maxClient;
        }
    }
}
