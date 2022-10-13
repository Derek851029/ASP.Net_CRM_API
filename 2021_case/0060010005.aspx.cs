using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _2021_case_0060010005 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string GetCaseList()
    {
        //string Agent_Company = HttpContext.Current.Session["Agent_Company"].ToString();
        string sqlstr = @"SELECT Top 5000 a.*,b.Case_Name FROM Case_Data_D a left join  Case_List b on b.SYSID = a.Case_List_type order by Create_Date desc";
        var a = DBTool.Query<Case_List>(sqlstr).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Case_Num = p.Case_Num,
            Case_Name = p.Case_Name,
            OpinionType = p.OpinionType,
            Create_Agent = p.Create_Agent,
            Dispatch = p.Dispatch,
            Create_Date = p.Create_Date,
            End_Date = p.End_Date,
            Process_Status = p.Process_Status,
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    public class Case_List
    {
        public string SYSID { get; set; }
        public string Case_Num { get; set; }
        public string Case_Name { get; set; }
        public string OpinionType { get; set; }
        public string Create_Agent { get; set; }
        public string Dispatch { get; set; }
        public string Create_Date { get; set; }
        public string End_Date { get; set; }
        public string Process_Status { get; set; }
    }
}