using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _2021_case_0010010004 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    public static string getParterList()
    {
        string sqlCommand = @"SELECT PID,ID,BUSINESSNAME,APPNAME FROM BusinessData WHERE [Type] = '保留'";
        var result = DBTool.Query<BusinessData>(sqlCommand).ToList();
        return JsonConvert.SerializeObject(result);
    }

    [WebMethod(EnableSession = true)]
    public static string getAgentNameList()
    {
        string sqlCommand = @"SELECT Agent_Name FROM DispatchSystem
                          WHERE Agent_Company IN ('極緻健康科技','極緻科技') 
	                      AND Agent_Status = '在職' AND Agent_Name NOT LIKE '系統%'";
        var result = DBTool.Query<Dispatchsystem>(sqlCommand).ToList();
        return JsonConvert.SerializeObject(result);
    }

    [WebMethod(EnableSession = true)]
    public static string getOEList()
    {
        string sqlCommand = @"SELECT * FROM OE_Product";
        var result = DBTool.Query<OE_Product>(sqlCommand).ToList();
        return JsonConvert.SerializeObject(result);
    }

    [WebMethod(EnableSession = true)]
    public static void saveCaseData(saveCaseDataList saveCaseDataList)
    {
        string sqlCommand =
            @"INSERT INTO Case_List ([Case_Name]
                ,[Clinet_Name]
                ,[Contact]
                ,[System_Data]
                ,[Personnel]
                ,[Project_Content]
                ,[Remark])
            VALUES(@txt_Case_Name,@txt_Clinet_Name,@ul_ContactList,@ol_OEList,@txt_Personnel,@txt_projectContext,@txt_projectRemark)";
        DBTool.Query(sqlCommand, saveCaseDataList);
    }
    public class BusinessData
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        public int PID { get; set; }
        /// <summary>
        /// 公司統編
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 公司名稱
        /// </summary>
        public string BUSINESSNAME { get; set; }
        /// <summary>
        /// 聯絡窗口
        /// </summary>
        public string APPNAME { get; set; }
    }
    public class Dispatchsystem
    {
        public string Agent_Name { get; set; }
    }
    public class OE_Product
    {
        public int OE_ID { get; set; }
        public string Product_Name { get; set; }
        public string Main_Classified { get; set; }
        public string Unit_Price { get; set; }
    }
    public class saveCaseDataList
    {
        public string txt_Case_Name { get; set; }
        public string txt_Clinet_Name { get; set; }
        public string txt_Personnel { get; set; }
        public string ul_ContactList { get; set; }
        public string ol_OEList { get; set; }
        public string txt_projectRemark { get; set; }
        public string txt_projectContext { get; set; }
    }
}