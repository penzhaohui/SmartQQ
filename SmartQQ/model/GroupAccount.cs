using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.model
{
    public class GroupAccount
    {
        public string flag;
        public string gid;
        public string code;
        public string name;
        public string createtime;
        public string owner;        
        public int face;
        public string memo;
        public string markname;
        public int level;
        public List<GroupMember> members;
    }
}
