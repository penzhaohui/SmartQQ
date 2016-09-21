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
            QQAccount qqProfile = new QQAccount();
            qqProfile.uin = smartQQWarapper.uin;
            qqProfile.name = smartQQWarapper.nick;
            qqProfile.category = "";
            qqProfile.markname = "";
            qqProfile.nick = smartQQWarapper.nick;
            qqProfile.gender = smartQQWarapper.gender;
            qqProfile.face = smartQQWarapper.face;
            qqProfile.client_type = 0;
            qqProfile.categories = 0;
            qqProfile.status = "";
            qqProfile.occupation = smartQQWarapper.occupation;
            qqProfile.college = smartQQWarapper.college;
            qqProfile.country = smartQQWarapper.country;
            qqProfile.province = smartQQWarapper.province;
            qqProfile.city = smartQQWarapper.city;
            qqProfile.personal = smartQQWarapper.personal;
            qqProfile.homepage = smartQQWarapper.homepage;
            qqProfile.email = smartQQWarapper.email;
            qqProfile.mobile = smartQQWarapper.mobile;
            qqProfile.phone = smartQQWarapper.phone;
            qqProfile.birthday = DateTime.MinValue;
            qqProfile.blood = smartQQWarapper.blood;
            qqProfile.shengxiao = smartQQWarapper.shengxiao;
            qqProfile.vip_info = smartQQWarapper.vip_info;
            qqProfile.onlineType = 0;

            addQQAccount(qqProfile);

            foreach (var friend in smartQQWarapper.Friends)
            {
                addQQAccount(friend);
            }
  
            return true;
        }

        #region QQ Profile

        private bool exists(string qqNum)
        {
            return false;
        }

        private bool addQQAccount(QQAccount account)
        {
            string sqlInsertQQAccount = @"INSERT INTO QQAccount (uin, name, category, markname, nick, gender, face, client_type, categories,
                                                                 status, occupation, college, country, province, city, personal, homepage,
                                                                 email, mobile, phone, birthday, blood, shengxiao, vip_info, onlineType) 
                                           VALUES (@uin, @name, @category, @markname, @nick, @gender, @face, @client_type, @categories,
                                                   @status, @occupation, @college, @country, @province, @city, @personal, @homepage,
                                                   @email, @mobile, @phone, @birthday, @blood, @shengxiao, @vip_info, @onlineType)";
            string uin = account.uin;
            string name = account.name;
            string category = "";
            string markname = "";
            string nick = "";
            string gender = "";
            int face = 0;
            int client_type = 0;
            int categories = 0;
            string status = "";
            string occupation = "";
            string college = "";
            string country = "";
            string province = "";
            string city = "";
            string personal = "";
            string homepage = "";
            string email = "";
            string mobile = "";
            string phone = "";
            DateTime birthday = DateTime.MinValue;
            string blood = "";
            string shengxiao = "";
            string vip_info = "";
            string onlineType = "";

            SQLiteParameter[] paraArray = new SQLiteParameter[25];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@uin", ParameterDirection.Input, uin);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@name", ParameterDirection.Input, name);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@category", ParameterDirection.Input, category);
            paraArray[3] = sqlHelper.InitSQLiteParameter("@markname", ParameterDirection.Input, markname);
            paraArray[4] = sqlHelper.InitSQLiteParameter("@nick", ParameterDirection.Input, nick);
            paraArray[5] = sqlHelper.InitSQLiteParameter("@gender", ParameterDirection.Input, gender);
            paraArray[6] = sqlHelper.InitSQLiteParameter("@face", ParameterDirection.Input, face);
            paraArray[7] = sqlHelper.InitSQLiteParameter("@client_type", ParameterDirection.Input, client_type);
            paraArray[8] = sqlHelper.InitSQLiteParameter("@categories", ParameterDirection.Input, categories);
            paraArray[9] = sqlHelper.InitSQLiteParameter("@status", ParameterDirection.Input, status);
            paraArray[10] = sqlHelper.InitSQLiteParameter("@occupation", ParameterDirection.Input, occupation);
            paraArray[11] = sqlHelper.InitSQLiteParameter("@college", ParameterDirection.Input, college);
            paraArray[12] = sqlHelper.InitSQLiteParameter("@country", ParameterDirection.Input, country);
            paraArray[13] = sqlHelper.InitSQLiteParameter("@province", ParameterDirection.Input, province);
            paraArray[14] = sqlHelper.InitSQLiteParameter("@city", ParameterDirection.Input, city);
            paraArray[15] = sqlHelper.InitSQLiteParameter("@personal", ParameterDirection.Input, personal);
            paraArray[16] = sqlHelper.InitSQLiteParameter("@homepage", ParameterDirection.Input, homepage);
            paraArray[17] = sqlHelper.InitSQLiteParameter("@email", ParameterDirection.Input, email);
            paraArray[18] = sqlHelper.InitSQLiteParameter("@mobile", ParameterDirection.Input, mobile);
            paraArray[19] = sqlHelper.InitSQLiteParameter("@phone", ParameterDirection.Input, phone);
            paraArray[20] = sqlHelper.InitSQLiteParameter("@birthday", ParameterDirection.Input, birthday);
            paraArray[21] = sqlHelper.InitSQLiteParameter("@blood", ParameterDirection.Input, blood);
            paraArray[22] = sqlHelper.InitSQLiteParameter("@shengxiao", ParameterDirection.Input, shengxiao);
            paraArray[23] = sqlHelper.InitSQLiteParameter("@vip_info", ParameterDirection.Input, vip_info);
            paraArray[24] = sqlHelper.InitSQLiteParameter("@onlineType", ParameterDirection.Input, onlineType);

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

        private bool addQQGroupAccount(string qqNum)
        {
            string sqlInsertQQGroupAccount = @"INSERT INTO QQGroupAccount (flag, gid, code, name, createtime, owner, face, memo, markname, level)
                                               VALUES (@flag, @gid, @code, @name, @createtime, @owner, @face, @memo, @markname, @level) ";
            string flag = "";
            string gid = "";
            string code = "";
            string name = "";
            string createtime = "";
            string owner = "";
            int face = 0;
            string memo = "";
            string markname = "";
            int level = 0;

            SQLiteParameter[] paraArray = new SQLiteParameter[10];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@flag", ParameterDirection.Input, flag);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@gid", ParameterDirection.Input, gid);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@code", ParameterDirection.Input, code);
            paraArray[3] = sqlHelper.InitSQLiteParameter("@name", ParameterDirection.Input, name);
            paraArray[4] = sqlHelper.InitSQLiteParameter("@createtime", ParameterDirection.Input, createtime);
            paraArray[5] = sqlHelper.InitSQLiteParameter("@owner", ParameterDirection.Input, owner);
            paraArray[6] = sqlHelper.InitSQLiteParameter("@face", ParameterDirection.Input, face);
            paraArray[7] = sqlHelper.InitSQLiteParameter("@memo", ParameterDirection.Input, memo);
            paraArray[8] = sqlHelper.InitSQLiteParameter("@markname", ParameterDirection.Input, markname);
            paraArray[9] = sqlHelper.InitSQLiteParameter("@level", ParameterDirection.Input, level);            

            try
            {
                if (sqlHelper.ExecuteNonQuery(sqlInsertQQGroupAccount, CommandType.Text, paraArray) > 0)
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

        private bool updateQQGroupAccount(string qqNum)
        {
            return false;
        }

        private bool addQQGroupMember(string qqNum)
        {
            string sqlInsertQQGroupMember = @"INSERT INTO QQGroupMember ( nick, card, province, gender, uin, country, city)
                                              VALUES(@nick, @card, @province, @gender, @uin, @country, @city)";
            string nick = "";
            string card = "";
            string province = "";
            string gender = "";
            string uin = "";
            string country = "";
            string city = "";

            SQLiteParameter[] paraArray = new SQLiteParameter[7];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@nick", ParameterDirection.Input, nick);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@card", ParameterDirection.Input, card);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@province", ParameterDirection.Input, province);
            paraArray[3] = sqlHelper.InitSQLiteParameter("@gender", ParameterDirection.Input, gender);
            paraArray[4] = sqlHelper.InitSQLiteParameter("@uin", ParameterDirection.Input, uin);
            paraArray[5] = sqlHelper.InitSQLiteParameter("@country", ParameterDirection.Input, country);
            paraArray[6] = sqlHelper.InitSQLiteParameter("@city", ParameterDirection.Input, city);
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

        private bool addQQDiscussionAccount(string qqNum)
        {
            string sqlInsertQQDiscussionAccount = @"INSERT INTO QQDiscussionAccount (did, name)
                                                    VALUES (@did, @name)";
            string did = "";
            string name = "";

            SQLiteParameter[] paraArray = new SQLiteParameter[2];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@did", ParameterDirection.Input, did);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@name", ParameterDirection.Input, name);
           
            try
            {
                if (sqlHelper.ExecuteNonQuery(sqlInsertQQDiscussionAccount, CommandType.Text, paraArray) > 0)
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

        private bool updateQQDiscussionAccount(string qqNum)
        {
            return false;
        }

        private bool addQQDiscussionMember(string qqNum)
        {
            string sqlInsertQQDiscussionMember = @"INSERT INTO QQDiscussionMember (uid, name, ruin)
                                                    VALUES (@uid, @name, @ruin)";
            string uid = "";
            string name = "";
            string ruin = "";

            SQLiteParameter[] paraArray = new SQLiteParameter[3];

            paraArray[0] = sqlHelper.InitSQLiteParameter("@uid", ParameterDirection.Input, uid);
            paraArray[1] = sqlHelper.InitSQLiteParameter("@name", ParameterDirection.Input, name);
            paraArray[2] = sqlHelper.InitSQLiteParameter("@ruin", ParameterDirection.Input, ruin);

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