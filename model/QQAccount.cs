using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.model
{
    public class QQAccount
    {
        public string uin { get; set; }
        public string name { get; set; }
        public string category { get; set; }

        public string markname;
        public string nick;
        public string gender;

        public int face;
        public int client_type;
        public int categories;
        public string status;

        public string occupation;   //职业                    
        public string college;
        public string country;
        public string province;
        public string city;
        public string personal;     //简介

        public string homepage;
        public string email;
        public string mobile;
        public string phone;

        public DateTime birthday;
        public int blood;
        public int shengxiao;
        public int vip_info;

        public int onlineType;
    }
}
