using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_009 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
            List_Team();
            List_Service();
        }
    }

    private void BindData()
    {
        DateTime date = DateTime.Now;
        txt_S_DATETime.Text = date.AddMonths(-1).ToString("yyyy-MM-01");
        txt_E_DATETime.Text = date.AddMonths(-1).ToString("yyyy-MM-") + DateTime.DaysInMonth(date.Year, date.AddMonths(-1).Month).ToString("00");
        txt_servicename.Text = "";
    }

    protected void List_Team()
    {
        string sqlstr = "SELECT Agent_Company FROM DispatchSystem WHERE Agent_Status = '在職' "
            + " GROUP BY Agent_Company "
            + " ";

        var list = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new { value = p.Agent_Company, text = p.Agent_Company });
        drop_team.DataSource = list;
        drop_team.DataTextField = "text";
        drop_team.DataValueField = "value";
        drop_team.DataBind();
        drop_team.Items.Insert(0, new ListItem("全部門", "全部門"));
    }

    protected void List_Service()
    {
        string sqlstr = "SELECT DISTINCT Agent_ID, Agent_Name FROM DispatchSystem Where Agent_Status = '在職'   order by Agent_ID ";
        var list = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new { value = p.Agent_ID, text = p.Agent_Name });
        drop_service.DataSource = list;
        drop_service.DataTextField = "text";
        drop_service.DataValueField = "value";
        drop_service.DataBind();
        drop_service.Items.Insert(0, new ListItem("全部員工", "全部員工"));
    }

   protected void drop_service_change(object sender, EventArgs e)
    {
         /*string service = drop_service.SelectedValue;
        string sqlstr = "SELECT Service_ID, ServiceName FROM Faremma.dbo.LaborTemplate WHERE Service = @Service Group by Service_ID, ServiceName";
        var list = DBTool.Query<ClassTemplate>(sqlstr, new { Service = service }).ToList().Select(p => new { value = p.Service_ID, text = p.ServiceName });
        drop_servicename.Items.Clear();
        drop_servicename.DataSource = list;
        drop_servicename.DataTextField = "text";
        drop_servicename.DataValueField = "value";
        drop_servicename.DataBind();
        drop_servicename.Items.Insert(0, new ListItem("全部服務內容", ""));//*/
    }   

    protected void Btn_Query_Click(object sender, EventArgs e)
    {
        var report = CreateReportRepository();
        if (report == null)
        {
            data.Attributes["src"] = "";
            return;
        }
        data.Attributes["src"] = "/temp/" + report.GetView();
    }

    protected void Btn_exportExcel_Click(object sender, EventArgs e)
    {
        DateTime date = DateTime.Parse(txt_E_DATETime.Text);
        var report = CreateReportRepository();
        if (report == null) return;
        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}月【員工打卡明細表】.xlsx", date.Month));
        Response.BinaryWrite(report.GetReport());
        Response.Flush();
        Response.End();
    }

    protected ReportRepository_009 CreateReportRepository()
    {
        try
        {
            DateTime Sdate = DateTime.Parse(txt_S_DATETime.Text);
            DateTime Edate = DateTime.Parse(txt_E_DATETime.Text);
            if (Sdate > Edate)
            {
                RegisterStartupScript("【開始時間】不能大於【結束時間】。");
                return null;
            }

            string team = drop_team.SelectedValue;
            string type = drop_type.SelectedValue;
            string service = drop_service.SelectedValue;
            if (service == "全部員工") { service = ""; }
            string service_id = "";
            if (txt_servicename.Text != "")
                service_id = "%" + txt_servicename.Text + "%";
            //string service_id = txt_servicename.Text;
            return new ReportRepository_009(txt_S_DATETime.Text, txt_E_DATETime.Text, team, type, service, service_id);
        }
        catch
        {
            RegisterStartupScript("【傳送系統參數錯誤】或【日期格式】不正確。");
            return null;
        }
    }

    private void RegisterStartupScript(string msg)
    {
        if (ScriptManager.GetCurrent(this.Page) == null)
        {
            Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "buttonStartup", "alert('" + msg + "');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "buttonStartupBySM", "alert('" + msg + "');", true);
        }
    }
}
