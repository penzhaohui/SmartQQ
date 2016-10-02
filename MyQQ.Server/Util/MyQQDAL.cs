using MyQQ.Entity;
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

        /// <summary>
        /// Initialize Smart QQ Information to SQLite database
        /// </summary>
        /// <param name="smartQQWarapper"></param>
        /// <returns></returns>
        public bool InitializeSmartQQ(SmartQQWrapper smartQQWarapper)
        {
            if (smartQQWarapper == null)
            {
                System.Console.WriteLine("No QQ Account needs to be initialized into the SQLite database.");
                return false;
            }

            string qqaccount = smartQQWarapper.QQAccount;
            if (exists(qqaccount) == false)
            {
                AddQQAccountProfile(smartQQWarapper);
            }
            else
            { 
            }

            if (smartQQWarapper.FriendAccounts != null)
            {
                foreach (var friend in smartQQWarapper.FriendAccounts)
                {
                    addQQFriendAccount(qqaccount, friend);
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
                    addQQGroupAccount(qqaccount, groupAccount);
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
                    addQQDiscussionAccount(qqaccount, discussionAccount);
                }
            }
            else
            {
                System.Console.WriteLine("No discussion group on the QQ : " + smartQQWarapper.QQAccount);
            }
  
            return true;
        }

        private bool RemoveAllAccounts(string qqAccount)
        {
            #region 1. Remove Friends

            // 1. Remove friends
            string sqlDeleteQQFriends = "DELETE FROM QQFriends WHERE QQAccount = @QQAccount ";
            SQLiteParameter[] paraDeleteQQFriends = new SQLiteParameter[1];
            paraDeleteQQFriends[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqAccount);

            try
            {
                sqlHelper.ExecuteNonQuery(sqlDeleteQQFriends, CommandType.Text, paraDeleteQQFriends);
            }
            catch
            {
                return false;
            }

            #endregion

            #region 2. Remove Groups
            // 2. Remove Groups
            string sqlDeleteQQGroupMembers = "DELETE FROM QQGroupMember WHERE Gid IN (SELECT Gid FROM QQGroupAccount WHERE QQAccount = @QQAccount) ";
            SQLiteParameter[] paraDeleteQQGroupMembers = new SQLiteParameter[1];
            paraDeleteQQGroupMembers[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqAccount);

            try
            {
                sqlHelper.ExecuteNonQuery(sqlDeleteQQGroupMembers, CommandType.Text, paraDeleteQQGroupMembers);
            }
            catch
            {
                return false;
            }

            string sqlDeleteQQGroups = "DELETE FROM QQGroupAccount WHERE QQAccount = @QQAccount ";
            SQLiteParameter[] paraDeleteQQGroups = new SQLiteParameter[1];
            paraDeleteQQGroups[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqAccount);

            try
            {
                sqlHelper.ExecuteNonQuery(sqlDeleteQQGroups, CommandType.Text, paraDeleteQQGroups);
            }
            catch
            {
                return false;
            }

            #endregion

            #region 3. Remove Dicussion
            // 3. Remove Dicussion
            string sqlDeleteQQDiscussionMembers = "DELETE FROM QQDiscussionMember WHERE Did IN (SELECT Did FROM QQDiscussionAccount WHERE QQAccount = @QQAccount) ";
            SQLiteParameter[] paraDeleteQQDiscussionMembers = new SQLiteParameter[1];
            paraDeleteQQDiscussionMembers[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqAccount);

            try
            {
                sqlHelper.ExecuteNonQuery(sqlDeleteQQDiscussionMembers, CommandType.Text, paraDeleteQQDiscussionMembers);
            }
            catch
            {
                return false;
            }

            string sqlDeleteQQDiscussions = "DELETE FROM QQDiscussionAccount WHERE QQAccount = @QQAccount ";
            SQLiteParameter[] paraDeleteQQDiscussions = new SQLiteParameter[1];
            paraDeleteQQDiscussions[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqAccount);

            try
            {
                sqlHelper.ExecuteNonQuery(sqlDeleteQQDiscussions, CommandType.Text, paraDeleteQQDiscussions);
            }
            catch
            {
                return false;
            }

            #endregion

            #region 4. Remove profile
            // 4. Remove profile
            string sqlDeleteQQAccountProfile = "DELETE FROM QQAccountProfile WHERE QQAccount = @QQAccount ";
            SQLiteParameter[] paraDeleteQQAccountProfile = new SQLiteParameter[1];
            paraDeleteQQAccountProfile[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqAccount);

            try
            {
                sqlHelper.ExecuteNonQuery(sqlDeleteQQAccountProfile, CommandType.Text, paraDeleteQQAccountProfile);
            }
            catch
            {
                return false;
            }

            string sqlDeleteQQAccount = "DELETE FROM QQAccount WHERE Account = @Account ";
            SQLiteParameter[] paraDeleteQQAccount = new SQLiteParameter[1];
            paraDeleteQQAccount[0] = sqlHelper.InitSQLiteParameter("@Account", ParameterDirection.Input, qqAccount);

            try
            {
                sqlHelper.ExecuteNonQuery(sqlDeleteQQAccount, CommandType.Text, paraDeleteQQAccount);
            }
            catch
            {
                return false;
            }

            #endregion

            return true;
        }

        /// <summary>
        /// Initialize MyQQ Entity
        /// </summary>
        /// <param name="myQQEntity"></param>
        public bool InitializeMyQQEnity(MyQQEntity myQQEntity, bool forceInit = false)
        {
            # region From QQAccount

            DateTime lastUpdateTime = DateTime.Now;

            String strSql = "SELECT Name, FriendAccount, GroupCount, DiscussionCount, LastUpdateTime FROM  QQAccount WHERE Account = @Account ";

            SQLiteParameter[] paraArray = new SQLiteParameter[1];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@Account", ParameterDirection.Input, myQQEntity.QQAccount);
           
            using (SQLiteDataReader sqlDr = sqlHelper.ExecuteReader(strSql, CommandType.Text, paraArray))
            {
                if (sqlDr.Read())
                {
                    lastUpdateTime = sqlDr.IsDBNull(4) == true ? DateTime.Now : sqlDr.GetDateTime(4);
                    if (forceInit || DateTime.Now.Subtract(lastUpdateTime).TotalHours < 1)
                    {
                        myQQEntity.Name = sqlDr.IsDBNull(0) == true ? String.Empty : sqlDr.GetString(0);
                        myQQEntity.FriendAccount = sqlDr.GetInt32(1);
                        myQQEntity.GroupCount = sqlDr.GetInt32(2);
                        myQQEntity.DiscussionCount = sqlDr.GetInt32(3);
                    }
                }
            }            

            if (forceInit == false && DateTime.Now.Subtract(lastUpdateTime).TotalHours > 1)
            {
                RemoveAllAccounts(myQQEntity.QQAccount); 
                return false;
            }

            #endregion

            #region From QQGroupAccount

            String strSql1 = "SELECT COUNT(*) FROM  QQGroupAccount WHERE QQAccount = @QQAccount ";

            SQLiteParameter[] paraArray1 = new SQLiteParameter[1];

            paraArray1[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, myQQEntity.QQAccount);

            using (SQLiteDataReader sqlDr = sqlHelper.ExecuteReader(strSql1, CommandType.Text, paraArray1))
            {
                if (sqlDr.Read())
                {
                    myQQEntity.GroupCount = sqlDr.GetInt32(0);
                }
            }

            #endregion

            #region From QQDiscussionAccount

            String strSql2 = "SELECT COUNT(*) FROM  QQDiscussionAccount WHERE QQAccount = @QQAccount ";

            SQLiteParameter[] paraArray2 = new SQLiteParameter[1];

            paraArray2[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, myQQEntity.QQAccount);

            using (SQLiteDataReader sqlDr = sqlHelper.ExecuteReader(strSql2, CommandType.Text, paraArray2))
            {
                if (sqlDr.Read())
                {
                    myQQEntity.DiscussionCount = sqlDr.GetInt32(0);
                }
            }

            #endregion

            #region From QQFirends

            String strSql3 = "SELECT COUNT(*) FROM  QQFriends WHERE QQAccount = @QQAccount ";

            SQLiteParameter[] paraArray3 = new SQLiteParameter[1];

            paraArray3[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, myQQEntity.QQAccount);

            using (SQLiteDataReader sqlDr = sqlHelper.ExecuteReader(strSql3, CommandType.Text, paraArray3))
            {
                if (sqlDr.Read())
                {
                    myQQEntity.FriendAccount = sqlDr.GetInt32(0);
                }
            }

            #endregion

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

        /// <summary>
        /// Update QQ Account profile
        /// </summary>
        /// <param name="smartQQWarapper"></param>
        /// <returns></returns>
        private bool UpdateQQAccountProfile(SmartQQWrapper smartQQWarapper)
        {
            string sqlInsertQQAccount = @"UPDATE QQAccount 
                                          SET Name = @Name, Online = @Online, PTWebqq = @PTWebqq, WFWebqq = @WFWebqq, 
                                          SessionId = @SessionId, Hash = @Hash, Initialized = @Initialized, 
                                          LastUpdateTime = @LastUpdateTime, FriendAccount = @FriendAccount, 
                                          GroupCount = @GroupCount, DiscussionCount = @DiscussionCount
                                          WHERE Account = @Account ";

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
                        
            paraArray[0] = sqlHelper.InitSQLiteParameter("@Name", ParameterDirection.Input, name);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@Online", ParameterDirection.Input, online);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@PTWebqq", ParameterDirection.Input, ptwebqq);
            paraArray[3] = sqlHelper.InitSQLiteParameter("@WFWebqq", ParameterDirection.Input, wfwebqq);
            paraArray[4] = sqlHelper.InitSQLiteParameter("@SessionId", ParameterDirection.Input, sessionId);
            paraArray[5] = sqlHelper.InitSQLiteParameter("@Hash", ParameterDirection.Input, hash);
            paraArray[6] = sqlHelper.InitSQLiteParameter("@Initialized", ParameterDirection.Input, initialized);
            paraArray[7] = sqlHelper.InitSQLiteParameter("@LastUpdateTime", ParameterDirection.Input, lastUpdateTime);
            paraArray[8] = sqlHelper.InitSQLiteParameter("@FriendAccount", ParameterDirection.Input, friendCount);
            paraArray[9] = sqlHelper.InitSQLiteParameter("@GroupCount", ParameterDirection.Input, groupCount);
            paraArray[10] = sqlHelper.InitSQLiteParameter("@DiscussionCount", ParameterDirection.Input, dicussionCount);
            paraArray[11] = sqlHelper.InitSQLiteParameter("@Account", ParameterDirection.Input, account);

            if (sqlHelper.ExecuteNonQuery(sqlInsertQQAccount, CommandType.Text, paraArray) > 0)
            {
                string sqlInsertQQAccountProfile = @"UPDATE QQAccountProfile 
                                                     SET Birthday = @Birthday, Blood = @Blood, City = @City, College = @College, 
                                                         Country = @Country, Email = @Email, Gender = @Gender, Homepage = @Homepage,
                                                         Lnick = @Lnick, Mobile = @Mobile, Nick = @Nick, Occupation = @Occupation, 
                                                         Personal = @Personal, Phone = @Phone, Province = @Province, Shengxiao = @Shengxiao
                                                     WHERE QQAccount = @QQAccount";

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

                
                paraArray1[0] = sqlHelper.InitSQLiteParameter("@Birthday", ParameterDirection.Input, birthday);
                paraArray1[1] = sqlHelper.InitSQLiteParameter("@Blood", ParameterDirection.Input, blood);
                paraArray1[2] = sqlHelper.InitSQLiteParameter("@City", ParameterDirection.Input, city);
                paraArray1[3] = sqlHelper.InitSQLiteParameter("@College", ParameterDirection.Input, college);
                paraArray1[4] = sqlHelper.InitSQLiteParameter("@Country", ParameterDirection.Input, country);
                paraArray1[5] = sqlHelper.InitSQLiteParameter("@Email", ParameterDirection.Input, email);
                paraArray1[6] = sqlHelper.InitSQLiteParameter("@Gender", ParameterDirection.Input, gender);
                paraArray1[7] = sqlHelper.InitSQLiteParameter("@Homepage", ParameterDirection.Input, homepage);
                paraArray1[8] = sqlHelper.InitSQLiteParameter("@Lnick", ParameterDirection.Input, lnick);
                paraArray1[9] = sqlHelper.InitSQLiteParameter("@Mobile", ParameterDirection.Input, mobile);
                paraArray1[10] = sqlHelper.InitSQLiteParameter("@Nick", ParameterDirection.Input, nick);
                paraArray1[11] = sqlHelper.InitSQLiteParameter("@Occupation", ParameterDirection.Input, occupation);
                paraArray1[12] = sqlHelper.InitSQLiteParameter("@Personal", ParameterDirection.Input, personal);
                paraArray1[13] = sqlHelper.InitSQLiteParameter("@Phone", ParameterDirection.Input, phone);
                paraArray1[14] = sqlHelper.InitSQLiteParameter("@Province", ParameterDirection.Input, province);
                paraArray1[15] = sqlHelper.InitSQLiteParameter("@Shengxiao", ParameterDirection.Input, shengxiao);
                paraArray1[16] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, account);

                if (sqlHelper.ExecuteNonQuery(sqlInsertQQAccountProfile, CommandType.Text, paraArray1) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Update QQ's online status
        /// </summary>
        /// <param name="qqAccount"></param>
        /// <param name="online"></param>
        /// <returns></returns>
        public bool UpdateOnlineStatus(string qqAccount, bool online)
        {
            string sqlInsertQQAccount = @"UPDATE QQAccount SET Online = @Online WHERE Account = @Account ";


            SQLiteParameter[] paraArray = new SQLiteParameter[2];
          
            paraArray[0] = sqlHelper.InitSQLiteParameter("@Online", ParameterDirection.Input, online);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@Account", ParameterDirection.Input, qqAccount);

            if (sqlHelper.ExecuteNonQuery(sqlInsertQQAccount, CommandType.Text, paraArray) > 0)
            {
                return true;
            }

            return false;
        }

        #region QQ Friend Profile

        private bool exists(string qqNum)
        {
            String strSql = "SELECT Name, FriendAccount, GroupCount, DiscussionCount FROM  QQAccount WHERE Account = @Account ";

            SQLiteParameter[] paraArray = new SQLiteParameter[1];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@Account", ParameterDirection.Input, qqNum);

            using (SQLiteDataReader sqlDr = sqlHelper.ExecuteReader(strSql, CommandType.Text, paraArray))
            {
                if (sqlDr.Read())
                {
                    return true;
                }
            }

            return false;
        }

        private bool addQQFriendAccount(string qqaccount, QQFriendAccount account)
        {
            string sqlInsertQQAccount = @"INSERT INTO QQFriends (Uin, Account, QQAccount, Brithday, Blood, City, College, Country, Email,
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

        /// <summary>
        /// Get QQ Account
        /// </summary>
        /// <param name="qqAccount"></param>
        /// <param name="friendUin"></param>
        /// <returns></returns>
        private string getQQFriendAccount(string qqAccount, string friendUin)
        {
            string friendAccount = "";

            String strSql = "SELECT Account FROM  QQFriends WHERE QQAccount = @QQAccount AND Uin = @Uin ";

            SQLiteParameter[] paraArray = new SQLiteParameter[2];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqAccount);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@Uin", ParameterDirection.Input, friendUin);

            using (SQLiteDataReader sqlDr = sqlHelper.ExecuteReader(strSql, CommandType.Text, paraArray))
            {
                if (sqlDr.Read())
                {
                    friendAccount = sqlDr.GetString(0);
                }
            }

            return friendAccount;
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
                            addQQGroupMember(qqaacount, gid, member);
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

        private bool addQQGroupMember(string qqAccount, string gid, GroupMember groupMember)
        {
            string sqlInsertQQGroupMember = @"INSERT INTO QQGroupMember ( Uin, Gid, City, Country, Gender, Nick, Province, Card, Account)
                                              VALUES ( @Uin, @Gid, @City, @Country, @Gender, @Nick, @Province, @Card, @Account )";

            string uin = groupMember.Uin;
            string city = groupMember.City;
            string country = groupMember.Country;
            string gender = groupMember.Gender;
            string nick = groupMember.Nick;
            string province = groupMember.Province;
            string card = groupMember.Card;
            string friendAccont = groupMember.Account;
            if (string.IsNullOrEmpty(friendAccont))
            {
                friendAccont = this.getQQFriendAccount(qqAccount, uin);
            }

            SQLiteParameter[] paraArray = new SQLiteParameter[9];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@Uin", ParameterDirection.Input, uin);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@Gid", ParameterDirection.Input, gid);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@City", ParameterDirection.Input, city);
            paraArray[3] = sqlHelper.InitSQLiteParameter("@Country", ParameterDirection.Input, country);
            paraArray[4] = sqlHelper.InitSQLiteParameter("@Gender", ParameterDirection.Input, gender);
            paraArray[5] = sqlHelper.InitSQLiteParameter("@Nick", ParameterDirection.Input, nick);
            paraArray[6] = sqlHelper.InitSQLiteParameter("@Province", ParameterDirection.Input, province);
            paraArray[7] = sqlHelper.InitSQLiteParameter("@Card", ParameterDirection.Input, card);
            paraArray[8] = sqlHelper.InitSQLiteParameter("@Account", ParameterDirection.Input, friendAccont); 
            
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

        #region Get Some data for QQ account

        /// <summary>
        /// Get Friend List By QQ Account
        /// </summary>
        /// <param name="qqaccount"></param>
        /// <returns></returns>
        public List<FriendEntity> GetFrisendsByQQAccount(string qqaccount)
        {
            string getFriendsByQQAccount = "SELECT Account, Nick FROM QQFriends WHERE QQAccount = @QQAccount ";

            List<FriendEntity> friends = new List<FriendEntity>();
            SQLiteParameter[] paraArray = new SQLiteParameter[1];
            paraArray[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqaccount);

            using (SQLiteDataReader sqlDr = sqlHelper.ExecuteReader(getFriendsByQQAccount, CommandType.Text, paraArray))
            {
                while (sqlDr.Read())
                {
                    FriendEntity friend = new FriendEntity();

                    friend.QQNo = sqlDr.IsDBNull(0) == true ? String.Empty : sqlDr.GetString(0);
                    friend.Name = sqlDr.IsDBNull(1) == true ? String.Empty : sqlDr.GetString(1);

                    friends.Add(friend);
                }
            }

            return friends;
        }

        /// <summary>
        /// Get QQ Group List By QQ Account
        /// </summary>
        /// <param name="qqaccount"></param>
        /// <returns></returns>
        public List<GroupEntity> GetQQGroupsByQQAccount(string qqaccount)
        {
            string getFriendsByQQAccount = "SELECT Gid, Name FROM QQGroupAccount WHERE QQAccount = @QQAccount ";

            List<GroupEntity> groups = new List<GroupEntity>();
            SQLiteParameter[] paraArray = new SQLiteParameter[1];
            paraArray[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqaccount);

            using (SQLiteDataReader sqlDr = sqlHelper.ExecuteReader(getFriendsByQQAccount, CommandType.Text, paraArray))
            {
                while (sqlDr.Read())
                {
                    GroupEntity group = new GroupEntity();

                    group.GroupID = sqlDr.IsDBNull(0) == true ? String.Empty : sqlDr.GetString(0);
                    group.GroupName = sqlDr.IsDBNull(1) == true ? String.Empty : sqlDr.GetString(1);

                    groups.Add(group);
                }
            }

            return groups;
        }

        /// <summary>
        /// Get QQ Discussion Group List By QQ Account
        /// </summary>
        /// <param name="qqaccount"></param>
        /// <returns></returns>
        public List<DiscussionGroupEntity> GetQQDicusstionGroupsByQQAccount(string qqaccount)
        {
            string getFriendsByQQAccount = "SELECT Did, Name FROM QQDiscussionAccount WHERE QQAccount = @QQAccount ";

            List<DiscussionGroupEntity> groups = new List<DiscussionGroupEntity>();
            SQLiteParameter[] paraArray = new SQLiteParameter[1];
            paraArray[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqaccount);

            using (SQLiteDataReader sqlDr = sqlHelper.ExecuteReader(getFriendsByQQAccount, CommandType.Text, paraArray))
            {
                while (sqlDr.Read())
                {
                    DiscussionGroupEntity group = new DiscussionGroupEntity();

                    group.GroupID = sqlDr.IsDBNull(0) == true ? String.Empty : sqlDr.GetString(0);
                    group.GroupName = sqlDr.IsDBNull(1) == true ? String.Empty : sqlDr.GetString(1);

                    groups.Add(group);
                }
            }

            return groups;
        }

        /// <summary>
        /// Get QQ Group Member By Group ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<GroupMemberEntity> GetQQGroupMembersByGroupId(string groupId)
        {
            string getFriendsByQQAccount = "SELECT Uin, Nick, Card FROM QQGroupMember WHERE Gid = @Gid ";

            List<GroupMemberEntity> members = new List<GroupMemberEntity>();
            SQLiteParameter[] paraArray = new SQLiteParameter[1];
            paraArray[0] = sqlHelper.InitSQLiteParameter("@Gid", ParameterDirection.Input, groupId);

            using (SQLiteDataReader sqlDr = sqlHelper.ExecuteReader(getFriendsByQQAccount, CommandType.Text, paraArray))
            {
                while (sqlDr.Read())
                {
                    GroupMemberEntity member = new GroupMemberEntity();

                    member.MemberID = sqlDr.IsDBNull(0) == true ? String.Empty : sqlDr.GetString(0);
                    member.MemberID = sqlDr.IsDBNull(1) == true ? String.Empty : sqlDr.GetString(1);

                    members.Add(member);
                }
            }

            return members;
        }

        /// <summary>
        /// Get QQ Discussion Group Member By Group ID
        /// </summary>
        /// <param name="dicussionGroupId"></param>
        /// <returns></returns>
        public List<DicussionGroupMemberEntity> GetQQDicusstionGroupMembersByGroupId(string groupId)
        {
            string getFriendsByQQAccount = "SELECT Uin, Name FROM QQDiscussionMember WHERE Gid = @Gid ";

            List<DicussionGroupMemberEntity> members = new List<DicussionGroupMemberEntity>();
            SQLiteParameter[] paraArray = new SQLiteParameter[1];
            paraArray[0] = sqlHelper.InitSQLiteParameter("@Gid", ParameterDirection.Input, groupId);

            using (SQLiteDataReader sqlDr = sqlHelper.ExecuteReader(getFriendsByQQAccount, CommandType.Text, paraArray))
            {
                while (sqlDr.Read())
                {
                    DicussionGroupMemberEntity member = new DicussionGroupMemberEntity();

                    member.MemberID = sqlDr.IsDBNull(0) == true ? String.Empty : sqlDr.GetString(0);
                    member.MemberID = sqlDr.IsDBNull(1) == true ? String.Empty : sqlDr.GetString(1);

                    members.Add(member);
                }
            }

            return members;
        }

        #endregion

        public bool AddOneMessage(string qqAccount, QQMessage message)
        {
            // 1. Do some initialization
            string accountType = message.AccountType.ToString();
            string friendId = message.FriendID;
            string groupId = message.GroupID;
            string discussionId = message.DiscussionID;
            string messageType = message.MessageType.ToString();
            string content = message.MessageContent;
            string ownerAccount = "";
            string ownerName = "";

            // 2. Get Message Owner's account and name
            string sqlGetMessageOwner = "";
            SQLiteParameter[] paraGetMessageArray = null;
            switch (message.AccountType)
            {
                case AccountType.Private:
                    sqlGetMessageOwner = "SELECT Account, Nick FROM QQFriends WHERE QQAccount = @QQAccount AND Uin = @Uin ";
                    paraGetMessageArray = new SQLiteParameter[2];
                    paraGetMessageArray[0] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqAccount);
                    paraGetMessageArray[1] = sqlHelper.InitSQLiteParameter("@Uin", ParameterDirection.Input, friendId);
                    break;
                case AccountType.Group:
                    sqlGetMessageOwner = "SELECT Account, Nick FROM QQGroupMember WHERE Gid = @Gid AND Uin = @Uin ";
                    paraGetMessageArray = new SQLiteParameter[2];
                    paraGetMessageArray[0] = sqlHelper.InitSQLiteParameter("@Gid", ParameterDirection.Input, groupId);
                    paraGetMessageArray[1] = sqlHelper.InitSQLiteParameter("@Uin", ParameterDirection.Input, friendId);
                    break;
                case AccountType.Discussion:
                    sqlGetMessageOwner = "SELECT RUin, Name FROM QQDiscussionMember WHERE Did = @Did AND Uin = @Uin ";
                    paraGetMessageArray = new SQLiteParameter[2];
                    paraGetMessageArray[0] = sqlHelper.InitSQLiteParameter("@Did", ParameterDirection.Input, discussionId);
                    paraGetMessageArray[1] = sqlHelper.InitSQLiteParameter("@Uin", ParameterDirection.Input, friendId);
                    break;
            }

            using (SQLiteDataReader sqlDr = sqlHelper.ExecuteReader(sqlGetMessageOwner, CommandType.Text, paraGetMessageArray))
            {
                while (sqlDr.Read())
                {
                    ownerAccount = sqlDr.IsDBNull(0) == true ? String.Empty : sqlDr.GetString(0);
                    ownerName = sqlDr.IsDBNull(1) == true ? String.Empty : sqlDr.GetString(1);
                    break;
                }
            }

            // 3. Insert the message into database
            string sqlInsertQQMessage = @"INSERT INTO QQMessage ( CreateTime, QQAccount, AccountType, QQFriendID, QQGroupID, QQDicussionID, MessageType, 
                                                                  MessageContent, MessageOwnerAccount, MessageOwnerName)
                                                 VALUES (@CreateTime, @QQAccount, @AccountType, @QQFriendID, @QQGroupID, @QQDicussionID, @MessageType, 
                                                         @MessageContent, @MessageOwnerAccount, @MessageOwnerName)";

            SQLiteParameter[] paraArray = new SQLiteParameter[10];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@CreateTime", ParameterDirection.Input, DateTime.Now);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@QQAccount", ParameterDirection.Input, qqAccount);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@AccountType", ParameterDirection.Input, accountType);
            paraArray[3] = sqlHelper.InitSQLiteParameter("@QQFriendID", ParameterDirection.Input, friendId);
            paraArray[4] = sqlHelper.InitSQLiteParameter("@QQGroupID", ParameterDirection.Input, groupId);
            paraArray[5] = sqlHelper.InitSQLiteParameter("@QQDicussionID", ParameterDirection.Input, discussionId);
            paraArray[6] = sqlHelper.InitSQLiteParameter("@MessageType", ParameterDirection.Input, messageType);
            paraArray[7] = sqlHelper.InitSQLiteParameter("@MessageContent", ParameterDirection.Input, content);
            paraArray[8] = sqlHelper.InitSQLiteParameter("@MessageOwnerAccount", ParameterDirection.Input, ownerAccount);
            paraArray[9] = sqlHelper.InitSQLiteParameter("@MessageOwnerName", ParameterDirection.Input, ownerName);

            if (sqlHelper.ExecuteNonQuery(sqlInsertQQMessage, CommandType.Text, paraArray) > 0)
            {
                return true;
            }

            return false;
        }
    }
}