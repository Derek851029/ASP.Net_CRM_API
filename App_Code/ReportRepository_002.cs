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
using NPOI.SS.Converter;
using log4net;
using log4net.Config;

/// <summary>
/// ReportRepository 的摘要描述
/// </summary>
public class ReportRepository_002
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public enum TimeType
    {
        填單日期 = 0,
        預定起始時間 = 1
    }
    #region 屬性
    private string StartDate { get; set; }
    private string EndDate { get; set; }
    private string Create_Team { get; set; }
    private string team
    {
        get
        {
            if (Create_Team == "全部門") return " Create_Team<>'' ";
            else return " Create_Team=@Create_Team ";
        }
    }
    private TimeType type { get; set; }
    private object whereobject
    {
        get
        {
            DateTime date = DateTime.Parse(EndDate);
            return new
            {
                StartDate,
                EndDate,
                Create_Team,
                StartDate1 = date.AddMonths(-6).ToString("yyyy-MM-01"),
                EndDate1 = date.AddMonths(-1).ToString("yyyy-MM-") + DateTime.DaysInMonth(date.Year, date.AddMonths(-1).Month).ToString("00")
            };
        }
    }
    private string time
    {
        get
        {
            string _time = string.Empty;
            switch (type)
            {
                case TimeType.填單日期:
                    _time = "Create_TIME";
                    break;
                case TimeType.預定起始時間:
                    _time = "Time_01";
                    break;
                default:
                    throw new Exception("no TimeType");
            }
            return _time;
        }
    }
    private string sqlstrgetheader
    {
        get
        {
            /*string Sqlstr = @"SELECT Create_Team FROM LaborTemplate WHERE {1} "
                + " GROUP BY Create_Team "
                + " ORDER BY charindex ( Create_Team, ( SELECT TOP 1 Team FROM Team_Rank ) ) "; //*/
            string Sqlstr = @"SELECT Product_Name FROM OE_Order WHERE Delete_Date is not null "
                + " GROUP BY Product_Name ";
            return string.Format(Sqlstr, time, team);
        }
    }

    private string CurMonthSqlStr
    {
        get
        {
            string sqlstr = @"select MonthStr,QMonth,Title,Labor_Country,{0}
                        from (
--當月-6個月
                        	select '雇主服務' as Title,CAST(YEAR({1}) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH({1}) AS VARCHAR(2)), 2) as QMonth,min(MONTH({1})) as MonthStr,Create_Team,'' as Labor_Country,count(*) as Cnt
                        	from LaborTemplate1
                        	where convert(varchar(10), {1} , 120) between @StartDate1 AND @EndDate1 AND Labor_ID=''
                        	group by CAST(YEAR({1}) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH({1}) AS VARCHAR(2)), 2),Create_Team
                            union
                            select '外勞服務' as Title,CAST(YEAR({1}) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH({1}) AS VARCHAR(2)), 2) as QMonth,min(MONTH({1})) as MonthStr,Create_Team,Labor_Country,count(*) as Cnt
                        	from LaborTemplate1
                        	where convert(varchar(10), {1} , 120) between @StartDate1 AND @EndDate1 AND Labor_ID<>'' 
                        	group by CAST(YEAR({1}) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH({1}) AS VARCHAR(2)), 2),Create_Team,Labor_Country
                            union
--當月
                            select '雇主服務' as Title,CAST(YEAR({1}) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH({1}) AS VARCHAR(2)), 2) as QMonth,min(MONTH({1})) as MonthStr,Create_Team,'' as Labor_Country,count(*) as Cnt
                        	from LaborTemplate1
                        	where convert(varchar(10), {1} , 120) between @StartDate AND @EndDate AND Labor_ID=''
                        	group by CAST(YEAR({1}) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH({1}) AS VARCHAR(2)), 2),Create_Team
                            union
                            select '外勞服務' as Title,CAST(YEAR({1}) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH({1}) AS VARCHAR(2)), 2) as QMonth,min(MONTH({1})) as MonthStr,Create_Team,Labor_Country,count(*) as Cnt
                        	from LaborTemplate1
                        	where convert(varchar(10), {1} , 120) between @StartDate AND @EndDate AND Labor_ID<>'' 
                        	group by CAST(YEAR({1}) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH({1}) AS VARCHAR(2)), 2),Create_Team,Labor_Country
                        ) Q
                        Pivot(
                        	SUM(Cnt)
                        	For Create_Team in ({0})
                        )P
                        order by QMonth,Title desc";
            return string.Format(sqlstr, Getheaderstr, time);
        }
    }

    private string CurMonth
    {
        get
        {
            try
            {
                return DateTime.Parse(EndDate).ToString("yyyy-MM");
            }
            catch (Exception)
            {
                return DateTime.Now.ToString("yyyy-MM");
            }
        }
    }

    private List<string> ColHeaderlist = new List<string>();
    protected string Getheaderstr
    {
        get
        {
            if (ColHeaderlist.Count == 0)
            {
                ColHeaderlist = DBTool.Query<string>(sqlstrgetheader, whereobject).ToList();
                if (!ColHeaderlist.Any())
                {
                    return string.Format("[{0}]", string.Join("],[", Create_Team));
                }
            }
            return string.Format("[{0}]", string.Join("],[", ColHeaderlist));
        }
    }

    #endregion
    public ReportRepository_002(string StartDate, string EndDate, string teamName, TimeType type)
    {
        this.StartDate = StartDate;
        this.EndDate = EndDate;
        this.type = type;
        this.Create_Team = teamName;
    }

    public byte[] GetReport()
    {
        var curlist = DBTool.Query(CurMonthSqlStr, whereobject);

        IWorkbook workbook = new XSSFWorkbook();
        SetSheet1(workbook, curlist.ToList());
        using (MemoryStream memorystream = new MemoryStream())
        {
            workbook.Write(memorystream);
            workbook.Close();
            return memorystream.ToArray();
        }
    }

    public string GetView()
    {
        DirectoryInfo info = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp"));
        if (info.Exists)
        {
            info.EnumerateFiles("*.html", SearchOption.TopDirectoryOnly)
              .Where(p => p.CreationTime.CompareTo(DateTime.Now.AddDays(-1)) < 0) //每次產生時刪除前一天 /temp 底下殘留
              .ToList()
              .ForEach(p => p.Delete());
        }
        var curlist = DBTool.Query(CurMonthSqlStr, whereobject);
        IWorkbook workbook = new XSSFWorkbook();
        SetSheet1(workbook, curlist.ToList());
        // Output the HTML file
        string filename = Guid.NewGuid().ToString("D") + ".html";
        string pathstr = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp", filename);
        ExcelTool.ConverHTML(workbook, pathstr);
        workbook.Close();
        return filename;
    }

    private void SetSheet1(IWorkbook workbook, List<dynamic> curlist)
    {
        ISheet sheet = workbook.CreateSheet("派工系統 - 以服務對象統計");
        int colindex = 0;
        int rowindex = 0;
        #region 樣式
        ICellStyle percentage_style = workbook.CreateCellStyle();
        percentage_style.DataFormat = workbook.CreateDataFormat().GetFormat("0%");
        #endregion
        #region 標題列
        IRow HeaderRow = sheet.CreateRow(rowindex);
        HeaderRow.CreateCell(colindex++).SetCellValue("月份");

        HeaderRow.CreateCell(colindex++).SetCellValue("服務對象");
        HeaderRow.CreateCell(colindex++).SetCellValue("");
        sheet.AddMergedRegion(new CellRangeAddress(rowindex, rowindex, 1, 2));//合併服務對象
        ColHeaderlist.ForEach(p => HeaderRow.CreateCell(colindex++).SetCellValue(p));
        logger.Info("Getheaderstr：" + Getheaderstr);
        HeaderRow.CreateCell(colindex++).SetCellValue("小計");
        HeaderRow.CreateCell(colindex++).SetCellValue("合計");
        HeaderRow.CreateCell(colindex++).SetCellValue("百分比");
        #endregion
        #region 服務內容
        var query = from a in curlist
                    group a by new { a.MonthStr, a.QMonth };

        foreach (var item in query)
        {
            rowindex++;
            int RowGroupFirstIndex = rowindex;
            int MonthMergeRowCount = item.Count();
            colindex = 0;
            #region 月份
            IRow row = sheet.CreateRow(RowGroupFirstIndex);
            ICell cell = row.CreateCell(colindex++);
            cell.SetCellValue(item.Key.MonthStr + "月");
            #endregion
            #region 服務內容
            if (item.Key.QMonth == this.CurMonth)
            {
                #region 當月
                foreach (var subitem in item)
                {
                    Printvalue(sheet, rowindex, subitem, subitem.Title, subitem.Labor_Country);
                    rowindex++;
                    row = sheet.CreateRow(rowindex);
                }
                #endregion
            }
            else
            {
                MonthMergeRowCount = 2;
                #region 雇主服務
                var employerservice = item.Where(p => p.Title == "雇主服務").FirstOrDefault();
                if (employerservice != null)
                {
                    Printvalue(sheet, rowindex, employerservice, "雇主服務", "");
                    rowindex++;
                    row = sheet.CreateRow(rowindex);
                }
                #endregion
                #region 外勞服務
                IDictionary<string, object> outservice = new Dictionary<string, object>(ColHeaderlist.Count);

                foreach (var subitem in item.Where(p => p.Title == "外勞服務"))
                {

                    var dictory = ((IDictionary<string, object>)subitem);
                    ColHeaderlist.ForEach(p =>
                    {
                        int value = dictory[p] == null ? 0 : (int)dictory[p];
                        if (outservice.Keys.Any(q => q == p))
                            outservice[p] = (int)outservice[p] + value;
                        else
                            outservice.Add(p, value);
                    });
                }
                Printvalue(sheet, rowindex, outservice, "外勞服務", "");
                rowindex++;
                row = sheet.CreateRow(rowindex);
                #endregion
            }
            #endregion
            #region 小計
            colindex = 1;
            row.CreateCell(colindex++).SetCellValue("合計");
            row.CreateCell(colindex++).SetCellValue("");
            sheet.AddMergedRegion(new CellRangeAddress(rowindex, rowindex, 1, 2));//合併
            ColHeaderlist.ForEach(p =>
            {
                row.CreateCell(colindex++).SetCellFormula(string.Format("SUM({0}{1}:{0}{2})",
                    ConverIndexToASCII(colindex - 1), rowindex - MonthMergeRowCount + 1, rowindex));
            });
            //=SUM(X7: BW7)
            ICell subTotal = row.CreateCell(colindex++);
            subTotal.SetCellFormula(string.Format("SUM({0}{1}:{2}{1})",
                ConverIndexToASCII(2), rowindex + 1, ConverIndexToASCII(colindex - 2)));
            #endregion
            #region 合計
            ICell Total = sheet.GetRow(RowGroupFirstIndex).CreateCell(subTotal.ColumnIndex + 1);
            Total.SetCellFormula(string.Format("{0}{1}", ConverIndexToASCII(subTotal.ColumnIndex), subTotal.RowIndex + 1));
            sheet.AddMergedRegion(new CellRangeAddress(Total.RowIndex, Total.RowIndex + MonthMergeRowCount, Total.ColumnIndex, Total.ColumnIndex));
            #endregion
            #region 百分比
            colindex = Total.ColumnIndex + 1;
            for (int i = 0; i <= MonthMergeRowCount; i++)
            {
                int percentindex = RowGroupFirstIndex + i;
                ICell percentcell = sheet.GetRow(RowGroupFirstIndex + i).CreateCell(colindex);
                percentcell.SetCellFormula(string.Format("IF({2}{3}=0,,{0}{1}/{2}{3})",
                    ConverIndexToASCII(colindex - 2), percentindex + 1,
                    ConverIndexToASCII(Total.ColumnIndex), Total.RowIndex + 1));
                percentcell.CellStyle = percentage_style;
            }
            #endregion
            #region 合併儲存格
            //月份
            sheet.AddMergedRegion(new CellRangeAddress(RowGroupFirstIndex, RowGroupFirstIndex + MonthMergeRowCount, 0, 0));
            #endregion
        }
        #endregion
    }

    private void Printvalue(ISheet sheet, int rowindex, IDictionary<string, object> dictory, string title, string Labor_Country)
    {
        IRow row = sheet.GetRow(rowindex);
        int colindex = 1;
        row.CreateCell(colindex++).SetCellValue(title);
        if (string.IsNullOrEmpty(Labor_Country))
        {
            row.CreateCell(colindex++).SetCellValue("");
            sheet.AddMergedRegion(new CellRangeAddress(rowindex, rowindex, 1, 2));//合併
        }
        else
            row.CreateCell(colindex++).SetCellValue(Labor_Country);
        int cnt = 0;
        ColHeaderlist.ForEach(p =>
        {
            int value = dictory[p] == null ? 0 : (int)dictory[p];
            cnt += value;
            row.CreateCell(colindex++).SetCellValue(value);
        });
        row.CreateCell(colindex++).SetCellValue(cnt);
    }

    private string ConverIndexToASCII(int index)
    {
        int AscIIindex = 65;
        char character = (char)(AscIIindex + index);
        return character.ToString();
    }
}