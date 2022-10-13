using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using log4net;
using log4net.Config;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.ComponentModel.ToolboxItem(false)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[ScriptService]
public class WebService_Backup : System.Web.Services.WebService
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string Default()
    {
        int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
        int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
        int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
        string rawSearch = HttpContext.Current.Request.Params["sSearch"];
        string participant = HttpContext.Current.Request.Params["iParticipant"];
        string Flag = HttpContext.Current.Request.Params["Flag"];
        var sb = new StringBuilder();
        var whereClause = string.Empty;
        if (participant.Length > 0)
        {
            sb.Append(" Where MNo like ");
            sb.Append("'%" + participant + "%'");
            whereClause = sb.ToString();
            logger.Info("whereClause = " + whereClause);
        }
        sb.Clear();

        var filteredWhere = string.Empty;

        var wrappedSearch = "'%" + rawSearch + "%'";

        if (rawSearch.Length > 0)
        {
            sb.Append(" WHERE MNo LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Time_01 LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Type LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Cust_Name LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR ServiceName LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Labor_CName LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Question LIKE ");
            sb.Append(wrappedSearch);

            filteredWhere = sb.ToString();
        }

        //ORDERING

        sb.Clear();

        string orderByClause = string.Empty;
        sb.Append(ToInt(HttpContext.Current.Request.Params["iSortCol_0"]));

        sb.Append(" ");

        sb.Append(HttpContext.Current.Request.Params["sSortDir_0"]);

        orderByClause = sb.ToString();

        if (!String.IsNullOrEmpty(orderByClause))
        {
            orderByClause = orderByClause.Replace("0", ", MNo ");
            orderByClause = orderByClause.Replace("1", ", Time_01 ");
            orderByClause = orderByClause.Replace("2", ", Type_Value ");
            orderByClause = orderByClause.Replace("3", ", Cust_Name ");
            orderByClause = orderByClause.Replace("4", ", ServiceName ");
            orderByClause = orderByClause.Replace("5", ", Labor_CName ");
            orderByClause = orderByClause.Replace("6", ", MNo ");
            orderByClause = orderByClause.Replace("7", ", MNo ");
            orderByClause = orderByClause.Remove(0, 1);
        }
        else
        {
            orderByClause = "ID ASC";
        }
        orderByClause = "ORDER BY " + orderByClause;
        logger.Info(orderByClause);
        sb.Clear();

        var numberOfRowsToReturn = "";
        numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

        string query = @"declare @MA TABLE " +
            "( MNo nvarchar(20), Time_01 datetime, Type nvarchar(8), Type_Value nvarchar(1), Cust_Name nvarchar(50), ServiceName nvarchar(20), Labor_CName nvarchar(20), Question nvarchar(300) ) " +
            "INSERT INTO @MA ( MNo, Time_01, Type, Type_Value, Cust_Name, ServiceName, Labor_CName, Question ) " +
            "Select MNo, Time_01, Type, Type_Value, Cust_Name, ServiceName, Labor_CName, Question " +
            //"FROM [View_MNo] {4} WHERE Cust_ID !=''" +
            "FROM [Faremma].[dbo].[LaborTemplate] {4} " +
            "SELECT * FROM " +
            "( SELECT row_number() OVER ({0}) AS RowNumber ,  * FROM " +
            "( SELECT ( SELECT count([@MA].MNo) FROM @MA) AS TotalRows , " +
            "( SELECT  count( [@MA].MNo) FROM @MA {1}) AS TotalDisplayRows , " +
            "[@MA].MNo ,[@MA].Time_01 ,[@MA].Type ,[@MA].Type_Value ,[@MA].Cust_Name ,[@MA].ServiceName ,[@MA].Labor_CName ,[@MA].Question " +
            "FROM @MA {1}) RawResults) Results " +
            "WHERE RowNumber BETWEEN {2} AND {3} ";
        query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);
        logger.Info(query);
        var connectionString = ConfigurationManager.ConnectionStrings["CMS_ENTConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionString);

        try
        {
            conn.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        var DB = new SqlCommand();
        DB.Connection = conn;
        DB.CommandText = query;
        var data = DB.ExecuteReader();

        var totalDisplayRecords = "";
        var totalRecords = "";
        string outputJson = string.Empty;

        var rowClass = "";
        var count = 0;
        string str_mno_1 = "";
        string str_mno_2 = "";
        while (data.Read())
        {
            if (totalRecords.Length == 0)
            {
                totalRecords = data["TotalRows"].ToString();
                totalDisplayRecords = data["TotalDisplayRows"].ToString();
            }
            sb.Append("{");
            sb.AppendFormat(@"""DT_RowId"": ""{0}""", count++);
            sb.Append(",");
            sb.AppendFormat(@"""DT_RowClass"": ""{0}""", rowClass);
            sb.Append(",");
            sb.AppendFormat(@"""0"": ""{0}""", data["MNo"]);
            sb.Append(",");
            string Time_01 = Convert.ToDateTime(data["Time_01"]).ToString("yyyy/MM/dd HH:mm");
            sb.AppendFormat(@"""1"": ""{0}""", Time_01);
            sb.Append(",");
            sb.AppendFormat(@"""2"": ""{0}""", data["Type"]);
            sb.Append(",");
            sb.AppendFormat(@"""3"": ""{0}""", data["Cust_Name"]);
            sb.Append(",");
            sb.AppendFormat(@"""4"": ""{0}""", data["ServiceName"].ToString());
            sb.Append(",");
            sb.AppendFormat(@"""5"": ""{0}""", data["Labor_CName"].ToString());
            sb.Append(",");
            sb.AppendFormat(@"""6"": ""{0}""", data["Question"].ToString());
            sb.Append(",");
            // 分割字串避免字串過長
            str_mno_1 = data["MNo"].ToString().Substring(0, 8);
            str_mno_2 = data["MNo"].ToString().Substring(8, data["MNo"].ToString().Length - 8);
            //sb.AppendFormat(@"""2"": ""{0}""", "<button id='but_url' type='button' class='btn btn-info btn-lg' onclick='moreFields(" + data["MNo"] + "," + Flag + ");'><span class='glyphicon glyphicon-search'></span>&nbsp;&nbsp;明細 " + data["MNo"] + " 件</button>");
            sb.AppendFormat(@"""7"": ""{0}""", "<button id='but_url' type='button' class='btn btn-info btn-lg' onclick='moreFields(" + str_mno_1 + "," + "1" + str_mno_2 + "," + Flag + ");'><span class='glyphicon glyphicon-search'></span>&nbsp;&nbsp;明細</button>");
            sb.Append("},");
        }

        // handles zero records
        if (totalRecords.Length == 0)
        {
            sb.Append("{");
            sb.Append(@"""sEcho"": ");
            sb.AppendFormat(@"""{0}""", sEcho);
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": 0");
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": 0");
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append("]}");
            outputJson = sb.ToString();
            return outputJson;
        }
        outputJson = sb.Remove(sb.Length - 1, 1).ToString();
        sb.Clear();

        sb.Append("{");
        sb.Append(@"""sEcho"": ");
        sb.AppendFormat(@"""{0}""", sEcho);
        sb.Append(",");
        sb.Append(@"""iTotalRecords"": ");
        sb.Append(totalRecords);
        sb.Append(",");
        sb.Append(@"""iTotalDisplayRecords"": ");
        sb.Append(totalDisplayRecords);
        sb.Append(", ");
        sb.Append(@"""aaData"": [ ");
        sb.Append(outputJson);
        sb.Append("]}");
        outputJson = sb.ToString();
        return outputJson;
    }

    public static int ToInt(string toParse)
    {
        int result;
        if (int.TryParse(toParse, out result)) return result;

        return result;
    }


}
