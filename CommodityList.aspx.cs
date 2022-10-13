using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommodityList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    public static string bindlist()
    {
        string sqlcmd = @"SELECT * FROM CommodityData WHERE DelFlag = 0 ";
        return JsonConvert.SerializeObject(DBTool.Query(sqlcmd));
    }


    [WebMethod(EnableSession = true)]
    public static string EditList(string type, CommodityData data)
    {
        string sqlcmd = "";
        try
        {
            string UserID = HttpContext.Current.Session["UserID"].ToString();
            data.EditUser = UserID;

            switch (type)
            {
                case "add":
                    sqlcmd = @" INSERT INTO CommodityData (ProductName,Ingredient,PlaceOfOrigin,Number,LocationAt,EffectDate,PurchaseDate,Barcode,EditUser)   
                                VALUES (@ProductName,@Ingredient,@PlaceOfOrigin,@Number,@LocationAt,@EffectDate,@PurchaseDate,@Barcode,@EditUser) ";
                    break;
                case "update":
                    sqlcmd = @" UPDATE CommodityData SET 
                                ProductName = @ProductName
                                ,Ingredient = @Ingredient
                                ,PlaceOfOrigin = @PlaceOfOrigin
                                ,Number = @Number
                                ,LocationAt = @LocationAt
                                ,EffectDate = @EffectDate
                                ,PurchaseDate = @PurchaseDate
                                ,Barcode = @Barcode
                                ,EditUser = @EditUser 
                                ,EditDate = GETDATE()
                                WHERE CDID = @CDID";
                    break;
                case "delete":
                    sqlcmd = @" UPDATE CommodityData SET 
                                DelFlag = 1
                                ,EditUser = @EditUser 
                                ,EditDate = GETDATE()
                                WHERE CDID = @CDID";
                    break;
            }

            DBTool.Query(sqlcmd, data);

            return "Success";
        }
        catch(Exception ex)
        {
            sqlcmd = @" INSERT INTO SystemLog (Page,FunctionName,EX) VALUES (@Page,@FunctionName,@EX) ";
            DBTool.Query(sqlcmd, new
            {
                Page = "CommodityList.aspx",
                FunctionName = "EditList",
                EX = JsonConvert.SerializeObject(new { Params = new { type = type, data = data}, Error = new { msg = ex.Message, stacktrace = ex.StackTrace} })
            });
            return "error";
        }
    }

    [WebMethod(EnableSession = true)]
    public static string SaveFile(string ImageFile, string Barcode)
    {
        string ImageFileURL = ""; 
        string ImagePath = HttpContext.Current.Server.MapPath("/CommodityImage/");
        //string ImagePath = @"C:\Derek\Smart_Drugstore\CommodityImage\";

        string imgName;
        byte[] imgByteArray;
        try
        {
            //if (Math.Floor(SizeKB) > 200)
            //{
            //    imgName = MemberID + ".png";
            //    byte[] imgByteArray = Convert.FromBase64String(file_base64);
            //    File.WriteAllBytes(ImagePath + imgName, imgByteArray);

            //    string oldPath = ImagePath + imgName;
            //    //需要新路徑壓縮圖片
            //    imgName = MemberID + ".jpg";
            //    string newPath = ImagePath + imgName;
            //    VaryQualityLevel(oldPath, newPath, 30L);
            //    //刪除舊檔案
            //    File.Delete(ImagePath + MemberID + ".png");
            //}
            //else
            //{
            //    imgName = Barcode + ".jpg";
            //    imgByteArray = Convert.FromBase64String(ImageFile);
            //    File.WriteAllBytes(ImagePath + imgName, imgByteArray);
            //}
            imgName = Barcode + ".jpg";
            imgByteArray = Convert.FromBase64String(ImageFile);
            File.WriteAllBytes(ImagePath + imgName, imgByteArray);

            ImagePath += imgName;
            //ImageFileURL = "http://192.168.2.229:6001/CommodityImage/"+ imgName + "";
            ImageFileURL = "http://210.68.227.123:6001/CommodityImage/"+ imgName + "";

            string sqlcmd = "update CommodityData set ImagePath = @ImagePath, ImageURL = @ImageURL " +
                "where Barcode = @Barcode";
            DBTool.Query(sqlcmd, new
            {
                ImagePath = ImagePath,
                ImageURL = ImageFileURL,
                Barcode = Barcode,
            });
            return "Success";
        }
        catch (Exception e)
        {
            string sqlcmd = @" INSERT INTO SystemLog (Page,FunctionName,EX) VALUES (@Page,@FunctionName,@EX) ";
            DBTool.Query(sqlcmd, new
            {
                Page = "CommodityList.aspx",
                FunctionName = "EditList",
                EX = e.ToString()
            });
            return "error";
        }
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

    public class CommodityData
    {
        public string CDID { get; set; }
        public string ProductName { get; set; }
        public string Ingredient { get; set; }
        public string PlaceOfOrigin { get; set; }
        public string Number { get; set; }
        public string LocationAt { get; set; }
        public string EffectDate { get; set; }
        public string PurchaseDate { get; set; }
        public string Barcode { get; set; }
        public string EditUser { get; set; }

    }

}