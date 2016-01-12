using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;

namespace CCH
{
    public class SqlHelper
    {
        /// <summary>
        /// 获取webconfig 中的连接串
        /// </summary>
        #region ConnectionString
        public static readonly string ConnString = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;
        #endregion

        /// <summary>
        /// 返回sql语句受影响的行数
        /// </summary>
        /// <param name="cmdType">获取CommandType类型的参数 cmdText</param>
        /// <param name="cmdText">获取string类型的sql语句作为参数</param>
        /// <param name="cmdParams">where语句条件的参数</param>
        /// <returns></returns>
        #region ExecuteNonQuery
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            int reValue = 0;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                    conn.Open();
                    cmd.CommandTimeout = 10000;
                    reValue = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            return reValue;
        }
        public static int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            int reValue = 0;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.CommandTimeout = 10000;
                reValue = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            return reValue;
        }
        #endregion

        #region ExecuteReader
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            SqlConnection conn = new SqlConnection(ConnString);
            try
            {
                SqlDataReader reReader;
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                    conn.Open();
                    cmd.CommandTimeout = 10000;
                    reReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                }
                return reReader;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        public static SqlDataReader ExecuteReader(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            SqlDataReader reReader;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.CommandTimeout = 10000;
                reReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
            }
            return reReader;
        }
        #endregion

        #region ExecuteScalar
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            object reValue;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                    conn.Open();
                    cmd.CommandTimeout = 10000;
                    reValue = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                }
            }
            return reValue;
        }
        public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            object reValue;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.CommandTimeout = 10000;
                reValue = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            return reValue;
        }
        #endregion

        #region ExecuteDataTable
        public static DataTable ExecuteDataTable(CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            DataTable reData = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        cmd.CommandTimeout = 10000;
                        sda.Fill(reData);
                    }
                    cmd.Parameters.Clear();
                }
            }
            return reData;
        }
        public static DataTable ExecuteDataTable(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParams)
        {
            DataTable reData = new DataTable();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                PrepareCommand(cmd, cmdType, cmdText, cmdParams);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.CommandTimeout = 10000;
                    sda.Fill(reData);
                }
                cmd.Parameters.Clear();
            }
            return reData;
        }
        #endregion

        #region CloseConnection
        public static void CloseConnection(SqlConnection conn)
        {
            if (conn.State != ConnectionState.Closed)
                conn.Close();
        }
        #endregion

        #region PrepareCommand
        private static void PrepareCommand(SqlCommand cmd, CommandType cmdType, string cmdText, SqlParameter[] cmdParams)
        {
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            if (cmdParams != null)
            {
                foreach (SqlParameter param in cmdParams)
                {
                    cmd.Parameters.Add(param);
                }
            }
        }
        #endregion

        public static SqlTransaction BeginTransaction(string con)
        {
            SqlConnection connection = new SqlConnection(con);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction();
            return tran;

        }

        public static List<T> TableToEntity<T>(DataTable dt) where T : class, new()
        {
            Type type = typeof(T);
            List<T> list = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                PropertyInfo[] pArray = type.GetProperties();
                T entity = new T();
                foreach (PropertyInfo p in pArray)
                {
                    if (row[p.Name] is Int64)
                    {
                        p.SetValue(entity, Convert.ToInt32(row[p.Name]), null);
                        continue;
                    }
                    p.SetValue(entity, row[p.Name], null);
                }
                list.Add(entity);
            }
            return list;
        }
    }
}