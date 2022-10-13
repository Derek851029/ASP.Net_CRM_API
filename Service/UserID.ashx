<%@ WebHandler Language="C#" Class="UserID" %>
using Dapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using log4net;
using log4net.Config;

public class UserID : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string str_sql = @"SELECT UserID FROM DispatchSystem WHERE UserID !='' AND UserID is not null AND login is null ";
        var chk = DBTool.Query<Agent>(str_sql);
        if (chk.Any())
        {
            int i = 0;
            foreach (var q in chk)
            {
                i += 1;
                str_sql = @"UPDATE DispatchSystem SET login=@login WHERE UserID=@UserID ";

                using (IDbConnection db = DBTool.GetConn())
                {
                    db.Execute(str_sql, new { UserID = q.UserID, login = JASON.Encryption(q.UserID).Trim() });
                    db.Close();
                };
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("UserID完成，共：" + i + " 筆");
        }
        else
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("沒有需要加密的UserID");
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public class Agent
    {
        public string UserID { get; set; }
    }
}