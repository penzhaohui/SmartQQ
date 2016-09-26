using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.json
{
    public class Message
    {
        public int retcode;     //状态码
        public string errmsg;   //错误信息
        public string t;        //被迫下线说明
        public string p;        //需要更换的ptwebqq
        public List<Result> result;

        public class Result
        {
            public string poll_type;
            public Value value;
            public class Value
            {
                //收到消息
                public List<object> content;
                public string from_uin;
                public string time;
                //群消息有send_uin，为特征群号；info_seq为群号
                public string send_uin;
                //public string info_seq;              
                //上线提示
                //public string uin;
                //public string status;
                //异地登录
                public string reason;
                //临时会话
                //public string id;
                //public string ruin;
                //public string service_type;
                //讨论组
                public string did;
            }
        }
    }
}
