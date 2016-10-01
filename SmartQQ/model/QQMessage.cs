using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.model
{
    public class QQMessage
    {
        public string QQAccount { get; set; }
        public AccountType AccountType { get; set; }
        public string FriendID { get; set; }
        public string GroupID { get; set; }
        public string DiscussionID { get; set; }
        public MessageType MessageType { get; set; }
        public string MessageContent;
    }
}
