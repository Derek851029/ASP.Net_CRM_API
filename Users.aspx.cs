using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Users_Users : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod(EnableSession = true)]
    public static string bindtable()
    {
        string sql = "select * from Person_List where Status = '新增'";
        var data = DBTool.Query(sql);
        return JsonConvert.SerializeObject(data);
    }

    [WebMethod(EnableSession = true)]
    public static string List_Parent()
    {
        string sql = "select MName,UserID from MemberData where MIdentity = '家屬'";
        var data = DBTool.Query(sql);
        return JsonConvert.SerializeObject(data);
    }

    [WebMethod(EnableSession = true)]
    public static string Add_Person(string btn_text,string Name,string Person_Number ,string Parent, string Sex,
        string Phone,string Email,string photo_base64,string Group_)
    {
        
        string sql;
        string SerialN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        
        if (btn_text == "新增")
        {
            sql = "select * from Person_List where ID = '" + Person_Number + "'";
            var a = DBTool.Query(sql);
            if (a.Any())
            {
                return "已有相同身分證字號，請重新輸入。";
            }
            //string path = "C:/acme/LinyuanCare/img/"; //實際使用位置
			string path = "C:/Qing/KarlSchool/img/"; //台北使用位置
            string imgName = Person_Number + ".png";
            byte[] imgByteArray = Convert.FromBase64String(photo_base64);

            File.WriteAllBytes(path + imgName, imgByteArray);

            string FilePath = path + imgName;
            //需要新路徑壓縮圖片
            imgName = Person_Number + ".jpg";
            string New_Path = path + imgName;
            VaryQualityLevel(FilePath, New_Path,90L);
            //刪除舊檔案
            File.Delete(path + Person_Number + ".png");


            string img_url = "http://192.168.30.17:8010/img/" + imgName;
            sql = string.Format(
            @"insert into Person_List(Name,ID,Sex,Phone,Email,Img_url,Parent_UserID,Status,Group_,SerialN )values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
            Name, Person_Number, Sex, Phone, Email, img_url, Parent, btn_text, Group_, SerialN);
            DBTool.Query(sql);
            return img_url;
        }
        else
        {
            sql = string.Format(
            @"update Person_List set Name = '{0}',ID = '{1}',Sex = '{2}',Phone = '{3}',Email = '{4}',Parent_UserID = '{5}' , Group_='{6}' where ID = '"+ Person_Number + "'",
            Name, Person_Number, Sex, Phone, Email, Parent, Group_);
            DBTool.Query(sql);
            return "Success";
        }
    }

    private static void VaryQualityLevel(string FilePath,string New_Path, long qualityLevel = 90L)
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

    private static byte[] ConvertBytestoJpegBytes(byte[] pixels24bpp, int W, int H)
    {
        GCHandle gch = GCHandle.Alloc(pixels24bpp, GCHandleType.Pinned);
        int stride = 4 * ((24 * W + 31) / 32);
        Bitmap bmp = new Bitmap(W, H, stride, PixelFormat.Format24bppRgb, gch.AddrOfPinnedObject());
        MemoryStream ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Jpeg);
        gch.Free();
        return ms.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public static string Listen_Flag(string Person_Number)
    {
        string sql = "select Flag from Person_List where Serial_Number = '"+ Person_Number + "'";
        var a = DBTool.Query(sql).FirstOrDefault();
        if (a.Flag == 0)
        {
            return "false";
        }
        else
        {
            return "Success";
        }
    }

    [WebMethod(EnableSession = true)]
    public static string Delete_Person(string SYSID)
    {
        string sql = "select ID from Person_List where SYSID = " + SYSID + "";
        string ID = DBTool.Query(sql).FirstOrDefault().ID;
        sql = "delete Person_List  where SYSID = " + SYSID + "";
        DBTool.Query(sql);
        
        return ID;
    }

    [WebMethod(EnableSession = true)]
    public static string Delete_Device(string data)
    {
        string url = "http://192.168.30.18/ISAPI/AccessControl/UserInfo/Delete?format=json";
        string url2 = "http://192.168.30.19/ISAPI/AccessControl/UserInfo/Delete?format=json";
        string function_name = "Delete_Device";
        Requests_Deivce(data, url, function_name);
        Requests_Deivce(data, url2, function_name);
        return "刪除成功。";
    }

    [WebMethod(EnableSession = true)]
    public static string Device_Control(string data, int type)
    {
        string url = "";
        string url2 = "";
        string function_name = "";
        string return_data = "";
        switch (type)
        {
            case 0:
                url = "http://192.168.30.18/ISAPI/AccessControl/UserInfo/SetUp?format=json";
                function_name = "建立人員";
                return_data = Requests_Deivce(data, url, function_name);

                url2 = "http://192.168.30.19/ISAPI/AccessControl/UserInfo/SetUp?format=json";
                return_data = Requests_Deivce(data, url2, function_name);
                break;
            case 1:
                url = "http://192.168.30.18/ISAPI/Intelligent/FDLib/FDSetUp?format=json";
                function_name = "放置人臉";
                return_data = Requests_Deivce(data, url, function_name);

                url2 = "http://192.168.30.19/ISAPI/Intelligent/FDLib/FDSetUp?format=json";
                return_data = Requests_Deivce(data, url2, function_name);
                break;
	    case 2:
                url = "http://192.168.30.18/ISAPI/AccessControl/UserInfo/Modify?format=json";
                function_name = "編輯人員";
                return_data = Requests_Deivce(data, url, function_name);

                url2 = "http://192.168.30.19/ISAPI/AccessControl/UserInfo/Modify?format=json";
                return_data = Requests_Deivce(data, url2, function_name);
                break;
        }
        return return_data;
        
    }

    public static string Requests_Deivce(string data, string url, string function_name)
    {
        string password = "";
        if (url.IndexOf("18") == -1)
        {
            password = "Acme70472615";
        }
        else
        {
            password = "k1111111";
        }
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
            return "Success";
        }
        catch (Exception ex)
        {
            string sqlcmd = @" INSERT INTO SystemLog (PageName,PageFunc,PageLog,EX,IP,UserID) 
                            VALUES ('Users','Device_Control',@function_name,@e,'','') ";
            DBTool.Query(sqlcmd, new
            {
                function_name = function_name,
                e = ex.Message + ";" + ex.StackTrace,
            });

            return "Add device error";
        }
    }
}