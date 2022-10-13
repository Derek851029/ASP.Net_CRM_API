using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MemberEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }


    /// <summary>
    /// 取得家長資料
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static string bindata()
    {
        string sqlcmd = @" SELECT * FROM LineMember WHERE LMBID = @ID ";

        string LMBID = HttpContext.Current.Session["MemberEdit"].ToString();

        var data = DBTool.Query(sqlcmd, new { ID = LMBID });//LINE會員資料


        if (data.Any())
        {
            sqlcmd = " SELECT * FROM MemberData WHERE UserID = @u ";
            string UserID = data.FirstOrDefault().UserID;
            var user = DBTool.Query(sqlcmd, new { u = UserID });//家長資料
            if (user.Any())
            {
                return JsonConvert.SerializeObject( new { 
                    user = user
                });
            }
            sqlcmd = @" INSERT INTO MemberData (UserID,MName,MIdentity,Sex,Phone,Email,MAddress) 
                        VALUES (@u,'','志工','','','','')
                        SELECT * FROM MemberData WHERE UserID = @u ";
            return JsonConvert.SerializeObject(new { 
                user = DBTool.Query(sqlcmd, new { u = UserID })
            });
        }

        return "";
    }


    /// <summary>
    /// 修改家長資料
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [WebMethod(EnableSession =true)]
    public static string EditUser(MemberData data)
    {
        string sqlcmd = "";

        try
        {
            //更新人員資料
            sqlcmd = @" UPDATE MemberData SET 
                            MName = @MName, MIdentity = @MIdentity, 
                            Sex = @Sex, Phone = @Phone, Email = @Email,
                            MAddress = @MAddress ,UpdateDate = GETDATE() WHERE MID = @MID ";
            DBTool.Query(sqlcmd, data);

            return "編輯成功";
        }
        catch(Exception ex)
        {
            sqlcmd = @" INSERT INTO SystemLog (PageName,PageFunc,PageLog,PageParams,EX,IP,UserID) 
                        VALUES ('MemberEdit','EditUser','修改家長資料',@data,@ex,@ip,@u ) ";
            DBTool.Query(sqlcmd, new { 
                data = JsonConvert.SerializeObject(data),
                ex = ex.Message+";"+ex.StackTrace,
                ip = "",
                u = ""
            });

            return "編輯失敗";

        }
    }


}