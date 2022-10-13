using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Btn_Query_Click(object sender, EventArgs e)
    {
        //ReportRepository.GetStatistics1("2017-01-01", "2017-01-31");
        string strat_time = txt_S_DATETime.Text;
        string end_time = txt_E_DATETime.Text;
        var report = new ReportRepository2(strat_time, end_time, "");

        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("Content-Disposition", "attachment; filename=report.xlsx");
        Response.BinaryWrite(report.GetReport());
        Response.Flush();
        Response.End();
    }
}
