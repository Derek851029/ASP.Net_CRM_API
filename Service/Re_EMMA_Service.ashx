<%@ WebHandler Language="C#" Class="Re_EMMA_Service" %>

using Dapper;
using System;
using System.Globalization;
using System.Web;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using log4net;
using log4net.Config;

public class Re_EMMA_Service : IHttpHandler
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    string cno;
    string login;
    string mail;
    string sqlstr;
    string EMMA;
    public void ProcessRequest(HttpContext context)
    {
        //int year = DateTime.Now.Year;
        //#region 判斷式
        //if (context.Request.Params["year"] != null
        //        && !int.TryParse(context.Request.Params["year"].ToString(), out year))
        //    context.Response.Write(JsonConvert.SerializeObject(
        //        new { status = "error", msg = "參數year只限輸入數字" }));
        //#endregion

        try
        {
            cno = "";
            login = "";
            mail = "";
            EMMA = "";
            sqlstr = @"SELECT [CNo] ,[UserID] ,[Agent_Mail] FROM CASEDetail " +
                          "WHERE StartTime < DATEADD(day, -1, getdate()) AND Type_Value != 5 " +
                          "ORDER BY StartTime";
            var list = DBTool.Query<ClassTemplate>(sqlstr);

            if (list.Count() > 0)
            {
                foreach (var q in list)
                {
                    cno = q.CNo;
                    login = q.UserID;
                    mail = q.Agent_Mail;
                    // page  1.  需求單審核    2.   員工派工管理  3.  個人派工及結案管理
                    EMMA = "http://127.0.0.1:5000/CheckLogin.aspx?seqno=" + cno + "&page=3&login=" + JASON.Encryption(login);
                    //EMMA = "http://dispatch.manstrong.com.tw:5000/CheckLogin.aspx?seqno=" + cno + "&page=3&login=" + login;
                    logger.Info("EMMA = " + EMMA);

                    ClassTemplate emma = new ClassTemplate()
                    {
                        AssignNo = "接單",  // "審核"  "派工"  "接單"  "暫結案"  "結案"
                        E_MAIL = mail,
                        ConnURL = EMMA
                    };

                    sqlstr = @"IF NOT EXISTS (SELECT SYSID  FROM tblAssign WHERE ConnURL=@ConnURL AND CreateDate > convert(varchar, getdate(), 111))  " +
                        "INSERT INTO tblAssign " +
                        "(AssignNo ,E_MAIL ,ConnURL) " +
                        "VALUES(@AssignNo ,@E_MAIL ,@ConnURL)";

                    using (IDbConnection db = DBTool.GetConn())
                    {
                        db.Execute(sqlstr, emma);
                        db.Close();
                    }
                }
            }
            context.Response.Write(JsonConvert.SerializeObject(new { status = "success", msg = DateTime.Now.AddDays(-1) + "前未【暫結案】之案件已發送提醒！！" }));
        }
        catch (Exception err)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { status = "error", msg = err.Message + Environment.NewLine + err.StackTrace }));
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}