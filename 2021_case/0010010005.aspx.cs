using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _2021_case_0010010005 : System.Web.UI.Page
{
    protected string seqno = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        seqno = Request.Params["seqno"];
        //string Agent_SYSID = HttpContext.Current.Session["SYSID"].ToString();
    }

    [WebMethod(EnableSession = true)]
    public static string bindtable(string Start_Date, string End_Date, string Personel)
    {
        var result = Case_List.Search(Start_Date,End_Date, Personel);
        return JsonConvert.SerializeObject(result);
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Load()
    {
        string Agent_ID;
        Agent_ID = HttpContext.Current.Session["UserID"].ToString();
        if (!string.IsNullOrEmpty(Agent_ID)) //如果不等於空清除空白處, 空的就空字串
        {
            Agent_ID = Agent_ID.Trim();
        }
        else
        {
            Agent_ID = "";
        }
        string Sqlstr = @"SELECT TOP 1 * FROM DispatchSystem WHERE Agent_ID = @Agent_ID AND Agent_Status != '離職'";      // 員工名單內且未離職
        var a = DBTool.Query<ClassTemplate>(Sqlstr, new { Agent_ID = Agent_ID }).ToList().Select(p => new
        {
            Agent_Team = p.Agent_Team,
            Agent_Name = p.Agent_Name,
            Agent_ID = p.Agent_ID,
            SYSID = p.SYSID,
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]
    public static string Load_Case(string SYSID)
    {
        string sqlCommand = string.Format(@"SELECT * FROM Case_List WHERE SYSID = N'{0}'", SYSID);
        var data = DBTool.Query<Case_List>(sqlCommand).ToList();
        string outputJson = JsonConvert.SerializeObject(data);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]
    public static string Load_Search(string SYSID)
    {
        string sqlCommand = string.Format(@"SELECT * FROM Case_List WHERE SYSID = N'{0}'", SYSID);
        var data = DBTool.Query<ClassTemplate>(sqlCommand).ToList();
        string outputJson = JsonConvert.SerializeObject(data);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string List_Assign_title(string SYSID)
    {
        string sqlstr = "SELECT * FROM Assign_Case WHERE Status='1' AND Case_SYSID = '"+ SYSID + "'";
        var data = DBTool.Query<Assign_List>(sqlstr).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Assign_title = p.Assign_title,
        });

        string outputJson = JsonConvert.SerializeObject(data);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string List_Customer()
    {
        string sqlstr = "SELECT BUSINESSNAME,ID,APPNAME,APP_MTEL FROM BusinessData WHERE Type='保留'";
        var data = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new
        {
            BUSINESSNAME = p.BUSINESSNAME,
            ID = p.ID,
            APPNAME = p.APPNAME,
            APP_MTEL = p.APP_MTEL,
        });

        string outputJson = JsonConvert.SerializeObject(data);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string List_Personel()
    {
        string sqlstr = "SELECT Personnel FROM Case_List";
        var data = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new
        {
            Personnel = p.Personnel,
        });

        data = data.Distinct().ToList();

        string outputJson = JsonConvert.SerializeObject(data);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string List_Commodity()
    {
        string sqlstr = "SELECT Product_Name,Unit_Price,OE_ID FROM OE_Product";
        var data = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new
        {
            Product_Name = p.Product_Name,
            Unit_Price = p.Unit_Price,
            OE_ID = p.OE_ID,
        });

        string outputJson = JsonConvert.SerializeObject(data);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Safe(
        string Case_SYSID, string Type,string Case_Name,string Clinet_Name, string Contact, string Phone, string System_Data, string System_Data_ID, string Work_List, string Total_Price, string Personnel, string Assit_Company, string Project_Content, string Remark)
    {
        string sqlstr = "";
        sqlstr = @"SELECT Case_Name FROM Case_List WHERE Case_Name = '"+Case_Name+"'";
        var data = DBTool.Query<Case_List>(sqlstr).ToList();
        if (data.Any())
        {
            return JsonConvert.SerializeObject(new { status = "已有相同的案件名稱。" });
        }

        try
        {
            if (Type == "0")
            {
                sqlstr = @"INSERT INTO Case_List(Case_Name,Clinet_Name, Contact , Phone, System_Data, System_Data_ID, Work_List, Total_price, Personnel,Assist_Company, Project_Content, Remark,Status)" +
                "VALUES('" + Case_Name + "','" + Clinet_Name + "', '" + Contact + "', '" + Phone + "', '" + System_Data + "','" + System_Data_ID + "', '" + Work_List + "', '" + Total_Price + "', '" + Personnel + "','" + Assit_Company + "','" + Project_Content + "','" + Remark + "','0')";
                DBTool.Query(sqlstr);
                return JsonConvert.SerializeObject(new { status = "案件新增完成。" });
            }
            else
            {
                sqlstr = @"UPDATE Case_List SET Case_Name = '" + Case_Name + "',Clinet_Name='" + Clinet_Name + "',Contact='" + Contact + "'," +
                    "Phone='" + Phone + "',System_Data='" + System_Data + "',System_Data_ID='" + System_Data_ID + "',Work_List='" + Work_List + "',Total_price='" + Total_Price + "',Personnel= '" + Personnel + "'," +
                    "Assist_Company='" + Assit_Company + "',Project_Content='" + Project_Content + "',Remark='" + Remark + "'WHERE SYSID = '" + Case_SYSID + "'";
                DBTool.Query(sqlstr);
                return JsonConvert.SerializeObject(new { status = "案件修改完成。" });
            }
        }catch(Exception ex)
        {
            sqlstr = "INSERT INTO System_Log(Funtion_Name,File_Owner,Error,trace) VALUES('Safe','0010010005.aspx','"+ex.Message+"','"+ex.StackTrace+"')";
            DBTool.Query(sqlstr);
            return JsonConvert.SerializeObject(new { status = "伺服器異常，請聯絡管理員。" });
        }
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string List_Work(string OE_ID)
    {
        string sqlstr = @"SELECT Work_List FROM OE_Product WHERE OE_ID = '{0}'";
        string sql_format = string.Format(sqlstr, OE_ID);
        var data = DBTool.Query<ClassTemplate>(sql_format).ToList().Select(p => new
        {
            Work_List = p.Work_List,
        });

        string outputJson = JsonConvert.SerializeObject(data);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Get_Work_List(string Single_Product_Name)
    {
        string sqlstr = @"SELECT Work_List FROM OE_Product WHERE Product_Name = '{0}'";
        string sql_format = string.Format(sqlstr, Single_Product_Name);
        var data = DBTool.Query<ClassTemplate>(sql_format).ToList().Select(p => new
        {
            Work_List = p.Work_List,
        });

        string outputJson = JsonConvert.SerializeObject(data);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string GetWorkLogs(string SYSID)
    {
        string sqlstr = "SELECT Create_Agent, Work_Log, Create_Date, SYSID, Work_Name FROM Case_Work_Log WHERE Case_Owner='{0}' order by Create_Date desc";
        string sqlCommand = string.Format(sqlstr,SYSID);
        var data = DBTool.Query<WorkLog>(sqlCommand).ToList();
        string outputJson = JsonConvert.SerializeObject(data);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Add_Work_Log(string SYSID,string select_ListCase, string txt_Work_Log,string btn_html,string Work_Log_SYSID)
    {
        if (btn_html == "修改")
        {
            string sqlstr2 = @"UPDATE Case_Work_Log SET Work_Log = '{0}',Update_Date = GetDate() WHERE SYSID = '{1}'";
            string sql_format2 = string.Format(sqlstr2,txt_Work_Log, Work_Log_SYSID);

            DBTool.Query(sql_format2);

            return JsonConvert.SerializeObject(new { status = "服務紀錄修改完成。" });
        }

        string Agent_SYSID = HttpContext.Current.Session["SYSID"].ToString();
        string Agent_Name = HttpContext.Current.Session["UserIDNAME"].ToString();
        string sqlstr = @"Insert Into Case_Work_Log (Case_Owner,Create_ID, Create_Agent, Work_Log, Work_Name) Values('{0}', '{1}','{2}', '{3}','{4}')";
        string sql_format = string.Format(sqlstr, SYSID, Agent_SYSID, Agent_Name, txt_Work_Log, select_ListCase);

        DBTool.Query(sql_format);

        return JsonConvert.SerializeObject(new { status = "服務紀錄新增完成。" });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string List_Depart()
    {
        string sqlstr = @"SELECT Agent_Team FROM DispatchSystem";
        var a = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new
        {
            Agent_Team = p.Agent_Team,
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string List_people(string Depart)
    {
        string sqlstr = "";
        if (Depart == "")
        {
            sqlstr = "SELECT SYSID,Agent_Name FROM DispatchSystem WHERE Agent_Status != '離職'";
        }
        else
        {
            sqlstr = @"SELECT SYSID,Agent_Name FROM DispatchSystem WHERE Agent_Status != '離職' AND Agent_Team = '{0}'";
        }
        string sql_format = string.Format(sqlstr, Depart);
        var a = DBTool.Query<ClassTemplate>(sql_format).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Agent_Name = p.Agent_Name,
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string List_Assist_Company()
    {
        string sqlstr = @"SELECT SYSID,Vendor_Name FROM Vendor_Data";
        var a = DBTool.Query<Vendor_List>(sqlstr).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Vendor_Name = p.Vendor_Name,
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Assign_To_People(
        string Assign_Depart,
        string Select_Assign_People, 
        string Urgent, 
        string Assign_Company_Connection,
        string Assign_Company_Phone,
        string Assign_title,
        string Assign_text,
        string End_date,
        string Assign_Create_Agent,
        string Case_SYSID)
    {
        string sqlstr = @"INSERT INTO Assign_Case (
            [Assign_Depart]
          ,[Assign_People]
          ,[Urgent]
          ,[Assign_Company_Connection]
          ,[Assign_Company_Phone]
          ,[Assign_title]
          ,[Assign_text]
          ,[End_Date]
          ,[Assign_Create_Agent]
          ,Case_SYSID
          ,Status
        ) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',0)";
        string sql_format = string.Format(sqlstr, Assign_Depart, Select_Assign_People, Urgent, Assign_Company_Connection, Assign_Company_Phone, Assign_title, Assign_text, End_date, Assign_Create_Agent, Case_SYSID);

        var a = DBTool.Query(sql_format);
        return JsonConvert.SerializeObject(new { status = "交辦成功。" });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Assign_Case_List(string SYSID)
    {
        string Agent_SYSID = HttpContext.Current.Session["SYSID"].ToString();
        string sqlstr = "SELECT a.*,b.Agent_Name FROM Assign_Case a left join DispatchSystem b on a.Assign_People = b.SYSID WHERE a.Case_SYSID = '{0}'";
        string sql_format = string.Format(sqlstr, SYSID);
        var data = DBTool.Query<Assign_List>(sql_format).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Assign_Depart = p.Assign_Depart,
            Agent_Name = p.Agent_Name,
            Assign_People = p.Assign_People,
            Urgent = p.Urgent,
            Assign_Company_Connection = p.Assign_Company_Connection,
            Assign_Company_Phone = p.Assign_Company_Phone,
            Assign_title = p.Assign_title,
            Assign_text = p.Assign_text,
            Create_Date = p.Create_Date,
            End_date = p.End_date.ToString("yyyy-MM-dd"),
            Assign_Create_Agent = p.Assign_Create_Agent,
            Status = p.Status,
            Chargeback_Content = p.Chargeback_Content,
        });

        string outputJson = JsonConvert.SerializeObject(data);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Change_Assign_Status(int Set,string Assign_SYSID,string Chargeback_text)
    {
        string sqlstr = "";
        string sql_format = "";
        if (Set == 1)
        {
            sqlstr = @"UPDATE Assign_Case SET Status = '{0}' WHERE SYSID = '{1}'";
            sql_format = string.Format(sqlstr, Set, Assign_SYSID);
        }
        else if (Set == -1)
        {
            sqlstr = @"UPDATE Assign_Case SET Status = '{0}',Chargeback_Content='{1}' WHERE SYSID = '{2}'";
            sql_format = string.Format(sqlstr, Set, Chargeback_text, Assign_SYSID);
        }

        var a = DBTool.Query(sql_format);
        if (Set == 1)
        {
            return JsonConvert.SerializeObject(new { status = "接單成功。" });
        }
        else if (Set == -1)
        {
            return JsonConvert.SerializeObject(new { status = "退單成功。" });
        }
        return JsonConvert.SerializeObject(new { status = "交辦成功。" });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Add_Work_List(string OE_ID,string Work_Name)
    {
        string sqlstr;
        sqlstr = "SELECT Work_List FROM OE_Product WHERE OE_ID = '"+OE_ID+"'";
        var a = DBTool.Query<OE_Work>(sqlstr).ToList().Select(p => new
        {
            Work_List = p.Work_List,
        }).FirstOrDefault();

        string[] Work_List = a.Work_List.Split(',');
        List<string> b = Work_List.ToList();
        b.Add(Work_Name);
        Work_List = b.ToArray();
        string str_Work_List = string.Join(",", Work_List);
        sqlstr = "UPDATE OE_Product SET Work_List = '"+ str_Work_List + "' WHERE OE_ID = '"+ OE_ID + "'";
        DBTool.Query(sqlstr);
        return JsonConvert.SerializeObject(new { status = "工作事項新增完成。" });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Change_Status(string Status,string Case_SYSID,string End_Reason)
    {
        string sqlstr;
        if (End_Reason == "")
        {
            sqlstr = "UPDATE Case_List SET Status = '" + Status + "' WHERE SYSID = '" + Case_SYSID + "'";
        }
        else
        {
            sqlstr = "UPDATE Case_List SET Status = '" + Status + "', End_Reason = '"+ End_Reason + "' WHERE SYSID = '" + Case_SYSID + "'";
        }
        
        DBTool.Query(sqlstr);
        return JsonConvert.SerializeObject(new { status = "案件狀態更改完成。" });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string test(string Status, string Case_SYSID, string End_Reason)
    {
        return JsonConvert.SerializeObject(new { status = "案件狀態更改完成。" });
    }



    public class inputList
    {
        public string txt_caseName { get; set; }
        public string select_caseList { get; set; }
        public string txt_contactPerson { get; set; }
        public string txt_contactPhoneNumber { get; set; }
        public string txt_Personnel { get; set; }
        public string List_system { get; set; }
        public string select_Assist_Company { get; set; }
        public string txt_projectContext { get; set; }
        public string txt_projectRemark { get; set; }
        
    }

    public class WorkLog
    {
        public string Work_Log { get; set; }
        public string Work_Name { get; set; }
        public string Create_Agent { get; set; }
        public string Create_Date { get; set; }
        public int SYSID { get; set; }
    }

    public class OE_Work
    {
        public string Work_List { get; set; }
    }

    public class Vendor_List
    {
        public int SYSID { get; set; }
        public string Vendor_Name { get; set; }
    }

    public class Assign_List
    {
        public string SYSID { get; set; }
        public string Assign_Depart { get; set; }
        public string Assign_People { get; set; }
        public string Agent_Name { get; set; }
        public string Urgent { get; set; }
        public string Assign_Company_Connection { get; set; }
        public string Assign_Company_Phone { get; set; }
        public string Assign_title { get; set; }
        public string Assign_text { get; set; }
        public string Create_Date { get; set; }
        public DateTime End_date { get; set; }
        public string Assign_Create_Agent { get; set; }
        public string Status { get; set; }
        public string Chargeback_Content{ get; set; }
    }

}