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

        /// <summary>
        /// Get the QQ profile
        /// </summary>
        /// <returns></returns>
        public SmartQQWrapper GetQQProfile()
        {
            string url = "http://s.web2.qq.com/api/get_self_info2?t=#{t}".Replace("#{t}", HTTP.AID_TimeStamp());
            string dat = HTTP.Get(url, "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1");
            FriendInfo info = (FriendInfo)JsonConvert.DeserializeObject(dat, typeof(FriendInfo));

            if (info.result != null)
            {
                smartQQ.Occupation = info.result.occupation;
                smartQQ.Phone = info.result.phone;
                smartQQ.College = info.result.college;
                smartQQ.Blood = info.result.blood;
                smartQQ.Homepage = info.result.homepage;
                smartQQ.Country = info.result.country;
                smartQQ.City = info.result.city;
                smartQQ.Personal = info.result.personal;
                smartQQ.Nick = info.result.nick;
                smartQQ.Email = info.result.email;
                smartQQ.Province = info.result.province;
                smartQQ.Gender = info.result.gender;
            }

            return smartQQ;
        }

        /// <summary>
        /// Get QQ Friend List
        /// </summary>
        /// <param name="isLoadAccountDetailedInfo">The flag whether or not to load the friend profile</param>
        /// <returns></returns>
        public List<QQFriendAccount> GetFriendList(bool isLoadAccountDetailedInfo = false)
        {
            string url = "http://s.web2.qq.com/api/get_user_friends2";
            string sendData = string.Format("r={{\"vfwebqq\":\"{0}\",\"hash\":\"{1}\"}}", smartQQ.VFWebQQ, smartQQ.Hash);
            string dat = HTTP.Post(url, sendData, "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1");

            Friends friend = (Friends)JsonConvert.DeserializeObject(dat, typeof(Friends));

            if (friend.result == null) return null;

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

            Dictionary<string, QQFriendAccount> accounts = new Dictionary<string, QQFriendAccount>();
            string uin = String.Empty;
            int categoryIndex = -1;
            string categoryName = String.Empty;

            if (friend.result.friends != null)
            {
                for (int i = 0; i < friend.result.friends.Count; i++)
                {
                    uin = friend.result.friends[i].uin;
                    

                    QQFriendAccount account = null;
                    if (accounts.ContainsKey(uin)) account = accounts[uin];
                    else
                    {
                        account = new QQFriendAccount();
                        accounts.Add(uin, account);
                    }

                    account.Uin = uin;
                    account.MarkName = MarkNameList.ContainsKey(uin) ? MarkNameList[uin] : "";

                    categoryIndex = friend.result.friends[i].categories;
                    account.CategotyIndex = categoryIndex;
                    account.CategotyName = CategoryList.ContainsKey(categoryIndex) ? CategoryList[categoryIndex] : "";

                    accounts[uin] = account;
                }
            }

            if (friend.result.info != null)
            {
                for (int i = 0; i < friend.result.info.Count; i++)
                {
                    uin = friend.result.info[i].uin;

                    QQFriendAccount account = null;
                    if (accounts.ContainsKey(uin)) account = accounts[uin];
                    else
                    {
                        account = new QQFriendAccount();
                        accounts.Add(uin, account);
                    }

                    account.Nick = friend.result.info[i].nick;                    

                    accounts[uin] = account;
                }
            }

            smartQQ.FriendAccounts = accounts.Values.ToList<QQFriendAccount>();
            if (isLoadAccountDetailedInfo)
            { 
                foreach(QQFriendAccount account in smartQQ.FriendAccounts)
                {
                    LoadAccountDetailedInfo(account);
                }
            }

            return smartQQ.FriendAccounts;
        }

        /// <summary>
        /// Load the detailed account profile
        /// </summary>
        /// <param name="account"></param>
        private void LoadAccountDetailedInfo(QQFriendAccount account)
        {
            string getFriendInfoUrl = "http://s.web2.qq.com/api/get_friend_info2?tuin=#{uin}&vfwebqq=#{vfwebqq}&clientid=53999199&psessionid=#{psessionid}&t=#{t}".Replace("#{t}", HTTP.AID_TimeStamp());
            getFriendInfoUrl = getFriendInfoUrl.Replace("#{uin}", account.Uin).Replace("#{vfwebqq}", smartQQ.VFWebQQ).Replace("#{psessionid}", smartQQ.PSessionId);
            string retFriendInfo = HTTP.Get(getFriendInfoUrl, "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1");
            FriendInfo friendInfo = (FriendInfo)JsonConvert.DeserializeObject(retFriendInfo, typeof(FriendInfo));

            if (friendInfo.result != null)
            {
                account.Blood = friendInfo.result.blood;
                account.Occupation = friendInfo.result.occupation;
                account.College = friendInfo.result.college;
                account.Homepage = friendInfo.result.homepage;
                account.Country = friendInfo.result.country;
                account.City = friendInfo.result.city;
                account.Nick = friendInfo.result.nick;
                account.Email = friendInfo.result.email;
                account.Phone = friendInfo.result.phone;
                account.Mobile = friendInfo.result.mobile;
                account.Province = friendInfo.result.province;
                account.Gender = friendInfo.result.gender;

                if (friendInfo.result.birthday.year != 0 && friendInfo.result.birthday.month != 0 && friendInfo.result.birthday.day != 0)
                {
                    account.Birthday = new DateTime(friendInfo.result.birthday.year, friendInfo.result.birthday.month, friendInfo.result.birthday.day);
                }
            }

            string getFriendAccountUrl = "http://s.web2.qq.com/api/get_friend_uin2?tuin=#{uin}&type=1&vfwebqq=#{vfwebqq}&t=#{t}";
            getFriendAccountUrl = getFriendAccountUrl.Replace("#{uin}", account.Uin).Replace("#{vfwebqq}", smartQQ.VFWebQQ);
            string retFriendAccoun = HTTP.Get(getFriendAccountUrl, "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1");
            FirendAccount firendAccount = (FirendAccount)JsonConvert.DeserializeObject(retFriendAccoun, typeof(FirendAccount));

            if (firendAccount.result != null)
            {
                account.Account = firendAccount.result.account;
            }
        }

        /// <summary>
        /// Get the QQ Group Name list
        /// </summary>
        /// <param name="isLoadGroupAccountDetailedInfo">The flag whether or not to load the group member profile</param>
        /// <returns></returns>
        public List<GroupAccount> GetGroupList(bool isLoadGroupAccountDetailedInfo = true)
        {
            string url = "http://s.web2.qq.com/api/get_group_name_list_mask2";
            string sendData = string.Format("r={{\"vfwebqq\":\"{0}\",\"hash\":\"{1}\"}}", smartQQ.VFWebQQ, smartQQ.Hash);
            string dat = HTTP.Post(url, sendData, "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2");

            Groups groups = (Groups)JsonConvert.DeserializeObject(dat, typeof(Groups));

            if (groups.result == null) return null;

            smartQQ.GroupAccounts = new List<GroupAccount>();
            foreach(var group in groups.result.gnamelist)
            {
                GroupAccount groupAccount = new GroupAccount();
                groupAccount.Code = group.code;
                groupAccount.Flag = group.flag;
                groupAccount.Gid = group.gid;
                groupAccount.Name = group.name;

                smartQQ.GroupAccounts.Add(groupAccount);
            }

            if (isLoadGroupAccountDetailedInfo)
            { 
                foreach(GroupAccount groupAccount in smartQQ.GroupAccounts)
                {
                    LoadGroupAccountDetailedInfo(groupAccount);
                }
            }

            return smartQQ.GroupAccounts;
        }

        /// <summary>
        /// Get the QQ group member profile
        /// </summary>
        /// <param name="groupAccount"></param>
        private void LoadGroupAccountDetailedInfo(GroupAccount groupAccount)
        {
            string gcode = groupAccount.Code;
            string url = "http://s.web2.qq.com/api/get_group_info_ext2?gcode=#{group_code}&vfwebqq=#{vfwebqq}&t=#{t}".Replace("#{group_code}", gcode).Replace("#{vfwebqq}", smartQQ.VFWebQQ).Replace("#{t}", HTTP.AID_TimeStamp());
            string dat = HTTP.Get(url, "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1");
            GroupInfo groupInfo = (GroupInfo)JsonConvert.DeserializeObject(dat, typeof(GroupInfo));

            if (groupInfo.result == null) return;

            groupAccount.Class = "" ; //groupInfo.result.ginfo.class;
            groupAccount.Code = groupInfo.result.ginfo.code;
            groupAccount.CreateTime = groupInfo.result.ginfo.createtime;
            groupAccount.Memo = groupInfo.result.ginfo.memo;
            groupAccount.Level = groupInfo.result.ginfo.level;
            groupAccount.Name = groupInfo.result.ginfo.name;
            groupAccount.Owner = groupInfo.result.ginfo.owner;

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
                    groupMember.Nick = minfo.nick;
                    groupMember.Province = minfo.province;
                    groupMember.Gender = minfo.gender;
                    groupMember.Uin = minfo.uin;
                    groupMember.Country = minfo.country;
                    groupMember.City = minfo.city;
                    groupMember.Card = GroupCardList.ContainsKey(minfo.uin) ? GroupCardList[minfo.uin] : "";

                    GroupMemberList.Add(minfo.uin, groupMember);
                }
            }

            groupAccount.Members = new List<GroupMember>();
            foreach (var member in groupInfo.result.ginfo.members)
            {
                if (GroupMemberList.ContainsKey(member.muin))
                {
                    groupAccount.Members.Add(GroupMemberList[member.muin]);
                }
            }           
        }

        /// <summary>
        /// Get the QQ discussion group list
        /// </summary>
        /// <param name="isDiscussionGroupAccountDetailedInfo"></param>
        /// <returns></returns>
        public List<DiscussionAccount> GetDiscussionGroupList(bool isDiscussionGroupAccountDetailedInfo = true)
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
                    discussionAccount.Did = discussion.did;
                    discussionAccount.Name = discussion.name;
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

            return smartQQ.DiscussionAccounts;
        }

        /// <summary>
        /// Get the QQ discussion group member profile
        /// </summary>
        /// <param name="discussionAccount"></param>
        private void LoadDiscussionAccountDetailedInfo(DiscussionAccount discussionAccount)
        {
            string url = "http://d1.web2.qq.com/channel/get_discu_info?did=#{discuss_id}&psessionid=#{psessionid}&vfwebqq=#{vfwebqq}&clientid=53999199&t=#{t}".Replace("#{t}", HTTP.AID_TimeStamp());
            url = url.Replace("#{discuss_id}", discussionAccount.Did).Replace("#{psessionid}", smartQQ.PSessionId).Replace("#{vfwebqq}", smartQQ.VFWebQQ);
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

            Dictionary<string, string> MemberStatusList = new Dictionary<string, string>();
            if (discussionInfo.result.mem_status != null)
            {
                foreach (var status in discussionInfo.result.mem_status)
                {
                    MemberStatusList.Add(status.uin, status.status);
                }
            }

            if (discussionInfo.result.info.mem_list != null)
            {
                discussionAccount.Members = new List<DiscussionMember>();
                foreach (var member in discussionInfo.result.info.mem_list)
                {
                    DiscussionMember discussionMember = new DiscussionMember();
                    discussionMember.Ruin = member.ruin;
                    discussionMember.Uin = member.mem_uin;
                    discussionMember.Name = MemberList.ContainsKey(discussionMember.Uin) ? MemberList[discussionMember.Uin] : "";
                    discussionMember.Status = MemberStatusList.ContainsKey(discussionMember.Uin) ? MemberStatusList[discussionMember.Uin] : "";

                    discussionAccount.Members.Add(discussionMember);
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

            foreach (var firend in smartQQ.FriendAccounts)
            {
                if (OnlineUserStatusMap.ContainsKey(firend.Uin))
                {
                    firend.OnlineType = OnlineUserStatusMap[firend.Uin];
                }
            }
        }
    }
}
