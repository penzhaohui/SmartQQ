using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.json
{
    internal class Friends
    {
        public int retcode;
        public Result result;
        
        public class Result
        {
            /// 好友汇总
            public List<Friend> friends;
            /// 备注名
            public List<MarkNames> markNames;
            /// 分组信息
            public List<Category> categories;
            /// 备注
            public List<Vipinfo> vipInfo;
            /// 好友信息
            public List<Info> info;

            /// 好友汇总 
            public class Friend
            {
                public int flag;
                public string uin;
                public int categories;
            }

            /// 备注 
            public class MarkNames
            {
                public string uin;
                public string markname;
                public int type;
            }

            /// 分组
            public class Category
            {
                public int index;
                public int sort;
                public string name;
            }

            /// vip信息
            public class Vipinfo
            {
                public int vip_level;
                public string u;
                public int is_vip;
            }

            /// 好友信息
            public class Info
            {
                public int face;
                public string flag;
                public string nick;
                public string uin;
            }            
        }
    }
}

/*
{
    "retcode": 0,
    "result": {
        "friends": [
            {
                "flag": 36,
                "uin": 3375505137,
                "categories": 9
            }
        ],
        "marknames": [
            {
                "uin": 3375505137,
                "markname": "弘博 - 岳建伟",
                "type": 0
            }
        ],
        "categories": [
            {
                "index": 1,
                "sort": 5,
                "name": "大学校友"
            }
        ],
        "vipinfo": [
            {
                "vip_level": 7,
                "u": 1164039666,
                "is_vip": 1
            }
        ],
        "info": [
            {
                "face": 594,
                "flag": 298320452,
                "nick": "岳建伟",
                "uin": 3375505137
            }
        ]
    }
}
 */
