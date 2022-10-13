using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GetLineMsg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    /// <summary>
    /// 紀錄line訊息的收發
    /// </summary>
    /// <param name="FromUserID"></param>
    /// <param name="ToUserID"></param>
    /// <param name="msg"></param>
    [WebMethod(EnableSession = true)]
    public static void getmsg(string FromUserID,string ToUserID,string MsgType,string msg)
    {
        string sqlcmd = " INSERT INTO LineMsg (FromUserID,ToUserID,MsgType,Msg,replyflag) VALUES (@FID,@TID,@MsgType,@msg,@r) ";
        string replyflag = "0";
        if(ToUserID != "")
        {
            replyflag = "1";
        }
        DBTool.Query(sqlcmd, new { 
            FID = FromUserID,
            TID = ToUserID,
            MsgType = MsgType,
            msg = msg,
            r = replyflag
        });
    }


    [WebMethod(EnableSession = true)]
    public static void getLinePost(string ID, string content)
    {
        string sqlcmd = " INSERT INTO LinePost (UserID,Content) VALUES (@ID,@content) ";
        DBTool.Query(sqlcmd, new
        {
            ID = ID,
            content = content
        });
    }
    
    /// <summary>
    /// 取得傳送訊息的userID 並且紀錄下line所發送過來的內容
    /// 回傳資料庫中儲存的回覆內容
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static string getLineReply(string ID, string content)
    {
        try
        {
            string sqlcmd = " INSERT INTO LinePost (UserID,Content) VALUES (@ID,@content) ";
            DBTool.Query(sqlcmd, new
            {
                ID = ID,
                content = content
            });

            sqlcmd = " SELECT * FROM LineReply WHERE flag = 0 ";

            return JsonConvert.SerializeObject(DBTool.Query(sqlcmd, new { }).Select(s => new { 
                Keys = s.Keys,
                Reply = s.Reply
            }));

        }
        catch(Exception ex)
        {
            string sqlcmd = @" INSERT INTO SystemLog (Page,FunctionName,Ex) 
                                VALUES (@Page,@FunctionName,@Ex) ";
            
            DBTool.Query(sqlcmd, new
            {
                PageName = "GetLineMsg",
                FunctionName = "getLineReply",
                EX = ex.ToString()
            });

            return "Error";
        }
    }



    [WebMethod(EnableSession = true)]
    public static void MemberStatus(string type, string ID, string name)
    {
        string sqlcmd = "SELECT * FROM LineMember WHERE UserID = @ID ";
        var member = DBTool.Query(sqlcmd, new { ID = ID });
        if (member.Any())
        {
            switch (type)
            {
                case "follow":
                    sqlcmd = " UPDATE LineMember SET flag = 0,UserName = @n,UpdateDate = GETDATE() WHERE UserID = @ID ";
                    DBTool.Query(sqlcmd, new { ID = ID, n = name });
                    break;
                case "unfollow":
                    sqlcmd = " UPDATE LineMember SET flag = 1,UpdateDate = GETDATE() WHERE UserID = @ID ";
                    DBTool.Query(sqlcmd, new { ID = ID });
                    break;
                default:
                    return;
            }
        }
        else if(type == "follow")
        {
            sqlcmd = " INSERT INTO LineMember (Num,UserID,UserName) VALUES (@num,@ID,@name) ";
            DBTool.Query(sqlcmd, new { num = "", ID = ID, name = name });
        }
    }


}