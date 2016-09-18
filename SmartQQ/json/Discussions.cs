using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.json
{
    internal class Discussions
    {
        public int retcode;
        public Result result;
        public class Result
        {
            public List<Gnamelist> dnamelist;
            public class Gnamelist
            {
                public string did;
                public string name;
            }
        }
    }
}

/*
{
    "retcode": 0,
    "result": {
        "dnamelist": [
            {
                "name": "MSSKY OA4.0 - 重构 & 任务管理",
                "did": 1910281305
            }
        ]
    }
}
*/