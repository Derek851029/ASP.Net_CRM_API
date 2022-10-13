using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _2021_case_0030010003 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string renderCalendar(DateTime start, DateTime end)
    {
        string Agent_SYSID = HttpContext.Current.Session["SYSID"].ToString();

        string sqlstr = @"SELECT a.SYSID as id, a.Process_Status as type, CONVERT(varchar(100), a.Create_Date, 111) as start, b.Case_Name as title FROM Case_Data_D a" +
                        " left join Case_List b on a.Case_List_type = b.SYSID WHERE a.Dispatch_Name = N'{0}'";
        string sql_format = string.Format(sqlstr, Agent_SYSID);
        var a = DBTool.Query<Case_List>(sql_format).ToList().Select(p => new
        {
            id = p.id,
            type = p.type,
            start = p.start,
            title = p.title
        });
        return JsonConvert.SerializeObject(a);
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string bindtalbe_Case(string SYSID)
    {
        string Agent_SYSID = HttpContext.Current.Session["SYSID"].ToString();
        string sqlstr = @"SELECT Top 1 a.*,b.Case_Name FROM Case_Data_D a left join  Case_List b on b.SYSID = a.Case_List_type WHERE a.SYSID = N'{0}' AND Dispatch_Name = N'{1}'";
        string sql_format = string.Format(sqlstr, SYSID, Agent_SYSID);
        var a = DBTool.Query<Case_List>(sql_format).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Case_Num = p.Case_Num,
            Case_Name = p.Case_Name,
            OpinionType = p.OpinionType,
            Create_Agent = p.Create_Agent,
            Create_Date = p.Create_Date,
            End_Date = p.End_Date.ToString("yyyy-MM-dd"),
            Process_Status = p.Process_Status,
        });
        return JsonConvert.SerializeObject(a);
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string SetCase_Type(string SYSID)
    {
        string sqlstr = @"UPDATE Case_Data_D SET Process_Status = '1' WHERE SYSID = '{0}'";
        string sql_format = string.Format(sqlstr, SYSID);
        var a = DBTool.Query(sql_format);
        return JsonConvert.SerializeObject(new { status = "接案成功。" });
    }
    public class Case_List
    {
        public string type { get; set; }
        public string start { get; set; }
        public string title { get; set; }
        public string id { get; set; }
        public string SYSID { get; set; }
        public string Case_Num { get; set; }
        public string Case_Name { get; set; }
        public string OpinionType { get; set; }
        public string Create_Agent { get; set; }
        public string Dispatch { get; set; }
        public string Create_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string Process_Status { get; set; }
    }
}