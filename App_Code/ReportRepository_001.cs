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
public class ReportRepository_001
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
            //if (Create_Team == "全部門") return "<>'' ";
            //else return "=@Create_Team ";
            if (Create_Team == "全部門") return "  ";
            else return "  ";
        }
    }
    private string T_Name
    {
        get
        {
            if (Create_Team == "全部門") return "全部門";
            else return Create_Team;
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
            string Sqlstr = @"Declare @Array table(Value nvarchar(20))
insert into @Array (Value) values ('總案件數')
insert into @Array (Value) values ('取消件數')
insert into @Array (Value) values ('完成件數')
insert into @Array (Value) values ('總時間(分鐘)')
insert into @Array (Value) values ('處理時間(分鐘)')
insert into @Array (Value) values ('計算用')
select * From @Array";
            /*
insert into @Array (Value) values ('退單')
insert into @Array (Value) values ('取消')
insert into @Array (Value) values ('尚未派工')
insert into @Array (Value) values ('非當月派工')
insert into @Array (Value) values ('尚未結案')
insert into @Array (Value) values ('已經結案')
insert into @Array (Value) values ('需求單合計')
insert into @Array (Value) values ('已分派')
insert into @Array (Value) values ('處理中')
insert into @Array (Value) values ('已到點')
insert into @Array (Value) values ('已完成')
insert into @Array (Value) values ('已填寫')
insert into @Array (Value) values ('暫結案')
insert into @Array (Value) values ('派工單合計')
             */
            return string.Format(Sqlstr);
        }
    }

    private string CurMonthSqlStr
    {
        get
        {
            string sqlstr = @"select MonthStr, QMonth, '外勞服務' as Title, Team, {0} 
                from ( 
select MonthStr,QMonth,[Type],Team,SUM(MN) MN FROM(
Select '總案件數'as Type,Handle_Agent as Team,count(*) as MN, CAST(YEAR(SetupTime) AS VARCHAR(4)) +
'-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) as QMonth,min(MONTH(SetupTime)) as MonthStr 
From CaseData Where convert(varchar(10), SetupTime , 120) between @StartDate AND @EndDate 
Group by CAST(YEAR(SetupTime) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) ,Handle_Agent
UNION ALL 
Select '取消件數'as Type,Handle_Agent as Team,count(*) as MN, CAST(YEAR(SetupTime) AS VARCHAR(4)) +
'-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) as QMonth,min(MONTH(SetupTime)) as MonthStr 
From CaseData Where convert(varchar(10), SetupTime , 120) between '2018-11-01' AND '2018-11-30' and Type='取消'
Group by CAST(YEAR(SetupTime) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) ,Handle_Agent
UNION ALL 
Select '完成件數'as Type,Handle_Agent as Team,count(*) as MN, CAST(YEAR(SetupTime) AS VARCHAR(4)) +
'-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) as QMonth,min(MONTH(SetupTime)) as MonthStr 
From CaseData Where convert(varchar(10), SetupTime , 120) between '2018-11-01' AND '2018-11-30' and Type in('已結案','已結案已簽核')
Group by CAST(YEAR(SetupTime) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) ,Handle_Agent
UNION ALL
Select '總時間(分鐘)'as Type,Handle_Agent as Team,
SUM(Datediff(Minute,SetupTime,FinishTime)) as MN, CAST(YEAR(SetupTime) AS VARCHAR(4)) +
'-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) as QMonth,min(MONTH(SetupTime)) as MonthStr 
From CaseData Where convert(varchar(10), SetupTime , 120) between '2018-11-01' AND '2018-11-30' and Type in('已結案','已結案已簽核')
Group by CAST(YEAR(SetupTime) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) ,Handle_Agent
-- SetupTime 建單時間 > FinishTime 完成時間
UNION ALL
Select '處理時間(分鐘)'as Type,Handle_Agent as Team,
SUM(Datediff(Minute,ReachTime,FinishTime)) as MN, CAST(YEAR(SetupTime) AS VARCHAR(4)) +
'-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) as QMonth,min(MONTH(SetupTime)) as MonthStr 
From CaseData Where convert(varchar(10), SetupTime , 120) between '2018-11-01' AND '2018-11-30' and Type in('已結案','已結案已簽核')
Group by CAST(YEAR(SetupTime) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(SetupTime) AS VARCHAR(2)), 2) ,Handle_Agent
-- ReachTime 到點時間 > FinishTime 完成時間

) as T1 
Group by MonthStr,QMonth,[Type],Team)Q
Pivot(SUM(MN) For [Type] in ({0}))P 
Order by QMonth, charindex(Team, ( '總案件數,取消件數,完成件數,總時間(分鐘),處理時間(分鐘),計算用' ) ) ";
            return string.Format(sqlstr, Getheaderstr, team);
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
                    return string.Format("[{0}]", string.Join("],[", ""));
                }
            }
            return string.Format("[{0}]", string.Join("],[", ColHeaderlist));
        }
    }

    #endregion
    public ReportRepository_001(string StartDate, string EndDate, string teamName)
    {
        this.StartDate = StartDate;
        this.EndDate = EndDate;
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
        ISheet sheet = workbook.CreateSheet("【服務單】工程師統計報表");
        int colindex = 0;
        int rowindex = 0;
        #region 樣式
        ICellStyle percentage_style = workbook.CreateCellStyle();
        percentage_style.DataFormat = workbook.CreateDataFormat().GetFormat("0%");
        #endregion
        #region 標題列
        IRow HeaderRow = sheet.CreateRow(rowindex);
        HeaderRow.CreateCell(0).SetCellValue("月份");
        HeaderRow.CreateCell(1).SetCellValue("部門");
        HeaderRow.CreateCell(2).SetCellValue("服務單（填單時間）");
        HeaderRow.CreateCell(7).SetCellValue(" ");
        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 2, 8));
        HeaderRow.CreateCell(9).SetCellValue("派工單（派工時間）");
        HeaderRow.CreateCell(15).SetCellValue(" ");
        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 9, 15));
        rowindex++;
        HeaderRow = sheet.CreateRow(rowindex);
        HeaderRow.CreateCell(colindex++).SetCellValue("月份");
        sheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));
        HeaderRow.CreateCell(colindex++).SetCellValue("部門");
        sheet.AddMergedRegion(new CellRangeAddress(0, 1, 1, 1));
        ColHeaderlist.ForEach(p => HeaderRow.CreateCell(colindex++).SetCellValue(p.ToString()));
        #endregion
        //=======================================================
        #region 服務內容
        var query = from a in curlist
                    group a by new { a.MonthStr, a.QMonth };
        rowindex++;
        foreach (var item in query)
        {
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
            #region 當月
            {
                foreach (var subitem in item)
                {
                    Printvalue(sheet, rowindex, subitem, subitem.Team);
                    rowindex++;
                    row = sheet.CreateRow(rowindex);
                }
                #region 小計
                colindex = 1;
                row.CreateCell(colindex++).SetCellValue("月合計");
                ColHeaderlist.ForEach(p =>
                {
                    row.CreateCell(colindex++).SetCellFormula(string.Format("SUM({0}{1}:{0}{2})",
                        ConverIndexToASCII(colindex - 1), rowindex - MonthMergeRowCount + 1, rowindex));
                });
                #endregion
                #region 合併儲存格
                //月份
                sheet.AddMergedRegion(new CellRangeAddress(RowGroupFirstIndex, RowGroupFirstIndex + MonthMergeRowCount, 0, 0));
                #endregion
            }
            #endregion
            else
            {
                foreach (var subitem in item)
                {
                    Printvalue(sheet, rowindex, subitem, subitem.Team);
                    rowindex++;
                    row = sheet.CreateRow(rowindex);
                }
            }
            #endregion

        }
        #endregion
    }

    private void Printvalue(ISheet sheet, int rowindex, IDictionary<string, object> dictory, string Team)
    {
        IRow row = sheet.GetRow(rowindex);
        int colindex = 1;
        row.CreateCell(colindex++).SetCellValue(Team);

        int cnt = 0;
        ColHeaderlist.ForEach(p =>
        {
            int value = dictory[p] == null ? 0 : (int)dictory[p];
            cnt += value;
            if (colindex == 7)
            {
                row.CreateCell(colindex++).SetCellValue(cnt);
                cnt = 0;
            }
            else if (colindex == 15)
            {
                row.CreateCell(colindex++).SetCellValue(cnt);
                cnt = 0;
            }
            else
            {
                row.CreateCell(colindex++).SetCellValue(value);
            }
        });
    }

    private string ConverIndexToASCII(int index)
    {
        int AscIIindex = 65;
        char character = (char)(AscIIindex + index);
        return character.ToString();
    }
}