using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Face 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class Face : System.Web.Services.WebService
{

    public Face()
    {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }

    [WebMethod]
    public string FaceService()
    {
        string sql;
        try
        {
            string data = HttpContext.Current.Request.Params["event_log"] == null ? null : HttpContext.Current.Request.Params["event_log"].ToString();
            
            //string sql233 = "insert into SystemLog(Ex) values('" + data.ToString() + "')";
            //DBTool.Query(sql233);
            if (string.IsNullOrEmpty(data))
            {
            }
            else
            {
                dynamic result = JsonConvert.DeserializeObject(data);
                if (result.ToString().IndexOf("AccessControllerEvent") > -1)
                {
                    var AccessControllerEvent = result.AccessControllerEvent.ToString();

                    dynamic result2 = JsonConvert.DeserializeObject(AccessControllerEvent);
                    if (result2.ToString().IndexOf("name") > -1)
                    {
                        var Name = result2.name.ToString();
                        var SourceIP = result.ipAddress.ToString();
                        var Mask = result2.mask.ToString();
                        var Date = result.dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        //Date = Date.Substring(0, 18).Replace("T", " ");

                        var Number = result2.employeeNoString.ToString();
                        sql = "select MemberNum from MemberData where MemberNum = @MemberNum";
                        var MemberOrNot = DBTool.Query(sql,new { MemberNum = Number });
                        if (MemberOrNot.Any())
                        {
                            sql = "insert into FaceRecognitionLog(SourceIP,Number,Position,Name,Mask,Date) values(@SourceIP,@Number,@Position,@Name, @Mask, @Date)";
                            DBTool.Query(sql, new
                            {
                                SourceIP = SourceIP,
                                Number = Number,
                                Position = "Member",
                                Name = Name,
                                Mask = Mask,
                                Date = Date,
                            });
                        }
                        else
                        {
                            sql = "insert into FaceRecognitionLog(SourceIP,Number,Position,Name,Mask,Date) values(@SourceIP,@Number,@Position,@Name, @Mask, @Date)";
                            DBTool.Query(sql, new
                            {
                                SourceIP = SourceIP,
                                Number = Number,
                                Position = "Staff",
                                Name = Name,
                                Mask = Mask,
                                Date = Date,
                            });
                        }
                    }
                }
            }
            
        }
        catch(Exception e)
        {
            sql = "insert into SystemLog(Page,FunctionName,Ex) values(@Page,@FunctionName.@Ex)";
            DBTool.Query(sql,new {
                Page = "Face.asmx",
                FunctionName = "FaceService",
                Ex = e.ToString(),
            });
        }
        
        return "200";
    }

    public class resultData{
        public string ipAddress;
        public string dateTime;
        public dynamic AccessControllerEvent;
    }
    public class result2Data
    {
        public string name;
        public string employeeNoString;
        public string mask;
    }
}
