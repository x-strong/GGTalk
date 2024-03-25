using ESBasic.Helpers;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OrayTalk.Server.Oracle
{
    public class OracleHelper
    {
        private static string db_ip, db_name,userId, password ,connStr;
        private static int db_port = 1521;

        public static void Initialize(string dbName, string userID, string psw, string dbIp, int port)
        {
            db_ip = dbIp;
            db_port = port;
            db_name = dbName;
            userId = userID;
            password = psw;
            connStr = string.Format("User Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVICE_NAME={4})))", userId, password, db_ip, db_port, db_name);
        }

        #region 执行SQL语句,返回受影响行数
        public static int ExecuteNonQuery(string sql, params OracleParameter[] parameters)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion
        #region 执行SQL语句,返回DataTable;只用来执行查询结果比较少的情况
        public static DataTable ExecuteDataTable(string sql, params OracleParameter[] parameters)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    DataTable datatable = new DataTable();
                    adapter.Fill(datatable);
                    return datatable;
                }
            }
        }
        #endregion

        /// <summary>
        /// 将对象中属性类型为string 值为null 的属性设置为string.Empty 字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void SetEmptyProperty4Null<T>(T t)
        {
            if (t == null)
            {
                return;
            }
            PropertyInfo[] sourcePros = t.GetType().GetProperties();
            foreach (PropertyInfo sourceProperty in sourcePros)
            {
                if (sourceProperty.CanRead)
                {
                    object val = ReflectionHelper.GetProperty(t, sourceProperty.Name);
                    if (val == null && ILGlobals.ProcessSpecialTypes(sourceProperty.PropertyType.ToString()) == "string")
                    {
                        ReflectionHelper.SetProperty(t, sourceProperty.Name, string.Empty);
                    }                    
                }
            }
        }
    }
}
