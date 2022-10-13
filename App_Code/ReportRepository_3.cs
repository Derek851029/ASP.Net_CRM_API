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
public class ReportRepository3
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
            string Sqlstr = @"select Agent_Team,CNo,Agent_Name,CREATE_DATE,[Service],[Type],StartTime,UpdateDate,LastUpdateDate,FinalUpdateDate
							  from View_CNo
							  where CREATE_DATE between @StartDate AND @EndDate {0}
							  order by Agent_Team,CREATE_DATE ";

            return string.Format(Sqlstr, string.IsNullOrEmpty(Create_Team) ? string.Empty : " AND Agent_Team=@Create_Team");
        }
    }

    #endregion


    public ReportRepository3(string StartDate, string EndDate, string Create_Team)
    {
        this.StartDate = StartDate;
        this.EndDate = EndDate;
        this.Create_Team = Create_Team;
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
        ISheet sheet = workbook.CreateSheet("派工系統 - 自到點及完成時間 ( 前 20 家 )");
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
            row.CreateCell(colindex++).SetCellValue("子單編號");
            row.CreateCell(colindex++).SetCellValue("被派人員姓名");
            row.CreateCell(colindex++).SetCellValue("派工時間");
            row.CreateCell(colindex++).SetCellValue("服務分類");
            row.CreateCell(colindex++).SetCellValue("派單類型");
            row.CreateCell(colindex++).SetCellValue("排定起始時間");
            row.CreateCell(colindex++).SetCellValue("已到點時間");
            row.CreateCell(colindex++).SetCellValue("已完成時間");
            row.CreateCell(colindex++).SetCellValue("暫結案時間");
            #endregion

            int total = 1;
            foreach (var subitem in item)
            {
                if (total > 20)
                    break;
                colindex = 0;
                row = sheet.CreateRow(rowindex++);

                row.CreateCell(colindex++).SetCellValue(subitem.CNo);
                row.CreateCell(colindex++).SetCellValue(subitem.Agent_Name);
                row.CreateCell(colindex++).SetCellValue(subitem.CREATE_DATE.ToString("yyyy/MM/dd HH:mm"));
                row.CreateCell(colindex++).SetCellValue(subitem.Service);
                row.CreateCell(colindex++).SetCellValue(subitem.Type);
                row.CreateCell(colindex++).SetCellValue(subitem.StartTime.ToString("yyyy/MM/dd HH:mm"));
                row.CreateCell(colindex++).SetCellValue(subitem.UpdateDate);
                row.CreateCell(colindex++).SetCellValue(subitem.LastUpdateDate);
                row.CreateCell(colindex++).SetCellValue(subitem.FinalUpdateDate);
                total++;
            }
            sheet.CreateRow(rowindex++);
        }
    }
    public class SelfCompleteServiceData
    {
        public string Agent_Team { get; set; }

        public string CNo { get; set; }

        public string Agent_Name { get; set; }

        public DateTime CREATE_DATE { get; set; }

        public string Service { get; set; }

        public string Type { get; set; }

        public DateTime StartTime { get; set; }

        public string UpdateDate { get; set; }

        public string LastUpdateDate { get; set; }

        public string FinalUpdateDate { get; set; }

    }

    private string ConverIndexToASCII(int index)
    {
        int AscIIindex = 65;
        char character = (char)(AscIIindex + index);
        return character.ToString();
    }
}