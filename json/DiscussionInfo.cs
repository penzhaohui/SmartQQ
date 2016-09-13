using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.json
{
    internal class DiscussionInfo
    {
        public int retcode;
        public Result result;
        public class Result
        {
            public Info info;
            public List<MemberInfo> mem_info;
            public List<MemberStatus> mem_status;
            public class Info
            {
                public string discu_owner;
                public string discu_name;
                public string did;
                public List<Member> mem_list;

                public class Member
                {
                    public string mem_uin;
                    public string ruin;
                }
            }
            public class MemberInfo
            {
                public string uin;
                public string nick;
            }
            public class MemberStatus
            {
                public string uin;
                public string status;
                public int client_type;
            }
        }
    }
}

/*
{
    "result": {
        "info": {
            "did": 1910281305,
            "discu_name": "MSSKY OA4.0 - 重构 & 任务管理",
            "mem_list": [
                {
                    "mem_uin": 42362548,
                    "ruin": 2481666
                }
            ]
        },
        "mem_info": [
            {
                "nick": "Ting",
                "uin": 42362548
            }
        ],
        "mem_status": []
    },
    "retcode": 0
}
*/