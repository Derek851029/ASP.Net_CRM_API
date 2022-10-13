using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// CaseList 的摘要描述
/// </summary>
public class Case_List
{
    public int SYSID { get; set; }
    public string Case_Name { get; set; }
    public string Clinet_Name { get; set; }
    public string BUSINESSNAME { get; set; }
    public string Contact { get; set; }
    public string Phone { get; set; }
    public string System_Data { get; set; }
    public string System_Data_ID { get; set; }
    public string Work_List { get; set; }
    public string Total_price { get; set; }
    public string Personnel { get; set; }
    public string Assist_Company { get; set; }
    public string Vendor_SYSID { get; set; }
    public string Project_Content { get; set; }
    public string Status { get; set; }
    public string End_Reason { get; set; }
    public DateTime createDate { get; set; }
    public string editor { get; set; }
    public DateTime updateDate { get; set; }

    /// <summary>
    /// 搜尋案件列表清單
    /// </summary>
    public static List<Case_List> Search(string Start_Date, string End_Date, string Personel)
    {
        string sqlCommand;
        if (Start_Date == "" || End_Date == "")
        {
            sqlCommand = "SELECT a.*, b.BUSINESSNAME, c.SYSID as Vendor_SYSID FROM Case_List a left join BusinessData b on a.Clinet_Name = b.ID left join Vendor_Data c on a.Assist_Company = c.SYSID";
            var data = DBTool.Query<Case_List>(sqlCommand).ToList();
            return data;
        }
        else
        {
            sqlCommand = "SELECT a.*, b.BUSINESSNAME, c.SYSID as Vendor_SYSID FROM Case_List a left join BusinessData b on a.Clinet_Name = b.ID left join Vendor_Data c on a.Assist_Company = c.SYSID " +
                "WHERE Personnel = '"+ Personel + "' AND convert(nvarchar(10), a.createDate, 23) BETWEEN '" + Start_Date + "' and '"+ End_Date + "'";
            var data = DBTool.Query<Case_List>(sqlCommand).ToList();
            return data;
        }
    }
}