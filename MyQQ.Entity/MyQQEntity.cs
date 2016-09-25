using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQQ.Entity
{
    public class MyQQEntity
    {
        public string ClientID { get; set; }
        public bool IsInitialized { get; set; }
        public string QQAccount { get; set; }
        public string Name { get; set; }
        public int FriendAccount { get; set; }
        public int GroupCount { get; set; }
        public int DiscussionCount { get; set; }
    }
}
