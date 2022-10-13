using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Windows.Forms;

public partial class MasterPage : System.Web.UI.MasterPage
{
    public string Role_ID;
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetNoServerCaching();
        HttpContext.Current.Response.Cache.SetNoStore();
        string UserIDNAME = (string)(Session["UserIDNAME"]);
        if (string.IsNullOrEmpty(UserIDNAME))
        {
            Response.Write("<script>alert('待機時間過久，將自動登出回到登入頁面。'); location.href='../Logout.aspx'; </script>");
            return;
        }
    }

    protected void Check_Session(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty((string)(Session["UserIDNAME"])))
        {
            Response.Redirect("~/Logout.aspx");
        };
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty((string)(Session["UserIDNAME"])))
        {
            Response.Redirect("~/Logout.aspx");
        };
    }
}
