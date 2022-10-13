using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_007 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
            List_Team();
            List_Service();
            //List_OpinionType();
        }
    }

    private void BindData()
    {
        DateTime date = DateTime.Now;
        txt_S_DATETime.Text = date.AddMonths(-1).ToString("yyyy-MM-01");
        txt_E_DATETime.Text = date.AddMonths(-1).ToString("yyyy-MM-") + DateTime.DaysInMonth(date.Year, date.AddMonths(-1).Month).ToString("00");
        txt_opinioncontent.Text = "";
        //txtSearchName.Text = "";
    }

    protected void List_Team()
    {
        /*string sqlstr = "select a.PID, b.BUSINESSNAME FROM CaseData as a "
            + " left join BusinessData as b on a.PID=b.PID where a.PID<>'' and b.BUSINESSNAME<>'' "
            + " group by a.PID, b.BUSINESSNAME order by a.PID ";//*/
        string sqlstr = "Select SYSID,mission_name as Name FROM InSpecation_Dimax.dbo.Mission_Title " +
            "";

        var list = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new { value = p.SYSID, text = "【" + p.SYSID + "】" + p.Name });//text = "【"+p.PID+"】"+p.BUSINESSNAME
        drop_businessname.DataSource = list;
        drop_businessname.DataTextField = "text";
        drop_businessname.DataValueField = "value";
        drop_businessname.DataBind();
        drop_businessname.Items.Insert(0, new ListItem("全任務", ""));
    }

    protected void List_Service()
    {
        string sqlstr = "Select Handle_Agent From InSpecation_Dimax.dbo.Mission_Case " +
            " Where Handle_Agent<>'' group by Handle_Agent ";
        var list = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new { value = p.Handle_Agent, text = p.Handle_Agent });
        drop_enginner.DataSource = list;
        drop_enginner.DataTextField = "text";
        drop_enginner.DataValueField = "value";
        drop_enginner.DataBind();
        drop_enginner.Items.Insert(0, new ListItem("全工程師", ""));
    }

    protected void List_OpinionType(object sender, EventArgs e)
    {
        /*string sqlstr = "select OpinionType From CaseData Where OpinionType<>'' and OpinionType!='1' Group by OpinionType ";
        var list = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new { value = p.OpinionType, text = p.OpinionType });
        drop_opinion_type.DataSource = list;
        drop_opinion_type.DataTextField = "text";
        drop_opinion_type.DataValueField = "value";
        drop_opinion_type.DataBind();
        drop_opinion_type.Items.Insert(0, new ListItem("全意見類型", ""));//*/
    }

   protected void drop_service_change(object sender, EventArgs e)
    {
         /*string service = drop_enginner.SelectedValue;
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

    protected void ShowName()
    {
        //string business = Search_Name.Text;
        drop_type.Items.FindByText("未到點").Selected = true;  //直接選某一選項(不能錯字)
    }

    protected void Btn_exportExcel_Click(object sender, EventArgs e)
    {
        DateTime date = DateTime.Parse(txt_E_DATETime.Text);
        var report = CreateReportRepository();
        if (report == null) return;
        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=【維護單明細表】.xlsx", date.Month));//{0}月
        Response.BinaryWrite(report.GetReport());
        Response.Flush();
        Response.End();
    }

    protected ReportRepository_007 CreateReportRepository()
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

            string business = drop_businessname.SelectedValue;
            string type = drop_type.SelectedValue;
            string enginner = drop_enginner.SelectedValue;  //
            string o_type = drop_servicename.SelectedValue;    //電信商
            string o_content = "";//停用
            return new ReportRepository_007(txt_S_DATETime.Text, txt_E_DATETime.Text, business, type, enginner, o_type, o_content);
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
