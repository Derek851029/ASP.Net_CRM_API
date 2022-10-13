using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LineMsgList : System.Web.UI.Page
{
    private static string CHANNEL_ACCESS_TOKEN = "DEqOBSijZHzTiFXTTFd8sFTIVg6om7v6YOB1ESG+B5clWRakxiNrMUmHSv3pVUbO/NXVG2NAqffVPLkZsSutw7I0gG0XAESmi/LeoHZ1UGRzlTz3bDYdOORTDcjh0kg/H0y0HpqBkcPy9xE9HoPwDwdB04t89/1O/w1cDnyilFU=";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string sqlcmd = "";
            try
            {
                //開啟這頁的時候，抓一下對方LINE的名稱看有沒有變動。
                string ID = HttpContext.Current.Session["LMBID"].ToString();
                sqlcmd = @" SELECT * FROM LineMember WHERE LMBID = @ID ";
                var user = DBTool.Query(sqlcmd, new { ID = ID }).FirstOrDefault();

                if(user.flag == 1)
                {
                    return;
                }

                //建立 WebRequest 並指定目標的 uri
                string url = "https://api.line.me/v2/bot/profile/" + user.UserID;
                WebRequest request = WebRequest.Create(url);

                //添加從LINE官方取得資料所需要的標頭(header)
                request.ContentType = "application/json; charset=UTF-8";
                request.Headers.Add("Authorization", "Bearer " + CHANNEL_ACCESS_TOKEN);

                //指定 request 使用的 http verb
                request.Method = "GET";
                //使用 GetResponse 方法將 request 送出，如果不是用 using 包覆，請記得手動 close WebResponse 物件，避免連線持續被佔用而無法送出新的 request
                using (var httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    //使用 GetResponseStream 方法從 server 回應中取得資料，stream 必需被關閉
                    //使用 stream.close 就可以直接關閉 WebResponse 及 stream，但同時使用 using 或是關閉兩者並不會造成錯誤，養成習慣遇到其他情境時就比較不會出錯
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        //讀GET的結果
                        var result = streamReader.ReadToEnd();
                        
                        //從字串解析出來
                        var userdata = JsonConvert.DeserializeObject<LineParams>(result.ToString());
                        
                        //如果名稱不同，做更新
                        if(userdata.displayName != user.UserName)
                        {
                            sqlcmd = @" UPDATE LineMember SET UserName = @u WHERE LMBID = @ID ";
                            DBTool.Query(sqlcmd, new { u = userdata.displayName, ID = ID });
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                sqlcmd = @" INSERT INTO SystemLog (PageName,PageFunc,PageLog,PageParams,EX,IP,UserID) 
                            VALUES ('LineMsgList','Page_Load','取得使用者資訊','',@e,'','') ";
                DBTool.Query(sqlcmd, new { e = ex.Message + ";" + ex.StackTrace });

            }
            
        }
    }


    /// <summary>
    /// 取得LINE的名稱
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static string bindata()
    {
        string sqlcmd = "",ID = "";
        if(HttpContext.Current.Session["LMBID"] == null)
        {
            sqlcmd = @" SELECT TOP 1 LMBID,UserName ,dbo.fnCountNoReadMsg(UserID) [SumNoRead]
                            FROM LineMember WHERE flag = 0 
                            order by dbo.fnNewestMsgDate(UserID) DESC
                            ";
            ID = DBTool.Query(sqlcmd, new { }).FirstOrDefault().LMBID.ToString();
        }
        else
        {
            ID = HttpContext.Current.Session["LMBID"].ToString();
        }
        
        sqlcmd = @" SELECT * FROM LineMember WHERE LMBID = @ID ";
        var user = DBTool.Query(sqlcmd, new { ID = ID }).FirstOrDefault();

        return JsonConvert.SerializeObject(new { 
            LMBID = ID,
            Name = user.UserName,
            status = user.flag
        });
    }

    /// <summary>
    /// 取得會員列表
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static string bindUserList(string time)
    {
        string sqlcmd = @" SELECT LMBID,UserName ,dbo.fnCountNoReadMsg(UserID) [SumNoRead]
                            FROM LineMember WHERE flag = 0 
                            order by dbo.fnNewestMsgDate(UserID) DESC
                            ";
        return JsonConvert.SerializeObject(DBTool.Query(sqlcmd,new { }));
    }

    [WebMethod(EnableSession = true)]
    public static void ClickUser(string ID)
    {
        HttpContext.Current.Session["LMBID"] = ID;
    }


    /// <summary>
    /// 取得傳送訊息的紀錄
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="sdate"></param>
    /// <param name="edate"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static string bindMsg(string ID, string sdate, string edate)
    {
        string sqlcmd = " SELECT * FROM LineMember WHERE LMBID = @ID ";
        var userdata = DBTool.Query(sqlcmd, new { ID = ID }).FirstOrDefault();

        sqlcmd = @" SELECT a.* FROM LineMsg a
                        WHERE ( FromUserID = @UserID OR ToUserID = @UserID)
                        AND Convert(nvarchar(10), a.CreateDate, 23) BETWEEN @sdate AND @edate
                        ORDER BY a.CreateDate ASC  ";
        var msglist = DBTool.Query(sqlcmd, new { UserID = userdata.UserID, sdate = sdate, edate = edate });

        if(edate == DateTime.Now.ToString("yyyy-MM-dd"))
        {
            sqlcmd = " UPDATE LineMsg SET readflag = 1 WHERE FromUserID = @UserID AND readflag = 0 ";
            DBTool.Query(sqlcmd, new { UserID = userdata.UserID });
        }

        return JsonConvert.SerializeObject(msglist);
    }


    /// <summary>
    /// 取得最新的訊息
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession =true)]
    public static string bindNewestMsg(string ID)
    {
        string sqlcmd = " SELECT * FROM LineMember WHERE LMBID = @ID ";
        var userdata = DBTool.Query(sqlcmd, new { ID = ID }).FirstOrDefault();

        sqlcmd = @" SELECT TOP 1 a.* FROM LineMsg a
                        WHERE ( FromUserID = @UserID OR ToUserID = @UserID)
                        ORDER BY CreateDate DESC  ";
        var msglist = DBTool.Query(sqlcmd, new { UserID = userdata.UserID });

        sqlcmd = " UPDATE LineMsg SET readflag = 1 WHERE FromUserID = @UserID AND readflag = 0 ";
        DBTool.Query(sqlcmd, new { UserID = userdata.UserID });

        return JsonConvert.SerializeObject(msglist);

    }


    /// <summary>
    /// 發送LINE訊息
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="msg"></param>
    [WebMethod(EnableSession = true)]
    public static void SendMsg(string ID, string msg)
    {
        string sqlcmd = "";

        StringBuilder json = new StringBuilder();

        try
        {
            // 取得LINE的userId
            sqlcmd = @" SELECT * FROM LineMember WHERE LMBID = @ID ";
            var user = DBTool.Query(sqlcmd, new { ID = ID }).FirstOrDefault();


            //建立 WebRequest 並指定目標的 uri
            string url = "https://api.line.me/v2/bot/message/push";
            WebRequest request = WebRequest.Create(url);

            //添加從LINE官方取得資料所需要的標頭(header)
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers.Add("Authorization", "Bearer " + CHANNEL_ACCESS_TOKEN);

            //指定 request 使用的 http verb
            request.Method = "POST";

            //將需 post 的資料內容轉為 stream 
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                //組成push LINE 訊息所需要的API
                json.Append("{\"to\":\"" + user.UserID + "\",");
                json.Append("\"messages\":[{\"type\":\"text\",\"text\":\""+msg+"\"}]}");
                streamWriter.Write(json.ToString());
                streamWriter.Flush();
            }

            //使用 GetResponse 方法將 request 送出，如果不是用 using 包覆，請記得手動 close WebResponse 物件，避免連線持續被佔用而無法送出新的 request
            var httpResponse = (HttpWebResponse)request.GetResponse();
            httpResponse.Close();

            //發送紀錄
            sqlcmd = @" INSERT INTO LineMsg (FromUserID,ToUserID,MsgType,Msg,pushflag) 
                        VALUES ('',@u,'text',@m,1) ";
            DBTool.Query(sqlcmd, new { u = user.UserID, m = msg });

        }
        catch (Exception ex)
        {
            sqlcmd = @" INSERT INTO SystemLog (PageName,PageFunc,PageLog,PageParams,EX,IP,UserID) 
                            VALUES ('LineMsgList','SendMsg','發送LINE訊息',@p,@e,'','') ";
            DBTool.Query(sqlcmd, new { 
                e = ex.Message + ";" + ex.StackTrace ,
                p = json.ToString()
            });

        }

    }


    public class LineParams
    {
        public string userId { get; set; }
        public string displayName { get; set; }
        public string pictureUrl { get; set; }
        public string language { get; set; } 
    }

}