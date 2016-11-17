using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.json
{
    internal class FirendAccount
    {
        public int retcode;
        public Result result;
        public class Result
        {
            public string account;
            public string uin;
            public string uiuin;
        }
    }
}
