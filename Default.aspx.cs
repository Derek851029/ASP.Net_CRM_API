using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using log4net.Config;

public partial class _Default : System.Web.UI.Page
{
    protected string str_time = "";
    protected string type = "";
    protected string Team = "";
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {

        string Agent_ID = (string)(Session["UserIDNO"]);
        string UserIDNAME = (string)(Session["UserIDNAME"]);
        string UserID = (string)(Session["UserID"]);
        string Agent_LV = (string)(Session["Agent_LV"]);
        Session["Time_Flag"] = "0";
        Team = (string)(Session["Agent_Team"]);
        if (Agent_LV == "20" || Agent_LV == "30")        {        }
        else        {
            //table_data.Style.Add("display", "none");    //一般員工 隱藏職代表
        }
        //string sql_txt = @"SELECT TOP 1 * FROM CaseData WHERE Agent_ID=@Agent_ID and Type in('未到點','處理中') and "+
        //    " (CONVERT(varchar, SetupTime+1, 23)<CONVERT(varchar, GETDATE(), 23) "+
        //    " or (CONVERT(varchar, SetupTime, 23)<CONVERT(varchar, GETDATE(), 23)and Urgency in('緊急故障','重要故障')) )";
        //var chk = DBTool.Query<ClassTemplate>(sql_txt, new { Agent_ID = Agent_ID });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Check_Prot_Time()  //停用 改用 Daily_Email.ashx
    {
        DateTime Today = DateTime.Now;
        DateTime CheckDate = DateTime.Now.AddDays(60);
        string EMMA = System.Web.Configuration.WebConfigurationManager.AppSettings["EMMA"].ToString();
        string EMMA_URL;
        string sql;
        string sql2;
        string sqlstr; 
        string PID;
        string Name;
        string Prot_Time;
        string mail = "";

        for (int i = 0; i < 5; i++) //維護終止日檢查  
        {            
            #region 維護終止60天通知    

            EMMA_URL = EMMA + "0060010001.aspx";
            sql = @"SELECT Top 1 Prot_Time as Time_01,* FROM BusinessData WHERE Prot_Time < @CheckDate AND Prot_Mail = '0' order by Prot_Time ";    //找維護日剩不到60天的
            var list = DBTool.Query<ClassTemplate>(sql, new { CheckDate = CheckDate });
            if (list.Count() > 0)
            {
                ClassTemplate schedule = list.First();  
                PID = schedule.PID;
                Name = schedule.BUSINESSNAME;
                Prot_Time = schedule.Time_01.ToString("yyyy/MM/dd");

                sql2 = @"SELECT Top 3 * FROM DispatchSystem WHERE Agent_Status='在職' And " +
                    " Agent_ID in('0002','0003','0004') "; // 192.168.2.170 用
                //" Agent_Name in('測試用員工','系統管理員','德瑪測試') "; // 德 測試
                    //" Agent_Name in('陳仁杰','簡子翔','陳沁妍') ";    //德瑪用
                var list2 = DBTool.Query<ClassTemplate>(sql2);
                if (list2.Any())
                {
                    foreach (var q in list2)
                    {
                        sqlstr = @"INSERT INTO tblAssign (AssignNo, E_MAIL, ConnURL) VALUES(@AssignNo ,@E_MAIL ,@ConnURL)"+ //發信給 客服
                            " UPDATE BusinessData SET Prot_Mail = '1' where PID=@PID ";
                        DBTool.Query<ClassTemplate>(sqlstr, new
                        {
                            AssignNo = "【維護到期通知】【編號：" + PID + "  的顧客：" + Name + "  維護將於 " + Prot_Time + " 到期】",
                            ConnURL = EMMA_URL,
                            E_MAIL = q.Agent_Mail,
                            PID = PID
                        });
                    }
                    //sqlstr = @"UPDATE BusinessData SET Prot_Mail = '1' where PID=@PID ";
                    //var a = DBTool.Query<ClassTemplate>(sqlstr, new { PID = PID });
                }
                //sql2 = @"SELECT Top 1 * FROM DispatchSystem WHERE Agent_Company='客服' and Agent_Name !='值班員' ";   //指定 值班員以外 客服
                /*sql2 = @"SELECT Top 1 * FROM DispatchSystem WHERE Agent_Company='MA_Sales' and Agent_Name ='林怡秀' ";   //指定 林怡秀
                list2 = DBTool.Query<ClassTemplate>(sql2);
                if (list.Any())
                {
                    ClassTemplate schedule2 = list2.First();  // 功能???            
                    mail = schedule2.Agent_Mail;

                    sqlstr = @"INSERT INTO tblAssign (AssignNo, E_MAIL, ConnURL) VALUES(@AssignNo ,@E_MAIL ,@ConnURL)"; //發信給 MA_Sales
                    DBTool.Query<ClassTemplate>(sqlstr, new
                    {
                        AssignNo = "【維護到期通知】【編號：" + PID + "  的顧客：" + Name + "  維護將於 " + Prot_Time + " 到期】",
                        ConnURL = EMMA_URL,
                        E_MAIL = mail
                    });
                }   //*/
                
                //return JsonConvert.SerializeObject(new { status = "A=" + PID + "  B=" + Name + "  C=" + Prot_Time + " D=" + mail });
            }
            #endregion  //*/
        }
        return JsonConvert.SerializeObject(new { status = CheckDate.ToString("yyyy/MM/dd") });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Alert_Table_List()       //案件列表程式
    {
        //Check();
        string Agent_ID = HttpContext.Current.Session["UserID"].ToString();
        //string Agent_LV = HttpContext.Current.Session["Agent_LV"].ToString();
        //string Agent_Company = HttpContext.Current.Session["Agent_Company"].ToString(); //部門
        //string Agent_Team = HttpContext.Current.Session["Agent_Team"].ToString();               //職位
        //string view = HttpContext.Current.Session["view"].ToString();

        string sqlstr2 = @" Select  a.* FROM CaseData a left join BusinessData b on a.PID=b.PID " +
            " Where Agent_ID=@Agent_ID and a.Type in('未到點','處理中') and Urgency not in('','其他') And " +//
            //" (CONVERT(varchar, SetupTime+1, 23)<CONVERT(varchar, GETDATE(), 23) " +    //'緊急故障','重要故障'隔天 其它兩天
            //" or (CONVERT(varchar, SetupTime, 23)<CONVERT(varchar, GETDATE(), 23)and Urgency in('緊急故障','重要故障')) ) "+
            //" Order by SetupTime desc";      
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
            " Select a.*  FROM CaseData a " +   //,DATEPART(WEEKDAY, SetupTime-1)
            " left join BusinessData b on a.PID=b.PID " +   //
            " where Agent_ID=@Agent_ID and a.Type in('未到點','處理中')and Urgency not in('','其他') And " +   //
            " (  " +   //
            " (DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency not in('緊急故障','重要故障')and Saturday_Work=0 and Sunday_Work=1) " +   //週四 非緊急 六休日上
            " or (DATEPART(WEEKDAY, SetupTime-1)=5 and Saturday_Work=0 and Sunday_Work=1) " +   //週五 六休日上
            " or (DATEPART(WEEKDAY, SetupTime-1)=5 and Urgency not in('緊急故障','重要故障')and (Saturday_Work=1 and Sunday_Work=0) ) " +   //週五 非緊急 日休六上
            " or (DATEPART(WEEKDAY, SetupTime-1)=6 and ((Saturday_Work=1 and Sunday_Work=0)or(Saturday_Work=0 and Sunday_Work=1)) ) " +   //週六 休一天
            " or (DATEPART(WEEKDAY, SetupTime-1)=7 and Sunday_Work=0 ) " +   //週日 日休
            " ) and " +   //
            " (CONVERT(varchar, SetupTime+2, 23)<CONVERT(varchar, GETDATE(), 23) or  " +   //
            " (CONVERT(varchar, SetupTime+1, 23)<CONVERT(varchar, GETDATE(), 23)and Urgency in('緊急故障','重要故障') ) ) " +   //
            " union all " +   //
            " Select a.*  FROM CaseData a " +   //,DATEPART(WEEKDAY, SetupTime-1)
            " left join BusinessData b on a.PID=b.PID " +   //
            " where Agent_ID=@Agent_ID and a.Type in('未到點','處理中')and Urgency not in('','其他') And " +   //
            " ( " +   //
            " (DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency not in('緊急故障','重要故障')and Saturday_Work=0 and Sunday_Work=0) " +   //週四 非緊急 周末休
            " or (DATEPART(WEEKDAY, SetupTime-1)=5 and Saturday_Work=0 and Sunday_Work=0) " +   //週五 非緊急 周末休
            " or (DATEPART(WEEKDAY, SetupTime-1)=6 and Saturday_Work=0 and Sunday_Work=0) " +   //週六 非緊急 周末休
            " ) and " +   //
            " (CONVERT(varchar, SetupTime+3, 23)<CONVERT(varchar, GETDATE(), 23) or  " +   //
            " (CONVERT(varchar, SetupTime+2, 23)<CONVERT(varchar, GETDATE(), 23)and Urgency in('緊急故障','重要故障') ) ) " +// 
            " Order by SetupTime "; //*/
        var a = DBTool.Query<T_Default>(sqlstr2, new
        {
            Agent_ID = Agent_ID
        });
        var b = a.ToList().Select(p => new
        {
            SetupTime = p.SetupTime.ToString("yyyy/MM/dd HH:mm"),
            UploadTime = p.UploadTime.ToString("MM/dd HH:mm"),
            Case_ID = p.Case_ID,
            C_ID2 = Value4(p.C_ID2, p.Case_ID),
            BUSINESSNAME = p.BUSINESSNAME,
            Urgency = p.Urgency,
            OpinionType = p.OpinionType,
            Handle_Agent = p.Handle_Agent,
            //Type = p.Type,
            Type = p.Type.Replace("處理中", "未完工"),
        });
        return JsonConvert.SerializeObject(b);
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string GetClassGroup()
    {
        string Time_Flag = "0";
        try
        {
            Time_Flag = HttpContext.Current.Session["Time_Flag"].ToString();
        }
        catch
        {
            Time_Flag = "0";
        }
        string Agent_Team = HttpContext.Current.Session["Agent_Team"].ToString();
        string Agent_LV = HttpContext.Current.Session["Agent_LV"].ToString();
        string Agent_ID = HttpContext.Current.Session["UserID"].ToString();
        return JsonConvert.SerializeObject(ClassScheduleRepository.Default_Table(Agent_Team, Agent_LV, Agent_ID, Time_Flag));
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Flag(string Team, string Flag, string Agent_LV, string Agent_ID)
    {
        if (Flag == "6" || Flag == "8")
        {
            var a = ClassScheduleRepository.Default_Flag(Team, Flag, Agent_LV, Agent_ID)
                   .Select(p => new
                   {
                       SYS_ID = p.SYSID,
                       MNo = p.CNo,
                       Time_01 = p.StartTime.ToString("MM/dd HH:mm"),
                       Type = p.Type,
                       ServiceName = p.ServiceName,
                       Labor_CName = p.Danger,
                       Question = p.Answer,
                       Create_Name = p.Agent_Name
                   });
            return JsonConvert.SerializeObject(a, Formatting.Indented);
        }
        else
        {
            var a = ClassScheduleRepository.Default_Flag(Team, Flag, Agent_LV, Agent_ID)
                   .Select(p => new
                   {
                       SYS_ID = p.SYS_ID,
                       MNo = p.MNo,
                       Time_01 = p.Time_01.ToString("MM/dd HH:mm"),
                       Type = p.Type,
                       ServiceName = p.ServiceName,
                       Labor_CName = p.Labor_CName,
                       Question = p.Question,
                       Create_Name = p.Cust_Name
                   });
            return JsonConvert.SerializeObject(a, Formatting.Indented);
        }
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string List_Record()
    {
        string Agent_LV = HttpContext.Current.Session["Agent_LV"].ToString();
        string Agent_Name = HttpContext.Current.Session["UserIDNAME"].ToString();
        string Sqlstr = @"SELECT b.Agent_Name, a.*, End_Day as time1 FROM Daily_Itinerary as a " +
  "left join  DispatchSystem as b on a.Agent_ID=b.Agent_ID " +
  "where Job_Agent = @Agent_Name and Day > GETDATE()-8 "; 

        //string Sqlstr = @"SELECT * " +
        //    "FROM Daily_Itinerary  WHERE Job_Agent=@Agent_Name ";   //         '楊少華' 
        var a = DBTool.Query<ClassTemplate>(Sqlstr, new {Agent_Name = Agent_Name  }).ToList().Select(p => new   //  
        {
            A = Value(p.DI_No),
            //B = Convert.ToDateTime(p.SetupTime).ToString("yyyy/MM/dd HH:mm"),
            B = Value(p.Agent_Name),
            C = Value3(p.Day),
            C2 = Value3(p.time1),
            D = Value(p.Schedule),
            E = Value2(p.S_UpdateTime),
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }
    //============= 接受職代 =============
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Accept(string DI_No, string Type)
    {
        string Schedule = "休假";
        if (Type == "1") Schedule = "休上午";
        else if (Type == "2") Schedule = "休下午";

        string sqlstr = @"Update Daily_Itinerary Set " +
                " Schedule=@Schedule, S_UpdateTime = @S_UpdateTime   " +
                " where DI_No = @DI_No ";      // 原始的不會 Group    CONVERT(varchar(100), Upload_Time, 111) 要整串放進 Group 中
                var chk = DBTool.Query<ClassTemplate>(sqlstr, new
                {
                    DI_No = DI_No,
                    Schedule = Schedule,
                    S_UpdateTime = DateTime.Now,  
                });
        return JsonConvert.SerializeObject(new { status = "success" });
    }
    //============= 拒絕職代 =============
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Reject(string DI_No)
    {
        string sqlstr = @"update Daily_Itinerary set " +
                " Schedule = '代理被拒', S_UpdateTime = @S_UpdateTime   " +
                " where DI_No = @DI_No ";      // 原始的不會 Group    CONVERT(varchar(100), Upload_Time, 111) 要整串放進 Group 中
        var chk = DBTool.Query<ClassTemplate>(sqlstr, new
        {
            DI_No = DI_No,
            S_UpdateTime = DateTime.Now,
        });
        return JsonConvert.SerializeObject(new { status = "success" });
    }

    public static string DealingProcess(string value)
    {
        if (value == "0")
        {
            return "結案";
        }
        else
        {
            return "處理中";
        }
    }
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string URL()
    {
        //Check();
        string str_url = "../0030010000/0030010006.aspx";         //轉跳補打卡頁面
        return JsonConvert.SerializeObject(new { status = str_url, type = "ok" });
    }

    public static string Value(string value)        // 當值為null時跳過  非 null 時去除後方空白
    {
        if (!string.IsNullOrEmpty(value))
        {
            value = value.Trim();
        }
        return value;
    }
    public static string Value2(string value)        // 當值為null時跳過  非 null 時改時間格式
    {
        if (!string.IsNullOrEmpty(value))
        {
            value = DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm");
        }
        return value;
    }
    public static string Value3(string value)        // 當值為null時跳過  非 null 時改時間格式
    {
        if (!string.IsNullOrEmpty(value))
        {
            value = DateTime.Parse(value).ToString("yyyy-MM-dd");
        }
        return value;
    }
    public static string Value4(string value, string value2)        // 當value值為""  非 value=value2
    {
        if (string.IsNullOrEmpty(value))
        {
            value = value2.Trim();
        }
        else
            value = value.Trim();
        return value;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string List_Schedule()
    {
        string Agent_SYSID = HttpContext.Current.Session["SYSID"].ToString();
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string tomorrow = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

        string sqlstr = @"SELECT a.*,b.Case_Name FROM Visit_Data a left join Case_List b on a.Case_SYSID = b.SYSID WHERE Create_Agent = '{0}' AND Visit_Date > '{1}' AND Visit_Date < '{2}' order by Visit_Date desc";
        string sql_format = string.Format(sqlstr, Agent_SYSID,today,tomorrow);
        var a = DBTool.Query<Calendar>(sql_format).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Case_Name = p.Case_Name,
            Case_SYSID = p.Case_SYSID,
            Visit_Customer = p.Visit_Customer,
            Visit_Content = p.Visit_Content,
            Visit_Date = p.Visit_Date,
        });
        return JsonConvert.SerializeObject(a);
    }
    public class Calendar
    {
        public string type { get; set; }
        public DateTime start { get; set; }
        public DateTime end_visit { get; set; }
        public string title { get; set; }
        public string id { get; set; }
        public string Agent_Name { get; set; }
        public string SYSID { get; set; }
        public string Case_SYSID { get; set; }
        public string Case_Name { get; set; }
        public string Visit_Customer { get; set; }
        public string Visit_Customer_ID { get; set; }
        public string Visit_Person { get; set; }
        public string Create_Agent { get; set; }
        public string Visit_Phone { get; set; }
        public string Visit_Content { get; set; }
        public string Create_Date { get; set; }
        public string Visit_Date { get; set; }
        public DateTime Visit_Leave_Date { get; set; }
        public string Status { get; set; }
    }
    public class T_Default
    {
        public string title { get; set; }
        public string end { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string Case_ID { get; set; }
        public string ReplyType { get; set; }
        public string OpinionSubject { get; set; }
        public string Type { get; set; }
        public string Urgency { get; set; }
        public string ID { get; set; }
        public string Cust_Name { get; set; }
        public DateTime start { get; set; }
        public DateTime Upload_Time { get; set; }
        public DateTime EstimatedFinishTime { get; set; }
        public DateTime SetupTime { get; set; }
        public DateTime UploadTime { get; set; }
        public string BUSINESSNAME { get; set; }
        public string OpinionType { get; set; }
        public string Handle_Agent { get; set; }
        public string C_ID2 { get; set; }
    }
}