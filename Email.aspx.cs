using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;
using log4net;
using log4net.Config;

public partial class Email : System.Web.UI.Page
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {
        EMAIL();
    }

    private void EMAIL()
    {
        string sqlstr;
        string time;
        string email_id = "";
        string email_pw = "";
        sqlstr = @"SELECT TOP 1 * FROM tblAssign WHERE Email_Flag !='1' AND Email_Flag !='2' " +
            " OR Email_Flag IS NULL AND CreateDate > convert(varchar, getdate()-7, 112)  order by CreateDate ";
        
            var list = DBTool.Query<ClassTemplate>(sqlstr);
            if (list.Count() > 0)
            {
                foreach (var q in list)
                {
                    string sql = "UPDATE tblAssign SET Email_Flag='1' WHERE SYSID=@SYSID";
                    string no = q.AssignNo.Trim(); //刪除字串中的空白

                    time = DateTime.Now.ToString("HH");
                    logger.Info(time);
                    if (time == "00" || time == "04" || time == "08" || time == "12" || time == "16" || time == "20")
                    {
                        email_id = "crm_server@dimax.com.tw";
                        email_pw = "crm123";
                    }
                    else if (time == "01" || time == "05" || time == "09" || time == "13" || time == "17" || time == "21")
                    {
                        email_id = "crm_server@dimax.com.tw";
                        email_pw = "crm123";
                    }
                    else if (time == "02" || time == "06" || time == "10" || time == "14" || time == "18" || time == "22")
                    {
                        email_id = "crm_server@dimax.com.tw";
                        email_pw = "crm123";
                    }
                    else
                    {
                        email_id = "crm_server@dimax.com.tw";
                        email_pw = "crm123";
                    }

                    //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port 
                    // Yahoo!	smtp.mail.yahoo.com	587	True
                    //GMail	smtp.gmail.com	587	True
                    //Hotmail	smtp.live.com	587	True
                    //hibox   www.hibox.hinet.net   587
                    try
                    {
                        System.Net.Mail.SmtpClient MySmtp = new System.Net.Mail.SmtpClient("www.hibox.hinet.net", 587);
                        //設定你的帳號密碼
                        MySmtp.Credentials = new System.Net.NetworkCredential(email_id, email_pw);
                        //Gmial 的 smtp 使用 SSL
                        MySmtp.EnableSsl = true;
                        //發送Email

                        //MySmtp.Send("manstrong.dispatch@gmail.com", q.E_MAIL, "派工系統通知", "需求單狀態：" + no + "\n" +
                        //MySmtp.Send("dispatch.service1@manstrong.com.tw", q.E_MAIL, "派工系統通知", "需求單狀態：" + no + "\n" +
                       
                        MySmtp.Send("crm_server@dimax.com.tw", q.E_MAIL, "派工系統通知", "需求單狀態：" + no + "\n" +   //代理寄信的信箱 需要先設好代寄功能
                            "需求單連結：" + q.ConnURL.Trim() + "\n*本郵件為自動發送請勿直接回覆 謝謝!!");
                        Response.Write("郵件發送狀態：成功" + "<br>");
                        Response.Write("發送編號：" + q.SYSID + "<br>");
                        Response.Write("目標信箱地址：" + q.E_MAIL);
                        using (IDbConnection db = DBTool.GetConn())
                        {
                            db.Execute(sql, new { SYSID = q.SYSID });
                            db.Close();
                        }
                    }
                    catch (Exception error)
                    {
                        Response.Write("郵件發送狀態：失敗" + "<br>");
                        Response.Write("發送編號：" + q.SYSID + "<br>");
                        Response.Write("目標信箱地址：" + q.E_MAIL);
                        logger.Info("===== Email_Delay =====");
                        logger.Info("郵件發送狀態：失敗");
                        logger.Info("發送編號：" + q.SYSID);
                        logger.Info("目標信箱地址：" + q.E_MAIL);
                        logger.Info("ERROR：" + error);
                        logger.Info("==================");

                        sql = "UPDATE tblAssign SET Email_Flag='2' WHERE SYSID=@SYSID";
                        no = q.AssignNo.Trim(); //刪除字串中的空白
                        using (IDbConnection db = DBTool.GetConn())
                        {
                            db.Execute(sql, new { SYSID = q.SYSID });
                            db.Close();
                        }
                    }
                }
            }
            else
            {
                Response.Write("郵件發送狀態：沒有需要發送的E-Mail");
            }
      
    }
}