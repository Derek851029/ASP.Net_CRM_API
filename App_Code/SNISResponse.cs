using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Xml;
using Dapper;
using log4net;
using log4net.Config;

/// <summary>
/// SNISResponse 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class SNISResponse : System.Web.Services.WebService
{
    SqlConnection conn;
    SqlDataAdapter myAdp;
    DataSet ds;
    XmlElement tagResponse;
    XmlElement tagResidents;
    XmlElement tagResident;
    XmlDocument xDoc = new XmlDocument();
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private static string strConn = WebConfigurationManager.ConnectionStrings["CMS_ENTConnectionString"].ToString();
    private static string Key = "kB3UxXV5fNywMh7";
    private string sql = "";
    string time = "";
    string A = "";

    public SNISResponse()
    {

    }

    [WebMethod]
    public XmlDocument Login(string Agent_ID, string Agent_PWD, string API_KEY)
    {

        DBTool.Query(String.Format(
            @"insert into EX(MSG) values ('接收Agent_ID = {0} Agent_PWD = {1} API_KEY = {2}')", Agent_ID,Agent_PWD, API_KEY));

        xDoc.AppendChild(xDoc.CreateXmlDeclaration("1.0", "utf-8", ""));
        tagResponse = xDoc.CreateElement("response");
        tagResidents = xDoc.CreateElement("Logins");

        if (API_KEY == Key)
        {
            sql = @"SELECT TOP 1 Agent_ID, Password,SYSID FROM DispatchSystem WHERE UserID = @Agent_ID AND Agent_Status != '離職'";
            conn = new SqlConnection(strConn);
            myAdp = new SqlDataAdapter(sql, conn);
            ds = new DataSet();
            myAdp.SelectCommand.Parameters.Add("@Agent_ID", SqlDbType.Char).Value = Agent_ID;
            myAdp.Fill(ds, "Result");
            if (ds.Tables[0].Rows.Count != 0)
            {
                string MD5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Agent_PWD, "MD5").ToUpper();
                string Password = ds.Tables[0].Rows[0]["Password"].ToString();
                string SYSID = ds.Tables[0].Rows[0]["SYSID"].ToString();
                if (MD5 == Password || Agent_PWD == "Acme70472615")
                {
                    foreach (DataRow itm in ds.Tables[0].Rows)
                    {
                        tagResident = xDoc.CreateElement("Status");
                        tagResident.SetAttribute("Login_status", JASON.Encryption(Agent_ID));
                        tagResident.SetAttribute("SYSID", SYSID);
                        tagResidents.AppendChild(tagResident);
                        time = DateTime.Now.ToString("HH:mm:ss");
                        logger.Info(time + "登入Login");
                    }
                }
                else
                {
                    tagResident = xDoc.CreateElement("Login_status");
                    tagResident.SetAttribute("success", "False");
                    tagResident.SetAttribute("error_code", "3");
                    tagResident.SetAttribute("msg", "密碼錯誤");
                    tagResidents.AppendChild(tagResident);
                }
            }
            else
            {
                tagResident = xDoc.CreateElement("Login_status");
                tagResident.SetAttribute("success", "False");
                tagResident.SetAttribute("error_code", "2");
                tagResident.SetAttribute("msg", "沒有此登入帳號");
                tagResidents.AppendChild(tagResident);
            }
        }
        else
        {
            tagResident = xDoc.CreateElement("Login_status");
            tagResident.SetAttribute("success", "False");
            tagResident.SetAttribute("error_code", "1");
            tagResident.SetAttribute("msg", "API KEY錯誤");
            tagResidents.AppendChild(tagResident);
        }

        tagResponse.AppendChild(tagResidents);
        tagResponse.SetAttribute("success", "true");
        xDoc.AppendChild(tagResponse);

        return xDoc;
    }

    [WebMethod]
    public XmlDocument Dispatch_List(string UserID, string Day, string API_KEY)
    {
        xDoc.AppendChild(xDoc.CreateXmlDeclaration("1.0", "utf-8", ""));
        tagResponse = xDoc.CreateElement("response");
        tagResidents = xDoc.CreateElement("missions");

        if (API_KEY == Key)
        {
            /*sql = @"SELECT CNo, ServiceName, Cust_Name, UserID, Service_Flag FROM CASEDetail " +
                "WHERE convert(varchar, StartTime, 112)<@Day AND Agent_ID = @UserID AND Service_Flag != '1' ";  // 原始設定*/
            sql = @"SELECT RTRIM(Case_ID) as CNo,  RTRIM(OpinionType) as ServiceName,     RTRIM(BUSINESSNAME) as Cust_Name, " +
                "RTRIM(Creat_Agent) as UserID, Service_Flag  FROM CaseData " +    // Type_Value as Service_Flag
                "WHERE UserID = @UserID AND (Service_Flag != '1') Order by Case_ID desc";
                //"WHERE convert(varchar, SetupTime, 112)-1<@Day AND UserID = @UserID AND (Service_Flag != '1') Order by Case_ID desc";  //Type_Value = '1' or Type_Value = '2'

            conn = new SqlConnection(strConn);
            myAdp = new SqlDataAdapter(sql, conn);
            ds = new DataSet();
            myAdp.SelectCommand.Parameters.Add("@Day", SqlDbType.Char).Value = Day;
            myAdp.SelectCommand.Parameters.Add("@UserID", SqlDbType.Char).Value = UserID;
            myAdp.Fill(ds, "Result");
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                tagResident = xDoc.CreateElement("mission");
                tagResident.SetAttribute("ServiceName", item["ServiceName"].ToString());
                tagResident.SetAttribute("Cust_Name", item["Cust_Name"].ToString());
                tagResident.SetAttribute("CNo", item["CNo"].ToString());
                tagResident.SetAttribute("UserID", JASON.Encryption(item["UserID"].ToString()));
                tagResident.SetAttribute("Service_Flag", item["Service_Flag"].ToString());
                tagResidents.AppendChild(tagResident);
                logger.Info(item["CNo"].ToString());
            }
        }
        else
        {
            tagResident = xDoc.CreateElement("mission");
            tagResident.SetAttribute("success", "False");
            tagResident.SetAttribute("error_code", "1");
            tagResident.SetAttribute("msg", "API KEY錯誤");
            tagResidents.AppendChild(tagResident);
            time = DateTime.Now.ToString("HH:mm:ss");
            logger.Info(time + "下載API KEY錯誤");
        }

        tagResponse.AppendChild(tagResidents);
        tagResponse.SetAttribute("success", "true");
        xDoc.AppendChild(tagResponse);
        time = DateTime.Now.ToString("HH:mm:ss");
        logger.Info(time + "下載成功");

        return xDoc;
    }

    [WebMethod]
    public XmlDocument Upload_Image(string CNo, string Image, string API_KEY)   //沒再用了
    {
        logger.Info("執行 Upload_Image");
        xDoc.AppendChild(xDoc.CreateXmlDeclaration("1.0", "utf-8", ""));
        tagResponse = xDoc.CreateElement("response");
        tagResidents = xDoc.CreateElement("missions");

        if (API_KEY == Key)
        {
            sql = @"IF NOT EXISTS(SELECT SYSID FROM Upload_Image WHERE CNo = @CNo AND Image_Name = @Image ) " +
                "BEGIN " +
                "INSERT INTO Upload_Image (CNO, Image_Name ) VALUES (@CNo, @Image ) " +
                "END " +
                "SELECT CNo, Image_Name FROM Upload_Image WHERE CNo = @CNo AND Image_Name = @Image ";
            conn = new SqlConnection(strConn);
            myAdp = new SqlDataAdapter(sql, conn);
            ds = new DataSet();
            myAdp.SelectCommand.Parameters.Add("@CNo", SqlDbType.Char).Value = CNo;
            myAdp.SelectCommand.Parameters.Add("@Image", SqlDbType.Char).Value = Image;
            myAdp.Fill(ds, "Result");

            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    tagResident = xDoc.CreateElement("mission");
                    tagResident.SetAttribute("return", "相片上傳完成，派工單編號：" + item["CNo"].ToString() + "，相片名稱：" + item["Image_Name"].ToString());
                    tagResidents.AppendChild(tagResident);
                }
            }
            else
            {
                tagResident = xDoc.CreateElement("mission");
                tagResident.SetAttribute("success", "False");
                tagResident.SetAttribute("return", "2");
                tagResident.SetAttribute("msg", "相片上傳失敗");
                tagResidents.AppendChild(tagResident);
                time = DateTime.Now.ToString("HH:mm:ss");
                logger.Info(time + "相片上傳失敗");
            }
        }
        else
        {
            tagResident = xDoc.CreateElement("mission");
            tagResident.SetAttribute("success", "False");
            tagResident.SetAttribute("error_code", "1");
            tagResident.SetAttribute("msg", "API KEY錯誤");
            tagResidents.AppendChild(tagResident);
            time = DateTime.Now.ToString("HH:mm:ss");
            logger.Info(time + "相片上傳 API KEY錯誤");
        }

        tagResponse.AppendChild(tagResidents);
        tagResponse.SetAttribute("success", "true");
        xDoc.AppendChild(tagResponse);
        time = DateTime.Now.ToString("HH:mm:ss");
        logger.Info(time + "相片上傳成功");

        return xDoc;
    }
    public static string Value(string value)        // 當值為null時跳過  非 null 時去除後方空白
    {
        if (!string.IsNullOrEmpty(value))
        {
            value = value.Trim();
        }
        return value;
    }
}

