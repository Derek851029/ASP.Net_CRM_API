using Dapper;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using log4net;
using log4net.Config;
using System.IO;

public partial class Report_Report_001 : System.Web.UI.Page
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected string seqno;
    protected void Page_Init(object sender, EventArgs e)
    {
        Check();
        seqno = Request.Params["seqno"];
        Session["CNo"] = Request.Params["seqno"];
        //if (string.IsNullOrEmpty(Request.Params["company"]))
        //    Session["company"] = Request.Params["company"];
        //else
        //    Session["company"] = "0";
        string str_company = Request.Params["company"];
        if (string.IsNullOrEmpty(seqno))
        {
            Response.Redirect("~/0030010000/0030010003.aspx");
        }
        //http://127.0.0.1:5000/Report/Report_001.aspx?seqno=161130131535020

        string Labor_CName = "";
        string Labor_EName = "";
        string Labor_Company = "";
        string Cust_Name = "";
        string ServiceName = "";
        string UpdateDate = "";
        string CQ = "";
        string LQ = "";
        string CA = "";
        string LA = "";
        string Day = "";
        string DEPT_Status = "";
        string Logo = "";
        string T_ID = "";
        //DateTime Time_01, Time_02, Time_03;
        string Agent_Sign = "";
        string sqlstr = @"SELECT Top 1 *, CONVERT(varchar(100), Service_DATE, 23) as Day, SetupTime as Time_01, ReachTime as Time_02, " +
            "FinishTime as Time_03, Handle_Agent as Labor_EName, BUSINESSNAME as Cust_Name, RTRIM(Telecomm_ID) FROM View_Case WHERE Case_ID=@CNo";
        var list = DBTool.Query<ClassTemplate>(sqlstr, new { CNo = seqno });
        if (list.Any())
        {
            ClassTemplate schedule = list.First();
            Cust_Name = schedule.Cust_Name;
            Labor_CName = schedule.C_ID2;
            Labor_EName = schedule.APPNAME;
            Labor_Company = schedule.APP_MTEL;
            ServiceName = schedule.HardWare;
            CA = schedule.PS;
            CQ = schedule.OpinionType;
            Day = schedule.Day;
            T_ID = Value(schedule.Telecomm_ID);
            if (string.IsNullOrEmpty(schedule.SetupTime))
            {
                UpdateDate = "　　年　月　日　時　分";
            }
            else
            {
                UpdateDate = schedule.Time_01.ToString("yyyy年MM月dd日 HH時mm分");
            }
            if (string.IsNullOrEmpty(schedule.ReachTime))
            {
                LQ = "　　年　月　日　時　分 ";
            }
            else
            {
                LQ = schedule.Time_02.ToString("yyyy年MM月dd日 HH時mm分");
            }
            if (string.IsNullOrEmpty(schedule.FinishTime))
            {
                LA = "　　年　月　日　時　分";
            }
            else
            {
                LA = schedule.Time_03.ToString("yyyy年MM月dd日 HH時mm分");
            }
            if (schedule.Service_Flag == "1")
            {
                Agent_Sign = schedule.Handle_Agent; // + "  " + schedule.Time_03.ToString("MM/dd")
            }
            else
            {
                Agent_Sign = "";
            }
        }

        if (!string.IsNullOrEmpty(T_ID))
            {
                if (T_ID == "中華電信" || T_ID == "德瑪")
                {
                    DEPT_Status = "德瑪科技股份有限公司 台北市堤頂大道二段297號8樓 TEL：886-2-7718-8250 FAX：886-2-2657-7739";
                    Logo = "Dimax";
                }
                else if (T_ID == "遠傳")
                {
                    DEPT_Status = "遠傳電信          台北市內湖區瑞光路218號  TEL：886-2-7718-8250";  //   ■   □
                    Logo = "Far";
                }
                else
                {
                    DEPT_Status = "";
                    Logo = "defaultimage";
                }
            }
            else
            {
                DEPT_Status = "";
                Logo = "defaultimage";
            }


        if (str_company == "2")
        {
            DEPT_Status = "遠傳電信          台北市內湖區瑞光路218號  TEL：886-2-7718-8250";  //   ■   □
            Logo = "Far";
        }
        else if (str_company == "1")
        {
            DEPT_Status = "德瑪科技股份有限公司 台北市堤頂大道二段297號8樓 TEL：886-2-7718-8250 FAX：886-2-2657-7739";
            Logo = "Dimax";
        }
        /*else
        {
            DEPT_Status ="";
            Logo = "defaultimage";
        }  //*/

        rptviewer.LocalReport.EnableExternalImages = true;
        rptviewer.LocalReport.ReportPath = Server.MapPath("/Report/Report.rdlc");
        rptviewer.LocalReport.DataSources.Clear();

        if (str_company != "")
        {
            sqlstr = @"SELECT Top 1 * FROM BusinessData ";
            var list_company = DBTool.Query<HRM2_Company>(sqlstr, new {  });
            if (list_company.Any())
            {
                HRM2_Company schedule = list_company.First();
                //Labor_Company = schedule.BUSINESSNAME;
            }
        }

        string imagepath = string.Format("{0}", seqno.Trim());
        logger.Info("Signature" + imagepath + ".jpg");
        if (!File.Exists(@"D:\Signature1\" + imagepath + "_sign.jpg"))
        {
            imagepath = "defaultimage";
        }
        string imagepath2 = string.Format("{0}", seqno.Trim());
        logger.Info("Signature" + imagepath2 + ".jpg");
        if (!File.Exists(@"D:\Signature1\" + imagepath2 + "_C_sign.jpg"))
        {
            imagepath2 = "defaultimage";
        }
        Microsoft.Reporting.WebForms.ReportParameter p_1 = new Microsoft.Reporting.WebForms.ReportParameter("Cust_Question", CQ);
        Microsoft.Reporting.WebForms.ReportParameter p_2 = new Microsoft.Reporting.WebForms.ReportParameter("Labor_Question", LQ);
        Microsoft.Reporting.WebForms.ReportParameter p_3 = new Microsoft.Reporting.WebForms.ReportParameter("Labor_CName", Labor_CName);
        Microsoft.Reporting.WebForms.ReportParameter p_4 = new Microsoft.Reporting.WebForms.ReportParameter("Labor_EName", Labor_EName);
        Microsoft.Reporting.WebForms.ReportParameter p_5 = new Microsoft.Reporting.WebForms.ReportParameter("Labor_Company", Labor_Company);
        Microsoft.Reporting.WebForms.ReportParameter p_6 = new Microsoft.Reporting.WebForms.ReportParameter("Cust_Name", Cust_Name);
        Microsoft.Reporting.WebForms.ReportParameter p_7 = new Microsoft.Reporting.WebForms.ReportParameter("ServiceName", ServiceName);
        Microsoft.Reporting.WebForms.ReportParameter p_8 = new Microsoft.Reporting.WebForms.ReportParameter("Time", UpdateDate);
        Microsoft.Reporting.WebForms.ReportParameter p_9 = new Microsoft.Reporting.WebForms.ReportParameter("Cust_Answer", CA);
        Microsoft.Reporting.WebForms.ReportParameter p_10 = new Microsoft.Reporting.WebForms.ReportParameter("Labor_Answer", LA);
        Microsoft.Reporting.WebForms.ReportParameter p_11 = new Microsoft.Reporting.WebForms.ReportParameter("DEPT_Status", DEPT_Status);
        Microsoft.Reporting.WebForms.ReportParameter p_12 = new Microsoft.Reporting.WebForms.ReportParameter("ImagePath", @"D:\Signature1\" + imagepath + "_sign.jpg");
        Microsoft.Reporting.WebForms.ReportParameter p_13 = new Microsoft.Reporting.WebForms.ReportParameter("ImagePath2", @"D:\Signature1\" + imagepath2 + "_C_sign.jpg");
        Microsoft.Reporting.WebForms.ReportParameter p_14 = new Microsoft.Reporting.WebForms.ReportParameter("AVAYA", @"D:\Signature1\AVAYA.png");
        Microsoft.Reporting.WebForms.ReportParameter p_15 = new Microsoft.Reporting.WebForms.ReportParameter("Logo", @"D:\Signature1\" + Logo + ".png");
        Microsoft.Reporting.WebForms.ReportParameter p_16 = new Microsoft.Reporting.WebForms.ReportParameter("Day", Day);
        Microsoft.Reporting.WebForms.ReportParameter p_17 = new Microsoft.Reporting.WebForms.ReportParameter("Agent_Sign", Agent_Sign);
        rptviewer.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { p_1, p_2, p_3, p_4, p_5, p_6, p_7, p_8, p_9, p_10, p_11, p_12, p_13, p_14, p_15, p_16, p_17 });
        rptviewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("View_Service", list));
        rptviewer.LocalReport.Refresh();
    }

    protected void Check()
    {
        string Check = JASON.Check_ID("0020010005.aspx");
        if (Check == "NO")
        {
            //System.Web.HttpContext.Current.Response.Redirect("~/Default.aspx");
        }
    }

    //============= 帶入【仲介】選單 =============
    [WebMethod(EnableSession = true)]
    public static string Company_List()
    {
        string sqlstr = "SELECT * FROM HRM2_Company ORDER BY Company_CName ";
        var a = DBTool.Query<HRM2_Company>(sqlstr).ToList().Select(p => new
        {
            SYSID = p.SYSID,
            Company_CName = p.Company_CName
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    public class HRM2_Company
    {
        public string SYSID { get; set; }
        public string Company_CName { get; set; }
        public string BUSINESSNAME { get; set; }
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string UploadPic0()
    {
        try
        {
            string CNo = HttpContext.Current.Session["CNo"].ToString();
            //string company = HttpContext.Current.Session["company"].ToString();
            string name = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            string sqlstr = @"UPDATE CaseData SET " +   //工程師簽名後改 Service_Flag 此案件不再被下載
           "Service_Flag='1', Service_DATE=GETDATE() " +
           "WHERE Case_ID=@Case_ID";
            using (IDbConnection db = DBTool.GetConn())
                db.Execute(sqlstr, new { Case_ID = CNo });
            return JsonConvert.SerializeObject(new { flag = "0", txt = "/Report/Report_001.aspx?seqno=" + CNo });
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new { flag = "1", txt = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。" + ex });
        }
    }
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string UploadPic(string imageData)
    {
        try
        {
            string CNo = HttpContext.Current.Session["CNo"].ToString();
            //string company = HttpContext.Current.Session["company"].ToString();
            string name = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            string Pic_Path = (@"D:\Signature1\" + CNo + "_sign.jpg");
            //string Pic_Path = (@"118.163.27.22:8080/Signature/" + CNo + "_sign.jpg");
            using (FileStream fs = new FileStream(Pic_Path, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageData);
                    bw.Write(data);
                    bw.Close();
                }
            }   // 這段產生了錯誤 */  

            string sqlstr = @"UPDATE CaseData SET " +   //工程師簽名後改 Service_Flag 此案件不再被下載
           "Service_Flag='1', Service_DATE=GETDATE() " +    
           "WHERE Case_ID=@Case_ID";

            using (IDbConnection db = DBTool.GetConn())
                db.Execute(sqlstr, new { Case_ID = CNo });

            /*sqlstr = @"insert into  CASEDetail (CNo, Service_DATE, Service_Flag) " +   //移除案件測試
           "Values(@Case_ID, GETDATE(), '1') ";

            using (IDbConnection db = DBTool.GetConn())
                db.Execute(sqlstr, new { Case_ID = CNo });  //*/

            return JsonConvert.SerializeObject(new { flag = "0", txt = "/Report/Report_001.aspx?seqno=" + CNo + "&company="  });//+ company 
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new { flag = "1", txt = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。" + ex });
        }
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string UploadPic_C(string imageData)
    {
        try
        {
            string CNo = HttpContext.Current.Session["CNo"].ToString();
            //string company = HttpContext.Current.Session["company"].ToString();
            string name = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            string Pic_Path = (@"D:\Signature1\" + CNo + "_C_sign.jpg");
            //string Pic_Path = (@"118.163.27.22:8080/Signature/" + CNo + "_sign.jpg");
            using (FileStream fs = new FileStream(Pic_Path, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageData);
                    bw.Write(data);
                    bw.Close();
                }
            }   // 這段產生了錯誤 */  

           /* string sqlstr = @"UPDATE CaseData SET " +
           "Service_Flag='1', Service_DATE=GETDATE() " +   
           "WHERE Case_ID=@Case_ID"; //目前交給工程師簽名判定 

            using (IDbConnection db = DBTool.GetConn())
                db.Execute(sqlstr, new { Case_ID = CNo });*/

            return JsonConvert.SerializeObject(new { flag = "0", txt = "/Report/Report_001.aspx?seqno=" + CNo + "&company="  });// + company
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new { flag = "1", txt = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。" + ex });
        }
    }

    public static string Value(string value)        // 當值為null時跳過  非 null 時去除後方空白
    {
        if (!string.IsNullOrEmpty(value))
        {
            value = value.Trim();
        }
        return value;
    }
}