using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Dapper;
using System.Data;
/// <summary>
/// DBTool 的摘要描述
/// </summary>
public class DBTool
{
    private static string connectionstr = WebConfigurationManager.ConnectionStrings["SQLString"].ToString() + ";Connection Timeout=300";

    public static IEnumerable<T> Query<T>(string sqlstr, object param = null)
    {
        using (SqlConnection conn = new SqlConnection(connectionstr))
        {
            return conn.Query<T>(sqlstr, param);
        }
    }

    public static IEnumerable<dynamic> Query(string sqlstr, object param = null)
    {
        using (SqlConnection conn = new SqlConnection(connectionstr))
        {
            return conn.Query(sqlstr, param);
        }
    }

    /// <summary>
    /// 建立連線，需要自行做關閉
    /// </summary>
    /// <returns></returns>
    public static IDbConnection GetConn()
    {
        return new SqlConnection(connectionstr);
    }
}