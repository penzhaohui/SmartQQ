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
        public string Name { get; set; }
        public string QQAccount { get; set; }
        public bool Online { get; set; }

        public string PTWebQQ { get; set; }
        public string VFWebQQ { get; set; }
        public string PSessionId { get; set; }       
        public string Hash { get; set; }

        public int FriendCount { get; set; }
        public int GroupCount { get; set; }
        public int DicussionCount { get; set; }

        public DateTime Birthday { get; set; }
        public int Blood { get; set; }
        public string City { get; set; }
        public string College { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Homepage { get; set; }
        public string Lnick { get; set; }
        public string Mobile { get; set; }
        public string Nick { get; set; }
        public string Occupation { get; set; }
        public string Personal { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public int Shengxiao { get; set; }
        public int Face { get; set; }

        public List<QQFriendAccount> FriendAccounts { get; set; }
        public List<GroupAccount> GroupAccounts { get; set; }
        public List<DiscussionAccount> DiscussionAccounts { get; set; }
        public CookieContainer CookieContainer { get; set; }
    }
}
