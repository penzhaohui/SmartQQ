using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.model
{
    public class GroupAccount
    {
        public string Gid { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Code { get; set; }
        public string CreateTime { get; set; }
        public string FingerMemo { get; set; }
        public string Memo { get; set; }
        public string Flag { get; set; }
        public int Level { get; set; }
        public string Owner { get; set; }
        public string MarkName { get; set; }

        public List<GroupMember> Members { get; set; }
    }
}
