using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_010 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
            List_Team();
            List_Service();
        }
        Btn_Query_Click(sender, e);
    }

    private void BindData()
    {
        DateTime date = DateTime.Now;
        txt_S_DATETime.Text = date.AddMonths(-1).ToString("yyyy-MM-01");
        txt_E_DATETime.Text = date.AddMonths(-1).ToString("yyyy-MM-") + DateTime.DaysInMonth(date.Year, date.AddMonths(-1).Month).ToString("00");
    }

    protected void List_Team()
    {
        /*string sqlstr = "SELECT Create_Team FROM LaborTemplate WHERE Create_Team != '' "
            + " GROUP BY Create_Team "
            + " ORDER BY charindex ( Create_Team, ( SELECT TOP 1 Team FROM Team_Rank ) ) "; //*/
        string sqlstr = "SELECT Product_Name FROM OE_Order WHERE Delete_Date is null "
            + " GROUP BY Product_Name ";

        var list = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new { value = p.Product_Name, text = p.Product_Name });
        drop_team.DataSource = list;
        drop_team.DataTextField = "text";
        drop_team.DataValueField = "value";
        drop_team.DataBind();
        drop_team.Items.Insert(0, new ListItem("全部門", "全部門"));
    }

    protected void List_Service()
    {
        //string sqlstr = "SELECT DISTINCT Service FROM LaborTemplate WHERE Service != '' ";
        string sqlstr = "SELECT DISTINCT OE_ID FROM OE_Order WHERE Delete_Date is null GROUP BY OE_ID ";
        var list = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new { value = p.OE_ID, text = p.OE_ID });
        drop_service.DataSource = list;
        drop_service.DataTextField = "text";
        drop_service.DataValueField = "value";
        drop_service.DataBind();
        drop_service.Items.Insert(0, new ListItem("全部服務項目", "全部服務項目"));
    }

    protected void drop_service_change(object sender, EventArgs e)
    {
        string service = drop_service.SelectedValue;
        //string sqlstr = "SELECT Service_ID, ServiceName FROM LaborTemplate WHERE Service = @Service Group by Service_ID, ServiceName";
        string sqlstr = "SELECT DISTINCT OE_ID FROM OE_Order WHERE Delete_Date is null GROUP BY OE_ID ";
        var list = DBTool.Query<ClassTemplate>(sqlstr, new { Service = service }).ToList().Select(p => new { value = p.OE_ID, text = p.OE_ID });
        drop_servicename.Items.Clear();
        drop_servicename.DataSource = list;
        drop_servicename.DataTextField = "text";
        drop_servicename.DataValueField = "value";
        drop_servicename.DataBind();
        drop_servicename.Items.Insert(0, new ListItem("全部服務內容", ""));
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
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}月【派工系統】需求單明細表.xlsx", date.Month));
        Response.BinaryWrite(report.GetReport());
        Response.Flush();
        Response.End();
    }

    protected ReportRepository_010 CreateReportRepository()
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
            if (service == "全部服務項目") { service = ""; }
            string service_id = drop_servicename.SelectedValue;
            string OE_Case_ID = "170703150318902";
            return new ReportRepository_010(txt_S_DATETime.Text, txt_E_DATETime.Text, team, type, service, service_id, OE_Case_ID);
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
