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
/// 工程師 統計 (服務單)
/// </summary>
public class ReportRepository_012
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
            string Sqlstr = @"select --MonthStr, QMonth, '外勞服務' as Title, 
Team, ISNULL([All], 0)as Sum , ISNULL([Cancel], 0)as Cancel, 
ISNULL([Finish], 0)as Finish, ISNULL([Total_Time], 0)as Total_Time, ISNULL([Processing_Time], 0)as Processing_Time
, ISNULL([Over_Time_1], 0)as Over_Time_1, ISNULL([Over_Time_2], 0)as Over_Time_2
from ( 
select --MonthStr,QMonth,
[Type],Team,SUM(MN) MN FROM(

Select 'All'as Type,Handle_Agent as Team,count(*) as MN--, CAST(YEAR(SetupTime) AS VARCHAR(4)) +'-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) as QMonth,min(MONTH(SetupTime)) as MonthStr 
From CaseData Where convert(varchar(10), SetupTime , 120) between @StartDate AND @EndDate And Handle_Agent<>'' {0} {1} {2} {4} 
Group by Handle_Agent --,CAST(YEAR(SetupTime) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2)
UNION ALL 
Select 'Cancel'as Type,Handle_Agent as Team,count(*) as MN--, CAST(YEAR(SetupTime) AS VARCHAR(4)) +'-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) as QMonth,min(MONTH(SetupTime)) as MonthStr 
From CaseData Where convert(varchar(10), SetupTime , 120) between @StartDate AND @EndDate and Type='取消' And Handle_Agent<>'' {0} {1} {2} {4} 
Group by Handle_Agent --,CAST(YEAR(SetupTime) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2)
UNION ALL 
Select 'Finish'as Type,Handle_Agent as Team,count(*) as MN--, CAST(YEAR(SetupTime) AS VARCHAR(4)) +'-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) as QMonth,min(MONTH(SetupTime)) as MonthStr 
From CaseData Where convert(varchar(10), SetupTime , 120) between @StartDate AND @EndDate and Type in('已結案','已結案已簽核') And Handle_Agent<>'' {0} {1} {2} {4} 
Group by Handle_Agent --,CAST(YEAR(SetupTime) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2)
UNION ALL
Select 'Total_Time'as Type,Handle_Agent as Team,SUM(Datediff(Minute,SetupTime,FinishTime)) as MN
--, CAST(YEAR(SetupTime) AS VARCHAR(4)) +'-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) as QMonth,min(MONTH(SetupTime)) as MonthStr 
From CaseData Where convert(varchar(10), SetupTime , 120) between @StartDate AND @EndDate and Type in('已結案','已結案已簽核') And Handle_Agent<>'' {0} {1} {2} {4} 
Group by Handle_Agent --,CAST(YEAR(SetupTime) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2)
-- SetupTime 建單時間 > FinishTime 完成時間
UNION ALL
Select 'Processing_Time'as Type,Handle_Agent as Team,SUM(Datediff(Minute,ReachTime,FinishTime)) as MN--, CAST(YEAR(SetupTime) AS VARCHAR(4)) +'-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) as QMonth,min(MONTH(SetupTime)) as MonthStr 
From CaseData Where convert(varchar(10), SetupTime , 120) between @StartDate AND @EndDate and Type in('已結案','已結案已簽核') And Handle_Agent<>'' {0} {1} {2} {4} 
Group by Handle_Agent --,CAST(YEAR(SetupTime) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2)
-- ReachTime 到點時間 > FinishTime 完成時間

UNION ALL
Select 'Over_Time_1'as Type,Handle_Agent as Team
,count(*) as MN--, CAST(YEAR(SetupTime) AS VARCHAR(4)) +'-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) as QMonth ,min(MONTH(SetupTime)) as MonthStr 
FROM CaseData a left join BusinessData b on a.PID=b.PID  Where convert(varchar(10), SetupTime , 120) between @StartDate AND @EndDate And Handle_Agent<>'' {0}  {2} {3} {4} 
and Urgency not in('','其他') and a.Type in('未到點','處理中')and
   ( (((Saturday_Work=1 and b.Sunday_Work=1)or DATEPART(WEEKDAY, SetupTime-1)<4 
   or (DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency in('緊急故障','重要故障')) 
   or (DATEPART(WEEKDAY, SetupTime-1)=4 and Saturday_Work=1) 
   or (DATEPART(WEEKDAY, SetupTime-1)=5 and Urgency in('緊急故障','重要故障')and Saturday_Work=1) 
   or (DATEPART(WEEKDAY, SetupTime-1)=7 and Sunday_Work=1) ) and 
  (CONVERT(varchar, SetupTime+1, 23)<CONVERT(varchar, GETDATE(), 23) 
   or (CONVERT(varchar, SetupTime, 23)<CONVERT(varchar, GETDATE(), 23)and Urgency in('緊急故障','重要故障')))) or 
 (((DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency not in('緊急故障','重要故障')and Saturday_Work=0 and Sunday_Work=1) 
   or (DATEPART(WEEKDAY, SetupTime-1)=5 and Saturday_Work=0 and Sunday_Work=1) 
   or (DATEPART(WEEKDAY, SetupTime-1)=5 and Urgency not in('緊急故障','重要故障')and (Saturday_Work=1 and Sunday_Work=0)) 
   or (DATEPART(WEEKDAY, SetupTime-1)=6 and ((Saturday_Work=1 and Sunday_Work=0)or(Saturday_Work=0 and Sunday_Work=1))) 
   or (DATEPART(WEEKDAY, SetupTime-1)=7 and Sunday_Work=0 ))and 
  (CONVERT(varchar, SetupTime+2, 23)<CONVERT(varchar, GETDATE(), 23) 
   or (CONVERT(varchar, SetupTime+1, 23)<CONVERT(varchar, GETDATE(), 23)and Urgency in('緊急故障','重要故障')))) or 
 (((DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency not in('緊急故障','重要故障')and Saturday_Work=0 and Sunday_Work=0) 
   or (DATEPART(WEEKDAY, SetupTime-1)=5 and Saturday_Work=0 and Sunday_Work=0) 
   or (DATEPART(WEEKDAY, SetupTime-1)=6 and Saturday_Work=0 and Sunday_Work=0))and 
  (CONVERT(varchar, SetupTime+3, 23)<CONVERT(varchar, GETDATE()-0, 23) 
   or(CONVERT(varchar, SetupTime+2, 23)<CONVERT(varchar, GETDATE()-0, 23)and Urgency in('緊急故障','重要故障')) 
   )))
   Group by Handle_Agent --,CAST(YEAR(SetupTime) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2)
--逾期未完成

UNION ALL
Select 'Over_Time_2'as Type,Handle_Agent as Team
,count(*) as MN 
FROM CaseData a left join BusinessData b on a.PID=b.PID  
Where convert(varchar(10), SetupTime , 120) between @StartDate AND @EndDate And Handle_Agent<>'' {0}  {2} {3} {4} 
and Urgency not in('','其他') and a.Type in('已結案','已結案已簽核')and
((((Saturday_Work=1 and b.Sunday_Work=1)or DATEPART(WEEKDAY, SetupTime-1)<4 
or (DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency in('緊急故障','重要故障')) 
or (DATEPART(WEEKDAY, SetupTime-1)=4 and Saturday_Work=1) 
or (DATEPART(WEEKDAY, SetupTime-1)=5 and Urgency in('緊急故障','重要故障')and Saturday_Work=1) 
or (DATEPART(WEEKDAY, SetupTime-1)=7 and Sunday_Work=1) ) and 
(CONVERT(varchar, SetupTime+1, 23)<CONVERT(varchar, FinishTime, 23) 
or (CONVERT(varchar, SetupTime, 23)<CONVERT(varchar, FinishTime, 23)and Urgency in('緊急故障','重要故障')))) or 
(((DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency not in('緊急故障','重要故障')and Saturday_Work=0 and Sunday_Work=1) 
or (DATEPART(WEEKDAY, SetupTime-1)=5 and Saturday_Work=0 and Sunday_Work=1) 
or (DATEPART(WEEKDAY, SetupTime-1)=5 and Urgency not in('緊急故障','重要故障')and (Saturday_Work=1 and Sunday_Work=0) ) 
or (DATEPART(WEEKDAY, SetupTime-1)=6 and ((Saturday_Work=1 and Sunday_Work=0)or(Saturday_Work=0 and Sunday_Work=1)) ) 
or (DATEPART(WEEKDAY, SetupTime-1)=7 and Sunday_Work=0 ))and 
(CONVERT(varchar, SetupTime+2, 23)<CONVERT(varchar, FinishTime, 23) 
or (CONVERT(varchar, SetupTime+1, 23)<CONVERT(varchar, FinishTime, 23)and Urgency in('緊急故障','重要故障')) )) or 
(((DATEPART(WEEKDAY, SetupTime-1)=4 and Urgency not in('緊急故障','重要故障')and Saturday_Work=0 and Sunday_Work=0) 
or (DATEPART(WEEKDAY, SetupTime-1)=5 and Saturday_Work=0 and Sunday_Work=0) 
or (DATEPART(WEEKDAY, SetupTime-1)=6 and Saturday_Work=0 and Sunday_Work=0) ) and 
(CONVERT(varchar, SetupTime+3, 23)<CONVERT(varchar, FinishTime, 23) or 
(CONVERT(varchar, SetupTime+2, 23)<CONVERT(varchar, FinishTime, 23)and Urgency in('緊急故障','重要故障') ) 
))) Group by Handle_Agent
--逾期完成

) as T1 
Group by --MonthStr,QMonth,
[Type],Team)Q
Pivot(SUM(MN) For [Type] in ([All],[Cancel],[Finish],[Total_Time],[Processing_Time],[Over_Time_1],[Over_Time_2]))P 
Order by --, QMonth
charindex(Team, ( 'All,Cancel,Finish,Total_Time,Processing_Time,Over_Time_1,Over_Time_2' ) )  ";    //  @StartDate AND @EndDate {0} {1} {2} {3} {4} 

            return string.Format(Sqlstr,
                string.IsNullOrEmpty(Type_Value) ? string.Empty : "", // AND Type = @Type_Value 
                string.IsNullOrEmpty(BusinessName) ? string.Empty : " AND PID=@BusinessName ",
                string.IsNullOrEmpty(Enginner) ? string.Empty : " AND Handle_Agent like @Enginner ",
                string.IsNullOrEmpty(BusinessName) ? string.Empty : " AND a.PID=@BusinessName ",    //
                string.IsNullOrEmpty(O_Content) ? string.Empty : "" //
            );
        }
    }

    #endregion

    public ReportRepository_012(string StartDate, string EndDate, string BusinessName, string Type_Value, string Enginner, string O_Type, string O_Content)
    {
        this.StartDate = StartDate;
        this.EndDate = EndDate;
        this.BusinessName = BusinessName;
        this.Type_Value = Type_Value;
        if (Enginner != "")
            Enginner = "%" + Enginner + "%";
        this.Enginner = Enginner;
        this.O_Type = O_Type;   //維護廠商
        this.O_Content = O_Content; //停用
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
        ISheet sheet = workbook.CreateSheet("【工程師服務單統計表】");
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
            row.CreateCell(colindex++).SetCellValue("工程師");
            row.CreateCell(colindex++).SetCellValue("總案件數");
            row.CreateCell(colindex++).SetCellValue("取消件數");
            row.CreateCell(colindex++).SetCellValue("逾期未完成件數");
            row.CreateCell(colindex++).SetCellValue("完成件數");
            row.CreateCell(colindex++).SetCellValue("逾期完成件數");
            row.CreateCell(colindex++).SetCellValue("總時間(開單>完成)");
            row.CreateCell(colindex++).SetCellValue("處理時間(到點>完成)");      //
            /*
            row.CreateCell(colindex++).SetCellValue("處理狀態");
            row.CreateCell(colindex++).SetCellValue("到點時間");
            row.CreateCell(colindex++).SetCellValue("完成時間");
            row.CreateCell(colindex++).SetCellValue("到點→完成");
            row.CreateCell(colindex++).SetCellValue("備註說明");
            row.CreateCell(colindex++).SetCellValue("客戶意見");     
             * */
            #endregion

            int total = 0;
            foreach (var subitem in item)
            {
                total++;
                colindex = 0;
                row = sheet.CreateRow(rowindex++);
                //row.CreateCell(colindex++).SetCellValue(total);
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Team));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Sum));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Cancel));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Over_Time_1));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Finish));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Over_Time_2));
                row.CreateCell(colindex++).SetCellValue(C_Minute(subitem.Total_Time));
                row.CreateCell(colindex++).SetCellValue(C_Minute(subitem.Processing_Time));
                /*
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Case_ID));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Title_Name));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.OnSpotTime));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Telecomm_ID));
                row.CreateCell(colindex++).SetCellValue(Check_Cycle(subitem.Cycle));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Handle_Agent));
                row.CreateCell(colindex++).SetCellValue(RE(subitem.Type));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.ReachTime));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.FinishTime));
                row.CreateCell(colindex++).SetCellValue(C_Minute(subitem.Min));
                row.CreateCell(colindex++).SetCellValue(Check_T_Kind(subitem.Telecomm_ID, subitem.K_Ans1, subitem.L_Ans1));
                row.CreateCell(colindex++).SetCellValue(Check_T_Kind(subitem.Telecomm_ID, subitem.L_Ans1, subitem.L_Ans2));
                 * 
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.Time_02));
                row.CreateCell(colindex++).SetCellValue(RE_Time(subitem.Cancel_Time));
                row.CreateCell(colindex++).SetCellValue(subitem.CREATE_DATE.ToString("yyyy/MM/dd HH:mm"));    */
            }
            sheet.CreateRow(rowindex++);
            row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("" + StartDate + " 至 " + EndDate + " 服務單 共 " + total + " 位 工程師");
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
        public string Case_ID { get; set; }
        public string BUSINESSNAME { get; set; }
        public string Title_Name { get; set; }
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
        public string Cycle { get; set; }
        public string K_Ans1 { get; set; }
        public string L_Ans1 { get; set; }
        public string L_Ans2 { get; set; }

        public string Team { get; set; }
        public string Sum { get; set; }
        public string Cancel { get; set; }
        public string Finish { get; set; }
        public string Total_Time { get; set; }
        public string Processing_Time { get; set; }
        public string Over_Time_1 { get; set; }
        public string Over_Time_2 { get; set; }
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
            int hh = (t / 60); // % 24
            //int dd = (t / 60) / 24;
            //if (dd > 0)                value = dd + " 天 ";
            if (hh > 0)
                value += hh+ " 小時 ";
            //return value + mm.ToString().PadLeft(2, '0') + " 分鐘";
            return value + mm+ " 分鐘";
        }
        else
            return "";
    }
    private string Check_Cycle(string Cycle)
    {
        if (Cycle == "0")
            return "單月";
        else if (Cycle == "1")
            return "雙月";
        else if (Cycle == "2")
            return "每季";
        else if (Cycle == "3")
            return "半年";
        else if (Cycle == "4")
            return "每年";
        else
            return "不維護";
    }
    private string Check_T_Kind(string value, string value2, string value3)
    {
        if (value == "德瑪" || value == "遠傳")
            return value2;
        else if (value == "中華電信")
            return value3;
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