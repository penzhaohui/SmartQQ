using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.model
{
    public class DiscussionAccount
    {
        public string Did { get; set; }
        public string Name { get; set; }
        public List<DiscussionMember> Members { get; set; }
    }
}
