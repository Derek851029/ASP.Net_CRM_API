using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
/// <summary>
/// ReportClassRepository 的摘要描述
/// </summary>
public class ReportClassRepository
{
    #region 建立資料表的SQL
    /// <summary>
    /// 一年建立一個資料表，需要有建立Table的權限才不會有問題
    /// </summary>

    public readonly string createtable_str =
        @"declare @year char(4)='{0}'
IF (not EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Rep_Classes{0}'))
BEGIN
    
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Rep_Classes{0}'](
	[SYS_ID] [int] IDENTITY(1,1) NOT NULL,
	[WORK_DATE] [datetime] NULL,
	[Class] [nvarchar](50) NULL,
	[DIAL_TIME] [datetime] NULL,
	[WORK_TIME] [datetime] NULL,
	[DRIVER_NAME] [nvarchar](50) NULL,
	[DRIVER_TEL] [varchar](20) NULL,
	[DRIVER_STATE] [nvarchar](20) NULL,
	[DRIVER_DIAL_TIME] [varchar](1) NULL,
	[MASTER1_NAME] [nvarchar](50) NULL,
	[MASTER1_TEL] [varchar](20) NULL,
	[MASTER1_STATE] [nvarchar](20) NULL,
	[MASTER1_DIAL_TIME] [varchar](1) NOT NULL,
	[MASTER2_NAME] [nvarchar](50) NULL,
	[MASTER2_TEL] [varchar](20) NULL,
	[MASTER2_STATE] [nvarchar](20) NULL,
	[MASTER2_DIAL_TIME] [varchar](1) NULL,
	[UPDATE_TIME] [datetime] NULL
) ON [PRIMARY]


SET ANSI_PADDING OFF
ALTER TABLE [dbo].[Rep_Classes{0}] ADD  CONSTRAINT [DF_Rep_Classes{0}+_UPDATE_TIME]  DEFAULT (getdate()) FOR [UPDATE_TIME]

END
";
    #endregion

    public ReportClassRepository()
    {
        isnullCreateTable();
    }

    /// <summary>
    /// 檢查資料表是否存在，若沒有則建立該年度的資料表
    /// </summary>
    public void isnullCreateTable()
    {
        string sqlstr = string.Format(createtable_str, DateTime.Now.Year);
        using (IDbConnection db = DBTool.GetConn())
            db.Execute(sqlstr);
    }

    /// <summary>
    /// 產生Rep_Classes資料
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    public void Create(int year, int month)
    {
        var classschedulelist = ClassScheduleRepository.GetClassScheduleList(year, month);
        var rep_classList = from p in classschedulelist
                            let isPartner = !string.IsNullOrEmpty(p.Partner_Company)
                            let L_DRIVER_NAME = isPartner ? p.Partner_Driver:p.MASTER_Name
                            let L_DRIVER_TEL = isPartner ? p.Partner_Phone: DispatchSystemRepository.GetAgentPhone(p.MASTER_ID)
                            select new Rep_Classes()
                            {
                                Class = p.ClassName,
                                WORK_DATE = p.WORK_DATETime,
                                DIAL_TIME = p.DIAL_DATETime,
                                WORK_TIME = p.WORK_DATETime,
                                DRIVER_NAME = L_DRIVER_NAME,
                                DRIVER_TEL = L_DRIVER_TEL,
                                DRIVER_DIAL_TIME = "0",
                                DRIVER_STATE = "等待外撥",
                                MASTER1_NAME = p.MASTER1_NAME,
                                MASTER1_TEL = p.MASTER1_TEL,
                                MASTER1_STATE = "等待外撥",
                                MASTER1_DIAL_TIME = "0",
                                MASTER2_NAME = p.MASTER2_NAME,
                                MASTER2_TEL = p.MASTER2_TEL,
                                MASTER2_STATE = "等待外撥",
                                MASTER2_DIAL_TIME = "0",
                                UPDATE_TIME = p.UPDATE_TIME.HasValue ? p.UPDATE_TIME.Value : p.Create_TIME
                            };

        string sqlstr = @"INSERT INTO [dbo].[Rep_Classes{0}]
                          (WORK_DATE,Class,DIAL_TIME,WORK_TIME,DRIVER_NAME,DRIVER_TEL,DRIVER_STATE,DRIVER_DIAL_TIME,MASTER1_NAME,MASTER1_TEL,MASTER1_STATE,MASTER1_DIAL_TIME,MASTER2_NAME,MASTER2_TEL,MASTER2_STATE,MASTER2_DIAL_TIME,UPDATE_TIME)
                          VALUES
                          (@WORK_DATE,@Class,@DIAL_TIME,@WORK_TIME,@DRIVER_NAME,@DRIVER_TEL,@DRIVER_STATE,@DRIVER_DIAL_TIME,@MASTER1_NAME,@MASTER1_TEL,@MASTER1_STATE,@MASTER1_DIAL_TIME,@MASTER2_NAME,@MASTER2_TEL,@MASTER2_STATE,@MASTER2_DIAL_TIME,@UPDATE_TIME)";

        IDbConnection conn = null;
        IDbTransaction tran = null;
        try
        {
            conn = DBTool.GetConn();
            conn.Open();
            tran = conn.BeginTransaction();
            //新增資料
            conn.Execute(string.Format(sqlstr,year), rep_classList, tran);
            tran.Commit();
        }
        catch (Exception err)
        {
            tran.Rollback();
            throw err;
        }
        finally
        {
            if (tran != null)
                tran.Dispose();
            if (conn != null)
                conn.Dispose();
        }
    }
}