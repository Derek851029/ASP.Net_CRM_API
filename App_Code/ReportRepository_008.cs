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
public class ReportRepository_008
{
    #region 屬性
    private string StartDate { get; set; }
    private string EndDate { get; set; }
    private string BusinessName { get; set; }
    private string Type_Value { get; set; }
    private string Enginner { get; set; }
    private string O_Type { get; set; }
    private string O_Content { get; set; }
    private object whereobject
    {
        get
        {
            //if (string.IsNullOrEmpty(BusinessName))
            //    return new { StartDate, EndDate, Type_Value, Enginner, O_Type };
            //else
            return new { StartDate, EndDate, BusinessName, Type_Value, Enginner, O_Type, O_Content };
        }
    }

    /// <summary>
    /// SQL
    /// </summary>
    private string QuerySqlStr
    {
        get
        {
            string Sqlstr = @"select C_ID2,a.Case_ID,BUSINESSNAME,ID,Telecomm_ID,HardWare,SoftwareLoad,SERVICEITEM,Urgency,sum(Quantity)as sum " +
                " ,OnSpotTime,OpinionType,ReplyType,OpinionContent,Type,Handle_Agent,ReachTime,FinishTime,PS, Count(Product_Name)as Pro " +
                " ,Datediff(Minute,ReachTime,FinishTime)as Min " +
                " From CaseData a left join Case_Order b on a.Case_ID=b.Case_ID " +
                " where SetupTime between @StartDate AND @EndDate and Delete_Date is null {0} {1} {2} {3} {4} " +   //{0} {1} {2} {3} 
                " group by C_ID2,a.Case_ID,BUSINESSNAME,ID,Telecomm_ID,HardWare,SoftwareLoad,SERVICEITEM,Urgency " +
                " ,OpinionType,ReplyType,OpinionContent,Type,Handle_Agent,ReachTime,FinishTime,PS,OnSpotTime,Datediff(Minute,ReachTime,FinishTime) " +
                " order by C_ID2 ";

            return string.Format(Sqlstr,
                string.IsNullOrEmpty(Type_Value) ? string.Empty : " AND Type = @Type_Value ",
                string.IsNullOrEmpty(Enginner) ? string.Empty : " AND Handle_Agent like @Enginner ",
                string.IsNullOrEmpty(O_Type) ? string.Empty : " AND OpinionType = @O_Type ",
                string.IsNullOrEmpty(O_Content) ? string.Empty : " AND OpinionContent like @O_Content ",
                string.IsNullOrEmpty(BusinessName) ? string.Empty : " AND PID=@BusinessName "
            );
        }
    }

    #endregion

    public ReportRepository_008(string StartDate, string EndDate, string BusinessName, string Type_Value, string Enginner, string O_Type, string O_Content)
    {
        this.StartDate = StartDate;
        this.EndDate = EndDate;
        this.BusinessName = BusinessName;
        this.Type_Value = Type_Value;
        if (Enginner != "")
            Enginner = "%" + Enginner + "%";
        this.Enginner = Enginner;
        this.O_Type = O_Type;
        this.O_Content = O_Content;
    }
    public string GetView()
    {
        //var list = DBTool.Query<SelfCompleteServiceData>(QuerySqlStr, whereobject);
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
        ISheet sheet = workbook.CreateSheet("【服務單明細表】");
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
            //row.CreateCell(colindex++).SetCellValue("序號");
            row.CreateCell(colindex++).SetCellValue("案件編號");
            row.CreateCell(colindex++).SetCellValue("客戶名稱");
            //row.CreateCell(colindex++).SetCellValue("統編");
            //row.CreateCell(colindex++).SetCellValue("負責電信商");
            //row.CreateCell(colindex++).SetCellValue("硬體");
            //row.CreateCell(colindex++).SetCellValue("軟體");
            //row.CreateCell(colindex++).SetCellValue("合約狀態");
            row.CreateCell(colindex++).SetCellValue("緊急程度");
            row.CreateCell(colindex++).SetCellValue("預計到點");
            row.CreateCell(colindex++).SetCellValue("意見類型");
            row.CreateCell(colindex++).SetCellValue("意見內容");
            //row.CreateCell(colindex++).SetCellValue("回覆方式");
            row.CreateCell(colindex++).SetCellValue("處理狀態");
            row.CreateCell(colindex++).SetCellValue("工程師"); 
            row.CreateCell(colindex++).SetCellValue("到點時間");
            row.CreateCell(colindex++).SetCellValue("完成時間");
            row.CreateCell(colindex++).SetCellValue("到點→完成");
            row.CreateCell(colindex++).SetCellValue("完成備註");
            row.CreateCell(colindex++).SetCellValue("更換零件種類");
            row.CreateCell(colindex++).SetCellValue("更換零件數量");
            /*row.CreateCell(colindex++).SetCellValue("勞工狀態");          //Labor_Valid
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
            row.CreateCell(colindex++).SetCellValue("取消時間");          //Cancel_Time    */
            #endregion

            int total = 0;
            foreach (var subitem in item)
            {
                total++;
                colindex = 0;
                row = sheet.CreateRow(rowindex++);
                //row.CreateCell(colindex++).SetCellValue(total);
                row.CreateCell(colindex++).SetCellValue(RE(subitem.C_ID2));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.BUSINESSNAME));
                //row.CreateCell(colindex++).SetCellValue(RE(subitem.ID));
                //row.CreateCell(colindex++).SetCellValue(RE(subitem.Telecomm_ID));
                //row.CreateCell(colindex++).SetCellValue(RE(subitem.HardWare));
                //row.CreateCell(colindex++).SetCellValue(RE(subitem.SoftwareLoad));
                //row.CreateCell(colindex++).SetCellValue(RE(subitem.SERVICEITEM));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Urgency));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.OnSpotTime));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.OpinionType));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.OpinionContent));
                //row.CreateCell(colindex++).SetCellValue(RE(subitem.ReplyType));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Type));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Handle_Agent));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.ReachTime));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.FinishTime));
                row.CreateCell(colindex++).SetCellValue(C_Minute(subitem.Min));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.PS));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Pro));
                row.CreateCell(colindex++).SetCellValue(subitem.sum + " 件");
                /*row.CreateCell(colindex++).SetCellValue(RE(subitem.Labor_Phone));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Labor_Address));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Labor_Address2));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Labor_Valid));
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
                //row.CreateCell(colindex++).SetCellValue(subitem.CREATE_DATE.ToString("yyyy/MM/dd HH:mm"));    */
            }
            sheet.CreateRow(rowindex++);
            row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("" + StartDate + " 至 " + EndDate + "  服務單 共 " + total + " 筆");
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 20));
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
        public DateTime Day { get; set; }
        public string Agent_Company { get; set; }
        public string Agent_Name { get; set; }
        public string Agent_ID { get; set; }
        public string Schedule { get; set; }
        public string Job_Agent { get; set; }
        public string Work { get; set; }
        public DateTime To_W_Time { get; set; }
        public DateTime Off_W_Time { get; set; }
        
        public string C_ID2 { get; set; }
        public string BUSINESSNAME { get; set; }
        public string ID { get; set; }
        public string Telecomm_ID { get; set; }
        public string HardWare { get; set; }
        public string SoftwareLoad { get; set; }
        public string SERVICEITEM { get; set; }
        public string ReplyType { get; set; }
        public string OpinionType { get; set; }
        public string OpinionContent { get; set; }
        public string Handle_Agent { get; set; }
        public string Urgency { get; set; }
        public string PS { get; set; }
        public string Pro { get; set; }
        public string Min { get; set; }
        public DateTime OnSpotTime { get; set; }
        public DateTime ReachTime { get; set; }
        public DateTime FinishTime { get; set; }
        public int sum { get; set; }
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
            string time = TXT.ToString("yyyy/MM/dd HH:mm");
            //return TXT.ToString("yyyy/MM/dd hh:mm:ss");
            if (time == "0001/01/01 00:00")
            {
                return " ";
            }
            else
            {
                return TXT.ToString("yyyy/MM/dd HH:mm");
            }
        }
        catch
        {
            return " ";
        }
    }

    private string RE_Day(DateTime TXT)
    {
        try
        {
            string time = TXT.ToString("yyyy/MM/dd");
            //return TXT.ToString("yyyy/MM/dd hh:mm:ss");
            if (time == "0001/01/01")
            {
                return " ";
            }
            else
            {
                return TXT.ToString("yyyy/MM/dd");
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
    private string C_Minute(string time)
    {
        string value = "";
        if (!string.IsNullOrEmpty(time))
        {
            int t = int.Parse(time);
            int mm = t % 60;
            int hh = (t / 60) % 24;
            int dd = (t / 60) / 24;
            if (dd > 0)
                value = dd + " 天 ";
            if (hh > 0)
                value += hh.ToString().PadLeft(2, '0') + " 小時 ";
            return value + mm.ToString().PadLeft(2, '0') + " 分鐘";
        }
        else
            return "";
    }

    private string ConverIndexToASCII(int index)
    {
        int AscIIindex = 65;
        char character = (char)(AscIIindex + index);
        return character.ToString();
    }
}