using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using log4net;
using log4net.Config;

public partial class Mobile_Login : System.Web.UI.Page
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {
        string login = Request.Params["login"];      //帳號
        string page = Request.Params["page"];      //連結
        string UserID = Request.Params["UserID"];

        string sqlstr = "";

        try
        {
            /*if (login.Length != 16)
            {
                logger.Info("不等於16碼");
                Response.Redirect("~/Login.aspx");
            }
            else
            {   //*/
            UserID = JASON.Decrypted(login);
            //}
        }
        catch
        {
            Response.Redirect("~/Login.aspx");
        }

        logger.Info("UserID = " + UserID);
        sqlstr = @"SELECT TOP 1 * FROM DispatchSystem WHERE ( UserID=@UserID OR Agent_ID=@UserID ) AND Agent_Status = '在職' ";
        var list = DBTool.Query<ClassTemplate>(sqlstr, new { UserID = UserID });
        if (list.Any())
        {
            ClassTemplate schedule = list.First();
            Session["SYSID"] = schedule.SYSID;
            Session["UserIDNO"] = schedule.Agent_ID;
            Session["UserIDNAME"] = schedule.Agent_Name;
            Session["Agent_Company"] = schedule.Agent_Company;
            Session["Agent_Team"] = schedule.Agent_Team;
            Session["Agent_Phone"] = schedule.Agent_Phone;
            Session["UserID"] = schedule.Agent_ID;
            Session["RoleID"] = schedule.Role_ID;
            Session["Agent_LV"] = schedule.Agent_LV;
            Session["Agent_Mail"] = schedule.Agent_Mail;
            logger.Info("Session[UserID] = "+Session["UserID"]);
            switch (page)
            {
                case "1":    // 個人事項導覽 
                    Response.Redirect("~/Default.aspx");
                    break;
                case "2":    // 行事曆
                    Response.Redirect("~/2021_case/0010010006.aspx");
                    break;
                case "6":    // 個人打卡/登記行程頁
                    Response.Redirect("~/0030010000/0030010000.aspx");
                    break;
                case "8":    // 任務頁
                    Response.Redirect("~/0060010005.aspx");
                    break;
                case "7":    // 工作日誌頁
                    Response.Redirect("~/0050010000/0050010001.aspx");
                    break;
                case "9":    // 通訊錄(客戶資料)
                    Response.Redirect("~/0060010001.aspx");
                    break;
                case "10": //個人交辦事項查詢
                    Response.Redirect("~/2021_case/Assign_List_Personal.aspx");
                    break;


                case "3":    // 個人派工及結案管理（瀏覽）
                    //logger.Info("case 3");
                    //Response.Redirect("~/0030010000/0030010003.aspx?view=0");     接單處理
                    Response.Redirect("~/0030010000/0030010008.aspx?view=0");   //個人維護瀏覽
                    break;
                case "4":    // 個人派工及結案管理（瀏覽）
                    string cno = Request.Params["cno"];      //連結
                    logger.Info("case 4");
                    //Response.Redirect("~/0020010000/0020010008.aspx?seqno=" + cno);
                    Response.Redirect("~/0030010097.aspx?seqno=" + cno);
                    break;
                case "5":    // 個人派工及結案管理（瀏覽）
                    logger.Info("case 5");
                    Response.Redirect("~/0030010099.aspx?seqno=0");
                    break;

                default:
                    logger.Info("else");
                    Response.Redirect("~/Default.aspx");
                    break;
            }
        }
        else
        {
            logger.Info("查無：" + UserID + " 此帳號");
            Response.Redirect("~/Login.aspx");
        };
    }
}