using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQQ.Entity
{
    public class MessageEntity
    {
        public string GroupName { get; set; }
        public string DiscussionName { get; set; }        
        public string MessageContent { get; set; }
        public string MessagerAccount { get; set; }
        public string MessagerName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
