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
using log4net;
using log4net.Config;

/// <summary>
/// 派工系統 - 以雇主服務統計 ( 前 10 家 )
/// </summary>
public class ReportRepository_006
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #region 屬性

    private string StartDate { get; set; }

    private string EndDate { get; set; }

    private string Create_Team { get; set; }
    private object whereobject
    {
        get
        {
            if (string.IsNullOrEmpty(Create_Team))
                return new { StartDate, EndDate };
            else
                return new { StartDate, EndDate, Create_Team };
        }
    }

    private string QuerySqlStr
    {
        get
        {
            string Sqlstr = @"select Agent_Team,Agent_Name, ISNULL( [派工單總數] , 0 ) as A, ISNULL( [暫結案] , 0 ) as B, ISNULL( [已分派] , 0 ) as C, ISNULL( [處理中] , 0 ) as D, ISNULL( [已到點] , 0 ) as E, ISNULL( [已完成] , 0 ) as F, ISNULL( [已填寫] , 0 ) as G from (
select Agent_Name, '派工單總數' as Type, Agent_Team , COUNT(*) as MN 
from [Faremma].[dbo].CASEDetail 
where convert(varchar(10), CREATE_DATE , 120)  between @StartDate AND @EndDate AND Agent_Team<>''  {1}
group by Agent_Name,Agent_Team
union
select Agent_Name, Type, Agent_Team , COUNT(*) as MN 
from [Faremma].[dbo].CASEDetail 
where convert(varchar(10), CREATE_DATE , 120)  between @StartDate AND @EndDate AND Agent_Team<>''  {1}
group by Agent_Name,Agent_Team,Type
) as T 
Pivot(SUM(MN) For [Type] in ([派工單總數],[暫結案],[已分派],[處理中],[已到點],[已完成],[已填寫]))P 
order by charindex ( Agent_Team, ( SELECT TOP 1 Team FROM [Faremma].[dbo].Team_Rank ) ), A DESC, B DESC, Agent_Name ";
            return string.Format(Sqlstr,
                string.IsNullOrEmpty(Create_Team) ? string.Empty : " AND Create_Team=@Create_Team",
                string.IsNullOrEmpty(Create_Team) ? string.Empty : " AND Agent_Team=@Create_Team");
        }
    }

    #endregion

    public ReportRepository_006(string StartDate, string EndDate, string Create_Team)
    {
        this.StartDate = StartDate;
        this.EndDate = EndDate;
        if (Create_Team == "全部門")
            Create_Team = "";
        this.Create_Team = Create_Team;
    }
    public string GetView()
    {
        var list = DBTool.Query<EmployerServiceData>(QuerySqlStr, whereobject);
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
        var list = DBTool.Query<EmployerServiceData>(QuerySqlStr, whereobject);

        IWorkbook workbook = new XSSFWorkbook();

        //SetSheet1(sheet, ToDataTable(croslist), ToDataTable(curlist));
        SetSheet(workbook, list.ToList());
        using (MemoryStream memorystream = new MemoryStream())
        {
            workbook.Write(memorystream);
            workbook.Close();
            return memorystream.ToArray();
        }
    }

    private void SetSheet(IWorkbook workbook, List<EmployerServiceData> list)
    {
        ISheet sheet = workbook.CreateSheet("派工系統 - 以雇主服務統計 ( 前 10 家 )");
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
            sheet.AddMergedRegion(new CellRangeAddress(rowindex-1, rowindex-1, 0, 7));
            #region 表頭
            colindex = 0;
            row = sheet.CreateRow(rowindex++);
            row.CreateCell(0).SetCellValue("被派人員");
            sheet.AddMergedRegion(new CellRangeAddress(rowindex - 1, rowindex, 0, 0));
            row.CreateCell(1).SetCellValue("派工單總數");
            sheet.AddMergedRegion(new CellRangeAddress(rowindex - 1, rowindex, 1, 1));
            row.CreateCell(2).SetCellValue("暫結案");
            sheet.AddMergedRegion(new CellRangeAddress(rowindex - 1, rowindex, 2, 2));
            row.CreateCell(3).SetCellValue("派工單處理中");
            sheet.AddMergedRegion(new CellRangeAddress(rowindex - 1, rowindex - 1, 3, 7));
            row = sheet.CreateRow(rowindex++);
            row.CreateCell(colindex++).SetCellValue("");
            row.CreateCell(colindex++).SetCellValue("");
            row.CreateCell(colindex++).SetCellValue("");
            row.CreateCell(colindex++).SetCellValue("已分派");
            row.CreateCell(colindex++).SetCellValue("處理中");
            row.CreateCell(colindex++).SetCellValue("已到點");
            row.CreateCell(colindex++).SetCellValue("已完成");
            row.CreateCell(colindex++).SetCellValue("已填寫");
            #endregion

            foreach (var subitem in item)
            {
                colindex = 0;
                row = sheet.CreateRow(rowindex++);
                row.CreateCell(colindex++).SetCellValue(subitem.Agent_Name);
                row.CreateCell(colindex++).SetCellValue(subitem.A);
                row.CreateCell(colindex++).SetCellValue(subitem.B);
                row.CreateCell(colindex++).SetCellValue(subitem.C);
                row.CreateCell(colindex++).SetCellValue(subitem.D);
                row.CreateCell(colindex++).SetCellValue(subitem.E);
                row.CreateCell(colindex++).SetCellValue(subitem.F);
                row.CreateCell(colindex++).SetCellValue(subitem.G);
            }
            sheet.CreateRow(rowindex++);
        }
    }
    public class EmployerServiceData
    {
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public string F { get; set; }
        public string G { get; set; }
        public string Agent_Name { get; set; }
        public string Agent_Team { get; set; }
        public string Cust_Name { get; set; }
        public string Create_Team { get; set; }
        public int NeedCnt { get; set; }
        public int AgentCnt { get; set; }
    }

    private string ConverIndexToASCII(int index)
    {
        int AscIIindex = 65;
        char character = (char)(AscIIindex + index);
        return character.ToString();
    }
}