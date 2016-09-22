using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.model
{
    public class SmartQQWrapper
    {
        public string QQNumber { get; set; }
        public string PTWebQQ { get; set; }
        public string VFWebQQ { get; set; }
        public string PSessionId { get; set; }
        public string UIN { get; set; }
        public string Hash { get; set; }

        public int face;
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

        public List<QQAccount> Friends { get; set; }
        public List<GroupAccount> GroupAccounts { get; set; }
        public List<DiscussionAccount> DiscussionAccounts { get; set; }
        public CookieContainer CookieContainer { get; set; }
    }
}
