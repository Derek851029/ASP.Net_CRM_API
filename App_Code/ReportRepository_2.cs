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
public class ReportRepository2
{
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

    /// <summary>
    /// SQL
    /// </summary>
    private string QuerySqlStr
    {
        get
        {
            string Sqlstr = @"select A.Cust_Name,A.Cnt as NeedCnt,B.Cnt as AgentCnt,Create_Team 
                              from (
	                              select Cust_Name,count(*) as CNT,Create_Team
	                              from View_MNo
	                              where Time_01 between @StartDate AND @EndDate  {0}
	                              group by Cust_Name,Create_Team
                              )A
                              left join (
	                              select Cust_Name,COUNT(*) as CNT
	                              from View_CNO V
	                              where CREATE_DATE between @StartDate AND @EndDate {1} 
	                              group by Cust_Name
                          
                              )B on(A.Cust_Name=B.Cust_Name)
                              order by Create_Team,A.CNT desc";
            return string.Format(Sqlstr,
                string.IsNullOrEmpty(Create_Team) ? string.Empty : " AND Create_Team=@Create_Team",
                string.IsNullOrEmpty(Create_Team) ? string.Empty : " AND Agent_Team=@Create_Team");
        }
    }

    #endregion


    public ReportRepository2(string StartDate, string EndDate, string Create_Team)
    {
        this.StartDate = StartDate;
        this.EndDate = EndDate;
        this.Create_Team = Create_Team;
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
        sheet.CreateRow(0).CreateCell(0).SetCellValue(StartDate + " 至 " + EndDate);
        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 3));
        rowindex++;

        var datagroup = from a in list
                        group a by a.Create_Team;
        IRow row = null;
        foreach (var item in datagroup)
        {
            colindex = 0;
            row = sheet.CreateRow(rowindex++);
            row.CreateCell(colindex).SetCellValue(item.Key);

            #region 表頭
            colindex = 0;
            row = sheet.CreateRow(rowindex++);
            row.CreateCell(colindex++).SetCellValue("項次");
            row.CreateCell(colindex++).SetCellValue(string.Format("服務廠商{0}家", item.Count()));
            row.CreateCell(colindex++).SetCellValue(string.Format("需求單({0})", item.Sum(p => p.NeedCnt)));
            row.CreateCell(colindex++).SetCellValue(string.Format("需求單({0})", item.Sum(p => p.AgentCnt)));
            #endregion

            int total = 1;
            foreach (var subitem in item)
            {
                if (total > 10)
                    break;
                colindex = 0;
                row = sheet.CreateRow(rowindex++);
                row.CreateCell(colindex++).SetCellValue(total);
                row.CreateCell(colindex++).SetCellValue(subitem.Cust_Name);
                row.CreateCell(colindex++).SetCellValue(subitem.NeedCnt);
                row.CreateCell(colindex++).SetCellValue(subitem.AgentCnt);
                total++;
            }
            sheet.CreateRow(rowindex++);
        }
    }
    public class EmployerServiceData
    {
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