using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard : System.Web.UI.Page
{
    protected string str_time = "";
    protected string type = "";
    protected string Team = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string Agent_ID = (string)(Session["UserIDNO"]);
        string UserIDNAME = (string)(Session["UserIDNAME"]);
        string UserID = (string)(Session["UserID"]);
        string Agent_LV = (string)(Session["Agent_LV"]);
        Session["Time_Flag"] = "0";
        Team = (string)(Session["Agent_Team"]);
    }

    [WebMethod(EnableSession = true)]
    public static string DailyTurnover()
    {
        string sql = @"select 
                        A.T_DAY,SUM(B.PRICE) [PRICE] 
                        from POS_H A
                        LEFT JOIN POS_I B ON A.T_DAY=A.T_DAY AND A.T_STORE=B.T_STORE AND A.TM_NO=B.TM_NO AND A.T_SER_NO=B.T_SER_NO
                        LEFT JOIN CASHMF C ON A.T_CASH=C.CASH_CODE
                        LEFT JOIN VIPMF D ON A.T_VIP=D.VIP_CODE
                        LEFT JOIN PRODUCT E ON B.PLU_CODE=E.PLU_CODE
                        WHERE B.DATA_TYPE='2' group by A.T_DAY";
        var data = DBTool2.Query(sql);
        return JsonConvert.SerializeObject(data);
    }

    [WebMethod(EnableSession = true)]
    public static string CommodityPayList()
    {
        string sql = @"select 
                         B.PLU_CODE,B.PC_NAME,SUM(B.CNT) [CNT],SUM(B.PRICE) [PRICE] 
                        from POS_H A
                        LEFT JOIN POS_I B ON A.T_DAY=A.T_DAY AND A.T_STORE=B.T_STORE AND A.TM_NO=B.TM_NO AND A.T_SER_NO=B.T_SER_NO
                        LEFT JOIN CASHMF C ON A.T_CASH=C.CASH_CODE
                        LEFT JOIN VIPMF D ON A.T_VIP=D.VIP_CODE
                        LEFT JOIN PRODUCT E ON B.PLU_CODE=E.PLU_CODE
                        WHERE B.DATA_TYPE='2' group by B.PLU_CODE, B.PC_NAME order by B.CNT desc";
        var data = DBTool2.Query(sql);
        return JsonConvert.SerializeObject(data);
    }
}