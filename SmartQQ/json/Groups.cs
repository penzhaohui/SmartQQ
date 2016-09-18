using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.json
{
    internal class Groups
    {
        public int retcode;
        public Result result;
        public class Result
        {
            public List<Gnamelist> gnamelist;
            public class Gnamelist
            {
                public string flag;
                public string gid;
                public string code;
                public string name;
            }
        }
    }    
}

/*

{
    "retcode": 0,
    "result": {
        "gmasklist": [],
        "gnamelist": [
            {
                "flag": 16778241,
                "name": "美丽心情107",
                "gid": 1501125188,
                "code": 1751887145
            }
        ],
        "gmarklist": []
    }
}

 * */
