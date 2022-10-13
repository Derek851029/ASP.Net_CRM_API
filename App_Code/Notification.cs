using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Notification 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class Notification : System.Web.Services.WebService
{

    public Notification()
    {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }

    [WebMethod]
    public string Login(string account,string password)
    {
        string sqlstr = @"SELECT SYSID, Agent_ID, Agent_Name, Agent_Company, Agent_Team, Agent_Mail, Agent_Phone, Role_ID, Agent_LV, Password FROM DispatchSystem WHERE UserID = '{0}' AND Agent_Status != '離職'";
        string sql_format = string.Format(sqlstr, account);
        var a = DBTool.Query<Login_class>(sql_format).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Password = p.Password,
        });

        password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5").ToUpper();

        if (a.Any())
        {
            if (a.FirstOrDefault().Password == password)
            {
                return a.FirstOrDefault().SYSID;
            }
            else
            {
                return "使用者名稱或密碼錯誤，請重新輸入。";
            }
            
        }
        else
        {
            return "使用者名稱不存在。";
        }
    }
    [WebMethod]
    public string Get_Schedule(string Agent_SYSID)
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string tomorrow = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        string sqlstr = @"SELECT SYSID,Visit_Customer,Visit_Content,Visit_Date FROM Visit_Data WHERE Create_Agent = '{0}' AND Visit_Date > '{1}' AND Visit_Date < '{2}' order by Visit_Date desc";
        string sql_format = string.Format(sqlstr, Agent_SYSID, today, tomorrow);
        var a = DBTool.Query<Calendar>(sql_format).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Visit_Customer = p.Visit_Customer,
            Visit_Content = p.Visit_Content,
            Visit_Date = p.Visit_Date
        });
        return JsonConvert.SerializeObject(a);
    }

    [WebMethod]
    public string Update_Device_token(string SYSID, string token,string os)
    {
        string sqlstr;
        string sql_format;
        sqlstr = @"SELECT Agent_SYSID FROM Device_token WHERE Agent_SYSID = '"+ SYSID + "' AND OS = '"+os+"'";
        var a = DBTool.Query<Calendar>(sqlstr).ToList().Select(p => new
        {
            Agent_SYSID = p.Agent_SYSID,
        });
        if (a.Any())
        {
            sqlstr = @"UPDATE Device_token SET token='{0}',Login_Date=getDate() WHERE Agent_SYSID = '{1}'";
            sql_format = string.Format(sqlstr, token, SYSID);
            DBTool.Query(sql_format);
        }
        else
        {
            sqlstr = @"INSERT INTO Device_token(token,Agent_SYSID,OS) VALUES('{0}','{1}','{2}')";
            sql_format = string.Format(sqlstr, token, SYSID,os);
            DBTool.Query(sql_format);

        }
        return "success";
    }

    public class Login_class
    {
        public string Password { get; set; }
        public string SYSID { get; set; }
    }
        public class Calendar
    {
        public string Agent_SYSID { get; set; }
        public string Agent_Name { get; set; }
        public string SYSID { get; set; }
        public string Case_SYSID { get; set; }
        public string Case_Name { get; set; }
        public string Visit_Customer { get; set; }
        public string Visit_Customer_ID { get; set; }
        public string Visit_Person { get; set; }
        public string Create_Agent { get; set; }
        public string Visit_Phone { get; set; }
        public string Visit_Content { get; set; }
        public string Create_Date { get; set; }
        public DateTime Visit_Date { get; set; }
        public DateTime Visit_Leave_Date { get; set; }
        public string Status { get; set; }

    }

}
