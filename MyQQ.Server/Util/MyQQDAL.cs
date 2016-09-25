using SmartQQ.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace MyQQ.Util
{
    public class MyQQDAL
    {
        private SQLiteHelper sqlHelper;

        public MyQQDAL()
        {
            sqlHelper = new SQLiteHelper();
        }

        public bool InitializedSmartQQ(SmartQQWrapper smartQQWarapper)
        {
            if (smartQQWarapper == null)
            {
                System.Console.WriteLine("No QQ Account needs to be initialized into the SQLite database.");
                return false;
            }

            AddQQAccountProfile(smartQQWarapper);
            string qquin = smartQQWarapper.QQAccount;

            if (smartQQWarapper.FriendAccounts != null)
            {
                foreach (var friend in smartQQWarapper.FriendAccounts)
                {
                    addQQFriendAccount(qquin, friend);
                }
            }
            else
            {
                System.Console.WriteLine("No friend on the QQ : " + smartQQWarapper.QQAccount);
            }

            if (smartQQWarapper.GroupAccounts != null)
            {
                foreach (var groupAccount in smartQQWarapper.GroupAccounts)
                {
                    addQQGroupAccount(qquin, groupAccount);
                }
            }
            else
            {
                System.Console.WriteLine("No group on the QQ : " + smartQQWarapper.QQAccount);
            }

            if (smartQQWarapper.DiscussionAccounts != null)
            {
                foreach (var discussionAccount in smartQQWarapper.DiscussionAccounts)
                {
                    addQQDiscussionAccount(qquin, discussionAccount);
                }
            }
            else
            {
                System.Console.WriteLine("No discussion group on the QQ : " + smartQQWarapper.QQAccount);
            }
  
            return true;
        }

        /// <summary>
        /// Add one QQ account
        /// </summary>
        /// <param name="smartQQWarapper"></param>
        /// <returns></returns>
        private bool AddQQAccountProfile(SmartQQWrapper smartQQWarapper)
        {
            string sqlInsertQQAccount = @"INSERT INTO QQAccount ( Account, Name, Online, PTWebqq, WFWebqq, SessionId, Hash, 
                                                                         Initialized, LastUpdateTime, FriendAccount, GroupCount, DiscussionCount)
                                                 VALUES (@Account, @Name, @Online, @PTWebqq, @WFWebqq, @SessionId, @Hash, 
                                                         @Initialized, @LastUpdateTime, @FriendAccount, @GroupCount, @DiscussionCount)";

            string name = smartQQWarapper.Name;
            string account = smartQQWarapper.QQAccount;
            bool online = smartQQWarapper.Online;
            string ptwebqq = smartQQWarapper.PTWebQQ;
            string wfwebqq = smartQQWarapper.PTWebQQ;
            string sessionId = smartQQWarapper.PSessionId;
            string hash = smartQQWarapper.Hash;            
            DateTime lastUpdateTime = DateTime.Now;
            int friendCount = smartQQWarapper.FriendAccounts != null ? smartQQWarapper.FriendAccounts.Count : 0;
            int groupCount = smartQQWarapper.GroupAccounts != null ? smartQQWarapper.GroupAccounts.Count : 0;
            int dicussionCount = smartQQWarapper.DicussionCount != null ? smartQQWarapper.DiscussionAccounts.Count : 0;

            bool initialized = false;
            if (friendCount > 0 || groupCount > 0 || dicussionCount > 0)
            {
                initialized = true;
            }

            SQLiteParameter[] paraArray = new SQLiteParameter[12];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@Account", ParameterDirection.Input, account);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@Name", ParameterDirection.Input, name);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@Online", ParameterDirection.Input, online);
            paraArray[3] = sqlHelper.InitSQLiteParameter("@PTWebqq", ParameterDirection.Input, ptwebqq);
            paraArray[4] = sqlHelper.InitSQLiteParameter("@WFWebqq", ParameterDirection.Input, wfwebqq);
            paraArray[5] = sqlHelper.InitSQLiteParameter("@SessionId", ParameterDirection.Input, sessionId);
            paraArray[6] = sqlHelper.InitSQLiteParameter("@Hash", ParameterDirection.Input, hash);
            paraArray[7] = sqlHelper.InitSQLiteParameter("@Initialized", ParameterDirection.Input, initialized);
            paraArray[8] = sqlHelper.InitSQLiteParameter("@LastUpdateTime", ParameterDirection.Input, lastUpdateTime);
            paraArray[9] = sqlHelper.InitSQLiteParameter("@FriendAccount", ParameterDirection.Input, friendCount);
            paraArray[10] = sqlHelper.InitSQLiteParameter("@GroupCount", ParameterDirection.Input, groupCount);
            paraArray[11] = sqlHelper.InitSQLiteParameter("@DiscussionCount", ParameterDirection.Input, dicussionCount);

            if (sqlHelper.ExecuteNonQuery(sqlInsertQQAccount, CommandType.Text, paraArray) > 0)
            {
                string sqlInsertQQAccountProfile = @"INSERT INTO QQAccountProfile (QQAccount, Birthday, Blood, City, College, Country, Email, Gender, Homepage,
                                                                                   Lnick, Mobile, Nick, Occupation, Personal, Phone, Province, Shengxiao)
                                                     VALUES (@QQAccount, @Birthday, @Blood, @City, @College, @Country, @Email, @Gender, @Homepage,
                                                                                   @Lnick, @Mobile, @Nick, @Occupation, @Personal, @Phone, @Province, @Shengxiao)";

                DateTime birthday = smartQQWarapper.Birthday;
                int blood = smartQQWarapper.Blood;
                string city = smartQQWarapper.City;
                string college = smartQQWarapper.College;
                string country = smartQQWarapper.Country;
                string email = smartQQWarapper.Email;
                string gender = smartQQWarapper.Gender;
                string homepage = smartQQWarapper.Homepage;
                string lnick = smartQQWarapper.Lnick;
                string mobile = smartQQWarapper.Mobile;
                string nick = smartQQWarapper.Nick;
                string occupation = smartQQWarapper.Occupation;
                string personal = smartQQWarapper.Personal;
                string phone = smartQQWarapper.Phone;
                string province = smartQQWarapper.Province;
                int shengxiao = smartQQWarapper.Shengxiao;

                SQLiteParameter[] paraArray1 = new SQLiteParameter[17];

                paraArray1[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, account);
                paraArray1[1] = sqlHelper.InitSQLiteParameter("@Birthday", ParameterDirection.Input, birthday);
                paraArray1[2] = sqlHelper.InitSQLiteParameter("@Blood", ParameterDirection.Input, blood);
                paraArray1[3] = sqlHelper.InitSQLiteParameter("@City", ParameterDirection.Input, city);
                paraArray1[4] = sqlHelper.InitSQLiteParameter("@College", ParameterDirection.Input, college);
                paraArray1[5] = sqlHelper.InitSQLiteParameter("@Country", ParameterDirection.Input, country);
                paraArray1[6] = sqlHelper.InitSQLiteParameter("@Email", ParameterDirection.Input, email);
                paraArray1[7] = sqlHelper.InitSQLiteParameter("@Gender", ParameterDirection.Input, gender);
                paraArray1[8] = sqlHelper.InitSQLiteParameter("@Homepage", ParameterDirection.Input, homepage);
                paraArray1[9] = sqlHelper.InitSQLiteParameter("@Lnick", ParameterDirection.Input, lnick);
                paraArray1[10] = sqlHelper.InitSQLiteParameter("@Mobile", ParameterDirection.Input, mobile);
                paraArray1[11] = sqlHelper.InitSQLiteParameter("@Nick", ParameterDirection.Input, nick);
                paraArray1[12] = sqlHelper.InitSQLiteParameter("@Occupation", ParameterDirection.Input, occupation);
                paraArray1[13] = sqlHelper.InitSQLiteParameter("@Personal", ParameterDirection.Input, personal);
                paraArray1[14] = sqlHelper.InitSQLiteParameter("@Phone", ParameterDirection.Input, phone);
                paraArray1[15] = sqlHelper.InitSQLiteParameter("@Province", ParameterDirection.Input, province);
                paraArray1[16] = sqlHelper.InitSQLiteParameter("@Shengxiao", ParameterDirection.Input, shengxiao);


                if (sqlHelper.ExecuteNonQuery(sqlInsertQQAccountProfile, CommandType.Text, paraArray1) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        #region QQ Friend Profile

        private bool exists(string qqNum)
        {
            return false;
        }

        private bool addQQFriendAccount(string qqaccount, QQFriendAccount account)
        {
            string sqlInsertQQAccount = @"INSERT INTO QQFirends (Uin, Account, QQAccount, Brithday, Blood, City, College, Country, Email,
                                                                 Gender, Homepage, Mobile, Nick, Occupation, Personal, Phone, Province) 
                                           VALUES (@Uin, @Account, @QQAccount, @Brithday, @Blood, @City, @College, @Country, @Email,
                                                                 @Gender, @Homepage, @Mobile, @Nick, @Occupation, @Personal, @Phone, @Province)";
            string uin = account.Uin;
            string qqnum = account.Account;            
            DateTime birthday = account.Birthday;
            int blood = account.Blood;
            string city = account.City;
            string college = account.College;
            string country = account.Country;
            string email = account.Email;
            string gender = account.Gender;
            string homepage = account.Homepage;
            string mobile = account.Mobile;
            string nick = account.Nick;
            string occupation = account.Occupation;
            string personal = account.Personal;
            string phone = account.Phone;
            string province = account.Province;            

            SQLiteParameter[] paraArray = new SQLiteParameter[17];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@Uin", ParameterDirection.Input, uin);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@Account", ParameterDirection.Input, qqnum);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqaccount);
            paraArray[3] = sqlHelper.InitSQLiteParameter("@Brithday", ParameterDirection.Input, birthday);
            paraArray[4] = sqlHelper.InitSQLiteParameter("@Blood", ParameterDirection.Input, blood);
            paraArray[5] = sqlHelper.InitSQLiteParameter("@City", ParameterDirection.Input, city);
            paraArray[6] = sqlHelper.InitSQLiteParameter("@College", ParameterDirection.Input, college);
            paraArray[7] = sqlHelper.InitSQLiteParameter("@Country", ParameterDirection.Input, country);
            paraArray[8] = sqlHelper.InitSQLiteParameter("@Email", ParameterDirection.Input, email);
            paraArray[9] = sqlHelper.InitSQLiteParameter("@Gender", ParameterDirection.Input, gender);
            paraArray[10] = sqlHelper.InitSQLiteParameter("@Homepage", ParameterDirection.Input, homepage);
            paraArray[11] = sqlHelper.InitSQLiteParameter("@Mobile", ParameterDirection.Input, mobile);
            paraArray[12] = sqlHelper.InitSQLiteParameter("@Nick", ParameterDirection.Input, nick);
            paraArray[13] = sqlHelper.InitSQLiteParameter("@Occupation", ParameterDirection.Input, occupation);
            paraArray[14] = sqlHelper.InitSQLiteParameter("@Personal", ParameterDirection.Input, personal);
            paraArray[15] = sqlHelper.InitSQLiteParameter("@Phone", ParameterDirection.Input, phone);
            paraArray[16] = sqlHelper.InitSQLiteParameter("@Province", ParameterDirection.Input, province);
         
            try
            {
                if (sqlHelper.ExecuteNonQuery(sqlInsertQQAccount, CommandType.Text, paraArray) > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool updateQQAccount(string qqNum)
        {
            return false;
        }

        #endregion

        #region QQ Group Account

        private bool addQQGroupAccount(string qqaacount, GroupAccount groupAccount)
        {
            string sqlInsertQQGroupAccount = @"INSERT INTO QQGroupAccount (Gid, QQAccount, Name, Code, CreateTime, FingerMemo, Flag, Level, Owner, Memo, MemberCount)
                                               VALUES (@Gid, @QQAccount, @Name, @Code, @CreateTime, @FingerMemo, @Flag, @Level, @Owner, @Memo, @MemberCount) ";

            string gid = groupAccount.Gid;           
            string name = groupAccount.Name;
            string code = groupAccount.Code;            
            string createtime = groupAccount.CreateTime;
            string fingerMemo = groupAccount.FingerMemo == null ? "" : groupAccount.FingerMemo;
            string memo = groupAccount.Memo == null ? "" : groupAccount.Memo;
            string flag = groupAccount.Flag;
            int level = groupAccount.Level;
            string owner = groupAccount.Owner;
            int memberCount = groupAccount.Members != null ? groupAccount.Members.Count : 0;

            SQLiteParameter[] paraArray = new SQLiteParameter[11];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@Gid", ParameterDirection.Input, gid);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqaacount);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@Name", ParameterDirection.Input, name);
            paraArray[3] = sqlHelper.InitSQLiteParameter("@Code", ParameterDirection.Input, code);
            paraArray[4] = sqlHelper.InitSQLiteParameter("@CreateTime", ParameterDirection.Input, createtime);
            paraArray[5] = sqlHelper.InitSQLiteParameter("@FingerMemo", ParameterDirection.Input, fingerMemo);
            paraArray[6] = sqlHelper.InitSQLiteParameter("@Flag", ParameterDirection.Input, flag);
            paraArray[7] = sqlHelper.InitSQLiteParameter("@Level", ParameterDirection.Input, level);
            paraArray[8] = sqlHelper.InitSQLiteParameter("@Owner", ParameterDirection.Input, owner);
            paraArray[9] = sqlHelper.InitSQLiteParameter("@Memo", ParameterDirection.Input, memo);
            paraArray[10] = sqlHelper.InitSQLiteParameter("@MemberCount", ParameterDirection.Input, memberCount);

            try
            {
                if (sqlHelper.ExecuteNonQuery(sqlInsertQQGroupAccount, CommandType.Text, paraArray) > 0)
                {
                    if (groupAccount.Members != null)
                    {
                        foreach (var member in groupAccount.Members)
                        {
                            addQQGroupMember(gid, member);
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("No group member group on the QQ group : " + groupAccount.Name);
                    }

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool updateQQGroupAccount(string qqNum)
        {
            return false;
        }

        private bool addQQGroupMember(string gid, GroupMember groupMember)
        {
            string sqlInsertQQGroupMember = @"INSERT INTO QQGroupMember ( Uin, Gid, City, Country, Gender, Nick, Province, Card)
                                              VALUES ( @Uin, @Gid, @City, @Country, @Gender, @Nick, @Province, @Card )";

            string uin = groupMember.Uin;
            string city = groupMember.City;
            string country = groupMember.Country;
            string gender = groupMember.Gender;
            string nick = groupMember.Nick;
            string province = groupMember.Province;
            string card = groupMember.Card; 

            SQLiteParameter[] paraArray = new SQLiteParameter[8];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@Uin", ParameterDirection.Input, uin);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@Gid", ParameterDirection.Input, gid);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@City", ParameterDirection.Input, city);
            paraArray[3] = sqlHelper.InitSQLiteParameter("@Country", ParameterDirection.Input, country);
            paraArray[4] = sqlHelper.InitSQLiteParameter("@Gender", ParameterDirection.Input, gender);
            paraArray[5] = sqlHelper.InitSQLiteParameter("@Nick", ParameterDirection.Input, nick);
            paraArray[6] = sqlHelper.InitSQLiteParameter("@Province", ParameterDirection.Input, province);
            paraArray[7] = sqlHelper.InitSQLiteParameter("@Card", ParameterDirection.Input, card);           
            
            try
            {
                if (sqlHelper.ExecuteNonQuery(sqlInsertQQGroupMember, CommandType.Text, paraArray) > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool updateQQGroupMember(string qqNum)
        {
            return false;
        }

        #endregion

        #region QQ Discussion

        private bool addQQDiscussionAccount(string qqaacount, DiscussionAccount discussionAccount)
        {
            string sqlInsertQQDiscussionAccount = @"INSERT INTO QQDiscussionAccount ( Did, Name, QQAccount, MemberCount )
                                                    VALUES ( @Did, @Name, @QQAccount, @MemberCount)";
            string did = discussionAccount.Did;
            string name = discussionAccount.Name;
            int memberCount = discussionAccount.Members.Count;

            SQLiteParameter[] paraArray = new SQLiteParameter[4];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@Did", ParameterDirection.Input, did);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@Name", ParameterDirection.Input, name);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqaacount);
            paraArray[3] = sqlHelper.InitSQLiteParameter("@MemberCount", ParameterDirection.Input, memberCount);
           
            try
            {
                if (sqlHelper.ExecuteNonQuery(sqlInsertQQDiscussionAccount, CommandType.Text, paraArray) > 0)
                {
                    if (discussionAccount.Members != null)
                    {
                        foreach (var discussionMember in discussionAccount.Members)
                        {
                            addQQDiscussionMember(did, discussionMember);
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("No group member group on the discussion group : " + discussionAccount.Name);
                    }

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool updateQQDiscussionAccount(string qqNum)
        {
            return false;
        }

        private bool addQQDiscussionMember(string did, DiscussionMember discussionMember)
        {
            string sqlInsertQQDiscussionMember = @"INSERT INTO QQDiscussionMember (RUin, Did, Uin, Name, Status)
                                                    VALUES (@RUin, @Did, @Uin, @Name, @Status)";
            
            string ruin = discussionMember.Ruin;
            string uin = discussionMember.Uin;
            string name = discussionMember.Name;
            string status = discussionMember.Status;
            

            SQLiteParameter[] paraArray = new SQLiteParameter[5];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@RUin", ParameterDirection.Input, ruin);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@Did", ParameterDirection.Input, did);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@Uin", ParameterDirection.Input, uin);
            paraArray[3] = sqlHelper.InitSQLiteParameter("@Name", ParameterDirection.Input, name);
            paraArray[4] = sqlHelper.InitSQLiteParameter("@Status", ParameterDirection.Input, status);

            try
            {
                if (sqlHelper.ExecuteNonQuery(sqlInsertQQDiscussionMember, CommandType.Text, paraArray) > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool updateQQDiscussionMember(string qqNum)
        {
            return false;
        }

        #endregion
    }
}