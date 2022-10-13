using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LineMsgToMany : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [WebMethod(EnableSession = true)]
    public static string bindata()
    {
        string sqlcmd = @" SELECT a.* ,b.UserName [Name],b.Class,b.StudentName
                            FROM LineMember a
                            LEFT JOIN Customer b ON a.UserID = b.UserID
                            WHERE a.flag = 0 ";
        var data = DBTool.Query(sqlcmd, new { });

        return JsonConvert.SerializeObject(data);
    }



}