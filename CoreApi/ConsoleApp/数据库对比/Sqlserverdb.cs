using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ConsoleApp
{
    public class Sqlserverdb
    {

        //连接数据库
        public static string connStr = "Server=服务器地址; Database=数据库名称; User Id =用户; Password=密码;Integrated Security=True";
        public static SqlConnection cnn = new SqlConnection(connStr);

        public Sqlserverdb(string conn)
        {
            connStr = conn;
            cnn = new SqlConnection(connStr);
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <returns></returns>
        public static int ExecuteSqlTran(List<string> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                command.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            command.CommandText = strsql;
                            count += command.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 执行增删改的操作
        /// </summary>
        /// <param name="sql">sql命令</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(string sql)
        {
            Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            int result = command.ExecuteNonQuery();
            cnn.Close();
            return result;
        }
        /// <summary>
        /// 查询单个值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql)
        {
            Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            object result = command.ExecuteScalar();
            cnn.Close();
            return result;
        }
        /// <summary>
        /// 返回数据表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds.Tables[0];
        }
        /// <summary>
        /// 返回DataReader对象，使用结束后，勿忘关闭DataReader与数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlDataReader GetDataReader(string sql)
        {
            Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            return command.ExecuteReader();
        }
        /// <summary>
        /// 打开数据库
        /// </summary>
        public static void Open()
        {
            if (cnn.State == ConnectionState.Broken || cnn.State == ConnectionState.Open)
            {
                cnn.Close();
            }
            cnn.Open();
        }
        /// <summary>
        /// 打开数据库
        /// </summary>
        public static void Close()
        {
            cnn.Close();
        }



        public DataTable GetMysqlDBData()
        {
            string sql = $@"SELECT
              表名 = A.TABLENAME,
              字段名 = A.FIELDNAME,
               类型=A.FIELDTYPE ,
               长度=A.LENGTH ,
              --小数位=isnull(A.LENSEC,0) ,
              --顺序= A.FIELDSNO,
               允许空值= str(A.ALLOWNULL)
            FROM (SELECT
                    TABLENAME = B.NAME,
                    FIELDNAME = A.NAME,
                    FIELDSNO = A.COLID,
                    FIELDTYPE = C.NAME,
                    LENGTH = A.LENGTH,
                    LENSEC = A.XSCALE,
                    ALLOWNULL = A.ISNULLABLE
                  FROM 
               dbo.SYSCOLUMNS A		
                    LEFT JOIN 
                dbo.SYSOBJECTS B		
                      ON A.ID = B.ID
                    LEFT JOIN 
               dbo.SYSTYPES C		
                      ON A.XUSERTYPE = C.XUSERTYPE
                  WHERE B.XTYPE = 'U') A";

            var dt = GetDataTable(sql);

            return dt;

        }
    }
}
