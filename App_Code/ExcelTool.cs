using NPOI.SS.Converter;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// ExcelTool 的摘要描述
/// </summary>
public class ExcelTool
{
    public static void ConverHTML(IWorkbook workbook, string OutFilePath)
    {
        XSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
        ExcelToHtmlConverter excelToHtmlConverter = new ExcelToHtmlConverter()
        {
            OutputColumnHeaders = false,
            OutputHiddenColumns = false,
            OutputHiddenRows = false,
            OutputLeadingSpacesAsNonBreaking = false,
            OutputRowNumbers = false,
            UseDivsToSpan = false
        };
        excelToHtmlConverter.ProcessWorkbook(workbook);
        excelToHtmlConverter.Document.Save(OutFilePath);
    }
}