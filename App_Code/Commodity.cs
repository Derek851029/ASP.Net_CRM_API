using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;

/// <summary>
/// Commodity 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class Commodity : System.Web.Services.WebService
{
    private static string CHANNEL_ACCESS_TOKEN = "mZqfRdPEiVyDgMGVaq/Nq5c3jrK9KXZGuCzleuRqpEvzROVrlF4V50mRQMNoFvJHMjac0hAzTclR4CsSc9fTQEK1NpA3RfGUGGHl1bZfFJHbfmVoMtDAe5R4CKU/45Rh6vGxKuMlI22W9BO8OvWuFQdB04t89/1O/w1cDnyilFU=";
    public Commodity()
    {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }

    [WebMethod]
    public string CommodityService()
    {
        string sql = @"select * from CommodityData";
        var a = DBTool.Query(sql);
        return JsonConvert.SerializeObject(a);
    }

    [WebMethod]
    public string UpdateCommodity(string device, string status)
    {
        try
        {
            string sql = "";
            if (device == "1")
            {
                if (status == "0")
                {
                    sql = @"update CommodityData set Number = '0' where CDID = 7";
                    SendMsg("Ub6440fbdd0ac174d3723366d4f6da57e", "商品:健家特-舒酸維他EX\\n貨架數量:0\\n狀態:缺貨");
                }
                else
                {
                    sql = @"update CommodityData set Number = '1' where CDID = 7";
                    SendMsg("Ub6440fbdd0ac174d3723366d4f6da57e", "商品:健家特-舒酸維他EX\\n貨架數量:1\\n狀態:補貨完成");
                }
            }

            if (device == "2")
            {
                if (status == "0")
                {
                    sql = @"update CommodityData set Number = '0' where CDID = 7";
                    SendMsg("Ub6440fbdd0ac174d3723366d4f6da57e", "商品:健家特-益生菌膠囊\\n貨架數量:0\\n狀態:缺貨");
                }
                else
                {
                    sql = @"update CommodityData set Number = '1' where CDID = 7";
                    SendMsg("Ub6440fbdd0ac174d3723366d4f6da57e", "商品:健家特-益生菌膠囊\\n貨架數量:1\\n狀態:補貨完成");
                }

            }
            var a = DBTool.Query(sql);

            string sql2 = @"insert into SystemLog(Page,FunctionName) values('" + device + "','" + status + "')";
            var b = DBTool.Query(sql2);
        }
        catch(Exception e)
        {
            string sql2 = @"insert into SystemLog(Page,FunctionName,Ex) values(@Page,@FunctionName,@Ex)";
            var b = DBTool.Query(sql2, new
            {
                Page = "",
                FunctionName = "",
                Ex = e.ToString()
            });
        }
        
        return "success";
    }

    public static void SendMsg(string ID, string msg)
    {
        string sqlcmd = "";

        StringBuilder json = new StringBuilder();

        try
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var user = ID;

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
                json.Append("{\"to\":\"" + user + "\",");
                json.Append("\"messages\":[{\"type\":\"text\",\"text\":\"" + msg + "\"}]}");
                streamWriter.Write(json.ToString());
                streamWriter.Flush();
            }

            //使用 GetResponse 方法將 request 送出，如果不是用 using 包覆，請記得手動 close WebResponse 物件，避免連線持續被佔用而無法送出新的 request
            var httpResponse = (HttpWebResponse)request.GetResponse();
            httpResponse.Close();

            //發送紀錄
            sqlcmd = @" INSERT INTO LineMsg (FromUserID,ToUserID,MsgType,Msg,pushflag) 
                        VALUES ('',@u,'text',@m,1) ";
            DBTool.Query(sqlcmd, new { u = user, m = msg });

        }
        catch (Exception ex)
        {
            sqlcmd = @"insert into SystemLog(Page,FunctionName,Ex) values(@Page,@FunctionName,@Ex)";
            DBTool.Query(sqlcmd, new
            {
                Page = "",
                FunctionName = "",
                Ex = ex.ToString()
            });

        }

    }

}
