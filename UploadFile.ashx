<%@ WebHandler Language="C#" Class="UploadFile" %>

using System;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using System.Web.SessionState;

public class UploadFile : IHttpHandler,IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {

        string sqlcmd = "", UserID = "",type = "",index = "";
        int total = 0;

        try
        {
            //取得form的參數
            type = context.Request.Form.Get("type");
            string DateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //先宣告一個變數等等放檔案名稱
            HttpFileCollection files = context.Request.Files;

            //確定存放檔案的位置存在
            string filePath = HttpContext.Current.Server.MapPath("/CommodityVideo/");
            //string filePath = @"C:\Derek\Smart_Drugstore\CommodityVideo\";
            //filePath = context.Server.MapPath("/file");
            //if ( ! Directory.Exists(filePath))
            //{
            //    Directory.CreateDirectory(filePath);
            //}
            //filePath = context.Server.MapPath("/file/"+UserID);
            //if ( ! Directory.Exists(filePath))
            //{
            //    Directory.CreateDirectory(filePath);
            //}

            //開始儲存檔案
            string fileName = "";
            if(files.Count > 0)
            {
                for(int i = 0; i<files.Count; i++)
                {
                    HttpPostedFile file = files[i];

                    //取得檔名
                    fileName = files[i].FileName;
                    //判斷附檔名  fileName.Substring(fileName.Length-4,4) != ".mp4"
                    if(type == "video")
                    {
                        index = context.Request.Form.Get("index");
                        total = Convert.ToInt32(context.Request.Form.Get("total"));
                        fileName = context.Request.Form.Get("name");
                    }
                    else
                    {
                        //確定檔名不衝突
                        fileName = SefFileName(filePath, fileName);
                    }

                    //儲存
                    file.SaveAs(filePath + fileName);

                    if(type == "video")//合併檔案
                    {
                        string ofileName = context.Request.Form.Get("name");
                        DirectoryInfo dirInfo = new DirectoryInfo(filePath);
                        var totalCount = dirInfo.GetFiles(ofileName + "_*").Length;
                        if(total == totalCount)//分割檔案數量對了就開始合併
                        {
                            //確定檔名不衝突
                            fileName = SefFileName(filePath, ofileName);
                            var fs = new FileStream(filePath + fileName, FileMode.Create);
                            for(int j = 1; j<=total; j++)
                            {
                                string part = filePath + ofileName + "_" + j.ToString();
                                var bytes = System.IO.File.ReadAllBytes(part);
                                fs.Write(bytes, 0, bytes.Length);
                                bytes = null;
                                System.IO.File.Delete(part);
                            }
                            fs.Close();
                        }
                    }

                }
                filePath = filePath + fileName;
                //string VideoFileURL = "http://192.168.2.229:6001/CommodityVideo/" + fileName + "";
                string VideoFileURL = "http://210.68.227.123:6001/CommodityVideo/" + fileName + "";
                string Barcode = context.Request.Form.Get("Barcode");
                sqlcmd = @"update CommodityData set VideoPath = @VideoPath, VideoURL = @VideoURL where Barcode = @Barcode";
                DBTool.Query(sqlcmd, new {
                    VideoPath = filePath,
                    VideoURL = VideoFileURL,
                    Barcode = Barcode,
                });
            }

            context.Response.Write(JsonConvert.SerializeObject( new {
                Status = "Success" ,
            }));
        }
        catch(Exception ex)
        {
            sqlcmd = @" INSERT INTO SystemLog (Page,FunctionName,EX) VALUES (@Page,@FunctionName,@EX) ";
            DBTool.Query(sqlcmd, new
            {
                Page = "CommodityList.aspx",
                FunctionName = "UploadFile",
                EX = ex.ToString()
            });
            context.Response.Write(JsonConvert.SerializeObject( new { Status = "fail" }));
        }


    }

    public bool IsReusable {
        get {
            return false;
        }
    }

    /// <summary>
    /// 判斷檔名是否有重複
    /// </summary>
    /// <param name="path">路徑</param>
    /// <param name="name">檔名</param>
    /// <returns></returns>
    private string SefFileName(string path, string name)
    {
        int count = 0;
        if(File.Exists(path + "/" + name))
        {
            count++;
            while (true)
            {
                if (File.Exists(path + "/" + count.ToString() + "_" + name))
                {
                    count++;
                }
                else
                {
                    name = count.ToString() + "_" + name;
                    break;
                }
            }
        }
        return name;
    }


}