<%@ WebHandler Language="C#" Class="ClassScheduleService" %>

using Dapper;
using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using log4net;
using log4net.Config;

public class ClassScheduleService : IHttpHandler
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
        string sql;
        string sqlstr;
        string Sqlstr;
        int i;
        int Count = 0;
        string Email_1 = "";
        string Email_2 = "";
        string Email_3 = "";
        string EMMA = System.Web.Configuration.WebConfigurationManager.AppSettings["EMMA"].ToString();
        string EMMA_URL;
        string page = "5";    // 1.  需求單審核    2.   員工派工管理  3.  個人派工及結案管理  4.  需求單（瀏覽） 結案  5.  巡(查看)
        
        
        DateTime date = DateTime.Now;

        logger.Info("InSpecation_Email 檢查巡檢未巡任務" + date);

        try        
        {
            /*sql = @"SELECT count(SYSID) as Count " +
            " FROM [InSpecation].[dbo].[Mission_Case] where Type_Value = '1' and Email_Flag = '0' and ( " +
            " (Cycle = '0' and CONVERT(varchar(100), Work_Time, 111) < CONVERT(varchar(100), GETDATE(), 111) ) or " +
            " (Cycle = '1' and CONVERT(varchar(100), Work_Time+1, 111) < CONVERT(varchar(100), GETDATE(), 111) ) or " +
            " (Cycle in ('2','3','4') and CONVERT(varchar(100), Work_Time+3, 111) < CONVERT(varchar(100), GETDATE(), 111) ) " +
            " ) ";
            var list0 = DBTool.Query<InSpecation_Email_str>(sql);
            if (list0.Any())
            {
                InSpecation_Email_str schedule = list0.First();
                Count = schedule.Count;
            }
            logger.Info("共有 " + Count.ToString() + " 個超時未巡任務");     //計算共有幾筆 超時未巡且未通知的任務*/
            
            sqlstr = @"SELECT SYSID,mission_name,CONVERT(varchar(100), Work_Time, 111)as W_Date,Cycle_Name,Hall " +
            " FROM [InSpecation].[dbo].[Mission_Case] where Type_Value = '1' and Email_Flag = '0' and ( " +
            " (Cycle = '0' and CONVERT(varchar(100), Work_Time, 111) < CONVERT(varchar(100), GETDATE(), 111) ) or " +
            " (Cycle = '1' and CONVERT(varchar(100), Work_Time+1, 111) < CONVERT(varchar(100), GETDATE(), 111) ) or " +
            " (Cycle in ('2','3','4') and CONVERT(varchar(100), Work_Time+3, 111) < CONVERT(varchar(100), GETDATE(), 111) ) " +
            " ) ";

            var list = DBTool.Query<ClassTemplate>(sqlstr);
            if (list.Count() > 0)
            {
                foreach (var q in list)
                {
                    logger.Info("編號:" + q.SYSID + "  名稱:" + q.mission_name + "  日期:" + q.W_Date + "  週期:" + q.Cycle_Name + "  館別:" + q.Hall);
                    EMMA_URL = EMMA + "CheckLogin.aspx?seqno=" + q.SYSID + "&page=" + page + "&login=";

                    Sqlstr = @"SELECT Email_1 as Flag_1, Email_2 as Flag_2, Email_3 as Flag_3 FROM InSpecation.dbo.Mission_Case as a " +
                        " left join InSpecation.dbo.Hall as b on b.Hall_name = a.Hall where a.SYSID = @SYSID ";
                    var list2 = DBTool.Query<ClassTemplate>(Sqlstr, new { SYSID = q.SYSID });
                    if (list2.Any())
                    {
                        ClassTemplate schedule = list2.First();
                        Email_1 = schedule.Flag_1;
                        Email_2 = schedule.Flag_2;
                        Email_3 = schedule.Flag_3;
                    }
                    if (!string.IsNullOrEmpty(Email_1))     //有填寫宿舍主管信箱 發信給宿舍主管
                    {
                        logger.Info("Email_1 = " + Email_1 + "    發信給 宿舍主管");
                        logger.Info("編號:" + q.SYSID + "  名稱:" + q.mission_name + "  日期:" + q.W_Date + "  週期:" + q.Cycle_Name + "  館別:" + q.Hall);
                        Sqlstr = @"INSERT INTO tblAssign (AssignNo, E_MAIL, ConnURL) VALUES(@AssignNo ,@E_MAIL ,@ConnURL) " +
                            //"";
                            " UPDATE InSpecation.dbo.Mission_Case SET Email_Flag = '1' WHERE SYSID = @SYSID ";

                        ClassTemplate emma = new ClassTemplate()
                        {
                            AssignNo = "【巡檢未巡通知】【" + q.mission_name + " 日期:" + q.W_Date + " 週期:" + q.Cycle_Name + "】",    // "審核"  "派工"  "接單"  "暫結案"  "結案"
                            ConnURL = EMMA_URL + JASON.Encryption("admin"),
                            E_MAIL = Email_1,
                            SYSID = q.SYSID
                        };
                        using (IDbConnection db = DBTool.GetConn())
                        {
                            db.Execute(Sqlstr, emma);
                            db.Close();
                        }                        

                        logger.Info("宿舍主管 發信結束");
                    }
                    if (!string.IsNullOrEmpty(Email_2))     //有填寫部門主管信箱 發信給部門主管
                    {
                        logger.Info("Email_2 = " + Email_2 + "    發信給 部門主管");
                        logger.Info("編號:" + q.SYSID + "  名稱:" + q.mission_name + "  日期:" + q.W_Date + "  週期:" + q.Cycle_Name + "  館別:" + q.Hall);
                        Sqlstr = @"INSERT INTO tblAssign (AssignNo, E_MAIL, ConnURL) VALUES(@AssignNo ,@E_MAIL ,@ConnURL) " +
                            //"";
                            " UPDATE InSpecation.dbo.Mission_Case SET Email_Flag = '1' WHERE SYSID = @SYSID ";

                        ClassTemplate emma = new ClassTemplate()
                        {
                            AssignNo = "【巡檢未巡通知】【" + q.mission_name + " 日期:" + q.W_Date + " 週期:" + q.Cycle_Name + "】",    // "審核"  "派工"  "接單"  "暫結案"  "結案"
                            ConnURL = EMMA_URL + JASON.Encryption("admin"),
                            E_MAIL = Email_2,
                            SYSID = q.SYSID
                        };
                        using (IDbConnection db = DBTool.GetConn())
                        {
                            db.Execute(Sqlstr, emma);
                            db.Close();
                        }
                        logger.Info("部門主管 發信結束");
                    }
                    if (!string.IsNullOrEmpty(Email_3))     //有填寫備用信箱 發信給備用
                    {
                        logger.Info("Email_3 = " + Email_3 + "    發信給 備用信箱");
                        logger.Info("編號:" + q.SYSID + "  名稱:" + q.mission_name + "  日期:" + q.W_Date + "  週期:" + q.Cycle_Name + "  館別:" + q.Hall);
                        Sqlstr = @"INSERT INTO tblAssign (AssignNo, E_MAIL, ConnURL) VALUES(@AssignNo ,@E_MAIL ,@ConnURL) " +
                            //"";
                            " UPDATE InSpecation.dbo.Mission_Case SET Email_Flag = '1' WHERE SYSID = @SYSID ";

                        ClassTemplate emma = new ClassTemplate()
                        {
                            AssignNo = "【巡檢未巡通知】【" + q.mission_name + " 日期:" + q.W_Date + " 週期:" + q.Cycle_Name + "】",    // "審核"  "派工"  "接單"  "暫結案"  "結案"
                            ConnURL = EMMA_URL + JASON.Encryption("admin"),
                            E_MAIL = Email_3,
                            SYSID = q.SYSID
                        };
                        using (IDbConnection db = DBTool.GetConn())
                        {
                            db.Execute(Sqlstr, emma);
                            db.Close();
                        }
                        logger.Info("備用信箱 發信結束");
                    }
                }
                context.Response.Write(JsonConvert.SerializeObject(new
                {
                    status = "success",
                    msg = string.Empty
                        + "巡檢共 " + list.Count() + " 筆超時未巡任務尚未通知"
                }));
                logger.Info("清除過期且未填信箱的的巡檢任務");   //預定日五天後清除 

                ClassTemplate template = new ClassTemplate(){};
                sqlstr = @"UPDATE InSpecation.dbo.Mission_Case SET Email_Flag = '2' WHERE Type_Value = '1' and Email_Flag = '0' " +
                    " and Work_Time < CONVERT(varchar(100), getdate()-5, 111)  ";
                using (IDbConnection db = DBTool.GetConn())
                {
                    db.Execute(sqlstr, template);
                    db.Close();
                };
            }
            else
            {
                logger.Info("沒有尚未通知的超時未巡任務");
                context.Response.Write(JsonConvert.SerializeObject(new
                {
                    status = "finish",
                    msg = string.Empty
                        + "沒有尚未通知的超時未巡任務"
                }));                
            }
            
        }
        catch (Exception err)
        {
            logger.Info("ERROE：" + err);
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
    public class InSpecation_Email_str
    {
        public int Count { get; set; }
    }
}