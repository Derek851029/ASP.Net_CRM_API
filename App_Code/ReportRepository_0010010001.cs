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
public class ReportRepository0010010001
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
            string Sqlstr = @"SELECT [SYSID],[Service],[ServiceName],[txt_Title],[Create_Name],[Create_ID],[Create_Time]
                              ,[Update_Name],[Update_ID],[Update_Time],[Click],[txt_Content],[FileName]
							  FROM [EASTERN].[dbo].[View_Forum] ";
            return string.Format(Sqlstr);
            //return string.Format(Sqlstr, string.IsNullOrEmpty(Create_Team) ? string.Empty : " AND Agent_Team=@Create_Team");
        }
    }

    #endregion


    public ReportRepository0010010001(string StartDate, string EndDate, string Create_Team)
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
        ISheet sheet = workbook.CreateSheet("知識庫");
        int colindex = 0;
        int rowindex = 0;
        #region 表頭
        IRow HeaderRow = sheet.CreateRow(rowindex++);
        HeaderRow = sheet.CreateRow(rowindex++);
        HeaderRow.CreateCell(colindex++).SetCellValue("編號");
        HeaderRow.CreateCell(colindex++).SetCellValue("服務");
        HeaderRow.CreateCell(colindex++).SetCellValue("服務分類");
        HeaderRow.CreateCell(colindex++).SetCellValue("文章標題");
        HeaderRow.CreateCell(colindex++).SetCellValue("發佈人員姓名");
        HeaderRow.CreateCell(colindex++).SetCellValue("發佈人員ID");
        HeaderRow.CreateCell(colindex++).SetCellValue("發佈時間");
        HeaderRow.CreateCell(colindex++).SetCellValue("修改人員");
        HeaderRow.CreateCell(colindex++).SetCellValue("修改人員ID");
        HeaderRow.CreateCell(colindex++).SetCellValue("修改時間");
        HeaderRow.CreateCell(colindex++).SetCellValue("點擊率");
        HeaderRow.CreateCell(colindex++).SetCellValue("文章內容");
        HeaderRow.CreateCell(colindex++).SetCellValue("附件檔名");

        #endregion

        var datagroup = from a in list
                        group a by a.SYSID;
        IRow row = null;
        foreach (var item in datagroup)
        {
           // colindex = 0;
           // row = sheet.CreateRow(rowindex++);
           // row.CreateCell(colindex).SetCellValue(item.Key);

            //int total = 1;
            foreach (var subitem in item)
            {
                //if (total > 20)
                  //  break;
                colindex = 0;
                row = sheet.CreateRow(rowindex++);

                row.CreateCell(colindex++).SetCellValue(subitem.SYSID);
                row.CreateCell(colindex++).SetCellValue(subitem.Service);
                row.CreateCell(colindex++).SetCellValue(subitem.ServiceName);
                row.CreateCell(colindex++).SetCellValue(subitem.txt_Title);
                row.CreateCell(colindex++).SetCellValue(subitem.Create_Name);
                row.CreateCell(colindex++).SetCellValue(subitem.Create_ID);
                row.CreateCell(colindex++).SetCellValue(subitem.CREATE_Time.ToString("yyyy/MM/dd HH:mm"));
                row.CreateCell(colindex++).SetCellValue(subitem.Update_Name);
                row.CreateCell(colindex++).SetCellValue(subitem.Update_ID);
                row.CreateCell(colindex++).SetCellValue(subitem.Update_Time.ToString("yyyy/MM/dd HH:mm"));
                row.CreateCell(colindex++).SetCellValue(subitem.Click);
                row.CreateCell(colindex++).SetCellValue(subitem.txt_Content);
                row.CreateCell(colindex++).SetCellValue(subitem.FileName);
                //total++;
            }
            //sheet.CreateRow(rowindex++);
        }
    }
    public class SelfCompleteServiceData
    {
        public string SYSID { get; set; }
        public string Service { get; set; }
        public string ServiceName { get; set; }
        public string txt_Title { get; set; }
        public string Create_Name { get; set; }
        public string Create_ID { get; set; }
        public string Update_Name { get; set; }
        public string Update_ID { get; set; }
        public string Click { get; set; }
        public string txt_Content { get; set; }
        public string FileName { get; set; }
        public DateTime CREATE_Time { get; set; }
        public DateTime Update_Time { get; set; }

    }

    private string ConverIndexToASCII(int index)
    {
        int AscIIindex = 65;
        char character = (char)(AscIIindex + index);
        return character.ToString();
    }
}