using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.json
{
    internal class OnlineFirends
    {
        public int retcode;
        public List<OnlineUser> result;

        public class OnlineUser
        {
            public int type;
            public string uin;
        }
    }
}
