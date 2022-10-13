using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

public partial class _0060010004 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetPartnerList(string[] Array)
    {
        string sqlstr = @"SELECT a.SYSID,
	               a.Agent_ID,
                   a.Agent_Name,
	               a.Agent_Status,
	               a.Agent_Company,
	               a.Agent_Team,
	               a.UserID,
	               a.Password,
                   b.ROLE_NAME,
                   a.Agent_LV
                   FROM DispatchSystem a left join ROLELIST b on a.ROLE_ID = b.ROLE_ID
                   WHERE a.Agent_ID !='' AND a. Agent_Status = '在職'";
        var GetPartnerList = DBTool.Query<ClassTemplate>(sqlstr).ToList();
        return JsonConvert.SerializeObject(GetPartnerList);
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Agent_Company_List()
    {
        //Check();
        string sqlstr = @"SELECT DISTINCT Agent_Company FROM DispatchSystem WHERE Agent_ID !='' AND Agent_Status = '在職' ORDER BY Agent_Company";
        var a = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new
        {
            Agent_Company = p.Agent_Company.Trim()
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Agent_Team_List()
    {
        //Check();
        string sqlstr = @"SELECT DISTINCT Agent_Team FROM DispatchSystem WHERE Agent_ID !='' AND Agent_Status = '在職' ORDER BY Agent_Team";
        var a = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new
        {
            Agent_Team = p.Agent_Team.Trim()
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    //============= 帶入【系統選單權限】資訊 =============
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string ROLELIST_List()
    {
        //Check();
        string sqlstr = @"SELECT ROLE_ID, ROLE_NAME FROM ROLELIST WHERE OPEN_DEL = 'Y' ";
        var a = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new
        {
            ROLE_ID = p.Role_ID.Trim(),
            ROLE_NAME = p.ROLE_NAME.Trim()
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    //============= 帶入【人員】資訊 =============
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string DispatchSystem(string Agent_ID)
    {
        //Check();
        string sqlstr = @"SELECT TOP 1 * FROM DispatchSystem WHERE Agent_ID = @Agent_ID AND Agent_Status = '在職' ";

        var a = DBTool.Query<ClassTemplate>(sqlstr, new { Agent_ID = Agent_ID }).ToList().Select(p => new
        {
            Agent_ID = p.Agent_ID,
            Agent_Name = p.Agent_Name,
            Agent_Company = p.Agent_Company,
            Agent_Team = p.Agent_Team,
            UserID = p.UserID,
            Agent_LV = p.Agent_LV,
            Role_ID = p.Role_ID,
            ImageURL = p.ImageURL,
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    //============= 刪除【人員】資訊 =============
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Delete(string Agent_ID)
    {
        //Check();
        string sqlstr = @"UPDATE DispatchSystem SET Agent_Status='離職', UpdateTime=@UpdateTime WHERE Agent_ID = @Agent_ID AND Agent_Status = '在職' AND Agent_LV != 30";
        DBTool.Query<ClassTemplate>(sqlstr, new { Agent_ID = Agent_ID, UpdateTime = DateTime.Now });
        return JsonConvert.SerializeObject(new { status = "success" });
    }

    //============= 新增【人員】資訊 =============
    [WebMethod(EnableSession = true)]
    public static string New_Agent(
        string Agent_ID, 
        string Agent_Name, 
        string Agent_Company, 
        string Agent_Team, 
        string UserID, 
        string Password, 
        string Role_ID, 
        string Agent_LV, 
        string Flag,
        double SizeKB,
        string Base64File)
    {
        string back = "",Sqlstr = "",sql_pass = "";
        string ImagePath = HttpContext.Current.Server.MapPath("/StaffImage/");
        string imgName = "";
        string ImageURL = "";

        //驗證 UserID 【登入帳號】
        UserID = UserID.Trim();
        if (String.IsNullOrEmpty(UserID)) return JsonConvert.SerializeObject(new { status = "請填寫【登入帳號】" });
        if (!String.IsNullOrEmpty(UserID) && JASON.IsNumericOrLetter(UserID) != true) return JsonConvert.SerializeObject(new { status = "【登入帳號】只能是英文或數字。" });

        //驗證 Agent_ID 【人員編號】
        Agent_ID = Agent_ID.Trim();
        if (String.IsNullOrEmpty(Agent_ID)) return JsonConvert.SerializeObject(new { status = "請填寫【人員編號】" });
        if (JASON.IsNumericOrLetter(Agent_ID) != true) return JsonConvert.SerializeObject(new { status = "【人員編號】只能輸入英文或數字。" });

        //驗證 Agent_Name 【人員姓名】
        Agent_Name = Agent_Name.Trim();
        if (String.IsNullOrEmpty(Agent_Name)) return JsonConvert.SerializeObject(new { status = "請填寫【人員姓名】" });
       
        //驗證 Agent_Company 【所屬單位】
        Agent_Company = Agent_Company.Trim();
        if (String.IsNullOrEmpty(Agent_Company)) return JsonConvert.SerializeObject(new { status = "請填寫【所屬單位】" });

        //驗證 Agent_Team 【所屬部門】
        Agent_Team = Agent_Team.Trim();
        if (String.IsNullOrEmpty(Agent_Team)) return JsonConvert.SerializeObject(new { status = "請填寫【所屬部門】" });
        
        string[] LevelArray = { "10", "20", "30", "40", "50" };
        if (!LevelArray.Contains(Agent_LV)) return JsonConvert.SerializeObject(new { status = "沒有此【人員權限】" });

        Sqlstr = "SELECT TOP 1 ROLE_ID FROM ROLELIST WHERE OPEN_DEL != 'N' AND Role_ID=@Role_ID";
        var CheckRoleID = DBTool.Query<ClassTemplate>(Sqlstr, new { Role_ID = Role_ID });
        if (!CheckRoleID.Any()) return JsonConvert.SerializeObject(new { status = "沒有此【系統選單權限】" });

        Sqlstr = @"SELECT TOP 1 SYSID FROM DispatchSystem WHERE Agent_Status != '離職' AND UserID=@UserID AND Agent_ID!=@Agent_ID";
        var CheckSameUserID = DBTool.Query<ClassTemplate>(Sqlstr, new { UserID = UserID, Agent_ID = Agent_ID });
        if (CheckSameUserID.Any()) return JsonConvert.SerializeObject(new { status = "已有相同的【登入帳號】" });

        //驗證 Password 【登入密碼】
        Password = Password.Trim();

        if (Math.Floor(SizeKB) > 200)
        {
            imgName = Agent_ID + ".png";
            byte[] imgByteArray = Convert.FromBase64String(Base64File);
            File.WriteAllBytes(ImagePath + imgName, imgByteArray);

            string oldPath = ImagePath + imgName;
            //需要新路徑壓縮圖片
            imgName = Agent_ID + ".jpg";
            string newPath = ImagePath + imgName;
            VaryQualityLevel(oldPath, newPath, 30L);
            //刪除舊檔案
            File.Delete(ImagePath + Agent_ID + ".png");
        }
        else
        {
            imgName = Agent_ID + ".jpg";
            byte[] imgByteArray = Convert.FromBase64String(Base64File);
            File.WriteAllBytes(ImagePath + imgName, imgByteArray);
        }
        //ImageURL = "http://192.168.2.229:6001/StaffImage/" + imgName;
        ImageURL = "http://210.68.227.123:6001/StaffImage/" + imgName;

        if (Flag == "0")
        {
            Sqlstr = "SELECT TOP 1 SYSID FROM DispatchSystem WHERE Agent_Status != '離職' AND Agent_ID=@Agent_ID";
            var CheckSameAgentID = DBTool.Query<ClassTemplate>(Sqlstr, new { Agent_ID = Agent_ID });
            if (CheckSameAgentID.Any()) return JsonConvert.SerializeObject(new { status = "已有相同的【人員編號】" });

            if (CheckPassword(Password) != "") return JsonConvert.SerializeObject(new { status = CheckPassword(Password) });

            Sqlstr = @"INSERT INTO DispatchSystem 
                    (Agent_ID, Agent_Name, Agent_Status, Agent_Company, Agent_Team, UserID, Password, Agent_LV, Role_ID,ImagePath,ImageURL)
                     VALUES
                    (@Agent_ID, @Agent_Name, @Agent_Status, @Agent_Company, @Agent_Team, @UserID, @Password, @Agent_LV, @Role_ID,@ImagePath,@ImageURL)";
            back = "new";
        }
        if (Flag == "1")
        {
            if (Password != "")
            {
                if (CheckPassword(Password) != "") return JsonConvert.SerializeObject(new { status = CheckPassword(Password) });
                sql_pass = ", Password = @Password ";
            }
            Sqlstr = @"UPDATE DispatchSystem SET
                       Agent_Name = @Agent_Name,
                       Agent_Company = @Agent_Company,
                       Agent_Team = @Agent_Team,
                       UserID = @UserID,
                       Agent_LV = @Agent_LV,
                       Role_ID = @Role_ID,
                       ImagePath = @ImagePath,
                       ImageURL = @ImageURL,
                       UpdateTime = @UpdateTime " +
                       sql_pass +
                       "WHERE Agent_ID = @Agent_ID AND Agent_Status != '離職' ";
            back = "update";
        }

        var b = DBTool.Query<ClassTemplate>(Sqlstr, new
        {
            Agent_ID = Agent_ID.Trim(),
            Agent_Name = Agent_Name.Trim(),
            Agent_Status = "在職",
            Agent_Company = Agent_Company.Trim(),
            Agent_Team = Agent_Team.Trim(),
            UserID = UserID.Trim(),
            Role_ID = Role_ID,
            Agent_LV = Agent_LV,
            ImagePath = ImagePath+ imgName,
            ImageURL = ImageURL,
            Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5").ToUpper(),
            UpdateTime = DateTime.Now
        });
        return JsonConvert.SerializeObject(new { status = back, ImageURL = ImageURL });
    }

    public static string CheckPassword(string Password)
    {
        //驗證 Password 【登入密碼】
        Password = Password.Trim();
        if (String.IsNullOrEmpty(Password)) return "請填寫【登入密碼】";
        if (!String.IsNullOrEmpty(Password) && JASON.IsNumericOrLetter(Password) != true) return "【登入密碼】只能是英文或數字。";
        return "";
    }

    private static void VaryQualityLevel(string FilePath, string New_Path, long qualityLevel)
    {

        using (Bitmap bmp1 = new Bitmap(FilePath))
        {
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;//給Compression無效

            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, qualityLevel);
            bmp1.Save(New_Path, jpgEncoder, myEncoderParameters);

        }//end using

    }
    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().Where(m => m.FormatID == format.Guid).FirstOrDefault();
        if (codec == null)
        {
            return null;
        }
        return codec;

    }

    [WebMethod(EnableSession = true)]
    public static string FaceControl(string data, int type)
    {
        string url = "";
        string function_name = "";
        string return_data = "";
        switch (type)
        {
            case 0:
                url = "http://192.168.2.117/ISAPI/AccessControl/UserInfo/SetUp?format=json";
                function_name = "建立人員";
                return_data = Requests_Deivce(data, url, function_name);
                break;
            case 1:
                url = "http://192.168.2.117/ISAPI/Intelligent/FDLib/FDSetUp?format=json";
                function_name = "放置人臉";
                return_data = Requests_Deivce(data, url, function_name);
                break;
            case 2:
                url = "http://192.168.2.117/ISAPI/AccessControl/UserInfo/Modify?format=json";
                function_name = "編輯人員";
                return_data = Requests_Deivce(data, url, function_name);
                break;
            case 3:
                url = "http://192.168.2.117/ISAPI/AccessControl/UserInfo/Delete?format=json";
                function_name = "刪除人員";
                return_data = Requests_Deivce(data, url, function_name);
                break;
        }
        return return_data;
    }

    public static string Requests_Deivce(string data, string url, string function_name)
    {
        string password = "apex85024828";
        try
        {
            WebRequest request = WebRequest.Create(url);
            CredentialCache mycache = new CredentialCache();
            mycache.Add(new Uri(url), "Digest", new NetworkCredential("admin", password));
            request.Credentials = mycache;
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers.Add("Authorization", "Digest");
            request.Method = "PUT";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
            }

            //發送
            var httpResponse = (HttpWebResponse)request.GetResponse();
            //讀取回傳資料
            Stream stream = httpResponse.GetResponseStream();
            byte[] rsByte = new Byte[httpResponse.ContentLength];
            stream.Read(rsByte, 0, (int)httpResponse.ContentLength);
            string test = System.Text.Encoding.UTF8.GetString(rsByte, 0, rsByte.Length).ToString();
            //關閉request
            httpResponse.Close();
            return "ok";
        }
        catch (Exception ex)
        {
            string sqlcmd = @" INSERT INTO SystemLog (Page,FunctionName,Ex) 
                            VALUES (@Page,@FunctionName,@Ex) ";
            DBTool.Query(sqlcmd, new
            {
                Page = "Member.aspx",
                FunctionName = function_name,
                Ex = ex.Message + ";" + ex.StackTrace,
            });

            return "fail";
        }
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Check()
    {
        string Check = JASON.Check_ID("0060010004.aspx");
        if (Check == "NO") System.Web.HttpContext.Current.Response.Redirect("~/Default.aspx");
        return "";
    }
}

