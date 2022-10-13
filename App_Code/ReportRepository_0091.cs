using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.IO;
using NPOI.SS.Util;

/// <summary>
/// 派工系統 - 以雇主服務統計 ( 前 10 家 )
/// </summary>
public class ReportRepository_0091
{
    #region 屬性
    private string StartDate { get; set; }
    private string EndDate { get; set; }
    private string Create_Team { get; set; }
    private string Type_Value { get; set; }
    private string Service { get; set; }
    private string Service_ID { get; set; }
    private object whereobject
    {
        get
        {
            if (string.IsNullOrEmpty(Create_Team))
                return new { StartDate, EndDate, Type_Value, Service, Service_ID };
            else
                return new { StartDate, EndDate, Create_Team, Type_Value, Service, Service_ID };
        }
    }

    /// <summary>
    /// SQL
    /// </summary>
    private string QuerySqlStr
    {
        get
        {
            /*string Sqlstr = @"select * from LaborTemplate " +
                "where convert(varchar(10), Create_TIME , 120) between @StartDate AND @EndDate {0} {1} {2} {3} " +
                "order by Type, Service, ServiceName, MNo, Create_TIME, SYS_ID ";   //*/
            string Sqlstr = @"select * from OE_Order where OE_Case_ID ='170630152407413' "; 
            return string.Format(Sqlstr,
                string.IsNullOrEmpty(Create_Team) ? string.Empty : " AND Create_Team=@Create_Team",
                string.IsNullOrEmpty(Type_Value) ? string.Empty : " AND Type_Value=@Type_Value ",
                string.IsNullOrEmpty(Service) ? string.Empty : " AND Service=@Service ",
                string.IsNullOrEmpty(Service_ID) ? string.Empty : " AND Service_ID=@Service_ID "
            );
        }
    }

    #endregion

    public ReportRepository_0091(string StartDate, string EndDate, string Create_Team, string Type_Value, string Service, string Service_ID)
    {
        this.StartDate = StartDate;
        this.EndDate = EndDate;
        if (Create_Team == "全部門")
            Create_Team = "";
        this.Create_Team = Create_Team;
        this.Type_Value = Type_Value;
        this.Service = Service;
        this.Service_ID = Service_ID;
    }
    public string GetView()
    {
        var list = DBTool.Query<SelfCompleteServiceData>(QuerySqlStr, whereobject);

        IWorkbook workbook = new XSSFWorkbook();
        SetSheet(workbook, list.ToList());
        // Output the HTML file
        string filename = Guid.NewGuid().ToString("D") + ".html";
        string pathstr = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp", filename);
        ExcelTool.ConverHTML(workbook, pathstr);
        workbook.Close();
        return filename;
    }
    public byte[] GetReport()
    {
        var list = DBTool.Query<SelfCompleteServiceData>(QuerySqlStr, whereobject);

        IWorkbook workbook = new XSSFWorkbook();
        SetSheet(workbook, list.ToList());
        using (MemoryStream memorystream = new MemoryStream())
        {
            workbook.Write(memorystream);
            workbook.Close();
            return memorystream.ToArray();
        }
    }

    private void SetSheet(IWorkbook workbook, List<SelfCompleteServiceData> list)
    {
        ISheet sheet = workbook.CreateSheet("【派工系統】需求單明細表A");
        int colindex = 0;
        int rowindex = 0;
        var datagroup = from a in list
                        group a by a.Agent_Team;
        IRow row = null;
        foreach (var item in datagroup)
        {
            colindex = 0;
            row = sheet.CreateRow(rowindex++);
            row.CreateCell(colindex).SetCellValue(item.Key);
            #region 表頭
            colindex = 0;
            row = sheet.CreateRow(rowindex++);
            row.CreateCell(colindex++).SetCellValue("序號");
            row.CreateCell(colindex++).SetCellValue("需求單編號");
            row.CreateCell(colindex++).SetCellValue("需求單狀態");
            row.CreateCell(colindex++).SetCellValue("服務項目");
            row.CreateCell(colindex++).SetCellValue("服務內容");
            row.CreateCell(colindex++).SetCellValue("客戶編號");
            row.CreateCell(colindex++).SetCellValue("客戶名稱");
            row.CreateCell(colindex++).SetCellValue("勞工公司");
            row.CreateCell(colindex++).SetCellValue("勞工部門");
            row.CreateCell(colindex++).SetCellValue("勞工編號");
            row.CreateCell(colindex++).SetCellValue("勞工英文名");
            row.CreateCell(colindex++).SetCellValue("勞工中文名");
            row.CreateCell(colindex++).SetCellValue("勞工國籍");
            row.CreateCell(colindex++).SetCellValue("勞工護照號碼");  //PID
            row.CreateCell(colindex++).SetCellValue("勞工居留證號");  //RID
            row.CreateCell(colindex++).SetCellValue("勞工職工編號");  //EID
            row.CreateCell(colindex++).SetCellValue("勞工手機號碼");
            row.CreateCell(colindex++).SetCellValue("勞工居留地址");  //Labor_Address
            row.CreateCell(colindex++).SetCellValue("勞工聯絡地址");  //Labor_Address2
            row.CreateCell(colindex++).SetCellValue("勞工狀態");          //Labor_Valid
            row.CreateCell(colindex++).SetCellValue("預定起始時間");  //Time_01
            row.CreateCell(colindex++).SetCellValue("預定終止時間");  //Time_02
            row.CreateCell(colindex++).SetCellValue("行程起點");          //LocationStart
            row.CreateCell(colindex++).SetCellValue("行程終點");          //LocationEnd
            row.CreateCell(colindex++).SetCellValue("地址");                  //Location
            row.CreateCell(colindex++).SetCellValue("郵遞區號");          //PostCode
            row.CreateCell(colindex++).SetCellValue("聯絡人姓名");
            row.CreateCell(colindex++).SetCellValue("聯絡人手機簡碼");
            row.CreateCell(colindex++).SetCellValue("聯絡人手機號碼");
            row.CreateCell(colindex++).SetCellValue("聯絡人公司電話");
            row.CreateCell(colindex++).SetCellValue("醫療院所");
            row.CreateCell(colindex++).SetCellValue("就醫類型");
            row.CreateCell(colindex++).SetCellValue("狀況說明");          //Question
            row.CreateCell(colindex++).SetCellValue("後續處理");          //Answer
            row.CreateCell(colindex++).SetCellValue("修改人員編號");
            row.CreateCell(colindex++).SetCellValue("修改人員姓名");
            row.CreateCell(colindex++).SetCellValue("修改時間");
            row.CreateCell(colindex++).SetCellValue("填單人員部門");
            row.CreateCell(colindex++).SetCellValue("填單人員編號");
            row.CreateCell(colindex++).SetCellValue("填單人員姓名");
            row.CreateCell(colindex++).SetCellValue("填單時間");          //Create_TIME    
            row.CreateCell(colindex++).SetCellValue("審核人員編號");
            row.CreateCell(colindex++).SetCellValue("審核人員姓名");
            row.CreateCell(colindex++).SetCellValue("審核時間");          //Allow_Time
            row.CreateCell(colindex++).SetCellValue("派工人員編號");
            row.CreateCell(colindex++).SetCellValue("派工人員姓名");
            row.CreateCell(colindex++).SetCellValue("派工時間");          //Dispatch_Time
            row.CreateCell(colindex++).SetCellValue("結案人員編號");
            row.CreateCell(colindex++).SetCellValue("結案人員姓名");
            row.CreateCell(colindex++).SetCellValue("結案時間");          //Close_Time    
            row.CreateCell(colindex++).SetCellValue("取消原因");          //Question2
            row.CreateCell(colindex++).SetCellValue("取消人員編號");
            row.CreateCell(colindex++).SetCellValue("取消人員姓名");
            row.CreateCell(colindex++).SetCellValue("取消時間");          //Cancel_Time    
            #endregion

            int total = 0;
            foreach (var subitem in item)
            {
                total++;
                colindex = 0;
                row = sheet.CreateRow(rowindex++);
                row.CreateCell(colindex++).SetCellValue(total);
                row.CreateCell(colindex++).SetCellValue(RE(subitem.OE_O_ID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.OE_Case_ID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.OE_ID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Unit_Price));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Quantity));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Product_Name));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Main_Classified));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Detail_Classified));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.T_Price));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Ceate_Date));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Delete_Date));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Labor_Country));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Labor_PID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Labor_RID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Labor_EID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Labor_Phone));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Labor_Address));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Labor_Address2));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Labor_Valid));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.Time_01));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.Time_02));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.LocationStart));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.LocationEnd));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Location));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.PostCode));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.ContactName));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.ContactPhone2));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.ContactPhone3));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Contact_Co_TEL));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Hospital));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.HospitalClass));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Question));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Answer));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.UPDATE_ID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.UPDATE_Name));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.UPDATE_TIME));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Create_Team));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Create_ID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Create_Name));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.Create_TIME));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Allow_ID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Allow_Name));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.Allow_Time));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Dispatch_ID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Dispatch_Name));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.Dispatch_Time));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Close_ID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Close_Name));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.Close_Time));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Question2));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Cancel_ID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Cancel_Name));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.Cancel_Time));
                //row.CreateCell(colindex++).SetCellValue(subitem.CREATE_DATE.ToString("yyyy/MM/dd HH:mm"));
            }
            sheet.CreateRow(rowindex++);
            row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("【派工系統】需求單明細表 " + StartDate + " 至 " + EndDate + " 需求單筆數共 " + total + " 筆");
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 53));
        }
    }
    public class SelfCompleteServiceData
    {
        public string Type_Value { get; set; }
        public string SYS_ID { get; set; }
        public string MNo { get; set; }
        public string Type { get; set; }
        public string Service { get; set; }
        public string ServiceName { get; set; }
        public string Agent_Team { get; set; }
        public string Cust_ID { get; set; }
        public string Cust_Name { get; set; }
        public string Labor_Company { get; set; }
        public string Labor_Team { get; set; }
        public string Labor_ID { get; set; }
        public string Labor_EName { get; set; }
        public string Labor_CName { get; set; }
        public string Labor_Country { get; set; }
        public string Labor_PID { get; set; }
        public string Labor_RID { get; set; }
        public string Labor_EID { get; set; }
        public string Labor_Phone { get; set; }
        public string Labor_Address { get; set; }
        public string Labor_Address2 { get; set; }
        public string Labor_Valid { get; set; }
        public string LocationStart { get; set; }
        public string LocationEnd { get; set; }
        public string Location { get; set; }
        public string PostCode { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone2 { get; set; }
        public string ContactPhone3 { get; set; }
        public string Contact_Co_TEL { get; set; }
        public string Hospital { get; set; }
        public string HospitalClass { get; set; }
        public string Question { get; set; }
        public string Question2 { get; set; }
        public string Answer { get; set; }
        public DateTime Time_01 { get; set; }
        public DateTime Time_02 { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime StartTime { get; set; }
        public string UpdateDate { get; set; }
        public string LastUpdateDate { get; set; }
        public string FinalUpdateDate { get; set; }

        public string UPDATE_ID { get; set; }
        public string UPDATE_Name { get; set; }
        public DateTime UPDATE_TIME { get; set; }
        public string Create_Team { get; set; }
        public string Create_ID { get; set; }
        public string Create_Name { get; set; }
        public DateTime Create_TIME { get; set; }
        public string Allow_ID { get; set; }
        public string Allow_Name { get; set; }
        public DateTime Allow_Time { get; set; }
        public string Dispatch_ID { get; set; }
        public string Dispatch_Name { get; set; }
        public DateTime Dispatch_Time { get; set; }
        public string Close_ID { get; set; }
        public string Close_Name { get; set; }
        public DateTime Close_Time { get; set; }
        public string Cancel_ID { get; set; }
        public string Cancel_Name { get; set; }
        public DateTime Cancel_Time { get; set; }

        public string OE_O_ID { get; set; }
        public string OE_Case_ID { get; set; }
        public string OE_ID { get; set; }
        public string Unit_Price { get; set; }
        public string Quantity { get; set; }
        public string Product_Name { get; set; }
        public string Main_Classified { get; set; }
        public string Detail_Classified { get; set; }
        public string T_Price { get; set; }
        public string Ceate_Date { get; set; }
        public string Delete_Date { get; set; }
    }

    private string RE(string TXT)
    {
        try
        {
            TXT = TXT.Trim();
            if (!string.IsNullOrEmpty(TXT))
            {
                return TXT;
            }
            else
            {
                return "";
            }
        }
        catch
        {
            return "";
        }
    }

    private string RE_Time(DateTime TXT)
    {
        try
        {
            string time = TXT.ToString("yyyy/MM/dd hh:mm:ss");
            //return TXT.ToString("yyyy/MM/dd hh:mm:ss");
            if (time == "0001/01/01 12:00:00")
            {
                return " ";
            }
            else
            {
                return TXT.ToString("yyyy/MM/dd hh:mm:ss");
            }
        }
        catch
        {
            return " ";
        }
    }

    private string time_change(string time)
    {
        try
        {
            //string A = "";
            int t = int.Parse(time);
            int mm = t % 60;
            int hh = (t / 60) % 24;
            int dd = (t / 60) / 24;
            //if (t < 0) { A = "提早 "; } else if (dd > 2) { A = "延遲 "; }
            return dd + " 天 " + hh.ToString().PadLeft(2, '0') + " 小時 " + mm.ToString().PadLeft(2, '0') + " 分";
        }
        catch
        {
            return "";
        }
    }

    private string ConverIndexToASCII(int index)
    {
        int AscIIindex = 65;
        char character = (char)(AscIIindex + index);
        return character.ToString();
    }
}