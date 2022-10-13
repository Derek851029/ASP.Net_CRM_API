using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _2021_case_0030010099 : System.Web.UI.Page
{
    protected string seqno = "";
    protected string new_mno = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        seqno = Request.Params["seqno"];
        int s = 0;

        new_mno = DateTime.Now.ToString("yyyyMMddHHmmss"); //("yyMMddHHmmssfff");
        string Sqlstr = @"select count(*) as Flag FROM CaseData where CONVERT(varchar(100), SetupTime, 111)=CONVERT(varchar(100), getdate(), 111)";
        var a = DBTool.Query<ClassTemplate>(Sqlstr);
        foreach (var q in a)
        {
            s = Int32.Parse(q.Flag);
        };
        new_mno += "001" + s;

        Session["Case_ID"] = new_mno;
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
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string bindSelect_ListCase()
    {
        var caseList = Case_List.Search("","");
        return JsonConvert.SerializeObject(caseList);
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string List_people(string depart)
    {
        string sqlstr = @"SELECT SYSID,Agent_Name FROM DispatchSystem WHERE Agent_Team = @depart AND Agent_Status != '離職'";      // 員工名單內且未離職
        var a = DBTool.Query<ClassTemplate>(sqlstr, new { depart = depart }).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Agent_Name = p.Agent_Name,
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Add(string str_sysid, string Text1, string Create_date, string select_ListCase, string SelectUrgency,
        string End_date, string SelectOpinionType, string ReplyType, string Opinion_content, string A_ID, string yes_or_no, string Chose_depart,
        string Dispatch_Name,string type)
    {
        string sqlstr = string.Format(
            @"INSERT INTO Case_Data_D(Case_Num,Caller_ID,Case_List_type,Urgency,End_Date,OpinionType,ReplyType,Opinion_Content,
            Create_Agent,Dispatch,Dispatch_Depart,Dispatch_Name,Process_Status,Create_Date) 
            VALUES(N'{0}',N'{1}',N'{2}',N'{3}','{4}',N'{5}',N'{6}',N'{7}',N'{8}',N'{9}',N'{10}',N'{11}',N'{12}',N'{13}')"
        , str_sysid,Text1,select_ListCase, SelectUrgency, End_date, SelectOpinionType, ReplyType, Opinion_content, A_ID, yes_or_no,
            Chose_depart, Dispatch_Name, type, Create_date);
        var a = DBTool.Query(sqlstr);
        return JsonConvert.SerializeObject(new { status = "服務單新增完成。" });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Load_Data(string SYSID)
    {
        string sqlstr = @"SELECT * FROM Case_Data_D WHERE SYSID = '{0}'";
        string sql_format = string.Format(sqlstr, SYSID);
        var a = DBTool.Query<Case>(sql_format).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Case_Num = p.Case_Num,
            Caller_ID = p.Caller_ID,
            Case_List_type = p.Case_List_type,
            Urgency = p.Urgency,
            OpinionType = p.OpinionType,
            ReplyType = p.ReplyType,
            Opinion_Content = p.Opinion_Content,
            Create_Agent = p.Create_Agent,
            Dispatch = p.Dispatch,
            Dispatch_Depart = p.Dispatch_Depart,
            Dispatch_Name = p.Dispatch_Name,
            End_Date = p.End_Date,
            Create_Date = p.Create_Date,
            Process_Status = p.Process_Status
        });
        return JsonConvert.SerializeObject(a);
    }

    public class Case
    {
        public string SYSID { get; set; }
        public string Case_Num { get; set; }
        public string Caller_ID { get; set; }
        public string Case_List_type { get; set; }
        public string Urgency { get; set; }
        public string Case_Name { get; set; }
        public string OpinionType { get; set; }
        public string ReplyType { get; set; }
        public string Opinion_Content { get; set; }
        public string Create_Agent { get; set; }
        public string Dispatch_Depart { get; set; }
        public string Dispatch_Name { get; set; }
        public string Dispatch { get; set; }
        public string Create_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string Process_Status { get; set; }
    }
}