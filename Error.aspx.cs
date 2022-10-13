using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;
using log4net;
using log4net.Config;

public partial class Error : System.Web.UI.Page
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {
        logger.Info("Error.aspx.cs");
        Session.Clear();
        Session.RemoveAll();
        Session.Abandon();
    }
}