<%@ WebHandler Language="C#" Class="Daily_Email" %>

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

public class Daily_Email : IHttpHandler
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
        string sql;
        string sqlstr;
        string Sqlstr;
        string Sql;
        string Sql02;
        string str_AssignNo = "【服務單逾期通知】";    //100字 要上調配合多筆同發
        string str_URLS = "";
        int i;
        int Count = 0;
        string EMMA = System.Web.Configuration.WebConfigurationManager.AppSettings["EMMA"].ToString();
        string EMMA_URL;
        string page = "4";    // 1.  需求單審核    2.   員工派工管理  3.  個人派工及結案管理  4.  需求單（瀏覽） 結案  5.  巡(查看)


        DateTime date = DateTime.Now;

        logger.Info("Daily_Email 檢查逾期服務單" + date);
        try
        {
            /*sqlstr = @"Select Case_ID,C_ID2,SetupTime,Email_Flag,BUSINESSNAME,Urgency,Type,Handle_Agent FROM CaseData " +
            " where Type in('未到點','處理中')and Email_Flag=0 and " +
            " (CONVERT(varchar, SetupTime+1, 23)<CONVERT(varchar, GETDATE(), 23) or " +
            " (CONVERT(varchar, SetupTime, 23)<CONVERT(varchar, GETDATE(), 23)and Urgency in('緊急故障','重要故障'))) " +
            " order by C_ID2 "; // 原版 沒考慮周末的版本*/
            
            sqlstr = @"Select Case_ID,C_ID2,SetupTime,Email_Flag,a.BUSINESSNAME,Urgency,a.Type,Handle_Agent FROM CaseData a " +
            " left join BusinessData b on a.PID=b.PID " +
            " where a.Type in('未到點','處理中')and Email_Flag=0 and Urgency not in('','其他') And " +
            " (  " +
            " (Saturday_Work=1 and b.Sunday_Work=1)  " +    //周末正常上班
            " or DATEPART(WEEKDAY, SetupTime-1)<4  " +  //周一~三
            " or (DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency in('緊急故障','重要故障')) " +   //週四 緊急
            " or (DATEPART(WEEKDAY, SetupTime-1)=4 and Saturday_Work=1) " +   //週四 六上班
            " or (DATEPART(WEEKDAY, SetupTime-1)=5 and Urgency in('緊急故障','重要故障')and Saturday_Work=1) " +   //週五 緊急 六上班
            " or (DATEPART(WEEKDAY, SetupTime-1)=7 and Sunday_Work=1) " +   //週日 日上班
            " ) and" +   //
            " (CONVERT(varchar, SetupTime+1, 23)<CONVERT(varchar, GETDATE(), 23) or  " +   //
            " (CONVERT(varchar, SetupTime, 23)<CONVERT(varchar, GETDATE(), 23)and Urgency in('緊急故障','重要故障'))) " +   //
            " union all " +   //
            " Select Case_ID,C_ID2,SetupTime,Email_Flag,a.BUSINESSNAME,Urgency,a.Type,Handle_Agent FROM CaseData a " +   //,DATEPART(WEEKDAY, SetupTime-1)
            " left join BusinessData b on a.PID=b.PID " +   //
            " where a.Type in('未到點','處理中')and Email_Flag=0 and Urgency not in('','其他') And " +   //
            " (  " +   //
            " (DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency not in('緊急故障','重要故障')and Saturday_Work=0 and Sunday_Work=1) " +   //週四 非緊急 六休日上
            " or (DATEPART(WEEKDAY, SetupTime-1)=5 and Saturday_Work=0 and Sunday_Work=1) " +   //週五 六休日上
            " or (DATEPART(WEEKDAY, SetupTime-1)=5 and Urgency not in('緊急故障','重要故障')and (Saturday_Work=1 and Sunday_Work=0) ) " +   //週五 非緊急 日休六上
            " or (DATEPART(WEEKDAY, SetupTime-1)=6 and ((Saturday_Work=1 and Sunday_Work=0)or(Saturday_Work=0 and Sunday_Work=1)) ) " +   //週六 休一天
            " or (DATEPART(WEEKDAY, SetupTime-1)=7 and Sunday_Work=0 ) " +   //週日 日休
            " ) and " +   //
            " (CONVERT(varchar, SetupTime+2, 23)<CONVERT(varchar, GETDATE(), 23) or  " +   //
            " (CONVERT(varchar, SetupTime+1, 23)<CONVERT(varchar, GETDATE(), 23)and Urgency in('緊急故障','重要故障') ) ) "+   //
            " union all " +   //
            " Select Case_ID,C_ID2,SetupTime,Email_Flag,a.BUSINESSNAME,Urgency,a.Type,Handle_Agent FROM CaseData a "+   //,DATEPART(WEEKDAY, SetupTime-1)
            " left join BusinessData b on a.PID=b.PID "+   //
            " where a.Type in('未到點','處理中')and Email_Flag=0 and Urgency not in('','其他') And " +   //
            " ( " +   //
            " (DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency not in('緊急故障','重要故障')and Saturday_Work=0 and Sunday_Work=0) " +   //週四 非緊急 周末休
            " or (DATEPART(WEEKDAY, SetupTime-1)=5 and Saturday_Work=0 and Sunday_Work=0) " +   //週五 非緊急 周末休
            " or (DATEPART(WEEKDAY, SetupTime-1)=6 and Saturday_Work=0 and Sunday_Work=0) " +   //週六 非緊急 周末休
            " ) and " +   //
            " (CONVERT(varchar, SetupTime+3, 23)<CONVERT(varchar, GETDATE(), 23) or  " +   //
            " (CONVERT(varchar, SetupTime+2, 23)<CONVERT(varchar, GETDATE(), 23)and Urgency in('緊急故障','重要故障') ) ) "+// 
            " Order by C_ID2  ";
            /*
Select Case_ID,C_ID2,SetupTime,Email_Flag,a.BUSINESSNAME,Urgency,a.Type,Handle_Agent,DATEPART(WEEKDAY, SetupTime-1) FROM CaseData a
left join BusinessData b on a.PID=b.PID
where a.Type in('未到點','處理中')and Email_Flag=0 and 
( 
(Saturday_Work=1 and b.Sunday_Work=1) --周末正常上班
or DATEPART(WEEKDAY, SetupTime-1)<4 --周一~三 
or (DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency in('緊急故障','重要故障')) --週四 緊急
or (DATEPART(WEEKDAY, SetupTime-1)=4 and Saturday_Work=1) --週四 六上班
or (DATEPART(WEEKDAY, SetupTime-1)=5 and Urgency in('緊急故障','重要故障')and Saturday_Work=1) --週五 緊急 六上班
or (DATEPART(WEEKDAY, SetupTime-1)=7 and Sunday_Work=1) --週日 日上班
)
and
(CONVERT(varchar, SetupTime+1, 23)<CONVERT(varchar, GETDATE()-0, 23) or 
(CONVERT(varchar, SetupTime, 23)<CONVERT(varchar, GETDATE()-0, 23)and Urgency in('緊急故障','重要故障'))) 
--order by C_ID2 
union all
Select Case_ID,C_ID2,SetupTime,Email_Flag,a.BUSINESSNAME,Urgency,a.Type,Handle_Agent,DATEPART(WEEKDAY, SetupTime-1) FROM CaseData a
left join BusinessData b on a.PID=b.PID
where a.Type in('未到點','處理中')and Email_Flag=0 and 
( 
(DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency not in('緊急故障','重要故障')and Saturday_Work=0 and Sunday_Work=1) --週四 非緊急 六休日上
or (DATEPART(WEEKDAY, SetupTime-1)=5 and Saturday_Work=0 and Sunday_Work=1) --週五 六休日上
or (DATEPART(WEEKDAY, SetupTime-1)=5 and Urgency not in('緊急故障','重要故障')and (Saturday_Work=1 and Sunday_Work=0) ) --週五 非緊急 日休六上
or (DATEPART(WEEKDAY, SetupTime-1)=6 and ((Saturday_Work=1 and Sunday_Work=0)or(Saturday_Work=0 and Sunday_Work=1)) ) --週六 休一天
or (DATEPART(WEEKDAY, SetupTime-1)=7 and Sunday_Work=0 ) --週日 日休
)
and
(CONVERT(varchar, SetupTime+2, 23)<CONVERT(varchar, GETDATE()-0, 23) or 
(CONVERT(varchar, SetupTime+1, 23)<CONVERT(varchar, GETDATE()-0, 23)and Urgency in('緊急故障','重要故障') ) )
--order by C_ID2 
union all
Select Case_ID,C_ID2,SetupTime,Email_Flag,a.BUSINESSNAME,Urgency,a.Type,Handle_Agent,DATEPART(WEEKDAY, SetupTime-1) FROM CaseData a
left join BusinessData b on a.PID=b.PID
where a.Type in('未到點','處理中')and Email_Flag=0 and 
( 
(DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency not in('緊急故障','重要故障')and Saturday_Work=0 and Sunday_Work=0) --週四 非緊急 周末休
or (DATEPART(WEEKDAY, SetupTime-1)=5 and Saturday_Work=0 and Sunday_Work=0) --週五 非緊急 周末休
or (DATEPART(WEEKDAY, SetupTime-1)=6 and Saturday_Work=0 and Sunday_Work=0) --週六 非緊急 周末休
)
and
(CONVERT(varchar, SetupTime+3, 23)<CONVERT(varchar, GETDATE()-0, 23) or 
(CONVERT(varchar, SetupTime+2, 23)<CONVERT(varchar, GETDATE()-0, 23)and Urgency in('緊急故障','重要故障') ) )
order by C_ID2 

             */
            var list = DBTool.Query<ClassTemplate>(sqlstr);
            if (list.Count() > 0)   //找逾期案件
            {
                foreach (var q in list)
                {
                    //logger.Info("編號:" + q.C_ID2 + "  名稱:" + q.BUSINESSNAME + "  日期:" + q.SetupTime + "  緊急程度:" + q.Urgency + "  工程師:" + q.Handle_Agent);
                    EMMA_URL = EMMA + "CheckLogin.aspx?seqno=" + q.Case_ID + "&page=4";

                    str_AssignNo = str_AssignNo + "【編號:" + q.C_ID2 + " 客戶:" + q.BUSINESSNAME + " 緊急程度:" + q.Urgency + " 處理狀態:" + q.Type + " 工程師:" + q.Handle_Agent + "】" + (char)10;
                    str_URLS = str_URLS + EMMA_URL + (char)10;
                }   //foreach CaseData
                logger.Info("逾期案件 "+str_AssignNo);
                logger.Info("案件連結 "+str_URLS);
                
                Sqlstr = @"Select Agent_Mail From DispatchSystem Where Agent_Status='在職'and " +
                        " Agent_Name in('張文信','簡子翔') ";
                //" (Agent_Company='MA_Sales' or(Agent_Company='Engineer'and Agent_Team='Leader')or "+
                //" (Agent_Company='Engineer'and Agent_Team='Supervisor')) ";
                var list2 = DBTool.Query<ClassTemplate>(Sqlstr);
                if (list2.Any())    //讀取收信者信箱
                {
                    foreach (var r in list2)
                    {
                        if (!string.IsNullOrEmpty(r.Agent_Mail))//有填寫信箱就發信
                        {
                            logger.Info("Email = " + r.Agent_Mail);
                            Sql = @"INSERT INTO tblAssign (AssignNo, E_MAIL, ConnURL) VALUES(@AssignNo ,@E_MAIL ,@ConnURL) ";
                            ClassTemplate emma = new ClassTemplate()    //發信並更新 Email_Flag
                            {
                                AssignNo = str_AssignNo,    //
                                ConnURL = str_URLS,
                                E_MAIL = r.Agent_Mail,
                            };
                            using (IDbConnection db = DBTool.GetConn())
                            {
                                db.Execute(Sql, emma);
                                db.Close();
                            }   //*/
                        }
                    }
                    logger.Info("發信結束 開始改發信標記");
                    foreach (var q in list)
                    {
                        logger.Info("改 CaseData" + q.Case_ID);
                        Sql = @"UPDATE CaseData SET Email_Flag = '1' WHERE Case_ID = @Case_ID ";
                        using (IDbConnection db = DBTool.GetConn())
                            db.Execute(Sql, new { Case_ID = q.Case_ID });    //*/
                    }
                }
                else{
                logger.Info("清除無主管信箱時 未通知的的服務單");   //預定日七天後清除 
                ClassTemplate template = new ClassTemplate() { };     //沒收信者時 七天清除未通知信件
                Sql02 = @"UPDATE CaseData SET Email_Flag = '2' WHERE Type in('未到點','處理中') and Email_Flag = '0' " +
                    " and CONVERT(varchar, SetupTime+7, 23)<CONVERT(varchar, GETDATE(), 23)  ";
                using (IDbConnection db = DBTool.GetConn())
                {
                    db.Execute(Sql02, template);
                    db.Close();
                };  //*/
                }                    
                context.Response.Write(JsonConvert.SerializeObject(new
                {
                    status = "success",
                    msg = string.Empty
                        + "服務單共 " + list.Count() + " 筆超時未結案 尚未通知"
                }));
            }
            else
            {
                logger.Info("沒有逾期且未通知的服務單");
                context.Response.Write(JsonConvert.SerializeObject(new
                {
                    status = "finish",
                    msg = string.Empty
                        + "沒有逾期且未通知的服務單"
                }));
            }

            //============================保固維護通知
            if (date.Day <= 3)  //保固維護逾期通知
            //if (date.Day == 10)  //測試用
            {
                //維護時間逾期通知開始
                EMMA_URL = EMMA + "0060010001.aspx";

                sqlstr = @"SELECT * FROM BusinessData WHERE Prot_Time<getdate()+92 " +//92
                    " AND Prot_Mail = '0' order by Prot_Time ";
                var list3 = DBTool.Query<ClassTemplate>(sqlstr);
                if (list3.Count() > 0)
                {
                    foreach (var s in list3)
                    {
                        logger.Info("編號:" + s.PID + "  名稱:" + s.BUSINESSNAME + "  維護日期:" + s.Prot_Time);

                        Sqlstr = @"SELECT Top 3 * FROM DispatchSystem WHERE Agent_Status='在職' And  " +
                            //" Agent_ID in('0002','0003','0004') "; // 192.168.2.170 用
                            //" Agent_Name in('測試用員工','系統管理員','德瑪測試') "; // 德 測試
                        " Agent_Name in('陳仁杰','陳沁妍') ";    //德瑪用    '簡子翔',
                        var list4 = DBTool.Query<ClassTemplate>(Sqlstr);
                        if (list4.Any())
                        {
                            foreach (var t in list4)
                            {
                                if (!string.IsNullOrEmpty(t.Agent_Mail))//有填寫信箱就發信
                                {
                                    logger.Info("Email = " + t.Agent_Mail + "  Case_ID=" + s.C_ID2);
                                    Sql = @"INSERT INTO tblAssign (AssignNo, E_MAIL, ConnURL) VALUES(@AssignNo ,@E_MAIL ,@ConnURL) " +
                                        " UPDATE BusinessData SET Prot_Mail = '1' where PID=@PID ";
                                    ClassTemplate emma = new ClassTemplate()
                                    {
                                        AssignNo = "【維護到期通知】【編號：" + s.PID + "  的顧客：" + s.BUSINESSNAME + "  維護將於 " + s.Prot_Time + " 到期】",
                                        ConnURL = EMMA_URL,
                                        E_MAIL = t.Agent_Mail,
                                        PID = s.PID
                                    };
                                    using (IDbConnection db = DBTool.GetConn())
                                    {
                                        db.Execute(Sql, emma);
                                        db.Close();
                                    }
                                }
                            }
                        }
                    }   //foreach CaseData
                    context.Response.Write(JsonConvert.SerializeObject(new
                    {
                        status = "success",
                        msg = string.Empty
                            + "共 " + list3.Count() + " 筆即將到期的維護客戶尚未通知"
                    }));
                }
                else
                {
                    logger.Info("沒有即將到期的維護客戶");
                    context.Response.Write(JsonConvert.SerializeObject(new
                    {
                        status = "finish",
                        msg = string.Empty
                            + "沒有即將到期的維護客戶"
                    }));
                }   //維護通知程式結束

                //保固時間逾期通知開始
                sqlstr = @"SELECT * FROM BusinessData WHERE Warr_Time<getdate()+92 " +
                    " AND (Prot_Time is null or Prot_Time<Warr_Time) " +
                    " AND Warr_Mail = '0' order by Warr_Time ";
                var list5 = DBTool.Query<ClassTemplate>(sqlstr);
                if (list5.Count() > 0)
                {
                    foreach (var u in list5)
                    {
                        logger.Info("編號:" + u.PID + "  名稱:" + u.BUSINESSNAME + "  保固日期:" + u.Warr_Time);

                        Sqlstr = @"SELECT Top 3 * FROM DispatchSystem WHERE Agent_Status='在職' And  " +
                            //" Agent_ID in('0002','0003','0004') "; // 192.168.2.170 用
                            //" Agent_Name in('測試用員工','系統管理員','德瑪測試') "; // 德 測試
                        " Agent_Name in('陳仁杰','陳沁妍') ";    //德瑪用  '簡子翔',
                        var list6 = DBTool.Query<ClassTemplate>(Sqlstr);
                        if (list6.Any())
                        {
                            foreach (var v in list6)
                            {
                                if (!string.IsNullOrEmpty(v.Agent_Mail))//有填寫信箱就發信
                                {
                                    logger.Info("Email = " + v.Agent_Mail + "  Case_ID=" + u.C_ID2);
                                    Sql = @"INSERT INTO tblAssign (AssignNo, E_MAIL, ConnURL) VALUES(@AssignNo ,@E_MAIL ,@ConnURL) " +
                                        " UPDATE BusinessData SET Warr_Mail = '1' Where PID=@PID ";
                                    ClassTemplate emma = new ClassTemplate()
                                    {
                                        AssignNo = "【保固到期通知】【編號：" + u.PID + "  的顧客：" + u.BUSINESSNAME + "  保固將於 " + u.Warr_Time + " 到期】",
                                        ConnURL = EMMA_URL,
                                        E_MAIL = v.Agent_Mail,
                                        PID = u.PID
                                    };
                                    using (IDbConnection db = DBTool.GetConn())
                                    {
                                        db.Execute(Sql, emma);
                                        db.Close();
                                    }
                                }
                            }
                        }
                    }   //foreach CaseData
                    context.Response.Write(JsonConvert.SerializeObject(new
                    {
                        status = "success",
                        msg = string.Empty
                            + "共 " + list5.Count() + " 筆即將到期的保固客戶尚未通知"
                    }));
                }
                else
                {
                    logger.Info("沒有即將到期的保固客戶");
                    context.Response.Write(JsonConvert.SerializeObject(new
                    {
                        status = "finish",
                        msg = string.Empty
                            + "沒有即將到期的保固客戶"
                    }));
                }
            }//保固及維護通知程式結束

            //=========================維護任務日期檢查
            logger.Info("維護任務日期檢查");
            //維護產單程式每月 25 0810 開始執行直到一號 要在24 就改好狀態
            sqlstr = @"SELECT a.PID FROM InSpecation_Dimax.dbo.Mission_Title a left join BusinessData b on a.PID=b.PID " +
                    " Where Flag='1' and " +
                    " (Warr_Time is null or Warr_Time<getdate() or (Year(Warr_Time)=Year(getdate())and Month(Warr_Time)=Month(getdate())) ) and " +
                    " (Prot_Time is null or Prot_Time<getdate() or (Year(Prot_Time)=Year(getdate())and Month(Prot_Time)=Month(getdate())) ) " +
                    " Group by a.PID order by PID ";
            var list7 = DBTool.Query<ClassTemplate>(sqlstr);
            if (list7.Count() > 0)
            {
                foreach (var w in list7)
                {
                    sql = @"Update InSpecation_Dimax.dbo.Mission_Title Set Flag='0' Where PID=@PID ";
                    var list8 = DBTool.Query<ClassTemplate>(sql, new { PID=w.PID});
                }
                logger.Info("共 " + list7.Count() + " 筆客戶的維護產單中止");
            }
            //*/
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