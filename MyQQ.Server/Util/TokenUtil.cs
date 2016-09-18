using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyQQ.Server.Util
{
    public class TokenUtil
    {
        /// <summary>
        /// New one token
        /// </summary>
        /// <returns></returns>
        public static string NewToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}