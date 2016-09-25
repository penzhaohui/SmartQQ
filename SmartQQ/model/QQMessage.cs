using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.model
{
    public class QQMessage
    {
        public AccountType AccountType { get; set; }
        public string Uin { get; set; }
        public string GroupID { get; set; }
        public string DiscussionID { get; set; }
        public MessageType MessageType { get; set; }
        public string MessageContent;
    }
}
