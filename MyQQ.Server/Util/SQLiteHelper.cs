using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Web;

namespace MyQQ.Util
{
    public class SQLiteHelper
    {
        #region Global Member

        private bool dbIsOpend;

        private String strConn;

        private SQLiteConnection sqlCon;

        private SQLiteCommand sqlCom;

        #endregion

        #region Init Class, Open And Close Database

        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLiteHelper()
        {
            this.sqlCom = null;
            this.sqlCon = null;
            this.dbIsOpend = false;
            this.strConn = String.Format(@"Data Source={0}App_Data\MyQQ.db", HttpRuntime.AppDomainAppPath);
        }


        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <returns></returns>
        public bool OpenConnection()
        {
            if (dbIsOpend)
            {
                return true;
            }

            this.sqlCon = new SQLiteConnection(this.strConn);
            try
            {
                this.sqlCon.Open();
                this.dbIsOpend = true;
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseConnection()
        {
            if (null != sqlCom)
            {
                this.sqlCom.Dispose();
                this.sqlCom = null;
            }

            if (this.dbIsOpend)
            {
                this.sqlCon.Close();
                this.dbIsOpend = false;
            }
        }

        #endregion

        #region Execute Fill Data

        /// <summary>
        /// 执行数据库操作来填充一个 DataSet
        /// </summary>
        /// <param name="strSql">要执行的 SQL</param>
        /// <param name="cmdType">执行命令的类型</param>
        /// <param name="paraArray">命令中所带的参数数组</param>
        /// <returns>返回填充有数据的 DataSet</returns>
        public DataSet ExecuteFillDataSet(String strSql, CommandType cmdType, SQLiteParameter[] paraArray)
        {
            if (!OpenConnection())
            {
                return null;
            }

            DataSet ds = new DataSet();

            using (this.sqlCom = sqlCon.CreateCommand())
            {
                this.sqlCom.CommandType = cmdType;
                this.sqlCom.CommandText = strSql;
                if (null != paraArray)
                {
                    this.sqlCom.Parameters.AddRange(paraArray);
                }

                using (SQLiteDataAdapter sqlDa = new SQLiteDataAdapter(sqlCom))
                {
                    sqlDa.Fill(ds);
                }
            }

            return ds;
        }


        /// <summary>
        /// 函数重载，执行不带参数的 SQL
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>

        public DataSet ExecuteFillDataSet(String strSql, CommandType cmdType)
        {
            return ExecuteFillDataSet(strSql, cmdType, null);
        }

        #endregion

        #region Execute Non Query

        /// <summary>
        /// 执行数据库操作来完成增删改操作
        /// </summary>
        /// <param name="strSql">要执行的 SQL</param>
        /// <param name="cmdType">执行命令的类型</param>
        /// <param name="paraArray">命令中所带的参数数组</param>
        /// <returns>返回受影响的行数</returns>
        public Int32 ExecuteNonQuery(String strSql, CommandType cmdType, SQLiteParameter[] paraArray)
        {
            Int32 rtAffectedRow = -1;

            if (!OpenConnection())
            {
                return rtAffectedRow;
            }

            using (this.sqlCom = this.sqlCon.CreateCommand())
            {
                this.sqlCom.CommandType = cmdType;
                this.sqlCom.CommandText = strSql;
                if (null != paraArray)
                {
                    this.sqlCom.Parameters.AddRange(paraArray);
                }

                rtAffectedRow = this.sqlCom.ExecuteNonQuery();
            }

            CloseConnection();

            return rtAffectedRow;
        }


        /// <summary>
        /// 函数重载，执行不带参数的增删改操作
        /// </summary>
        /// <param name="strSql">要执行的 SQL</param>
        /// <param name="cmdType">执行的 SQL 的命令类型</param>
        /// <returns>返回执行 SQL 后受影响的函数</returns>
        public Int32 ExecuteNonQuery(String strSql, CommandType cmdType)
        {
            return this.ExecuteNonQuery(strSql, cmdType, null);
        }


        /// <summary>
        /// 函数重载，一次性执行多条增删改的 SQL 命令，其中包括了事务处理
        /// </summary>
        /// <param name="strSqlArray">将要执行的 SQL 命令的数组</param>
        /// <param name="cmdTypeArray">将每一条要执行的 SQL 的命令类型保存在数组中</param>
        /// <param name="paraArrayList">每一条将要执行的 SQL 的命令中所带的参数数组，将这些数组保存在 List 中</param>
        /// <returns>返回执行操作是否成功</returns>
        public bool ExecuteNonQuery(String[] strSqlArray, CommandType[] cmdTypeArray, List<SQLiteParameter[]> paraArrayList)
        {
            bool isOK = false;

            if (!OpenConnection())
            {
                return isOK;
            }

            using (this.sqlCom = this.sqlCon.CreateCommand())
            {
                using (SQLiteTransaction sqlTsc = this.sqlCon.BeginTransaction())
                {
                    this.sqlCom.Transaction = sqlTsc;
                    try
                    {
                        for (int i = 0; i < strSqlArray.Length; i++)
                        {
                            this.sqlCom.CommandType = cmdTypeArray[i];
                            this.sqlCom.CommandText = strSqlArray[i];
                            if (null != paraArrayList && null != paraArrayList[i])
                            {
                                this.sqlCom.Parameters.AddRange(paraArrayList[i]);
                            }

                            this.sqlCom.ExecuteNonQuery();
                        }

                        sqlTsc.Commit();
                        isOK = true;
                    }
                    catch
                    {
                        sqlTsc.Rollback();
                        isOK = false;
                    }
                }
            }

            CloseConnection();
            return isOK;
        }


        /// <summary>
        /// 函数重载，一次性执行多条增删改的 SQL 命令，并且这些命令中都不带有参数，函数做了事务处理
        /// </summary>
        /// <param name="strSqlArray">将要执行的 SQL 命令的数组</param>
        /// <param name="cmdTypeArray">将要执行的每一条 SQL 命令的命令类型保存在数组中</param>
        /// <returns>返回执行多条 SQL 命令是否成功</returns>

        public bool ExecuteNonQuery(String[] strSqlArray, CommandType[] cmdTypeArray)
        {
            return ExecuteNonQuery(strSqlArray, cmdTypeArray, null);
        }

        #endregion

        #region Execute Reader

        /// <summary>
        /// 执行数据库查询操作，在调用该函数后必须在外部再次手动调用 CloseConnection 关闭数据库连接
        /// </summary>
        /// <param name="strSql">将要执行的查询 SQL 命令</param>
        /// <param name="cmdType">SQL 命令的类型</param>
        /// <param name="paraArray">SQL 命令中所带的参数</param>
        /// <returns>返回一个数据库读取对象</returns>
        public SQLiteDataReader ExecuteReader(String strSql, CommandType cmdType, SQLiteParameter[] paraArray)
        {
            if (!OpenConnection())
            {
                return null;
            }

            this.sqlCom = this.sqlCon.CreateCommand();
            this.sqlCom.CommandType = cmdType;
            this.sqlCom.CommandText = strSql;

            if (null != paraArray)
            {
                this.sqlCom.Parameters.AddRange(paraArray);
            }

            return this.sqlCom.ExecuteReader();
        }


        /// <summary>
        /// 函数重载，执行不带操作的数据库查询操作
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>

        public SQLiteDataReader ExecuteReader(String strSql, CommandType cmdType)
        {
            return ExecuteReader(strSql, cmdType, null);
        }

        #endregion

        #region Execute Scalar

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="strSql">将要执行的 SQL 命令</param>
        /// <param name="cmdType">将要执行的 SQL 命令的命令类型</param>
        /// <param name="paraArray">将要执行的 SQL 命令所带的参数放在数组中</param>
        /// <returns>返回查询所返回的结果集中第一行的第一列</returns>
        public object ExecuteScalar(String strSql, CommandType cmdType, SQLiteParameter[] paraArray)
        {
            object rtScalar = -1;

            if (!OpenConnection())
            {
                return rtScalar;
            }

            using (this.sqlCom = this.sqlCon.CreateCommand())
            {
                this.sqlCom.CommandType = cmdType;
                this.sqlCom.CommandText = strSql;

                if (null != paraArray)
                {
                    this.sqlCom.Parameters.AddRange(paraArray);
                }

                rtScalar = this.sqlCom.ExecuteScalar();
            }

            CloseConnection();
            return rtScalar;
        }


        /// <summary>
        /// 函数重载，执行不带参数的 SQL 查询命令
        /// </summary>
        /// <param name="strSql">将要执行的 SQL 命令</param>
        /// <param name="cmdType">将要执行的 SQL 命令的命令类型</param>
        /// <returns>返回查询所返回的结果集中第一行的第一列</returns>
        public object ExecuteScalar(String strSql, CommandType cmdType)
        {
            return ExecuteScalar(strSql, cmdType, null);
        }

        #endregion

        #region Static - Init SQLite Parameter

        /// <summary>
        /// 实现命令参数的初始化
        /// </summary>
        /// <param name="strParaName">参数名称</param>
        /// <param name="paraDbType">参数的数据类型</param>
        /// <param name="paraDirection">参数方向</param>
        /// <param name="paraValue">参数值</param>
        /// <returns>返回初始化好后的参数</returns>
        public SQLiteParameter InitSQLiteParameter(String strParaName, DbType paraDbType, ParameterDirection paraDirection, object paraValue)
        {
            SQLiteParameter para = new SQLiteParameter();

            para.ParameterName = strParaName;
            para.DbType = paraDbType;
            para.Direction = paraDirection;
            para.Value = paraValue;

            return para;
        }


        /// <summary>
        /// 函数重载，执行 BOOL 类型的参数的初始化
        /// </summary>
        /// <param name="strParaName"></param>
        /// <param name="paraDirection"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        public SQLiteParameter InitSQLiteParameter(String strParaName, ParameterDirection paraDirection, bool paraValue)
        {
            return InitSQLiteParameter(strParaName, DbType.Boolean, paraDirection, (bool)paraValue);
        }


        /// <summary>
        /// 函数重载，执行 String 类型的参数的初始化
        /// </summary>
        /// <param name="strParaName"></param>
        /// <param name="paraDirection"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        public SQLiteParameter InitSQLiteParameter(String strParaName, ParameterDirection paraDirection, String paraValue)
        {
            return InitSQLiteParameter(strParaName, DbType.String, paraDirection, (String)paraValue);
        }


        /// <summary>
        /// 函数重载，执行 Int32 类型的参数的初始化
        /// </summary>
        /// <param name="strParaName"></param>
        /// <param name="paraDirection"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        public SQLiteParameter InitSQLiteParameter(String strParaName, ParameterDirection paraDirection, Int32 paraValue)
        {
            return InitSQLiteParameter(strParaName, DbType.Int32, paraDirection, (Int32)paraValue);
        }


        /// <summary>
        /// 函数重载，执行 Double 类型的参数的初始化
        /// </summary>
        /// <param name="strParaName"></param>
        /// <param name="paraDirection"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        public SQLiteParameter InitSQLiteParameter(String strParaName, ParameterDirection paraDirection, Double paraValue)
        {
            return InitSQLiteParameter(strParaName, DbType.Double, paraDirection, (Double)paraValue);
        }


        /// <summary>
        /// 函数重载，执行 Single 类型的参数的初始化
        /// </summary>
        /// <param name="strParaName"></param>
        /// <param name="paraDirection"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        public SQLiteParameter InitSQLiteParameter(String strParaName, ParameterDirection paraDirection, Single paraValue)
        {
            return InitSQLiteParameter(strParaName, DbType.Single, paraDirection, (Single)paraValue);
        }


        /// <summary>
        /// 函数重载，执行 DateTime 类型的参数的初始化
        /// </summary>
        /// <param name="strParaName"></param>
        /// <param name="paraDirection"></param>
        /// <param name="paraValue"></param>
        /// <returns></returns>
        public SQLiteParameter InitSQLiteParameter(String strParaName, ParameterDirection paraDirection, DateTime paraValue)
        {
            return InitSQLiteParameter(strParaName, DbType.DateTime, paraDirection, (DateTime)paraValue);
        }

        #endregion
    }
}