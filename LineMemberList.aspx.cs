using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LineMemberList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    public static string bindMember()
    {
        string sqlcmd = @" SELECT a.* ,b.Agent_Name, b.Agent_Team
                            FROM LineMember a
                            LEFT JOIN DispatchSystem b ON a.UserID = b.LineUserID ";
        var data = DBTool.Query(sqlcmd, new { });

        return JsonConvert.SerializeObject(data);
    }


    [WebMethod(EnableSession = true)]
    public static void openpage(string ID)
    {
        HttpContext.Current.Session["LMBID"] = ID;
    }


    [WebMethod(EnableSession = true)]
    public static void openedit(string ID)
    {
        HttpContext.Current.Session["MemberEdit"] = ID;
    }


}