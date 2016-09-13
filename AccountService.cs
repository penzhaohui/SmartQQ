using Newtonsoft.Json;
using SmartQQ.json;
using SmartQQ.model;
using SmartQQ.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SmartQQ
{
    public class AccountService
    {
        SmartQQWrapper smartQQ = null;
        
        public AccountService(SmartQQWrapper smartQQ)
        {
            this.smartQQ = smartQQ;
        }

        public void GetQQProfile()
        {
            string url = "http://s.web2.qq.com/api/get_self_info2?t=#{t}".Replace("#{t}", HTTP.AID_TimeStamp());
            string dat = HTTP.Get(url, "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1");
            FriendInfo info = (FriendInfo)JsonConvert.DeserializeObject(dat, typeof(FriendInfo));

            smartQQ.face = info.result.face;
            smartQQ.occupation = info.result.occupation;
            smartQQ.phone = info.result.phone;
            smartQQ.college = info.result.college;
            smartQQ.blood = info.result.blood;
            smartQQ.homepage = info.result.homepage;
            smartQQ.vip_info = info.result.vip_info;
            smartQQ.country = info.result.country;
            smartQQ.city = info.result.city;
            smartQQ.personal = info.result.personal;
            smartQQ.nick = info.result.nick;
            smartQQ.shengxiao = info.result.shengxiao;
            smartQQ.email = info.result.email;
            smartQQ.province = info.result.province;
            smartQQ.gender = info.result.gender;
        }

        public void GetFriendList(bool isLoadAccountDetailedInfo = false)
        {
            string url = "http://s.web2.qq.com/api/get_user_friends2";
            string sendData = string.Format("r={{\"vfwebqq\":\"{0}\",\"hash\":\"{1}\"}}", smartQQ.VFWebQQ, smartQQ.Hash);
            string dat = HTTP.Post(url, sendData, "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1");

            Friends friend = (Friends)JsonConvert.DeserializeObject(dat, typeof(Friends));

            if (friend.result == null) return;

            Dictionary<string, string> MarkNameList = new Dictionary<string, string>();
            if (friend.result.markNames != null)
            {
                for (int i = 0; i < friend.result.markNames.Count; i++)
                {
                    MarkNameList.Add(friend.result.markNames[i].uin, friend.result.markNames[i].markname);
                }
            }

            Dictionary<int, string> CategoryList = new Dictionary<int, string>();
            if (friend.result.categories != null)
            {
                for (int i = 0; i < friend.result.categories.Count; i++)
                {
                    CategoryList.Add(friend.result.categories[i].index, friend.result.categories[i].name);
                }
            }

            Dictionary<string, QQAccount> accounts = new Dictionary<string, QQAccount>();
            string uin = String.Empty;
            int categoryIndex = -1;
            string categoryName = String.Empty;

            if (friend.result.friends != null)
            {
                for (int i = 0; i < friend.result.friends.Count; i++)
                {
                    uin = friend.result.friends[i].uin;
                    categoryIndex = friend.result.friends[i].categories;

                    QQAccount account = null;
                    if (accounts.ContainsKey(uin)) account = accounts[uin];
                    else account = new QQAccount();

                    account.uin = uin;
                    account.markname = MarkNameList.ContainsKey(uin) ? MarkNameList[uin] : "";
                    account.name = MarkNameList.ContainsKey(uin) ? MarkNameList[uin] : "";
                    account.category = CategoryList.ContainsKey(categoryIndex) ? CategoryList[categoryIndex] : "";

                    accounts[uin] = account;
                }
            }

            if (friend.result.info != null)
            {
                for (int i = 0; i < friend.result.info.Count; i++)
                {
                    uin = friend.result.info[i].uin;

                    QQAccount account = null;
                    if (accounts.ContainsKey(uin)) account = accounts[uin];
                    else account = new QQAccount();

                    account.nick = friend.result.info[i].nick;
                    if (account.name == "") account.name = friend.result.info[i].nick;

                    accounts[uin] = account;
                }
            }

            smartQQ.Friends = accounts.Values.ToList<QQAccount>();
            if (isLoadAccountDetailedInfo)
            { 
                foreach(QQAccount account in smartQQ.Friends)
                {
                    LoadAccountDetailedInfo(account);
                }
            }

            smartQQ.Friends = smartQQ.Friends;
        }

        private void LoadAccountDetailedInfo(QQAccount account)
        {
            string url = "http://s.web2.qq.com/api/get_friend_info2?tuin=#{uin}&vfwebqq=#{vfwebqq}&clientid=53999199&psessionid=#{psessionid}&t=#{t}".Replace("#{t}", HTTP.AID_TimeStamp());
            url = url.Replace("#{uin}", account.uin).Replace("#{vfwebqq}", smartQQ.VFWebQQ).Replace("#{psessionid}", smartQQ.PSessionId);
            
            string dat = HTTP.Get(url, "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1");

            FriendInfo inf = (FriendInfo)JsonConvert.DeserializeObject(dat, typeof(FriendInfo));
            
            if (inf.result == null) return;

            account.face = inf.result.face;
            account.occupation = inf.result.occupation;
            account.phone = inf.result.phone;
            account.college = inf.result.college;
            account.blood = inf.result.blood;
            account.homepage = inf.result.homepage;
            account.vip_info = inf.result.vip_info;
            account.country = inf.result.country;
            account.city = inf.result.city;
            account.personal = inf.result.personal;
            account.nick = inf.result.nick;
            account.shengxiao = inf.result.shengxiao;
            account.email = inf.result.email;
            account.province = inf.result.province;
            account.gender = inf.result.gender;
            if (inf.result.birthday.year != 0 && inf.result.birthday.month != 0 && inf.result.birthday.day != 0)
                account.birthday = new DateTime(inf.result.birthday.year, inf.result.birthday.month, inf.result.birthday.day);           
        }

        public void GetGroupList(bool isLoadGroupAccountDetailedInfo = true)
        {
            string url = "http://s.web2.qq.com/api/get_group_name_list_mask2";
            string sendData = string.Format("r={{\"vfwebqq\":\"{0}\",\"hash\":\"{1}\"}}", smartQQ.VFWebQQ, smartQQ.Hash);
            string dat = HTTP.Post(url, sendData, "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2");

            Groups groups = (Groups)JsonConvert.DeserializeObject(dat, typeof(Groups));

            if (groups.result == null) return;

            smartQQ.GroupAccounts = new List<GroupAccount>();
            foreach(var group in groups.result.gnamelist)
            {
                GroupAccount groupAccount = new GroupAccount();
                groupAccount.code = group.code;
                groupAccount.flag = group.flag;
                groupAccount.gid = group.gid;
                groupAccount.name = group.name;

                smartQQ.GroupAccounts.Add(groupAccount);
            }

            if (isLoadGroupAccountDetailedInfo)
            { 
                foreach(GroupAccount groupAccount in smartQQ.GroupAccounts)
                {
                    LoadGroupAccountDetailedInfo(groupAccount);
                }
            }
        }

        private void LoadGroupAccountDetailedInfo(GroupAccount groupAccount)
        {
            string gcode = groupAccount.code;
            string url = "http://s.web2.qq.com/api/get_group_info_ext2?gcode=#{group_code}&vfwebqq=#{vfwebqq}&t=#{t}".Replace("#{group_code}", gcode).Replace("#{vfwebqq}", smartQQ.VFWebQQ).Replace("#{t}", HTTP.AID_TimeStamp());
            string dat = HTTP.Get(url, "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1");
            GroupInfo groupInfo = (GroupInfo)JsonConvert.DeserializeObject(dat, typeof(GroupInfo));

            if (groupInfo.result == null) return;

            groupAccount.createtime = groupInfo.result.ginfo.createtime;
            groupAccount.face = groupInfo.result.ginfo.face;
            groupAccount.owner = groupInfo.result.ginfo.owner;
            groupAccount.memo = groupInfo.result.ginfo.memo;
            groupAccount.markname = groupInfo.result.ginfo.markname;
            groupAccount.level = groupInfo.result.ginfo.level;

            Dictionary<string, string> GroupCardList = new Dictionary<string, string>();
            if (groupInfo.result.cards != null)
            {
                foreach (var goupCard in groupInfo.result.cards)
                {
                    GroupCardList.Add(goupCard.muin, goupCard.card);
                }
            }

            Dictionary<string, GroupMember> GroupMemberList = new Dictionary<string, GroupMember>();
            if (groupInfo.result.minfo != null)
            {
                foreach (var minfo in groupInfo.result.minfo)
                {
                    GroupMember groupMember = new GroupMember();
                    groupMember.nick = minfo.nick;
                    groupMember.province = minfo.province;
                    groupMember.gender = minfo.gender;
                    groupMember.uin = minfo.uin;
                    groupMember.country = minfo.country;
                    groupMember.city = minfo.city;
                    groupMember.card = GroupCardList.ContainsKey(minfo.uin) ? GroupCardList[minfo.uin] : "";

                    GroupMemberList.Add(minfo.uin, groupMember);
                }
            }

            groupAccount.members = new List<GroupMember>();
            foreach (var member in groupInfo.result.ginfo.members)
            {
                if (GroupMemberList.ContainsKey(member.muin))
                {
                    groupAccount.members.Add(GroupMemberList[member.muin]);
                }
            }           
        }

        public void GetDiscussionGroupList(bool isDiscussionGroupAccountDetailedInfo = true)
        {
            string url = "http://s.web2.qq.com/api/get_discus_list?clientid=53999199&psessionid=#{psessionid}&vfwebqq=#{vfwebqq}&t=#{t}".Replace("#{psessionid}", smartQQ.PSessionId).Replace("#{vfwebqq}", smartQQ.VFWebQQ).Replace("#{t}", HTTP.AID_TimeStamp());
            string dat = HTTP.Get(url, "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2");
            Discussions disscuss = (Discussions)JsonConvert.DeserializeObject(dat, typeof(Discussions));

            smartQQ.DiscussionAccounts = new List<DiscussionAccount>();
            if (disscuss.result != null)
            {
                foreach (var discussion in disscuss.result.dnamelist)
                {
                    DiscussionAccount discussionAccount = new DiscussionAccount();
                    discussionAccount.did = discussion.did;
                    discussionAccount.name = discussion.name;
                    smartQQ.DiscussionAccounts.Add(discussionAccount);
                }
            }

            if (isDiscussionGroupAccountDetailedInfo)
            {
                foreach (var discussion in smartQQ.DiscussionAccounts)
                {
                    LoadDiscussionAccountDetailedInfo(discussion);
                }
            }
        }

        private void LoadDiscussionAccountDetailedInfo(DiscussionAccount discussionAccount)
        {
            string url = "http://d1.web2.qq.com/channel/get_discu_info?did=#{discuss_id}&psessionid=#{psessionid}&vfwebqq=#{vfwebqq}&clientid=53999199&t=#{t}".Replace("#{t}", HTTP.AID_TimeStamp());
            url = url.Replace("#{discuss_id}", discussionAccount.did).Replace("#{psessionid}", smartQQ.PSessionId).Replace("#{vfwebqq}", smartQQ.VFWebQQ);
            string dat = HTTP.Get(url, "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2");
            DiscussionInfo discussionInfo = (DiscussionInfo)JsonConvert.DeserializeObject(dat, typeof(DiscussionInfo));

            Dictionary<string, string> MemberList = new Dictionary<string, string>();
            if (discussionInfo.result.mem_info != null)
            {
                foreach (var memberInfo in discussionInfo.result.mem_info)
                {
                    MemberList.Add(memberInfo.uin, memberInfo.nick);
                }
            }

            if (discussionInfo.result.info.mem_list != null)
            {
                discussionAccount.members = new List<DiscussionMember>();
                foreach (var member in discussionInfo.result.info.mem_list)
                {
                    DiscussionMember discussionMember = new DiscussionMember();
                    discussionMember.uid = member.mem_uin;
                    discussionMember.ruin = member.ruin;
                    discussionMember.name = MemberList.ContainsKey(member.mem_uin) ? MemberList[member.mem_uin] : "";
                    discussionAccount.members.Add(discussionMember);
                }
            }
        }

        public void GetOnlineAccounts()
        {
            string url = "http://d1.web2.qq.com/channel/get_online_buddies2?vfwebqq=#{vfwebqq}&clientid=53999199&psessionid=#{psessionid}&t=#{t}".Replace("#{vfwebqq}", smartQQ.VFWebQQ).Replace("#{psessionid}", smartQQ.PSessionId).Replace("#{t}", HTTP.AID_TimeStamp());
            HTTP.Get(url, "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2");

            url = "http://d1.web2.qq.com/channel/get_recent_list2";
            string url1 = "{\"vfwebqq\":\"#{vfwebqq}\",\"clientid\":53999199,\"psessionid\":\"#{psessionid}\"}".Replace("#{vfwebqq}", smartQQ.VFWebQQ).Replace("#{psessionid}", smartQQ.PSessionId);
            string dat = HTTP.Post(url, "r=" + HttpUtility.UrlEncode(url1), "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2");
            OnlineFirends onlineFirends = (OnlineFirends)JsonConvert.DeserializeObject(dat, typeof(OnlineFirends));

            if (onlineFirends.result == null) return;

            Dictionary<string, int> OnlineUserStatusMap = new Dictionary<string, int>();
            foreach (var onlineFirend in onlineFirends.result)
            {
                OnlineUserStatusMap.Add(onlineFirend.uin, onlineFirend.type);
            }

            foreach (var firend in smartQQ.Friends)
            {
                if (OnlineUserStatusMap.ContainsKey(firend.uin))
                {
                    firend.onlineType = OnlineUserStatusMap[firend.uin];
                }
            }
        }
    }
}
