using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// WorkLogs 的摘要描述
/// </summary>
public class WorkLogs
{
    public string SYSID { get; set; }
    public string caseListID { get; set; }
    public string caseListName { get; set; }
    public string Work_Log { get; set; }
    public DateTime Create_Time { get; set; }
    public string Create_ID { get; set; }
    public string Create_Agent { get; set; }
    public DateTime Update_Time { get; set; }
    public string Update_ID { get; set; }
    public string Update_Agent { get; set; }

    /// <summary>
    /// 搜尋：Work_Log相關資料
    /// </summary>
    /// <param name="searchType"></param>
    /// <returns></returns>
    public static List<WorkLogs> Search(string searchType ="ALL")
    {
        string sqlCommand = "";
        switch (searchType)
        {
            default:
                sqlCommand = "Select *,ISNULL(dbo.fnInput_CaseListID_Get_CaseListName(CaseListID),'') caseListName FROM Work_Log";
                break;
        }
        return DBTool.Query<WorkLogs>(sqlCommand).ToList();
    }
}