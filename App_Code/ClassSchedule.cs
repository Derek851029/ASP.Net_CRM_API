using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ClassSchedule 的摘要描述
/// </summary>
public class ClassSchedule
{
    /// <summary>    
    /// 流水碼    
    /// </summary>
    public int SYS_ID { get; set; }
    public int SYSID { get; set; }
    public string Case_ID { get; set; }
    public string CNo { get; set; }
    /// <summary>
    /// 到班日期
    /// </summary>
    public DateTime WORK_DATE { get; set; }
    /// <summary>
    /// 負責司機
    /// </summary>
    public string DRIVER_ID { get; set; }
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
    /// 負責司機
    /// </summary>
    public string DRIVER_Team { get; set; }
    /// <summary>
    ///  司機公司
    /// </summary>
    public string DRIVER_Company { get; set; }
    /// <summary>
    /// ClassTemplate.SYS_ID
    /// </summary>
    public int Template_SYS_ID { get; set; }

    /// <summary>    
    /// 班次名稱    
    /// </summary>
    public string ClassName { get; set; }
    /// <summary>    
    /// 班次名稱    
    /// </summary>
    public string Class { get; set; }
    /// <summary>    
    /// 班次時間    
    /// </summary>
    public string ClassTimeType { get; set; }
    /// <summary>    
    /// 班次日期時間(幾月幾日幾點幾分)     
    /// </summary>
    public DateTime WORK_TIME { get; set; }
    /// <summary>    
    /// 班次日期時間(幾月幾日幾點幾分)     
    /// </summary>
    public DateTime DIAL_TIME { get; set; }
    /// <summary>    
    /// 班次日期時間(幾月幾日幾點幾分)     
    /// </summary>
    public DateTime WORK_DATETime { get; set; }
    /// <summary>    
    /// 通知日期時間(幾月幾日幾點幾分)     
    /// </summary>
    public DateTime DIAL_DATETime { get; set; }
    /// <summary>    
    /// 負責主管[DispatchSystem].Agent_ID    
    /// </summary>
    public string MASTER1_ID { get; set; }
    /// <summary>    
    /// 負責主管[DispatchSystem].Agent_Name    
    /// </summary>
    public string MASTER1_NAME { get; set; }
    /// <summary>    
    /// 負責主管[DispatchSystem].Agent_Phone    
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
    /// 代理主管[DispatchSystem].Agent_ID    
    /// </summary>
    public string MASTER2_ID { get; set; }
    /// <summary>    
    /// 代理主管[DispatchSystem].Agent_Name    
    /// </summary>
    public string MASTER2_NAME { get; set; }
    /// <summary>    
    /// 代理主管[DispatchSystem].Agent_Phone    
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
    /// 負責所屬公司    
    /// </summary>
    public string MASTER_Company { get; set; }
    /// <summary>    
    /// 負責所屬部門    
    /// </summary>
    public string MASTER_Team { get; set; }
    /// <summary>    
    /// 負責所屬人員ID    
    /// </summary>
    public string MASTER_ID { get; set; }
    /// <summary>    
    /// 負責所屬人員    
    /// </summary>
    public string MASTER_Name { get; set; }
    /// <summary>    
    /// 負責所屬人員    
    /// </summary>
    public string MASTER_TEL { get; set; }
    /// <summary>    
    /// 配合廠商    
    /// </summary>
    public string Partner_Company { get; set; }
    /// <summary>    
    /// 配合司機    
    /// </summary>
    public string Partner_Driver { get; set; }
    /// <summary>    
    /// 配合司機電話    
    /// </summary>
    public string Partner_Phone { get; set; }
    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime Create_TIME { get;set; }
    /// <summary>    
    /// 更新時間    
    /// </summary>
    public DateTime? UPDATE_TIME { get; set; }
    /// <summary>    
    /// 更新人員   
    /// </summary>
    public string UPDATE_Name { get; set; }


    //=========== 權限角色維護 ===========
    /// <summary>    
    /// 角色代碼    
    /// </summary>
    public string ROLE_ID { get; set; }
    /// <summary>    
    /// 角色名稱   
    /// </summary>
    public string ROLE_NAME { get; set; }
    /// <summary>    
    /// 異動者   
    /// </summary>
    public string Agent_Name { get; set; }
    /// <summary>    
    /// 異動日期   
    /// </summary>
    public DateTime? UpDateDate { get; set; }


    //=========== 勞工需求單 ===========
    public string ServiceName { get; set; }
    public string OpinionSubject { get; set; }
    public string Labor_CName { get; set; }
    public string ID { get; set; }
    public string Labor_ID { get; set; }
    public string Agent_ID { get; set; }
    public string Type { get; set; }
    public string ReplyType { get; set; }
    public string Type_Value { get; set; }
    public string Create_ID { get; set; }
    public string Danger { get; set; }
    public string Create_Name { get; set; }
    public string Cust_Name { get; set; }
    public string Urgency { get; set; }
    public string Question { get; set; }
    public string Question2 { get; set; }
    public DateTime Time_01 { get; set; }
    public DateTime Upload_Time { get; set; }
    public DateTime EstimatedFinishTime { get; set; }
    public DateTime Work_Day { get; set; }
    public DateTime OverTime { get; set; }
    public DateTime CREATE_DATE { get; set; }
    //public DateTime str_time { get; set; }
    public string Agent_Mail { get; set; }
    public string UserID { get; set; }
    public DateTime datetime { get; set; }
    //1017 維護單
    public string ADDR { get; set; }
    public string Name { get; set; }
    public string MTEL { get; set; }
    public string Telecomm_ID { get; set; }
    public string Handle_Agent { get; set; }
}

