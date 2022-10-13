using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_001 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
            //List_Team();
        }
    }

    private void BindData()
    {
        DateTime date = DateTime.Now;
        txt_S_DATETime.Text = date.AddMonths(-1).ToString("yyyy-MM-01");
        txt_E_DATETime.Text = date.AddMonths(-1).ToString("yyyy-MM-") + DateTime.DaysInMonth(date.Year, date.AddMonths(-1).Month).ToString("00");
    }

    protected void List_Team()
    {
        string sqlstr = "SELECT Create_Team FROM [Faremma].[dbo].LaborTemplate WHERE Create_Team != '' "
            + " GROUP BY Create_Team "
            + " ORDER BY charindex ( Create_Team, ( SELECT TOP 1 Team FROM [Faremma].[dbo].Team_Rank ) ) ";

        var list = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new { value = p.Create_Team, text = p.Create_Team });
        drop_team.DataSource = list;
        drop_team.DataTextField = "text";
        drop_team.DataValueField = "value";
        drop_team.DataBind();
        drop_team.Items.Insert(0, new ListItem("全部門", "全部門"));
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
        //ReportRepository.GetStatistics1("2017-01-01", "2017-01-31");

        DateTime date = DateTime.Parse(txt_E_DATETime.Text);

        var report = CreateReportRepository();
        if (report == null) return;
        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}月【服務單】工程師統計報表.xlsx", date.Month));
        Response.BinaryWrite(report.GetReport());
        Response.Flush();
        Response.End();
    }

    protected ReportRepository_001 CreateReportRepository()
    {
        try
        {
            DateTime Sdate = DateTime.Parse(txt_S_DATETime.Text);
            DateTime Edate = DateTime.Parse(txt_E_DATETime.Text);
            /*if (Sdate.Year != Edate.Year)
            {
                RegisterStartupScript("【開始時間】與【結束時間】只能選擇【同年與同月份】。");
                return null;
            }

            if (Sdate.Month != Edate.Month)
            {
                RegisterStartupScript("【開始時間】與【結束時間】只能選擇【同年與同月份】。");
                return null;
            }//*/

            if (Sdate > Edate)
            {
                RegisterStartupScript("【開始時間】不能大於【結束時間】。");
                return null;
            }

            //string team = drop_team.SelectedValue;
            string team = "全部門";
            report_title.InnerText = DateTime.Parse(txt_S_DATETime.Text).ToString("yyyy年MM月dd日") + " 至 " + DateTime.Parse(txt_E_DATETime.Text).ToString("yyyy年MM月dd日") + "【" + team + "】" + "工程師統計報表";
            return new ReportRepository_001(txt_S_DATETime.Text, txt_E_DATETime.Text, team);
        }
        catch
        {
            RegisterStartupScript("【傳送系統參數錯誤】或【日期格式】不正確。");
            return null;
        }
    }

    // <summary>
    /// Render結束後要執行的javascript，有ScriptManager的話要透過ScriptManager註冊
    /// </summary>
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
