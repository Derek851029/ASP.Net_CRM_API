using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// PartnerHeader 的摘要描述
/// </summary>
public class PartnerHeader
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int SYSID { get; set; }
    public int SYS_ID { get; set; }
    /// <summary>
    /// 配合廠商
    /// </summary>
    public string Partner_Company { get; set; }
    /// <summary>
    /// 配合司機
    /// </summary>
    public string Partner_Driver { get; set; }
    /// <summary>
    /// 電話
    /// </summary>
    public string Partner_Phone { get; set; }
    /// <summary>
    /// 建立日期
    /// </summary>
    public DateTime Create_TIME { get; set; }
    /// <summary>
    /// 更新日期
    /// </summary>
    public DateTime? UPDATE_TIME { get; set; }
    public DateTime StartTime { get; set; }
    public string UpdateDate { get; set; }
    public string LastUpdateDate { get; set; }
    public string FinalUpdateDate { get; set; }
    public string UpdateUser { get; set; }
    public string LastUpdateUser { get; set; }
    public string FinalUpdateUser { get; set; }    
    public string Hospital { get; set; }
    public string CarSeat { get; set; }
    public string CarNumber { get; set; }
    public string CarAgent_Name { get; set; }
    public string ClassName { get; set; }
    public string ClassTimeType { get; set; }
    public string Agent_ID { get; set; }
    public string Agent_Name { get; set; }
    public string Agent_Company { get; set; }
    public string Agent_Team { get; set; }
    public string MASTER_Company { get; set; }
    public string MASTER_Team { get; set; }
    public string MASTER_ID { get; set; }
    public string MASTER_Name { get; set; }
    public string MASTER_TEL { get; set; }
    public string MASTER1_ID { get; set; }
    public string MASTER1_TEL { get; set; }
    public string MASTER1_NAME { get; set; }
    public string MASTER2_ID { get; set; }
    public string MASTER2_TEL { get; set; }
    public string MASTER2_NAME { get; set; }
    public string WORK_TimeType { get; set; }
    public string WORK_TimeHour { get; set; }
    public string WORK_TimeHour_2 { get; set; }
    public string WORK_TimeMin { get; set; }
    public string DIAL_TimeHour { get; set; }
    public string DIAL_TimeMin { get; set; }
    public string CNo { get; set; }
    public string Type { get; set; }
    public string Answer { get; set; }
    public string Answer2 { get; set; }
    /// <summary>    
    /// 班次日期(星期一) 0:沒勾;1:打勾    
    /// </summary> 
    public bool ClassWeek_Mon { get; set; }
    /// <summary>    
    /// 班次日期(星期二) 0:沒勾;1:打勾    
    /// </summary> 
    public bool ClassWeek_Tue { get; set; }
    /// <summary>    
    /// 班次日期(星期三) 0:沒勾;1:打勾    
    /// </summary> 
    public bool ClassWeek_Wed { get; set; }
    /// <summary>    
    /// 班次日期(星期四) 0:沒勾;1:打勾    
    /// </summary> 
    public bool ClassWeek_Thu { get; set; }
    /// <summary>    
    /// 班次日期(星期五) 0:沒勾;1:打勾    
    /// </summary> 
    public bool ClassWeek_Fri { get; set; }
    /// <summary>    
    /// 班次日期(星期六) 0:沒勾;1:打勾    
    /// </summary> 
    public bool ClassWeek_Sat { get; set; }
    /// <summary>    
    /// 班次日期(星期日) 0:沒勾;1:打勾    
    /// </summary> 
    public bool ClassWeek_Sun { get; set; }

}