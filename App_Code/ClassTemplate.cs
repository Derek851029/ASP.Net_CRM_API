using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ClassTemplate 的摘要描述
/// </summary>
public class ClassTemplate
{
    /// <summary>    
    /// 流水號
    /// </summary> 
    /// 
    public string Work_List { get; set; }
    public string Telecomm_Name { get; set; }
    public string Telecomm_ID { get; set; }
    public string ImagePath { get; set; }
    public string ImageURL { get; set; }
    public int SYS_ID { get; set; }
    public string ID { get; set; }
    public string PID { get; set; }
    public string APPNAME { get; set; }
    public string APP_OTEL_AREA { get; set; }
    public string APP_OTEL { get; set; }
    public string APP_MTEL { get; set; }
    public string HardWare { get; set; }
    public string SoftwareLoad { get; set; }
    public string SERVICEITEM { get; set; }
    public string SYSID { get; set; }
    /// <summary>    
    /// 班次名稱    
    /// </summary> 
    public string ClassName { get; set; }
    /// <summary>    
    /// 班次時間(上午/下午)    
    /// </summary> 
    public string ClassTimeType { get; set; }
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
    /// <summary>    
    /// 到班時間(上午/下午)    
    /// </summary> 
    public string WORK_TimeType { get; set; }
    /// <summary>    
    /// 到班時間(時)    
    /// </summary> 
    public string WORK_TimeHour { get; set; }
    /// <summary>    
    /// 到班時間(分)    
    /// </summary> 
    public string WORK_TimeMin { get; set; }
    /// <summary>    
    /// 提早通知時間(時)    
    /// </summary> 
    public string DIAL_TimeHour { get; set; }
    /// <summary>    
    /// 提早通知時間(分)    
    /// </summary> 
    public string DIAL_TimeMin { get; set; }
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
    /// 負責所屬公司    
    /// </summary> 
    public string MASTER_Company { get; set; }
    /// <summary>    
    /// 負責所屬部門    
    /// </summary> 
    public string MASTER_Team { get; set; }
    /// <summary>    
    /// 負責所屬人員ID(用於取得    
    /// </summary> 
    public string MASTER_ID { get; set; }
    /// <summary>    
    /// 負責所屬人員ID(用於取得    
    /// </summary> 
    public string MASTER_TEL { get; set; }
    /// <summary>    
    /// 負責所屬人員    
    /// </summary> 
    public string MASTER_Name { get; set; }
    public string MASTER_MAIL { get; set; }
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
    /// 是否停用該班次0:啟用;1:停用    
    /// </summary> 
    public bool ClassDisable { get; set; }
    /// <summary>    
    ///     
    /// </summary> 
    public DateTime? UPDATE_TIME { get; set; }
    ///
    public string START_TIME { get; set; }

    public string END_TIME { get; set; }

    // ===== 雇主工資訊 =====

    public string Cust_ID { get; set; }
    public string Cust_Class { get; set; }
    public string Cust_Name { get; set; }

    // ===== 勞工資訊 =====
    public string Cust_FullName { get; set; }
    public string Labor_Country { get; set; }
    public string Labor_Valid { get; set; }
    public string Labor_ID { get; set; }
    public string Labor_PID { get; set; }
    public string Labor_RID { get; set; }
    public string Labor_EID { get; set; }
    public string Labor_EName { get; set; }
    public string Labor_CName { get; set; }
    public string Labor_Company { get; set; }
    public string Labor_Team { get; set; }
    public string Labor_Language { get; set; }
    public string Labor_Phone { get; set; }
    public string Labor_Address { get; set; }
    public string Labor_Address2 { get; set; }
    public string Labor_Room { get; set; }
    public string Service_ID { get; set; }
    public string Service { get; set; }
    public string ServiceName { get; set; }
    public string Agent_ID { get; set; }
    public string Agent_Name { get; set; }
    public string Agent_Company { get; set; }
    public string Agent_Team { get; set; }
    public string Agent_Phone { get; set; }
    public string Agent_Phone_2 { get; set; }
    public string Agent_Phone_3 { get; set; }
    public string Agent_TEL { get; set; }
    public string Agent_Co_TEL { get; set; }
    public string Agent_Status { get; set; }
    public string Agent_Mail { get; set; }
    public string Role_ID { get; set; }
    public DateTime Time_01 { get; set; }
    public DateTime Time_02 { get; set; }
    public DateTime Time_03 { get; set; }
    public DateTime Time_04 { get; set; }
    public string UpdateDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public string CarName { get; set; }
    public string CarNumber { get; set; }
    public string PostCode { get; set; }
    public string Location { get; set; }
    public string LocationStart { get; set; }
    public string LocationEnd { get; set; }
    public string ContactName { get; set; }
    public string ContactPhone { get; set; }
    public string ContactPhone2 { get; set; }
    public string ContactPhone3 { get; set; }
    public string Contact_TEL { get; set; }
    public string Contact_Co_TEL { get; set; }
    public string Hospital { get; set; }
    public string HospitalName { get; set; }
    public string HospitalClass { get; set; }
    public string Torna { get; set; }
    public string Question { get; set; }
    public string Question2 { get; set; }
    public string Answer { get; set; }
    public string Answer2 { get; set; }
    public string UPDATE_ID { get; set; }
    public string UPDATE_Name { get; set; }
    public string Create_Company { get; set; }
    public string Create_Team { get; set; }
    public string Create_ID { get; set; }
    public string Create_Name { get; set; }
    public string Type { get; set; }
    public string Type_Value { get; set; }
    public string Chargeback { get; set; }
    public string DEPT_Status { get; set; }

    //======= 子母單 =======

    public string MNo { get; set; }
    public string CNo { get; set; }
    public string CarSeat { get; set; }
    public string CarAgent_Name { get; set; }
    public string CarAgent_Team { get; set; }
    public string Danger { get; set; }
    public string Danger_Value { get; set; }
    public string CREATE_USER { get; set; }
    public string str_time { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime OverTime { get; set; }
    public DateTime Working { get; set; }
    public DateTime WorkOff { get; set; }
    public string E_MAIL { get; set; }
    public string ConnURL { get; set; }
    public string AssignNo { get; set; }
    public string UserID { get; set; }
    public string Password { get; set; }

    public string Cancel_ID { get; set; }
    public string Cancel_Name { get; set; }
    public DateTime Cancel_Time { get; set; }
    public string Allow_ID { get; set; }
    public string Allow_Name { get; set; }
    public DateTime Allow_Time { get; set; }
    public string Close_ID { get; set; }
    public string Close_Name { get; set; }
    public DateTime Close_Time { get; set; }
    public string Dispatch_ID { get; set; }
    public string Dispatch_Name { get; set; }
    public DateTime Dispatch_Time { get; set; }

    public string Agent_LV { get; set; }
    //public string ROLE_ID { get; set; }
    public string ROLE_NAME { get; set; }
    public string OPEN_DEL { get; set; }
    public string Flag { get; set; }
    public string Flag_1 { get; set; }
    public string Flag_2 { get; set; }
    public string Flag_3 { get; set; }
    public string Flag_4 { get; set; }
    public string Flag_5 { get; set; }
    public string Flag_6 { get; set; }
    public string Del_Flag { get; set; }
    public string Email_Flag { get; set; }
    public string NowStatus { get; set; }
    public string UpDateUser { get; set; }
    public string UpDateDate { get; set; }
    public string OpenUser { get; set; }
    public string OpenUserID { get; set; }
    public DateTime OpenDate { get; set; }

    //DIMAX
    public string BUSINESSNAME { get; set; }

    public string Case_ID { get; set; }
    public string ReplyType { get; set; }
    public string PS { get; set; }
    public string DealingProcess { get; set; }
    public string Reply { get; set; }
    public string Upload_Time { get; set; }
    public string Urgency { get; set; }
    public string EstimatedFinishTime { get; set; }
    public string OpinionSource { get; set; }
    public string OpinionType { get; set; }
    public string OpinionSubject { get; set; }
    public string OnSpotTime { get; set; }
    public string OpinionContent { get; set; }

    // OE 頁面用
    public string Product_Name { get; set; }
    public string Product_ID { get; set; }
    public string Main_Classified { get; set; }
    public string Detail_Classified { get; set; }
    public string Price { get; set; }
    public string Contact_ADDR { get; set; }

    //客戶資料頁面用

    public string BUSINESSID { get; set; }
    public string BUS_CREATE_DATE { get; set; }
    public string APP_SUBTITLE { get; set; }
    public string APP_EMAIL { get; set; }
    public string APPNAME_2 { get; set; }
    public string APP_SUBTITLE_2 { get; set; }
    public string APP_MTEL_2 { get; set; }
    public string APP_EMAIL_2 { get; set; }
    public string REGISTER_ADDR { get; set; }
    public string Card_Name { get; set; }
    public string Card_Base64 { get; set; }
    public string CONTACT_ADDR { get; set; }
    public string APP_FTEL { get; set; }
    public string INVOICENAME { get; set; }
    public string Inads { get; set; }
    public string Mail_Type { get; set; }
    public string OE_Number { get; set; }
    public string SalseAgent { get; set; }
    public string Salse { get; set; }
    public string Salse_TEL { get; set; }
    public string SID { get; set; }
    public string Serial_Number { get; set; }
    public string License_Host { get; set; }
    public string Licence_Name { get; set; }
    public string LAC { get; set; }
    public string Our_Reference { get; set; }
    public string Your_Reference { get; set; }
    public string Auth_File_ID { get; set; }
    public string FL { get; set; }
    public string Group_Name_ID { get; set; }
    public string SED { get; set; }
    public string Warranty_Date { get; set; }
    public string Warr_Time { get; set; }
    public string Protect_Date { get; set; }
    public string Prot_Time { get; set; }
    public string Receipt_Date { get; set; }
    public string Receipt_PS { get; set; }
    public string Close_Out_Date { get; set; }
    public string Close_Out_PS { get; set; }
    public string Account_PS { get; set; }
    public string Information_PS { get; set; }
    public string SetupDate { get; set; }

    //0628 子公司
    public string PNumber { get; set; }
    public string Name { get; set; }
    public string ADDR { get; set; }
    public string Contac_ADDR { get; set; }

    public string MEMO { get; set; }
    //0703 員工編輯
    public string Agent_Code { get; set; }
    public string Agent_MVPN { get; set; }
    //0704 子公司編輯
    public string s { get; set; }
    //0710 099新增派工
    public string agent_id { get; set; }

    public string SetupTime { get; set; }
    public string id { get; set; }
    public string A_ID { get; set; }
    //0713 
    public string DI_No { get; set; }
    //07 17
    public string Work { get; set; }
    //0719
    public string Day { get; set; }
    public string Schedule { get; set; }
    public string S_UpdateTime { get; set; }
    //0727
    public string time1 { get; set; }
    public string time2 { get; set; }
    //0822
    public string C_ID2 { get; set; }
    public string Handle_Agent { get; set; }
    public string ReachTime { get; set; }
    public string FinishTime { get; set; }
    public string Service_DATE { get; set; }
    public string Service_Flag { get; set; }

    public string mission_name { get; set; }
    public string person_charge { get; set; }
    public string Alphabet { get; set; }
    public string equipment_X { get; set; }
    public string equipment_Y { get; set; }
    public string equipment_name { get; set; }
    public string equipment_type1 { get; set; }
    public string equipment_question1 { get; set; }
    public string equipment_question2 { get; set; }
    public string equipment_question3 { get; set; }
    public string equipment_question4 { get; set; }
    public string equipment_question5 { get; set; }
    public string File_name { get; set; }
    public string B_N { get; set; }
    public string C_N { get; set; }
    public string T_ID { get; set; }
    //1016 維護單
    public string MTEL { get; set; }
    public string CycleTime { get; set; }

    public string OE_ID { get; set; }
    public string Unit_Price { get; set;}
    public string Case_Name { get; set; }
    public string Personnel { get; set; }
    public string Clinet_Name { get; set;}
    public string Contact { get; set; }
    public string Phone { get; set; }
    public string System_Data { get; set; }
    public string Total_price { get; set; }
    public string Assist_Company { get; set; }
    public string Project_Content { get; set; }
    public string Remark { get; set; }
}







