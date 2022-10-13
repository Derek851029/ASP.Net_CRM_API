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
    public static string randomWord(string result1)
    {
        string sql = "select MemberNum from MemberData where MemberNum = @MemberNum";
        var check = DBTool.Query(sql,new {
            MemberNum = result1
        });
        if (check.Any())
        {
            return "same";
        }
        return "ok";
    }

    [WebMethod(EnableSession = true)]
    public static string bindtable()
    {
        string sql = "select * from MemberData where Status = 'New'";
        var data = DBTool.Query(sql);
        return JsonConvert.SerializeObject(data);
    }

    [WebMethod(EnableSession = true)]
    public static string test123()
    {
        string sql = @"select A.T_DAY,A.T_CASH,C.CASHIERPER,A.T_VIP,D.VIP_NAME ,B.PLU_CODE,B.PC_NAME,E.BARCODE,B.CNT,B.PRICE,B.TOTAL
                            from POS_H A
                            LEFT JOIN POS_I B ON A.T_DAY = A.T_DAY AND A.T_STORE = B.T_STORE AND A.TM_NO = B.TM_NO AND A.T_SER_NO = B.T_SER_NO
                            LEFT JOIN CASHMF C ON A.T_CASH = C.CASH_CODE
                            LEFT JOIN VIPMF D ON A.T_VIP = D.VIP_CODE
                            LEFT JOIN PRODUCT E ON B.PLU_CODE = E.PLU_CODE
                            WHERE B.DATA_TYPE = '2' and A.T_VIP = ''";
        var data = DBTool2.Query(sql);
        return JsonConvert.SerializeObject(data);
    }

    [WebMethod(EnableSession = true)]
    public static string MemberHistory(string MemberNum)
    {
        string sql;
        sql = "select * from FaceRecognitionLog where Number = @MemberNum order by Date desc";
        var data = DBTool.Query<result>(sql, new { MemberNum = MemberNum });

        sql = @"select A.T_DAY,Sum(B.TOTAL) as Total from POS_H A 
                LEFT JOIN POS_I B ON A.T_DAY=A.T_DAY AND A.T_STORE=B.T_STORE AND A.TM_NO=B.TM_NO AND A.T_SER_NO=B.T_SER_NO 
                WHERE B.DATA_TYPE='2' and A.T_VIP = @MemberNum group by A.T_DAY";
        //string sql = @"select A.T_DAY,A.T_CASH,C.CASHIERPER,A.T_VIP,D.VIP_NAME ,B.PLU_CODE,B.PC_NAME,E.BARCODE,B.CNT,B.PRICE,B.TOTAL
        //                    from POS_H A
        //                    LEFT JOIN POS_I B ON A.T_DAY = A.T_DAY AND A.T_STORE = B.T_STORE AND A.TM_NO = B.TM_NO AND A.T_SER_NO = B.T_SER_NO
        //                    LEFT JOIN CASHMF C ON A.T_CASH = C.CASH_CODE
        //                    LEFT JOIN VIPMF D ON A.T_VIP = D.VIP_CODE
        //                    LEFT JOIN PRODUCT E ON B.PLU_CODE = E.PLU_CODE
        //                    WHERE B.DATA_TYPE = '2' and A.T_VIP = @MemberNum";
        var data2 = DBTool2.Query<result>(sql,new { MemberNum = MemberNum });
        List<result> result = new List<result>();
        
        List<result> Log = data.ToList();
        List<result> Pay = data2.ToList();

        result = Log.Concat(Pay).ToList();
        return JsonConvert.SerializeObject(result);
    }


    [WebMethod(EnableSession = true)]
    public static string Add_Member(int type,string MemberNum, string MemberName, string MemberID, string MemberDate, string Sex,
        string MemberPhone, string MemberEmail, double SizeKB, string file_base64)
    {
        string sql;
        string ImagePath = HttpContext.Current.Server.MapPath("/MemberImage/");
        string imgName;

        if (type == 0)
        {
            sql = @"select MemberID from MemberData where MemberID = @MemberID and Status = @Status";
            var check = DBTool.Query(sql, new
            {
                MemberID = MemberID,
                Status = "New",
            });
            if (check.Any())
            {
                return "fail";
            }
        }
        

        
        if (Math.Floor(SizeKB) > 200)
        {
            imgName = MemberID + ".png";
            byte[] imgByteArray = Convert.FromBase64String(file_base64);
            File.WriteAllBytes(ImagePath + imgName, imgByteArray);

            string oldPath = ImagePath + imgName;
            //需要新路徑壓縮圖片
            imgName = MemberID + ".jpg";
            string newPath = ImagePath + imgName;
            VaryQualityLevel(oldPath, newPath, 10L);
            //刪除舊檔案
            File.Delete(ImagePath + MemberID + ".png");
        }
        else
        {
            imgName = MemberID + ".jpg";
            byte[] imgByteArray = Convert.FromBase64String(file_base64);
            File.WriteAllBytes(ImagePath + imgName, imgByteArray);
        }

        //string ImageURL = "http://192.168.2.229:6001/MemberImage/" + imgName;
        string ImageURL = "http://210.68.227.123:6001/MemberImage/" + imgName;

        if (type == 0)
        {
            sql = @"insert into MemberData(MemberNum,MemberName,MemberID,MemberDate,Sex,MemberPhone,MemberEmail,ImagePath,ImageURL,Status)
                            values(@MemberNum,@MemberName,@MemberID,@MemberDate,@Sex,@MemberPhone,@MemberEmail,@ImagePath,@ImageURL,@Status)";
            DBTool.Query(sql, new
            {
                MemberNum = MemberNum,
                MemberName = MemberName,
                MemberID = MemberID,
                MemberDate = MemberDate,
                Sex = Sex,
                MemberPhone = MemberPhone,
                MemberEmail = MemberEmail,
                ImagePath = ImagePath + imgName,
                ImageURL = ImageURL,
                Status = "New",
            });
        }
        else
        {
            sql = "update MemberData set MemberName = @MemberName, MemberID = @MemberID" +
            ",MemberDate = @MemberDate, Sex = @Sex, MemberPhone = @MemberPhone, MemberEmail = @MemberEmail, ImagePath = @ImagePath,ImageURL = @ImageURL where MemberNum = @MemberNum";
            DBTool.Query(sql, new
            {
                MemberNum = MemberNum,
                MemberName = MemberName,
                MemberID = MemberID,
                MemberDate = MemberDate,
                Sex = Sex,
                MemberPhone = MemberPhone,
                MemberEmail = MemberEmail,
                ImagePath = ImagePath + imgName,
                ImageURL = ImageURL,
            });
        }
        
        return ImageURL;
    }

    [WebMethod(EnableSession = true)]
    public static string EditMember(string MemberNum, string MemberName, string MemberID, string MemberDate, string Sex,
        string MemberPhone, string MemberEmail)
    {
        string sql;
        try
        {
            sql = "update MemberData set MemberName = @MemberName, MemberID = @MemberID" +
            ",MemberDate = @MemberDate, Sex = @Sex, MemberPhone = @MemberPhone, MemberEmail = @MemberEmail where MemberNum = @MemberNum";
            DBTool.Query(sql, new
            {
                MemberNum = MemberNum,
                MemberName = MemberName,
                MemberID = MemberID,
                MemberDate = MemberDate,
                Sex = Sex,
                MemberPhone = MemberPhone,
                MemberEmail = MemberEmail,
            });
        }
        catch(Exception e)
        {
            sql = @" INSERT INTO SystemLog (Page,FunctionName,Ex) 
                            VALUES (@Page,@FunctionName,@Ex) ";
            DBTool.Query(sql, new
            {
                Page = "Member.aspx",
                FunctionName = "EditMember",
                Ex = e.ToString(),
            });

        }
        
        return "ok";
    }

    [WebMethod(EnableSession = true)]
    public static string DeleteStatus(string SYSID)
    {
        string sql;
        try
        {
            sql = "update MemberData set Status = 'Delete' where SYSID = @SYSID";
            DBTool.Query(sql,new { SYSID = SYSID });
        }catch(Exception e)
        {
            sql = @" INSERT INTO SystemLog (Page,FunctionName,Ex) 
                            VALUES (@Page,@FunctionName,@Ex) ";
            DBTool.Query(sql, new
            {
                Page = "Member.aspx",
                FunctionName = "DeleteStatus",
                Ex = e.ToString(),
            });
        }
        return "ok";
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

    //private static byte[] ConvertBytestoJpegBytes(byte[] pixels24bpp, int W, int H)
    //{
    //    GCHandle gch = GCHandle.Alloc(pixels24bpp, GCHandleType.Pinned);
    //    int stride = 4 * ((24 * W + 31) / 32);
    //    Bitmap bmp = new Bitmap(W, H, stride, PixelFormat.Format24bppRgb, gch.AddrOfPinnedObject());
    //    MemoryStream ms = new MemoryStream();
    //    bmp.Save(ms, ImageFormat.Jpeg);
    //    gch.Free();
    //    return ms.ToArray();
    //}

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
}
public class result
{
    public int SYSID { get; set; }
    public string SourceIP { get; set; }
    public string Number { get; set; }
    public string Position { get; set; }
    public string Mask { get; set; }
    public string Date { get; set; }
    public string T_DAY { get; set; }
    public string Total { get; set; }
}