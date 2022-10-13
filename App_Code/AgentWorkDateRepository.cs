using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using Dapper;
using log4net;
using log4net.Config;
/// <summary>
/// ClassScheduleRepository 的摘要描述
/// </summary>
public class AgentWorkDateRepository : System.Web.UI.Page
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public AgentWorkDateRepository()
    {
        // TODO: 在這裡新增建構函式邏輯
    }
    /// 從範本產生schedule資料
    public static void GenClassClassSchedule(int year, int month, int day, string MASTER_ID)
    {
        logger.Info("Year = " + year + "Month = " + month + "Day = " + day + "MASTER_ID = " + MASTER_ID);
        string insertsqlstr = @"if not exists (select * from [dbo].[AgentWorkDate] where MASTER_ID = @MASTER_ID and convert(VARCHAR(10),[WORK_DATETime], 111) between convert(VARCHAR(10),@WORK_DATETime, 111) AND convert(VARCHAR(10),@WORK_DATETime, 111) ) begin INSERT INTO [dbo].[AgentWorkDate]
                          ([WORK_DATETime],[MASTER_ID],[MASTER_Name]) VALUES(@WORK_DATETime,@MASTER_ID,@MASTER_Name) end ";
        IDbConnection conn = null;
        IDbTransaction tran = null;
        try
        {
            conn = DBTool.GetConn();
            conn.Open();
            tran = conn.BeginTransaction();
            //刪除資料
            //conn.Execute("delete ClassSchedule where WORK_DATETime between @startdate AND @enddate AND UPDATE_TIME IS NULL",
            //    new
            //    {
            //        startdate = new DateTime(year, month, 1).ToString("yyyy-MM-dd 00:00:00"),
            //        enddate = new DateTime(year, month, new TaiwanCalendar().GetDaysInMonth(year, month)).ToString("yyyy-MM-dd 23:59:59")
            //    }, tran);
            //新增資料
            conn.Execute(insertsqlstr, GetClassSchedule(year, month, day, MASTER_ID), tran);
            //throw new Exception("test");
            tran.Commit();
        }
        catch (Exception err)
        {
            tran.Rollback();
            logger.Info(err);
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

    private static List<ClassSchedule> GetClassSchedule(int year, int month, int day, string MASTER_ID)
    {
        List<ClassSchedule> result = new List<ClassSchedule>();
        //宣告台灣日期
        Calendar calendar1 = new TaiwanCalendar();
        //取得範本資料
        List<ClassTemplate> classtemplatelist = getClassTemplateList(false, MASTER_ID).ToList();

        //將設定年月執行
        for (int i = day; i <= calendar1.GetDaysInMonth(year, month); i++)
        {
            Func<ClassTemplate, bool> func = null;
            DateTime date = new DateTime(year, month, i);
            switch (calendar1.GetDayOfWeek(date))
            {
                case DayOfWeek.Sunday:
                    func = (ClassTemplate p) => { return p.ClassWeek_Sun; };
                    logger.Info("第" + i + "次,年：" + year + ",月：" + month + ", Week：" + calendar1.GetDayOfWeek(date) + "," + date + ", func = " + func);
                    break;
                case DayOfWeek.Monday:
                    func = (ClassTemplate p) => { return p.ClassWeek_Mon; };
                    logger.Info("第" + i + "次,年：" + year + ",月：" + month + ", Week：" + calendar1.GetDayOfWeek(date) + "," + date + ", func = " + func);
                    break;
                case DayOfWeek.Tuesday:
                    func = (ClassTemplate p) => { return p.ClassWeek_Tue; };
                    logger.Info("第" + i + "次,年：" + year + ",月：" + month + ", Week：" + calendar1.GetDayOfWeek(date) + "," + date + ", func = " + func);
                    break;
                case DayOfWeek.Wednesday:
                    func = (ClassTemplate p) => { return p.ClassWeek_Wed; };
                    logger.Info("第" + i + "次,年：" + year + ",月：" + month + ", Week：" + calendar1.GetDayOfWeek(date) + "," + date + ", func = " + func);
                    break;
                case DayOfWeek.Thursday:
                    func = (ClassTemplate p) => { return p.ClassWeek_Thu; };
                    logger.Info("第" + i + "次,年：" + year + ",月：" + month + ", Week：" + calendar1.GetDayOfWeek(date) + "," + date + ", func = " + func);
                    break;
                case DayOfWeek.Friday:
                    func = (ClassTemplate p) => { return p.ClassWeek_Fri; };
                    logger.Info("第" + i + "次,年：" + year + ",月：" + month + ", Week：" + calendar1.GetDayOfWeek(date) + "," + date + ", func = " + func);
                    break;
                case DayOfWeek.Saturday:
                    func = (ClassTemplate p) => { return p.ClassWeek_Sat; };
                    logger.Info("第" + i + "次,年：" + year + ",月：" + month + ", Week：" + calendar1.GetDayOfWeek(date) + "," + date + ", func = " + func);
                    break;
            }
            result.AddRange(classtemplatelist.Where(func).Select(p => combineSchedule(p, date)));
        }
        return result;
    }  

    private static ClassSchedule combineSchedule(ClassTemplate classtemplate, DateTime datetime)
    {
        //4/4/2010 4:20:00 PM
        string workstr = string.Format("{0} 00:00:00", datetime.ToString("d/M/yyyy"));
        DateTime WORK_DATETime = DateTime.ParseExact(workstr, "d/M/yyyy h:m:ss", CultureInfo.GetCultureInfo("zh-tw"));

        logger.Info("WORK_DATETime" + WORK_DATETime + "MASTER_ID = " + classtemplate.MASTER_ID + "MASTER_Name = " + classtemplate.MASTER_Name + "SYS_ID = " + classtemplate.SYS_ID);
        logger.Info("現在時間為：" + DateTime.Now.TimeOfDay);

        return new ClassSchedule()
        {
            #region 組合資料
            WORK_DATETime = WORK_DATETime,
            MASTER_ID = classtemplate.MASTER_ID,
            MASTER_Name = classtemplate.MASTER_Name
            #endregion
        };
    }
    private static IEnumerable<ClassTemplate> getClassTemplateList(bool ClassDisable, string MASTER_ID)
    {
        string sqlstr = "select * from AgentWorkTemplate where ClassDisable=@ClassDisable AND MASTER_ID = @MASTER_ID";
        return DBTool.Query<ClassTemplate>(sqlstr, new { ClassDisable = ClassDisable, MASTER_ID = MASTER_ID });
    }  
}