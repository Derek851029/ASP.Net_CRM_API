using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using System.Threading;

public partial class Excel : System.Web.UI.Page
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    string sql_txt;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string SQL(string No, string Name, string TEL, string Phone, string Company)
    {
        Excel_Value template = new Excel_Value()
        {
            No = No,
            Name = Name,
            TEL = TEL,
            Phone = Phone,
            Company = Company
        };

        string sqlstr = @"INSERT INTO Excel_Table (No, Name, TEL, Phone, Company) VALUES(@No, @Name, @TEL, @Phone, @Company)";
        using (IDbConnection db = DBTool.GetConn())
        {
            db.Execute(sqlstr, template);
            db.Close();
        };

        return "success";
    }

    public class Excel_Value
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string TEL { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
    }
}