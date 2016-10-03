using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TulingRobot
{
    public class RobotPool
    {
        private int _maxRobot = 0;
        private int _minRobot = 0;
        private int _newRobot = 0;
        private int _maxClientPerRobot = 0;
        //private int _idledRobot = 0;
        private int _maxIdleMinutes = 0;

        /// <summary>
        /// 机器人池
        /// </summary>
        private readonly Dictionary<string, RobotStruct> _robots = new Dictionary<string, RobotStruct>();
        /// <summary>
        /// 图灵机器人 - API Url
        /// </summary>
        private readonly string APIBaseURL = string.Empty;
        /// <summary>
        /// 图灵机器人 - API Key
        /// </summary>
        private readonly List<string> APIKeys = new List<string>();
        /// <summary>
        /// 机器人花名册
        /// </summary>
        private readonly Dictionary<string, string> _roster = new Dictionary<string, string>();
        /// <summary>
        /// 机器人回收计时器
        /// </summary>
        private System.Timers.Timer timer;

        #region 机器人池构造函数

        private static RobotPool _pool = null;
        private readonly static object _locker = new object();

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private RobotPool()
        {
            APIBaseURL = "http://www.tuling123.com/openapi/api";

            APIKeys.Add("a4cbd07df23e40b08833bf92765e5dbc");
            APIKeys.Add("a0471ae861f34009a20950aadd93c88f");
            APIKeys.Add("24fb876dd5294fecd18fb66cdc7633b1");
            APIKeys.Add("34b73e523c02045a9701387b66ad63db");
            APIKeys.Add("531352803455eaf17317cd5b315a347d");
            APIKeys.Add("11f83b13f0784e77a897b1acaec86f33");
            APIKeys.Add("6c2cfaf7a7f088e843b550b0c5b89c26");            

            timer = new System.Timers.Timer(60000);            
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            this.timer.Start();

            Init();
        }

        /// <summary>
        /// 初始化函数
        /// </summary>
        private void Init()
        {
            _maxRobot = APIKeys.Count;  // 最多同时在线的机器人数
            _minRobot = 1;              // 最少在线的机器人数
            _newRobot = 2;              // 一次生成的机器人数
            _maxClientPerRobot = 5;     // 单个共享类型机器人可服务的最大客户数
            _maxIdleMinutes = 5;        // 机器人最大闲置时间长度(单位：分钟)
        }

        /// <summary>
        /// 机器人每个1秒自动回收闲置时长超过规定分钟数的机器人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<string> idledApiKey = new List<string>();

            foreach (string apiKey in _robots.Keys)
            {
                RobotStruct robotStruct = _robots[apiKey];
                IRobot robot = robotStruct.Robot;
                if (robot.IdledMinutes > _maxIdleMinutes)
                {
                    idledApiKey.Add(apiKey);

                    foreach (string clientKey in robotStruct.RegisteredClientKeys)
                    {
                        if (_roster.ContainsKey(clientKey))
                        {
                            if (apiKey == _roster[clientKey])
                            {
                                // 先从花名册中删除 Client Key
                                _roster.Remove(clientKey);
                            }
                        }
                    }
                }
            }

            // 后从机器人池删除机器人
            foreach (string apiKey in idledApiKey)
            {
                _robots.Remove(apiKey);
            }

            // 创建最少数量的共享类型机器人
            lock (this)
            {
                int newRobotCount = (_minRobot) - (_robots.Count);
                foreach (string key in APIKeys)
                {
                    if (newRobotCount <= 0)
                    {
                        break;
                    }

                    if (!_robots.ContainsKey(key))
                    {
                        newRobotCount--;

                        IRobot robot = new Robot(APIBaseURL, key);
                        RobotStruct robotStruct = new RobotStruct(robot, RobotType.Public, _maxClientPerRobot);
                        _robots.Add(key, robotStruct);
                    }
                }
            }
        }


        /// <summary>
        /// 单实例生成方法
        /// </summary>
        /// <returns></returns>
        public static RobotPool NewInstance()
        {
            if (_pool == null)
            {
                lock (_locker)
                {
                    if (_pool == null)
                    {
                        _pool = new RobotPool();
                    }
                }
            }

            return _pool;
        }

        #endregion

        /// <summary>
        /// 获取一个可以工作的机器人
        /// </summary>
        /// <param name="clientKey">客户的唯一标识</param>
        /// <param name="type">申请机器人的类型</param>
        /// <returns>一台可工作的机器人</returns>
        public IRobot ApplyOneAvailableRobot(RobotType type, string clientKey = null)
        {
            IRobot robot = null;

            if (String.IsNullOrEmpty(clientKey))
            {
                throw new ArgumentException("Client Key 不能为空");
            }

            // 1. 如果之前有注册机器人，则继续使用
            if (_roster.ContainsKey(clientKey))
            {
                string apiKey = _roster[clientKey];
                RobotStruct robotStruct = _robots[apiKey];

                if (robotStruct.RobotType != type)
                {
                    throw new UnmatchedTypeException(robotStruct.RobotType, "已经注册了不同类型的机器人");
                }

                robot = robotStruct.Robot;
                return robot;
            }

            // 2. 如果是共享类型的机器人，则搜索
            if (RobotType.Public == type)
            {
                foreach (string apiKey in _robots.Keys)
                {
                    RobotStruct robotStruct = _robots[apiKey];
                    if (RobotType.Public == robotStruct.RobotType
                        && robotStruct.CanRegister())
                    {
                        if (robotStruct.Register(clientKey))
                        {
                            robot = robotStruct.Robot;
                            return robot;
                        }
                    }
                }
            }

            // 3. 找不到合适的机器人，则创建新的机器人，并加入池中
            robot = CreateRobot(clientKey, type);

            return robot;
        }

        /// <summary>
        /// 创建机器人实例
        /// </summary>
        private IRobot CreateRobot(string clientKey, RobotType type)
        {
            IRobot registeredRobot = null;

            // 加锁，主要防止同时并发创建机器人实例对象
            lock (this)
            {
                if (_robots.Count >= _maxRobot)
                {
                    throw new MaxRobotException(_maxRobot);
                }

                int newRobotCount = 0;
                if (RobotType.Private == type)
                {
                    newRobotCount = 1;
                }
                else if (RobotType.Public == type)
                {
                    newRobotCount = _newRobot;                    
                    if (_robots.Count + _newRobot >= _maxRobot)
                    {
                        newRobotCount = _maxRobot - _robots.Count;
                    }
                }

                // 创建机器人   
                int maxClientPerRobot = _maxClientPerRobot;
                if (RobotType.Private == type)
                {
                    maxClientPerRobot = 1;
                }

                foreach (string key in APIKeys)
                {
                    if (newRobotCount <= 0)
                    {
                        break;
                    }

                    if (!_robots.ContainsKey(key))
                    {
                        newRobotCount--;

                        IRobot robot = new Robot(APIBaseURL, key);
                        RobotStruct robotStruct = new RobotStruct(robot, type, maxClientPerRobot);

                        if (null == registeredRobot)
                        {
                            robotStruct.Register(clientKey);
                            registeredRobot = robotStruct.Robot;
                            _roster.Add(clientKey,key);
                        }

                        _robots.Add(key, robotStruct);
                    }
                }
            }

            return registeredRobot;
        }
    }
}
