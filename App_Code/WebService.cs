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
public class WebService : System.Web.Services.WebService
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    [ScriptMethod(UseHttpGet = true)]
    public string Default_MNo()
    {
        try
        {
            string MNo_Flag = System.Web.Configuration.WebConfigurationManager.AppSettings["MNo_Flag"].ToString();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            string rawSearch = HttpContext.Current.Request.Params["sSearch"];
            string participant = HttpContext.Current.Request.Params["iParticipant"];
            string Flag = HttpContext.Current.Request.Params["Flag"];
            string Time_Flag = HttpContext.Current.Request.Params["Time_Flag"];

            //=============================================
            string Agent_LV = HttpContext.Current.Session["Agent_LV"].ToString();
            string Agent_ID = HttpContext.Current.Session["UserID"].ToString();
            string Team = HttpContext.Current.Session["Agent_Team"].ToString();
            //=============================================

            string sqlstr = "";
            var sb = new StringBuilder();
            var whereClause = string.Empty;
            string sql_time = "";
            switch (Time_Flag)
            {
                case "2":
                    sql_time = "";
                    break;
                case "1":
                    sql_time = " AND convert(char(6),Time_01,112) = convert(char(6),GETDATE(),112)";
                    break;
                default:
                    sql_time = " AND convert(char,Time_01,112) = convert(char,GETDATE(),112)";
                    break;
            };

            if (Agent_LV == "30")
            {
                sqlstr = "WHERE Type_Value = " + Flag + sql_time;
            }
            else if (Agent_LV == "20")
            {
                sqlstr = "WHERE Type_Value = " + Flag + sql_time + " AND Create_Team = '" + Team + "'";
            }
            else
            {
                if (MNo_Flag == "0")
                {
                    sqlstr = "WHERE Type_Value = " + Flag + sql_time + " AND Create_Team = '" + Team + "'";
                }
                else
                {
                    sqlstr = "WHERE Type_Value = " + Flag + sql_time + " AND Create_ID = '" + Agent_ID + "'";
                }
            }


            if (Flag == "7")
            {
                switch (Agent_LV)
                {
                    case "20":
                        sqlstr = "WHERE Create_Team = '" + Team + "'" + sql_time;
                        break;
                    case "30":
                        sqlstr = "WHERE Type_Value != 99 " + sql_time;
                        break;
                    default:
                        if (MNo_Flag == "0")
                        {
                            sqlstr = "WHERE Type_Value != 0 AND Create_Team = '" + Team + "'" + sql_time;
                        }
                        else
                        {
                            sqlstr = "WHERE Type_Value != 0 AND Create_ID = '" + Agent_ID + "'" + sql_time;
                        }
                        break;
                }
            };

            if (participant.Length > 0)
            {
                sb.Append(" Where MNo like ");
                sb.Append("'%" + participant + "%'");
                whereClause = sb.ToString();
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
                sb.Append(" OR Create_Name LIKE ");
                sb.Append(wrappedSearch);
                sb.Append(" OR Cust_Name LIKE ");
                sb.Append(wrappedSearch);
                sb.Append(" OR ServiceName LIKE ");
                sb.Append(wrappedSearch);
                sb.Append(" OR Labor_CName LIKE ");
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
                orderByClause = orderByClause.Replace("0", ", Time ");
                orderByClause = orderByClause.Replace("1", ", MNo ");
                orderByClause = orderByClause.Replace("2", ", Type_Value ");
                orderByClause = orderByClause.Replace("3", ", Create_Name ");
                orderByClause = orderByClause.Replace("4", ", Cust_Name ");
                orderByClause = orderByClause.Replace("5", ", ServiceName ");
                orderByClause = orderByClause.Replace("6", ", Labor_CName ");
                orderByClause = orderByClause.Replace("7", ", MNo ");
                orderByClause = orderByClause.Remove(0, 1);
            }
            else
            {
                orderByClause = "ID ASC";
            }
            orderByClause = "ORDER BY " + orderByClause;
            sb.Clear();

            var numberOfRowsToReturn = "";
            numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

            string query = @"declare @MA TABLE " +
                "( MNo nvarchar(20), Time_01 datetime, Type nvarchar(8), Type_Value nvarchar(1), Create_Name nvarchar(50), Cust_Name nvarchar(50), ServiceName nvarchar(20), Labor_CName nvarchar(20) ) " +
                "INSERT INTO @MA ( MNo, Time_01, Type, Type_Value, Create_Name, Cust_Name, ServiceName, Labor_CName ) " +
                "Select MNo, Time_01, Type, Type_Value, Create_Name, Cust_Name, ServiceName, Labor_CName " +
                //"FROM [LaborTemplate] {4} WHERE Cust_ID !=''" +
                "FROM [Faremma].[dbo].[LaborTemplate] {4} " +
                sqlstr +
                " SELECT * FROM " +
                "( SELECT row_number() OVER ({0}) AS RowNumber ,  * FROM " +
                "( SELECT ( SELECT count([@MA].MNo) FROM @MA) AS TotalRows , " +
                "( SELECT  count( [@MA].MNo) FROM @MA {1}) AS TotalDisplayRows , " +
                "[@MA].MNo ,[@MA].Time_01 as Time, [@MA].Type, [@MA].Type_Value, [@MA].Create_Name, [@MA].Cust_Name, [@MA].ServiceName, [@MA].Labor_CName " +
                "FROM @MA {1}) RawResults) Results " +
                "WHERE RowNumber BETWEEN {2} AND {3} ";
            query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);
            var connectionString = ConfigurationManager.ConnectionStrings["CMS_ENTConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

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
            string Time_01 = "";
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
                Time_01 = Convert.ToDateTime(data["Time"]).ToString("yyyy/MM/dd HH:mm");
                sb.AppendFormat(@"""0"": ""{0}""", Time_01);
                sb.Append(",");
                sb.AppendFormat(@"""1"": ""{0}""", data["MNo"].ToString().Trim());
                sb.Append(",");
                sb.AppendFormat(@"""2"": ""{0}""", data["Type"].ToString().Trim());
                sb.Append(",");
                sb.AppendFormat(@"""3"": ""{0}""", data["Create_Name"].ToString().Trim());
                sb.Append(",");
                sb.AppendFormat(@"""4"": ""{0}""", data["Cust_Name"].ToString().Trim());
                sb.Append(",");
                sb.AppendFormat(@"""5"": ""{0}""", data["ServiceName"].ToString().Trim());
                sb.Append(",");
                sb.AppendFormat(@"""6"": ""{0}""", data["Labor_CName"].ToString().Trim());
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
                sb.Clear();
                conn.Close();
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
            sb.Clear();
            conn.Close();
            return outputJson;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            logger.Info("Default_MNo ERROR");
            var sb = new StringBuilder();
            string outputJson = string.Empty;
            sb.Append("{");
            sb.Append(@"""sEcho"": 1");
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": 0");
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": 0");
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append("]}");
            outputJson = sb.ToString();
            sb.Clear();
            return outputJson;
        }
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    [ScriptMethod(UseHttpGet = true)]
    public string Default_CNo()
    {
        try
        {
            string CNo_Flag = System.Web.Configuration.WebConfigurationManager.AppSettings["CNo_Flag"].ToString();
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            string rawSearch = HttpContext.Current.Request.Params["sSearch"];
            string participant = HttpContext.Current.Request.Params["iParticipant"];
            string Flag = HttpContext.Current.Request.Params["Flag"];
            string Time_Flag = HttpContext.Current.Request.Params["Time_Flag"];

            //=============================================
            string Agent_LV = HttpContext.Current.Session["Agent_LV"].ToString();
            string Agent_ID = HttpContext.Current.Session["UserID"].ToString();
            string Team = HttpContext.Current.Session["Agent_Team"].ToString();
            //=============================================

            string sqlstr = "";
            var sb = new StringBuilder();
            var whereClause = string.Empty;
            string sql_time = "";
            switch (Time_Flag)
            {
                case "2":
                    sql_time = "";
                    break;
                case "1":
                    sql_time = " AND convert(char(6),StartTime,112) = convert(char(6),GETDATE(),112)";
                    break;
                default:
                    sql_time = " AND convert(char,StartTime,112) = convert(char,GETDATE(),112)";
                    break;
            };

            if (Flag == "6")
            {
                switch (Agent_LV)
                {
                    case "20":
                        sqlstr = "WHERE Type_Value != '5' AND Agent_Team = '" + Team + "'" + sql_time;
                        break;
                    case "30":
                        sqlstr = "WHERE Type_Value != '5'" + sql_time;
                        break;
                    default:
                        if (CNo_Flag == "0")
                        {
                            sqlstr = "WHERE Type_Value != '5' AND Agent_Team = '" + Team + "'" + sql_time;
                        }
                        else
                        {
                            sqlstr = "WHERE Type_Value != '5' AND Agent_ID = '" + Agent_ID + "'" + sql_time;
                        }
                        break;
                }
            };

            if (Flag == "8")
            {
                switch (Agent_LV)
                {
                    case "20":
                        sqlstr = "WHERE Agent_Team = '" + Team + "'" + sql_time;
                        break;
                    case "30":
                        sqlstr = "WHERE Type_Value != '99'" + sql_time;
                        break;
                    default:
                        if (CNo_Flag == "0")
                        {
                            sqlstr = "WHERE Agent_Team = '" + Team + "'" + sql_time;
                        }
                        else
                        {
                            sqlstr = "WHERE Agent_ID = '" + Agent_ID + "'" + sql_time;
                        }
                        break;
                }
            };

            if (Flag == "9")
            {
                switch (Agent_LV)
                {
                    case "20":
                        sqlstr = "WHERE Type_Value = '5' AND Agent_Team = '" + Team + "'" + sql_time;
                        break;
                    case "30":
                        sqlstr = "WHERE Type_Value = '5'" + sql_time;
                        break;
                    default:
                        if (CNo_Flag == "0")
                        {
                            sqlstr = "WHERE Type_Value = '5' AND Agent_Team = '" + Team + "'" + sql_time;
                        }
                        else
                        {
                            sqlstr = "WHERE Type_Value = '5' AND Agent_ID = '" + Agent_ID + "'" + sql_time;
                        }
                        break;
                }
            };

            if (participant.Length > 0)
            {
                sb.Append(" Where CNo like ");
                sb.Append("'%" + participant + "%'");
                whereClause = sb.ToString();
            }
            sb.Clear();

            var filteredWhere = string.Empty;

            var wrappedSearch = "'%" + rawSearch + "%'";

            if (rawSearch.Length > 0)
            {
                sb.Append(" WHERE CNo LIKE ");
                sb.Append(wrappedSearch);
                sb.Append(" OR StartTime LIKE ");
                sb.Append(wrappedSearch);
                sb.Append(" OR Type LIKE ");
                sb.Append(wrappedSearch);
                sb.Append(" OR Cust_Name LIKE ");
                sb.Append(wrappedSearch);
                sb.Append(" OR Danger LIKE ");
                sb.Append(wrappedSearch);
                sb.Append(" OR ServiceName LIKE ");
                sb.Append(wrappedSearch);
                sb.Append(" OR Agent_Name LIKE ");
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
                orderByClause = orderByClause.Replace("0", ", StartTime ");
                orderByClause = orderByClause.Replace("1", ", CNo ");
                orderByClause = orderByClause.Replace("2", ", Type_Value ");
                orderByClause = orderByClause.Replace("3", ", Agent_Name ");
                orderByClause = orderByClause.Replace("4", ", Cust_Name ");
                orderByClause = orderByClause.Replace("5", ", ServiceName ");
                orderByClause = orderByClause.Replace("6", ", Danger ");
                orderByClause = orderByClause.Replace("7", ", CNo ");
                orderByClause = orderByClause.Remove(0, 1);
            }
            else
            {
                orderByClause = "ID ASC";
            }
            orderByClause = "ORDER BY " + orderByClause;
            sb.Clear();

            var numberOfRowsToReturn = "";
            numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

            string query = @"declare @MA TABLE " +
                "( CNo nvarchar(20), StartTime datetime, Type nvarchar(8), Type_Value nvarchar(1), Danger nvarchar(4), Cust_Name nvarchar(50), ServiceName nvarchar(20), Agent_Name nvarchar(20) ) " +
                "INSERT INTO @MA ( CNo, StartTime, Type, Type_Value, Danger, Cust_Name, ServiceName, Agent_Name ) " +
                "Select CNo, StartTime, Type, Type_Value, Danger, Cust_Name, ServiceName, Agent_Name " +
                "FROM [Faremma].[dbo].[CASEDetail] {4} " +
                sqlstr +
                " SELECT * FROM " +
                "( SELECT row_number() OVER ({0}) AS RowNumber ,  * FROM " +
                "( SELECT ( SELECT count([@MA].CNo) FROM @MA) AS TotalRows , " +
                "( SELECT  count( [@MA].CNo) FROM @MA {1}) AS TotalDisplayRows , " +
                "[@MA].CNo, [@MA].StartTime, [@MA].Type, [@MA].Type_Value, [@MA].Danger, [@MA].Cust_Name, [@MA].ServiceName, [@MA].Agent_Name " +
                "FROM @MA {1}) RawResults) Results " +
                "WHERE RowNumber BETWEEN {2} AND {3} ";
            query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);
            var connectionString = ConfigurationManager.ConnectionStrings["CMS_ENTConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

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
            string Time_01 = "";
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
                Time_01 = Convert.ToDateTime(data["StartTime"]).ToString("yyyy/MM/dd HH:mm");
                sb.AppendFormat(@"""0"": ""{0}""", Time_01);
                sb.Append(",");
                sb.AppendFormat(@"""1"": ""{0}""", data["CNo"].ToString().Trim());
                sb.Append(",");
                sb.AppendFormat(@"""2"": ""{0}""", data["Type"].ToString().Trim());
                sb.Append(",");
                sb.AppendFormat(@"""3"": ""{0}""", data["Agent_Name"].ToString().Trim());
                sb.Append(",");
                sb.AppendFormat(@"""4"": ""{0}""", data["Cust_Name"].ToString().Trim());
                sb.Append(",");
                sb.AppendFormat(@"""5"": ""{0}""", data["ServiceName"].ToString().Trim());
                sb.Append(",");
                sb.AppendFormat(@"""6"": ""{0}""", data["Danger"].ToString().Trim());
                sb.Append(",");
                // 分割字串避免字串過長
                str_mno_1 = data["CNo"].ToString().Substring(0, 8);
                str_mno_2 = data["CNo"].ToString().Substring(8, data["CNo"].ToString().Length - 8);
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
                sb.Clear();
                conn.Close();
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
            sb.Clear();
            conn.Close();
            return outputJson;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            logger.Info("Default_CNo ERROR");
            var sb = new StringBuilder();
            string outputJson = string.Empty;
            sb.Append("{");
            sb.Append(@"""sEcho"": 1");
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": 0");
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": 0");
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append("]}");
            outputJson = sb.ToString();
            sb.Clear();
            return outputJson;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string Labor_System()
    {
        int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
        int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
        int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
        string rawSearch = HttpContext.Current.Request.Params["sSearch"];
        string participant = HttpContext.Current.Request.Params["iParticipant"];
        string Cust_ID = HttpContext.Current.Request.Params["Cust_ID"];
        string Flag = HttpContext.Current.Request.Params["Flag"];
        string Button_Value = "";
        var sb = new StringBuilder();
        var whereClause = string.Empty;

        if (Flag == "A")
        {
            Button_Value = ", 1);' data-dismiss='modal'><span class='glyphicon glyphicon-plus'></span>&nbsp;&nbsp;雇主</button>";
        }
        else
        {
            Button_Value = ");' data-toggle='modal' data-target='#myModal'><span class='glyphicon glyphicon-pencil'></span>&nbsp;&nbsp;修改</button>";
        }

        if (participant.Length > 0)
        {
            sb.Append(" Where Labor_ID like ");
            sb.Append("'%" + participant + "%'");
            whereClause = sb.ToString();
        }

        string sql_Cust_ID = "";
        if (Cust_ID.Length > 0)
        {
            sql_Cust_ID = "AND Cust_ID = '" + Cust_ID + "'";
        }
        sb.Clear();

        var filteredWhere = string.Empty;

        var wrappedSearch = "'%" + rawSearch + "%'";

        if (rawSearch.Length > 0)
        {
            sb.Append(" WHERE Cust_Name LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Cust_ID LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Labor_CName LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Labor_ID LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Labor_PID LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Labor_RID LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Labor_EID LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Labor_Phone LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Labor_Country LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Labor_Valid LIKE ");
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
            orderByClause = orderByClause.Replace("0", ", Cust_ID ");
            orderByClause = orderByClause.Replace("1", ", Cust_Name ");
            orderByClause = orderByClause.Replace("2", ", Labor_CName ");
            orderByClause = orderByClause.Replace("3", ", Labor_ID ");
            orderByClause = orderByClause.Replace("4", ", Labor_PID ");
            orderByClause = orderByClause.Replace("5", ", Labor_RID ");
            orderByClause = orderByClause.Replace("6", ", Labor_EID ");
            orderByClause = orderByClause.Replace("7", ", Labor_Phone ");
            orderByClause = orderByClause.Replace("8", ", Labor_Country ");
            orderByClause = orderByClause.Replace("9", ", Labor_Valid ");
            orderByClause = orderByClause.Replace("10", ", Labor_ID ");
            orderByClause = orderByClause.Replace("11", ", Labor_ID ");
            orderByClause = orderByClause.Remove(0, 1);
        }
        else
        {
            orderByClause = "ID ASC";
        }
        orderByClause = "ORDER BY " + orderByClause;
        sb.Clear();

        var numberOfRowsToReturn = "";
        numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

        string query = @"declare @MA TABLE " +
            "( Cust_Name nvarchar(50), " +
            "SYSID nvarchar(10), " +
            "Cust_ID nvarchar(20), " +
            "Labor_ID nvarchar(20), " +
            "Labor_CName nvarchar(20), " +
            "Labor_Country nvarchar(20), " +
            "Labor_PID nvarchar(20), " +
            "Labor_RID nvarchar(20), " +
            "Labor_EID nvarchar(20), " +
            "Labor_Valid nvarchar(10), " +
            "Labor_Phone nvarchar(20)) " +
            "INSERT INTO @MA ( Cust_Name, SYSID, Cust_ID, Labor_ID, Labor_CName, Labor_Country, Labor_PID, Labor_RID, Labor_EID,  Labor_Valid, Labor_Phone) " +
            "Select Cust_Name, SYSID, Cust_ID, Labor_ID, Labor_CName, Labor_Country, Labor_PID, Labor_RID, Labor_EID, Labor_Valid, Labor_Phone " +
            "FROM [Faremma].[dbo].[Labor_System] {4} " +
            " WHERE Cust_ID != '' AND Labor_Type = '任用' " +
            sql_Cust_ID +
            " SELECT * FROM " +
            "( SELECT row_number() OVER ({0}) AS RowNumber ,  * FROM " +
            "( SELECT ( SELECT count([@MA].Labor_ID) FROM @MA) AS TotalRows , " +
            "( SELECT  count( [@MA].Labor_ID) FROM @MA {1}) AS TotalDisplayRows , " +
            "[@MA].Labor_ID , [@MA].SYSID , [@MA].Cust_Name, [@MA].Cust_ID ,[@MA].Labor_CName ,[@MA].Labor_Country ,[@MA].Labor_PID ,[@MA].Labor_RID ,[@MA].Labor_EID ,[@MA].Labor_Valid ,[@MA].Labor_Phone " +
            "FROM @MA {1}) RawResults) Results " +
            "WHERE RowNumber BETWEEN {2} AND {3} ";
        query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);
        var connectionString = ConfigurationManager.ConnectionStrings["CMS_ENTConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionString);

        try
        {
            conn.Open();
        }
        catch (Exception e)
        {
            sb.Clear();
            conn.Close();
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
            sb.AppendFormat(@"""0"": ""{0}""", data["Cust_ID"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""1"": ""{0}""", data["Cust_Name"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""2"": ""{0}""", data["Labor_CName"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""3"": ""{0}""", data["Labor_ID"]).ToString().Trim();
            sb.Append(",");
            sb.AppendFormat(@"""4"": ""{0}""", data["Labor_PID"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""5"": ""{0}""", data["Labor_RID"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""6"": ""{0}""", data["Labor_EID"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""7"": ""{0}""", data["Labor_Phone"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""8"": ""{0}""", data["Labor_Country"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""9"": ""{0}""", data["Labor_Valid"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""10"": ""{0}""", "<button id='but_url' type='button' class='btn btn-primary btn-lg' onclick='Labor_add(" + data["SYSID"] + Button_Value);
            sb.Append(",");
            sb.AppendFormat(@"""11"": ""{0}""", "<button id='but_url' type='button' class='btn btn-info btn-lg' onclick='Labor_add(" + data["SYSID"] + ", 2);' data-dismiss='modal'>" +
            "<span class='glyphicon glyphicon-plus'>" +
            "</span>&nbsp;&nbsp;勞工</button>");
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
            sb.Clear();
            conn.Close();
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
        sb.Clear();
        conn.Close();
        return outputJson;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string Cust_System()
    {
        int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
        int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
        int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
        string rawSearch = HttpContext.Current.Request.Params["sSearch"];
        string participant = HttpContext.Current.Request.Params["iParticipant"];
        string Cust_ID = HttpContext.Current.Request.Params["Cust_ID"];
        var sb = new StringBuilder();
        var whereClause = string.Empty;

        if (participant.Length > 0)
        {
            sb.Append(" Where Cust_ID like ");
            sb.Append("'%" + participant + "%'");
            whereClause = sb.ToString();
        }

        string sql_Cust_ID = "";
        if (Cust_ID.Length > 0)
        {
            sql_Cust_ID = "AND Cust_ID = '" + Cust_ID + "'";
        }
        sb.Clear();

        var filteredWhere = string.Empty;

        var wrappedSearch = "'%" + rawSearch + "%'";

        if (rawSearch.Length > 0)
        {
            sb.Append(" WHERE Cust_ID LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Cust_FullName LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Cust_Postcode LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Cust_Address_ZH LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Cust_TEL LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Cust_TaxID LIKE ");
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
            orderByClause = orderByClause.Replace("0", ", Cust_ID ");
            orderByClause = orderByClause.Replace("1", ", Cust_FullName ");
            orderByClause = orderByClause.Replace("2", ", Cust_Postcode ");
            orderByClause = orderByClause.Replace("3", ", Cust_Address_ZH ");
            orderByClause = orderByClause.Replace("4", ", Cust_TEL ");
            orderByClause = orderByClause.Replace("5", ", Cust_TaxID ");
            orderByClause = orderByClause.Replace("6", ", SYS_ID ");
            orderByClause = orderByClause.Remove(0, 1);
        }
        else
        {
            orderByClause = "ID ASC";
        }
        orderByClause = "ORDER BY " + orderByClause;
        sb.Clear();

        var numberOfRowsToReturn = "";
        numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

        string query = @"declare @MA TABLE " +
            "( Cust_ID nvarchar(20), " +
            "SYS_ID int, " +
            "Cust_FullName nvarchar(40), " +
            "Cust_Postcode nvarchar(10), " +
            "Cust_Address_ZH nvarchar(250), " +
            "Cust_TEL nvarchar(30), " +
            "Cust_TaxID nvarchar(20)) " +
            "INSERT INTO @MA ( Cust_ID, SYS_ID, Cust_FullName, Cust_Postcode, Cust_Address_ZH, Cust_TEL, Cust_TaxID ) " +
            "Select  Cust_ID, SYS_ID, Cust_FullName, Cust_Postcode, Cust_Address_ZH, Cust_TEL, Cust_TaxID " +
            "FROM [Faremma].[dbo].[HRM2_Cust_System] {4} " +
            " WHERE Cust_ID != '' " +
            sql_Cust_ID +
            " SELECT * FROM " +
            "( SELECT row_number() OVER ({0}) AS RowNumber ,  * FROM " +
            "( SELECT ( SELECT count([@MA].Cust_ID) FROM @MA) AS TotalRows , " +
            "( SELECT  count( [@MA].Cust_ID) FROM @MA {1}) AS TotalDisplayRows , " +
            "[@MA].Cust_ID , [@MA].SYS_ID , [@MA].Cust_FullName , [@MA].Cust_Postcode, [@MA].Cust_Address_ZH ,[@MA].Cust_TEL ,[@MA].Cust_TaxID " +
            "FROM @MA {1}) RawResults) Results " +
            "WHERE RowNumber BETWEEN {2} AND {3} ";
        query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);
        var connectionString = ConfigurationManager.ConnectionStrings["CMS_ENTConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionString);

        try
        {
            conn.Open();
        }
        catch (Exception e)
        {
            sb.Clear();
            conn.Close();
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
            sb.AppendFormat(@"""0"": ""{0}""", data["Cust_ID"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""1"": ""{0}""", data["Cust_FullName"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""2"": ""{0}""", data["Cust_Postcode"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""3"": ""{0}""", data["Cust_Address_ZH"]).ToString().Trim();
            sb.Append(",");
            sb.AppendFormat(@"""4"": ""{0}""", data["Cust_TEL"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""5"": ""{0}""", data["Cust_TaxID"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""6"": ""{0}""", "<button id='but_url' type='button' class='btn btn-success btn-lg' onclick='Add_Location(" + data["SYS_ID"] + ", 1);' data-dismiss='modal'>" +
            "<span class='glyphicon glyphicon-ok'>" +
            "</span>&nbsp;&nbsp;選擇</button>");
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
            sb.Clear();
            conn.Close();
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
        sb.Clear();
        conn.Close();
        return outputJson;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string ServiceName()
    {
        string Service = HttpContext.Current.Request.Params["Service"].ToString().Trim();
        var sb = new StringBuilder();
        string query = @"SELECT Service_ID ,ServiceName FROM Data WHERE Open_Flag != '0' AND Service !='外包商' AND Service = '" + Service + "'";
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
        string outputJson = string.Empty;

        sb.Append("{");
        sb.Append(@"""Table"": [ ");
        while (data.Read())
        {
            sb.Append("{");
            sb.AppendFormat(@"""stateid"": ""{0}""", data["Service_ID"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""statename"": ""{0}""", data["ServiceName"].ToString().Trim());
            sb.Append("}");
            sb.Append(",");
        }
        sb = new StringBuilder(sb.ToString().TrimEnd(','));
        sb.Append("]}");
        outputJson = sb.ToString();
        sb.Clear();
        return outputJson;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string Table_CNo()
    {
        int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
        int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
        int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
        string rawSearch = HttpContext.Current.Request.Params["sSearch"];
        string participant = HttpContext.Current.Request.Params["iParticipant"];
        var sb = new StringBuilder();
        var whereClause = string.Empty;

        if (participant.Length > 0)
        {
            sb.Append(" Where CNo like ");
            sb.Append("'%" + participant + "%'");
            whereClause = sb.ToString();
        }
        sb.Clear();

        var filteredWhere = string.Empty;

        var wrappedSearch = "'%" + rawSearch + "%'";

        sb.Append(" WHERE Type_Value = '5' ");
        if (rawSearch.Length > 0)
        {
            sb.Append(" AND CNo LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Type LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Service LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR ServiceName LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Cust_Name LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Agent_Name LIKE ");
            sb.Append(wrappedSearch);
            sb.Append(" OR Agent_Team LIKE ");
            sb.Append(wrappedSearch);

        }

        filteredWhere = sb.ToString();
        sb.Clear();
        string orderByClause = string.Empty;
        string value = string.Empty;
        sb.Append(ToInt(HttpContext.Current.Request.Params["iSortCol_0"]));
        sb.Append(" ");
        sb.Append(HttpContext.Current.Request.Params["sSortDir_0"]);
        orderByClause = sb.ToString();
        value = ToInt(HttpContext.Current.Request.Params["iSortCol_0"]).ToString();
        if (!String.IsNullOrEmpty(orderByClause))
        {
            switch (value)
            {
                case "10":
                    orderByClause = orderByClause.Replace("10", ", Time_04 ");
                    break;
                case "11":
                    orderByClause = orderByClause.Replace("11", ", Time_05 ");
                    break;
                default:
                    orderByClause = orderByClause.Replace("0", ", CNo ");
                    orderByClause = orderByClause.Replace("1", ", Type ");
                    orderByClause = orderByClause.Replace("2", ", Service ");
                    orderByClause = orderByClause.Replace("3", ", ServiceName ");
                    orderByClause = orderByClause.Replace("4", ", Cust_Name ");
                    orderByClause = orderByClause.Replace("5", ", Agent_Name ");
                    orderByClause = orderByClause.Replace("6", ", Agent_Team ");
                    orderByClause = orderByClause.Replace("7", ", Time_01 ");
                    orderByClause = orderByClause.Replace("8", ", Time_02 ");
                    orderByClause = orderByClause.Replace("9", ", Time_03 ");
                    break;
            }
            orderByClause = orderByClause.Remove(0, 1);
        }
        else
        {
            orderByClause = "Time_01 DESC";
        }
        orderByClause = "ORDER BY " + orderByClause;
        sb.Clear();
        var numberOfRowsToReturn = "";
        numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

        string query = @"SELECT * FROM " +
            "( SELECT row_number() OVER ({0}) AS RowNumber ,  * FROM " +
            "( SELECT ( SELECT count(SYSID) FROM CASEDetail {1}) AS TotalRows , " +
            "( SELECT  count(SYSID) FROM CASEDetail {1}) AS TotalDisplayRows , " +
            "CNo, Type, Service, ServiceName, Cust_Name, Agent_Name, Agent_Team , " +

            //======================== 派單 → 接單 ========================
            "DATEDIFF(MINUTE, CREATE_DATE, OpenDate) as Time_01, " +

            //======================== 接單 → 到點 ========================
            "DATEDIFF(MINUTE, OpenDate, UpdateDate) as Time_02, " +

            //======================== 到點 → 完成 ========================
            "DATEDIFF(MINUTE, UpdateDate, LastUpdateDate) as Time_03, " +

            //======================== 完成 → 暫結案 ========================
            "DATEDIFF(MINUTE, LastUpdateDate, FinalUpdateDate) as Time_04, " +

             //======================== 完成 → 暫結案 ========================
            "DATEDIFF(MINUTE, CREATE_DATE, FinalUpdateDate) as Time_05 " +

            "FROM CASEDetail {1}) RawResults) Results " +
            "WHERE RowNumber BETWEEN {2} AND {3} ";
        query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);
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
        while (data.Read())
        {
            // CNo, Type, Service, ServiceName, Cust_Name, Agent_Name, Agent_Team
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
            sb.AppendFormat(@"""0"": ""{0}""", data["CNo"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""1"": ""{0}""", data["Type"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""2"": ""{0}""", data["Service"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""3"": ""{0}""", data["ServiceName"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""4"": ""{0}""", data["Cust_Name"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""5"": ""{0}""", data["Agent_Name"].ToString().Trim());
            sb.Append(",");
            sb.AppendFormat(@"""6"": ""{0}""", data["Agent_Team"].ToString().Trim());
            sb.Append(",");
            if (data["Time_01"].ToString().Trim() == "")
            {
                sb.AppendFormat(@"""7"": ""{0}""", "尚無時間紀錄");
            }
            else
            {
                sb.AppendFormat(@"""7"": ""{0}""",
                    ((int)decimal.Parse(data["Time_01"].ToString().Trim()) / 1440).ToString() + "天 " +
                    (((int)decimal.Parse(data["Time_01"].ToString().Trim()) % 1440) / 60).ToString().PadLeft(2, '0') + "時 " +
                    ((int)decimal.Parse(data["Time_01"].ToString().Trim()) % 60).ToString().PadLeft(2, '0') + "分");
            }
            sb.Append(",");
            if (data["Time_02"].ToString().Trim() == "")
            {
                sb.AppendFormat(@"""8"": ""{0}""", "尚無時間紀錄");
            }
            else
            {
                sb.AppendFormat(@"""8"": ""{0}""",
                    ((int)decimal.Parse(data["Time_02"].ToString().Trim()) / 1440).ToString() + "天 " +
                    (((int)decimal.Parse(data["Time_02"].ToString().Trim()) % 1440) / 60).ToString().PadLeft(2, '0') + "時 " +
                    ((int)decimal.Parse(data["Time_02"].ToString().Trim()) % 60).ToString().PadLeft(2, '0') + "分");
            }
            sb.Append(",");
            if (data["Time_03"].ToString().Trim() == "")
            {
                sb.AppendFormat(@"""9"": ""{0}""", "尚無時間紀錄");
            }
            else
            {
                sb.AppendFormat(@"""9"": ""{0}""",
                    ((int)decimal.Parse(data["Time_03"].ToString().Trim()) / 1440).ToString() + "天 " +
                    (((int)decimal.Parse(data["Time_03"].ToString().Trim()) % 1440) / 60).ToString().PadLeft(2, '0') + "時 " +
                    ((int)decimal.Parse(data["Time_03"].ToString().Trim()) % 60).ToString().PadLeft(2, '0') + "分");
            }
            sb.Append(",");
            if (data["Time_04"].ToString().Trim() == "")
            {
                sb.AppendFormat(@"""10"": ""{0}""", "尚無時間紀錄");
            }
            else
            {
                sb.AppendFormat(@"""10"": ""{0}""",
                    ((int)decimal.Parse(data["Time_04"].ToString().Trim()) / 1440).ToString() + "天 " +
                    (((int)decimal.Parse(data["Time_04"].ToString().Trim()) % 1440) / 60).ToString().PadLeft(2, '0') + "時 " +
                    ((int)decimal.Parse(data["Time_04"].ToString().Trim()) % 60).ToString().PadLeft(2, '0') + "分");
            }
            sb.Append(",");
            if (data["Time_05"].ToString().Trim() == "")
            {
                sb.AppendFormat(@"""11"": ""{0}""", "尚無時間紀錄");
            }
            else
            {
                sb.AppendFormat(@"""11"": ""{0}""",
                    ((int)decimal.Parse(data["Time_05"].ToString().Trim()) / 1440).ToString() + "天 " +
                    (((int)decimal.Parse(data["Time_05"].ToString().Trim()) % 1440) / 60).ToString().PadLeft(2, '0') + "時 " +
                    ((int)decimal.Parse(data["Time_05"].ToString().Trim()) % 60).ToString().PadLeft(2, '0') + "分");
            }
            sb.Append("},");
        }

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
            sb.Clear();
            conn.Close();
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
        sb.Clear();
        conn.Close();
        return outputJson;
    }

    public static int ToInt(string toParse)
    {
        int result;
        if (int.TryParse(toParse, out result)) return result;

        return result;
    }
}
