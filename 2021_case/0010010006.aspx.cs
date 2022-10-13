using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _2021_case_0010010006 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Session.Clear();
        //Session.RemoveAll();
        //Session.Abandon();

    }
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Load()
    {
        string Agent_ID;
        Agent_ID = HttpContext.Current.Session["UserID"].ToString();
        if (!string.IsNullOrEmpty(Agent_ID)) //如果不等於空清除空白處, 空的就空字串
        {
            Agent_ID = Agent_ID.Trim();
        }
        else
        {
            Agent_ID = "";
        }
        string Sqlstr = @"SELECT TOP 1 * FROM DispatchSystem WHERE Agent_ID = @Agent_ID AND Agent_Status != '離職'";      // 員工名單內且未離職
        var a = DBTool.Query<ClassTemplate>(Sqlstr, new { Agent_ID = Agent_ID }).ToList().Select(p => new
        {
            Agent_Team = p.Agent_Team,
            Agent_Name = p.Agent_Name,
            Agent_ID = p.Agent_ID,
            SYSID = p.SYSID,
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string renderCalendar(DateTime start, DateTime end)
    {
        string Agent_SYSID = HttpContext.Current.Session["SYSID"].ToString();

        string sqlstr = @"SELECT a.SYSID as id, a.Status as type, a.Visit_Date as start,a.Visit_Leave_Date as end_visit, b.Case_Name as title,c.Agent_Name FROM Visit_Data a" +
                        " left join Case_List b on a.Case_SYSID = b.SYSID left join DispatchSystem c on a.Create_Agent = c.SYSID WHERE a.Create_Agent = N'{0}' order by Visit_Date desc";
        string sql_format = string.Format(sqlstr, Agent_SYSID);
        var a = DBTool.Query<Calendar>(sql_format).ToList().Select(p => new
        {
            id = p.id,
            type = p.type,
            start = p.start,
            end_visit = p.end_visit,
            title = p.title,
            Agent_Name = p.Agent_Name
        });
        return JsonConvert.SerializeObject(a);
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string bindtalbe(string SYSID)
    {
        string Agent_SYSID = HttpContext.Current.Session["SYSID"].ToString();
        string sqlstr = @"SELECT Top 1 a.*,b.Case_Name,c.Agent_Name FROM Visit_Data a left join Case_List b on b.SYSID = a.Case_SYSID left join DispatchSystem c on 
        a.Create_Agent = c.SYSID  WHERE a.SYSID = N'{0}' AND a.Create_Agent = N'{1}'";
        string sql_format = string.Format(sqlstr, SYSID, Agent_SYSID);
        var a = DBTool.Query<Calendar>(sql_format).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Case_SYSID = p.Case_SYSID,
            Case_Name = p.Case_Name,
            Visit_Person = p.Visit_Person,
            Visit_Phone = p.Visit_Phone,
            Visit_Content = p.Visit_Content,
            Agent_Name = p.Agent_Name,
            Create_Date = p.Create_Date,
            Visit_Date = p.Visit_Date,
            Status = p.Status,
        });
        return JsonConvert.SerializeObject(a);
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Load_Data(string Visit_SYSID)
    {
        string sqlstr = @"SELECT a.*,c.Agent_Name FROM Visit_Data a left join DispatchSystem c on a.Create_Agent = c.SYSID WHERE a.SYSID = '{0}'";
        string sql_format = string.Format(sqlstr, Visit_SYSID);
        var a = DBTool.Query<Calendar>(sql_format).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Case_SYSID = p.Case_SYSID,
            Case_Name = p.Case_Name,
            Visit_Customer = p.Visit_Customer,
            Visit_Customer_ID = p.Visit_Customer_ID,
            Visit_Person = p.Visit_Person,
            Visit_Phone = p.Visit_Phone,
            Visit_Content = p.Visit_Content,
            Agent_Name = p.Agent_Name,
            Visit_Date = p.Visit_Date,
            Visit_Leave_Date = p.Visit_Leave_Date,
        });
        return JsonConvert.SerializeObject(a);
    }

    [WebMethod(EnableSession = true)]
    public static string Get_Case_Data()
    {
        var result = Case_List.Search("", "", "");
        return JsonConvert.SerializeObject(result);
    }

    [WebMethod(EnableSession = true)]
    public static string Safe_Visit(string Case, string Bussiness_Name, string Bussiness_ID, string Visit_Person, string Visit_Phone, string Visit_Date,
        string Visit_Leave_Date, string txt_Vistit_Content, string Create_Agent,string Peoloe_list,Boolean Notification_Line, Boolean Notification_App)
    {
        string sqlstr;
        string sql_format;
        string[] arr;
        sqlstr = @"INSERT INTO Visit_Data([Case_SYSID]
        ,[Visit_Customer]
        ,[Visit_Customer_ID]
        ,[Visit_Person]
        ,[Visit_Phone]
        ,[Visit_Content]
        ,[Create_Agent]
        ,[Visit_Date]
        ,[Visit_Leave_Date]
        ,[Status]) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";
        sql_format = string.Format(sqlstr, Case, Bussiness_Name, Bussiness_ID, Visit_Person, Visit_Phone, txt_Vistit_Content, Create_Agent, Visit_Date, Visit_Leave_Date, "0");
        DBTool.Query(sql_format);

        if(Peoloe_list != "")
        {
            sqlstr = @"INSERT INTO Visit_Data([Case_SYSID]         
          ,[Visit_Customer]
          ,[Visit_Customer_ID]
          ,[Visit_Person]
          ,[Visit_Phone]
          ,[Visit_Content]
          ,[Create_Agent]
          ,[Visit_Date]
          ,[Visit_Leave_Date]
          ,[Status]) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";/*same up*/
            arr = Peoloe_list.Split(',');
            foreach (var item in arr)
            {
                sql_format = string.Format(sqlstr, Case, Bussiness_Name, Bussiness_ID, Visit_Person, Visit_Phone, txt_Vistit_Content, item, Visit_Date, Visit_Leave_Date, "0");
                DBTool.Query(sql_format);
            }

            
        }
        if (Notification_Line == true)
        {

        }
        if (Notification_App == true)
        {

        }
        return JsonConvert.SerializeObject(new { status = "行程新增成功。" });
    }

    [WebMethod(EnableSession = true)]
    public static string Add_Log(string Case_SYSID,string Agent_Name, string Service_Context)
    {
        string Agent_SYSID = HttpContext.Current.Session["SYSID"].ToString();
        string sqlstr = @"INSERT INTO Case_Work_Log([Case_Owner]
      ,[Work_Name]
      ,[Work_Log]
      ,[Create_ID]
      ,[Create_Agent]) VALUES('{0}','{1}','{2}','{3}','{4}')";
        string sql_format = string.Format(sqlstr, Case_SYSID,"拜訪紀錄", Service_Context, Agent_SYSID, Agent_Name);
        DBTool.Query(sql_format);
        return JsonConvert.SerializeObject(new { status = "服務紀錄新增成功。" });
    }

    [WebMethod(EnableSession = true)]
    public static string Send_Notification_Line()
    {
        return JsonConvert.SerializeObject(new { status = "服務紀錄新增成功。" });
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
        public DateTime Visit_Date { get; set; }
        public DateTime Visit_Leave_Date { get; set; }
        public string Status { get; set; }
    }
}