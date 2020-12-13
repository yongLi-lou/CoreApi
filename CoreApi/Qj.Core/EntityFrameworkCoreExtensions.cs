using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Qj.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Qj.Core
{
    public static class EntityFrameworkCoreExtensions
    {
        private static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection connection, DbTransaction db = null, params object[] parameters)
        {
            var conn = facade.GetDbConnection();
            connection = conn;
            conn.Open();
            var cmd = conn.CreateCommand();
            //if (facade.IsSqlServer())
            //{
            cmd.CommandText = sql;
            cmd.Transaction = db;
            cmd.Parameters.AddRange(parameters);
            //}
            return cmd;
        }

        public static DataTable SqlQuery(this DatabaseFacade facade, string sql, DbTransaction db = null, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, db, parameters);
            var reader = command.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }

        public static int ExecuteScalar(this DatabaseFacade facade, string sql, DbTransaction db = null, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, db, parameters);
            int count = (int)command.ExecuteScalar();
            conn.Close();
            return count;
        }

        public static int ExecuteNonQuery(this DatabaseFacade facade, string sql, DbTransaction db = null, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, db, parameters);
            var count = command.ExecuteNonQuery();
            conn.Close();
            return count;
        }

        /// <summary>
        /// 执行多个sql 事务
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="listsql"></param>
        /// <param name="db"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this DatabaseFacade facade, List<string> listsql, DbTransaction db = null, params object[] parameters)
        {
            var conn = facade.GetDbConnection();
            conn.Open();
            db = conn.BeginTransaction();
            try
            {
                foreach (var sql in listsql)
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.Transaction = db;
                    cmd.Parameters.AddRange(parameters);
                    int t = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                db.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                db.Rollback();
                throw ex;
                return 0;
            }



        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable SqlQueryByPage(this DatabaseFacade facade, string sql, ref int count, DbTransaction db = null, params object[] parameters)
        {
            try
            {
                var command = CreateCommand(facade, sql, out DbConnection conn, db, parameters);
                var reader = command.ExecuteReader();
                var dt = new DataTable();
                var ds = new DataSet();
                ds.Load(reader, LoadOption.Upsert, "ds,count".Split(','));
                dt = ds.Tables[0];
                count = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                reader.Close();
                conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<T> SqlQueryByPage<T>(this DatabaseFacade facade, string sql, ref int count, DbTransaction db = null, params object[] parameters) where T : class, new()
        {
            var dt = SqlQueryByPage(facade, sql, ref count, db, parameters);
            return dt.ToList<T>();
        }

        public static List<T> SqlQuery<T>(this DatabaseFacade facade, string sql, DbTransaction db = null, params object[] parameters) where T : class, new()
        {
            var dt = SqlQuery(facade, sql, db, parameters);
            return dt.ToList<T>();
        }

        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            var propertyInfos = typeof(T).GetProperties();
            var list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                var t = new T();

                foreach (PropertyInfo p in propertyInfos)
                {
                    if (dt.Columns.IndexOf(p.Name) != -1 && row[p.Name] != DBNull.Value)
                    {
                        var value = row[p.Name];
                        if (p.PropertyType.FullName == typeof(Boolean).FullName)
                        {
                            p.SetValue(t, Convert.ToBoolean(value), null);
                        }
                        else if (p.PropertyType.FullName == typeof(Int64).FullName)
                        {
                            p.SetValue(t, Convert.ToInt64(value), null);
                        }
                        else if (p.PropertyType.FullName == typeof(Int32).FullName)
                        {
                            p.SetValue(t, value.FormatToInt(0), null);
                        }
                        else
                        {
                            p.SetValue(t, value, null);
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }

        public static T SqlQueryModel<T>(this DatabaseFacade facade, string sql, DbTransaction db = null, params object[] parameters) where T : class, new()
        {
            var dt = SqlQuery(facade, sql, db, parameters);

            if (dt.ToList<T>() != null && dt.ToList<T>().Count > 0)
            {
                return dt.ToList<T>()[0];
            }
            return null;
        }

        public static DbDataReader SqlQuery2(this DatabaseFacade facade, string sql, DbTransaction db = null, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, db, parameters);
            var reader = command.ExecuteReader();
            //reader.Close();
            //conn.Close();
            return reader;
        }

        public static DbDataReader SqlQuery3(this DatabaseFacade facade, string sql, out DbConnection conn, DbTransaction db = null, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out conn, db, parameters);
            var reader = command.ExecuteReader();
            return reader;
        }

        public static DbCommand GetCommand(this DatabaseFacade facade, string sql, out DbConnection connection, DbTransaction db = null, params object[] parameters)
        {
            var conn = facade.GetDbConnection();
            connection = conn;
            conn.Open();
            var cmd = conn.CreateCommand();
            //if (facade.IsSqlServer())
            //{
            cmd.CommandText = sql;
            cmd.Parameters.AddRange(parameters);
            //}
            return cmd;
        }
    }
}