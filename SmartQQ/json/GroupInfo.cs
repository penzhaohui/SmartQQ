using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.json
{
    internal class GroupInfo
    {
        public int retcode;
        public Result result;
        public class Result
        {
            public List<Minfo> minfo;
            public List<Card> cards;
            public Ginfo ginfo;
            public class Minfo
            {
                public string nick;
                public string province;
                public string gender;
                public string uin;
                public string country;
                public string city;
            }
            public class Card
            {
                public string muin;
                public string card;
            }
            public class Ginfo
            {
                public string code;
                public string createtime;
                public string flag;
                public string name;
                public string gid;
                public string owner;
                public List<Members> members;
                public int face;
                public string memo;
                public int level;

                public class Members
                {
                    public string muin;
                    public int mflag;
                }
            }
        }
    }
}

/*
{
    "retcode": 0,
    "result": {
        "stats": [
            {
                "client_type": 1,
                "uin": 3854912521,
                "stat": 10
            }
        ],
        "minfo": [
            {
                "nick": "Wangzd",
                "province": "",
                "gender": "male",
                "uin": 4087874276,
                "country": "",
                "city": ""
            }
        ],
        "ginfo": {
            "face": 4,
            "memo": "元旦快乐~\r\n               \r\n",
            "class": 10014,
            "fingermemo": "",
            "code": 561591007,
            "createtime": 1074526480,
            "flag": 16778241,
            "level": 0,
            "name": "美丽心情107",
            "gid": 4052157118,
            "owner": 1296664712,
            "members": [
                {
                    "muin": 4087874276,
                    "mflag": 1
                }
            ],
            "option": 2
        },
        "cards": [
            {
                "muin": 2259052166,
                "card": "谭娟"
            }
        ],
        "vipinfo": [
            {
                "vip_level": 0,
                "u": 4087874276,
                "is_vip": 0
            }
        ]
    }
}
*/