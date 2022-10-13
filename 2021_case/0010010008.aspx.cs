using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _2021_case_0010010008 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Search(string People,string Case)
    {
        string sqlstr;
        if (Case == "-1")
        {
            sqlstr = "SELECT a.*,b.BUSINESSNAME FROM [Case_List] a left join [BusinessData] b on a.Clinet_Name = b.ID WHERE Personnel = '" + People + "'";
        }
        else
        {
            sqlstr = "SELECT a.*,b.BUSINESSNAME FROM [Case_List] a left join [BusinessData] b on a.Clinet_Name = b.ID WHERE Personnel = '" + People + "' AND Status='" + Case + "'";
        }
        var data = DBTool.Query<Case_List>(sqlstr).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Case_Name = p.Case_Name,
            BUSINESSNAME = p.BUSINESSNAME,
            Contact = p.Contact,
            Phone = p.Phone,
            Personnel = p.Personnel,
            Status = p.Status,
        });

        string outputJson = JsonConvert.SerializeObject(data);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Search_Assign(string People,string Case_SYSID)
    {
        string sqlstr;
        if (Case_SYSID == "0")
        {
            sqlstr = "SELECT a.*,b.Case_Name,c.Agent_Name FROM [Assign_Case] a left join [Case_List] b on a.Case_SYSID = b.SYSID left join [DispatchSystem] c on a.Assign_People = c.SYSID WHERE a.Assign_People = '" + People + "'";
        }
        else
        {
            sqlstr = "SELECT a.*,b.Case_Name,c.Agent_Name FROM [Assign_Case] a left join [Case_List] b on a.Case_SYSID = b.SYSID left join [DispatchSystem] c on a.Assign_People = c.SYSID WHERE  a.Case_SYSID = '"+Case_SYSID+"'";
        }
        var data = DBTool.Query<Assign_List>(sqlstr).ToList().Select(p => new
        {
            Case_Name = p.Case_Name,
            Urgent = p.Urgent,
            Assign_text = p.Assign_text,
            Agent_Name = p.Agent_Name,
            Status = p.Status,
        });

        string outputJson = JsonConvert.SerializeObject(data);
        return outputJson;
    }

    public class Assign_List
    {
        public string SYSID { get; set; }
        public string Assign_Depart { get; set; }
        public string Assign_People { get; set; }
        public string Agent_Name { get; set; }
        public string Urgent { get; set; }
        public string Assign_Company_Connection { get; set; }
        public string Assign_Company_Phone { get; set; }
        public string Assign_text { get; set; }
        public string Create_Date { get; set; }
        public DateTime End_date { get; set; }
        public string Assign_Create_Agent { get; set; }
        public string Status { get; set; }
        public string Chargeback_Content { get; set; }
        public string Case_Name { get; set; }
    }
}
