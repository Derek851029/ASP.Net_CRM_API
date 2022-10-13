using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _0060010002 : System.Web.UI.Page
{
    protected string seqno = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        seqno = Request.Params["seqno"];
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Load()
    {
        string Agent_ID;
        Agent_ID = HttpContext.Current.Session["UserID"].ToString();
        if (!string.IsNullOrEmpty(Agent_ID)) //如果不等於空清除空白處, 空的就空字串
        {
            Agent_ID = Agent_ID.Trim();
        }
        else
        {
            Agent_ID = "";
        }
        string Sqlstr = @"SELECT TOP 1 * FROM DispatchSystem WHERE Agent_ID = @Agent_ID AND Agent_Status != '離職'";      // 員工名單內且未離職
        var a = DBTool.Query<ClassTemplate>(Sqlstr, new { Agent_ID = Agent_ID }).ToList().Select(p => new
        {
            Agent_Team = p.Agent_Team,
            Agent_Name = p.Agent_Name,
            Agent_ID = p.Agent_ID,
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Load_Vendor_Data(string seqno)
    {
        string sqlstr = @"SELECT TOP 1 * FROM Vendor_Data WHERE SYSID = '{0}'";
        string sql_format = string.Format(sqlstr, seqno);

        var a = DBTool.Query<Vendor_List>(sql_format).ToList().Select(p => new
        {
            Vendor_Name = p.Vendor_Name,
            Vendor_ID = p.Vendor_ID,
            Vendor_Connection = p.Vendor_Connection,
            Vendor_phone = p.Vendor_phone,
            Select_Main = p.Select_Main,
            Create_Agent = p.Create_Agent,
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Get_VendorList()
    {
        //string Agent_Company = HttpContext.Current.Session["Agent_Company"].ToString();
        string sqlstr = @"SELECT Top 5000 * FROM Vendor_Data";
        var a = DBTool.Query<Vendor_List>(sqlstr).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Vendor_Name = p.Vendor_Name,
            Vendor_phone = p.Vendor_phone,
            Vendor_Connection = p.Vendor_Connection,
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Get_Main()
    {
        //string Agent_Company = HttpContext.Current.Session["Agent_Company"].ToString();
        string sqlstr = @"SELECT OE_T_ID,Main,Detail FROM OE_Type WHERE Type = 2";
        var a = DBTool.Query<Main_List>(sqlstr).ToList().Select(p => new
        {
            OE_T_ID = p.OE_T_ID,
            Main = p.Main,
            Detail = p.Detail
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Add_Vendor(string Vendor_Name, string Vendor_ID, string Vendor_Connection, string Vendor_phone, string Select_Main, string Create_Agent,int seqno)
    {
        string sqlstr;
        string sql_format;
        if (seqno != 0)
        {
            //Get vendor SYSID because not thing to check. Select_Main is string so string to array and one by one to update
            sqlstr = @"SELECT SYSID FROM Vendor_Data WHERE SYSID = '" + seqno + "'";
            var b = DBTool.Query(sqlstr).FirstOrDefault();

            sqlstr = @"SELECT Select_Main FROM Vendor_Data WHERE SYSID = '" + seqno + "'";
            var d = DBTool.Query(sqlstr).FirstOrDefault();

            string[] Ori_Main = d.Select_Main.Split(','); //original select data
            string[] Main_ID = Select_Main.Split(',');
            //for (int i = 0; i< Main_ID.Length; i++) //if select content not in original select data set owner = 0 else set oe_type owner = vendro sysid
            //{
            //    int location = Array.IndexOf(Ori_Main,Main_ID[i]);
            //    if (location == -1)
            //    {
            //        sqlstr = @"UPDATE OE_Type SET Owner_Vendor = '{0}' WHERE OE_T_ID = '{1}'";
            //        sql_format = string.Format(sqlstr, '0', Main_ID[i]);
            //        var c = DBTool.Query(sql_format);
            //    }
            //    else
            //    {
            //        sqlstr = @"UPDATE OE_Type SET Owner_Vendor = '{0}' WHERE OE_T_ID = '{1}'";
            //        sql_format = string.Format(sqlstr, b.SYSID.ToString(), Main_ID[i]);
            //        var c = DBTool.Query(sql_format);
            //    }

            //}
            //first to update owner oe_type and remove original array select data
            foreach (var ID in Main_ID)
            {
                Ori_Main = Ori_Main.Where(val => val != ID).ToArray(); //Remove ori_main ID
                sqlstr = @"UPDATE OE_Type SET Owner_Vendor = '{0}' WHERE OE_T_ID = '{1}'";
                sql_format = string.Format(sqlstr, b.SYSID.ToString(), ID);
                var c = DBTool.Query(sql_format);
            }
            // remain original array is remove item
            foreach (var Ori_ID in Ori_Main)
            {
                sqlstr = @"UPDATE OE_Type SET Owner_Vendor = '{0}' WHERE OE_T_ID = '{1}'";
                sql_format = string.Format(sqlstr, '0', Ori_ID);
                var c = DBTool.Query(sql_format);
            }

            sqlstr = @"UPDATE Vendor_Data SET Vendor_Name = '{0}',Vendor_ID = '{1}',Vendor_Connection = '{2}',Vendor_phone = '{3}',Select_Main = '{4}',Create_Agent = '{5}' WHERE SYSID = '{6}'";

            sql_format = string.Format(sqlstr, Vendor_Name, Vendor_ID, Vendor_Connection, Vendor_phone, Select_Main, Create_Agent, seqno);
            var a = DBTool.Query(sql_format);

            return JsonConvert.SerializeObject(new { status = "廠商修改完成。" });
        }
        else
        {

            sqlstr = @"INSERT INTO Vendor_Data ([Vendor_Name]
          ,[Vendor_ID]
          ,[Vendor_Connection]
          ,[Vendor_phone]
          ,[Select_Main]
          ,[Create_Agent]) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')";

            sql_format = string.Format(sqlstr, Vendor_Name, Vendor_ID, Vendor_Connection, Vendor_phone, Select_Main, Create_Agent);
            var a = DBTool.Query(sql_format);

            //Get vendor SYSID because not thing to check. Select_Main is string so string to array and one by one to update
            sqlstr = @"SELECT SYSID FROM Vendor_Data WHERE Vendor_ID = '" + Vendor_ID + "'";
            var b = DBTool.Query(sqlstr).FirstOrDefault();

            string[] Main_ID = Select_Main.Split(',');
            foreach (var ID in Main_ID)
            {
                sqlstr = @"UPDATE OE_Type SET Owner_Vendor = '{0}' WHERE OE_T_ID = '{1}'";
                sql_format = string.Format(sqlstr, b.SYSID.ToString(), ID);
                var c = DBTool.Query(sql_format);
            }
            

            return JsonConvert.SerializeObject(new { status = "廠商新增完成。" });
        }
        
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]  
    public static string List_Owner_Main(string Vendor_SYSID)  
    {
        string sqlstr = @"SELECT * FROM OE_Type WHERE Owner_Vendor = '{0}'";
        string sql_format = string.Format(sqlstr, Vendor_SYSID);
        var a = DBTool.Query<Main_List>(sql_format).ToList().Select(p => new
        {
            OE_T_ID = p.OE_T_ID,
            Main = p.Main,
            Detail = p.Detail
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }


    public class Vendor_List
    {
        public string SYSID { get; set; }
        public string Vendor_Name { get; set; }
        public string Vendor_ID { get; set; }
        public string Vendor_Connection { get; set; }
        public string Vendor_phone { get; set; }
        public string Select_Main { get; set; }
        public string Create_Agent { get; set; }
    }

    public class Main_List
    {
        public string OE_T_ID { get; set; }
        public string Type { get; set; }
        public string Main { get; set; }
        public string Detail { get; set; }
    }
}