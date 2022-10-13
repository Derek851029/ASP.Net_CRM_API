using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using log4net;
using log4net.Config;

public partial class CheckLogin : System.Web.UI.Page
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {
        //string pass = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Request.Params["pass"], "MD5").ToUpper();
        string EMMA = System.Web.Configuration.WebConfigurationManager.AppSettings["EMMA"].ToString();
        string seqno = Request.Params["seqno"];   //母單編號
        string login = Request.Params["login"];       //帳號
        string UserID = Request.Params["login"];
        string page = Request.Params["page"];      //連結
        string type = Request.Params["type"];   //狀態

        switch (page)       //跳頁
        {
            //http://dispatch.manstrong.com.tw:5000/CheckLogin.aspx?page=99&login=1130698      
            case "1":    //  OE單（審查）
                Response.Redirect("~/0030010015.aspx?seqno=" + seqno);
                break;
            case "2":    // OE單（修改）
                Response.Redirect("~/0030010005.aspx?seqno=" + seqno);
                break;
            case "3":   //  派工需求單（修改）+查看
                Response.Redirect("~/0030010099.aspx?seqno=" + seqno);
                break;
            case "4":   //  派工需求單（瀏覽）
                Response.Redirect("~/0030010097.aspx?seqno=" + seqno);
                break;
            /*case "5":   //  審核OE單（瀏覽）
                Response.Redirect("~/0030010015.aspx?seqno=" + seqno);
                break;
            case "6":   //  派工需求單（瀏覽）
                Response.Redirect("~/0030010015.aspx?seqno=" + seqno);
                break;  //*/
            default:
                Response.Redirect("~/Default.aspx");
                break;
        }
        Response.Redirect("~/Default.aspx");


        string sqlstr = "";
        string url = "";
        string url_2 = "";

        try
        {
            if (login.Length > 32 )
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                UserID = JASON.Decrypted(login);
            }
        }
        catch
        {
            Response.Redirect("~/Login.aspx");
        }

        url = EMMA + "CheckLogin.aspx?seqno=" + seqno;
        try
        {
            if (type.Length > 0) { url += "&type=" + type; };
        }
        catch
        {

        }

        //=============檢查 tblAssign & tblAssign_Issued 有沒有這筆 EMMA================
        //=========================== 時效14天 ============================

        url_2 = url + "&login=" + login + "&page=" + page;
        url += "&page=" + page + "&login=" + login;
        logger.Info("CheckLogin.aspx 登入：" + url);
        logger.Info("CheckLogin.aspx 登入：" + url_2);
        sqlstr = @"SELECT SYSID FROM tblAssign WHERE ConnURL in ( @ConnURL, @ConnURL_2 ) AND CreateDate > convert(varchar, getdate()-14, 112) " +
            " union all " +
            " SELECT SYSID FROM tblAssign_Issued WHERE ConnURL in ( @ConnURL, @ConnURL_2 ) AND CreateDate > convert(varchar, getdate()-14, 112) ";
        var list = DBTool.Query<ClassTemplate>(sqlstr, new { ConnURL = url, ConnURL_2 = url_2 });

        //==========================================

        if (list.Any())
        {
            logger.Info("URL 登入成功");
            sqlstr = @"SELECT TOP 1 * FROM DispatchSystem WHERE ( UserID=@UserID OR Agent_ID=@UserID ) AND Agent_Status = '在職' ";
            list = DBTool.Query<ClassTemplate>(sqlstr, new { UserID = UserID });
            if (list.Any())
            {
                ClassTemplate schedule = list.First();
                Session["UserIDNO"] = schedule.Agent_ID;
                Session["UserIDNAME"] = schedule.Agent_Name;
                Session["Agent_Company"] = schedule.Agent_Company;
                Session["Agent_Team"] = schedule.Agent_Team;
                Session["Agent_Phone"] = schedule.Agent_Phone;
                Session["UserID"] = schedule.Agent_ID;
                Session["RoleID"] = schedule.Role_ID;
                Session["Agent_LV"] = schedule.Agent_LV;
                logger.Info(Session["UserIDNO"]);
                
            }
            else
            {
                logger.Info("查無：" + UserID + " 此帳號");
                Response.Redirect("~/Login.aspx");
            };
        }
        else
        {
            Response.Redirect("~/Login.aspx");
        };
    }
}