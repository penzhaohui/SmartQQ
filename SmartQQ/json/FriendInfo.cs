using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.json
{
    internal class FriendInfo
    {
        public int retcode;
        public Result result;
        public class Result
        {
            public int face;
            public Birthday birthday;
            public string occupation;
            public string phone;
            public int allow;
            public string college;
            public string uin;
            public int constel;
            public int blood;
            public string homepage;
            public int stat;
            public string city;
            public string personal;
            public string nick;
            public int shengxiao;
            public string email;
            public string province;
            public string gender;
            public string mobile;
            public string country;
            public int vip_info;

            public class Birthday
            {
                public int month;
                public int year;
                public int day;
            }
        }        
    }

    /*
    {
    "retcode": 0,
    "result": {
        "face": 594,
        "birthday": {
            "month": 7,
            "year": 1974,
            "day": 21
        },
        "occupation": "教育/培训",
        "phone": "13823572133",
        "allow": 1,
        "college": "郑州大学",
        "uin": 2312665869,
        "constel": 0,
        "blood": 0,
        "homepage": "www.szpmp.cn",
        "stat": 20,
        "vip_info": 6,
        "country": "中国",
        "city": "深圳",
        "personal": "PMP国际项目管理师培训，软考培训，国际敏捷项目管理师ACP认证，ITIL认证，PRINCE2认证，网络信息安全工程师认证培训，企业项目管理内训，个人职业生涯规划指导",
        "nick": "岳建伟",
        "shengxiao": 0,
        "email": "",
        "province": "广东",
        "gender": "male",
        "mobile": "-"
    }*/
}
