using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TulingRobot
{
    internal class RobotStruct : IDisposable
    {
        private readonly RobotType _type;
        private readonly IRobot _robot;
        private readonly int _maxClientCount;
        private readonly List<string> _clientKeys = new List<string>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="robot">机器人</param>        
        /// <param name="type">机器人类型</param>
        /// <param name="maxClientCount">最大服务客户数</param>
        public RobotStruct(IRobot robot, RobotType type = RobotType.Public, int maxClientCount = 1)
        {
            if (RobotType.Private == type && maxClientCount > 1)
            {
                throw new ArgumentException("私有类型机器人的最大服务客户数为 1 ");
            }

            this._type = type;
            this._maxClientCount = maxClientCount;
            this._robot = robot;
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

        /// <summary>
        /// 机器人实例
        /// </summary>
        public IRobot Robot
        {
            get 
            {
                return _robot;
            }
        }

        /// <summary>
        /// 迭代返回注册的Client key
        /// </summary>
        public IEnumerable<string> RegisteredClientKeys
        {
            get
            {
                foreach (string clientKey in _clientKeys)
                {
                    yield return clientKey;
                }
            }
        }

        /// <summary>
        /// 检测是否可以注册
        /// </summary>
        /// <returns>true - 表示可以注册</returns>
        public bool CanRegister()
        {
            return _clientKeys.Count < _maxClientCount;
        }


        /// <summary>
        /// 注册机器人
        /// </summary>
        /// <param name="cientKey"></param>
        /// <returns></returns>
        public bool Register(string cientKey)
        {
            if (RobotType.Private == this._type)
            {
                return false;
            }
            else
            {
                if (CanRegister())
                {
                    this._clientKeys.Add(cientKey);
                }
                else
                {
                    throw new MaxClientException(_maxClientCount);
                }
            }

            return true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
