using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _2021_case_Assign_List_Personal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string bindtalbe()
    {
        string Agent_SYSID = HttpContext.Current.Session["SYSID"].ToString();
        string sqlstr = @"SELECT a.*,b.Case_Name FROM Assign_Case a left join Case_List b on a.Case_SYSID = b.SYSID "+ 
        "WHERE a.Assign_People = '{0}'";
        string sql_format = string.Format(sqlstr, Agent_SYSID);
        var a = DBTool.Query<Assign_List>(sql_format).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Case_SYSID = p.Case_SYSID,
            Case_Name = p.Case_Name,
            Urgent = p.Urgent,
            Assign_People = p.Assign_People,
            Assign_title = p.Assign_title,
            End_date = p.End_date,
            Status = p.Status,
        });
        return JsonConvert.SerializeObject(a);
    }

    public class Assign_List
    {
        public string SYSID { get; set; }
        public string Case_SYSID { get; set; }
        public string Case_Name { get; set; }
        public string Assign_Depart { get; set; }
        public string Assign_People { get; set; }
        public string Agent_Name { get; set; }
        public string Urgent { get; set; }
        public string Assign_title { get; set; }
        public string Assign_text { get; set; }
        public string Create_Date { get; set; }
        public DateTime End_date { get; set; }
        public string Assign_Create_Agent { get; set; }
        public string Status { get; set; }
        public string Chargeback_Content { get; set; }
    }
}