using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ReportClass 的摘要描述
/// </summary>
public class Rep_Classes
{
    /// <summary>
    /// 編號 PK
    /// </summary>
    public int SYS_ID { get; set; }
    /// <summary>
    /// 到班日期
    /// </summary>
    public DateTime? WORK_DATE { get; set; }
    /// <summary>
    /// 班次
    /// </summary>
    public string Class { get; set; }
    /// <summary>
    /// 到班提醒時間：為到班時間(WORK_TIME)-15分鐘
    /// </summary>
    public DateTime? DIAL_TIME { get; set; }
    /// <summary>
    /// 到班時間
    /// </summary>
    public DateTime? WORK_TIME { get; set; }
    /// <summary>
    /// 負責司機
    /// </summary>
    public string DRIVER_NAME { get; set; }
    /// <summary>
    /// 人員電話
    /// </summary>
    public string DRIVER_TEL { get; set; }
    /// <summary>
    /// 司機狀態
    /// 預設值為【等待外撥】到班﹑無法到班﹑未接
    /// </summary>
    public string DRIVER_STATE { get; set; }
    /// <summary>
    ///  外撥次數
    /// </summary>
    public string DRIVER_DIAL_TIME { get; set; }
    /// <summary>
    /// 負責主管
    /// </summary>
    public string MASTER1_NAME { get; set; }
    /// <summary>
    ///  主管電話1
    /// </summary>
    public string MASTER1_TEL { get; set; }
    /// <summary>
    /// 主管狀態1
    /// 預設值為【等待外撥】未接
    /// </summary>
    public string MASTER1_STATE { get; set; }
    /// <summary>
    /// 主管外撥次數
    /// </summary>
    public string MASTER1_DIAL_TIME { get; set; }
    /// <summary>
    /// 負責主管2
    /// </summary>
    public string MASTER2_NAME { get; set; }
    /// <summary>
    /// 主管電話2 
    /// </summary>
    public string MASTER2_TEL { get; set; }
    /// <summary>
    /// 主管狀態2 
    /// 預設值為【等待外撥】未接
    /// </summary>
    public string MASTER2_STATE { get; set; }
    /// <summary>
    /// 主管2外撥次數
    /// </summary>
    public string MASTER2_DIAL_TIME { get; set; }
    /// <summary>
    /// 更新日期時間
    /// </summary>
    public DateTime? UPDATE_TIME { get; set; }

}



