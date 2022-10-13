<%@ WebHandler Language="C#" Class="ClassScheduleService" %>

using System;
using System.Globalization;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using log4net;
using log4net.Config;

public class ClassScheduleService : IHttpHandler
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        
        DateTime date = DateTime.Now;
        logger.Info(date);
        if (date.Day >= 25)
        {
            logger.Info("超過25號");
            context.Response.Write(JsonConvert.SerializeObject(new { status = "success", msg = "日期超過25號" }));
            month = date.AddMonths(1).Month;
            logger.Info("month = date.AddMonths(1).Month = " + month);
            if (month == 1)
            {
                year = date.AddYears(1).Year;
                logger.Info("year = date.AddYears(1).Year = " + year);
            }
            else
            {
                year = date.Year;
                logger.Info("date.Year = " + year);
            }
        }
        else
        {
            logger.Info("日期未於25號之後");
            context.Response.Write(JsonConvert.SerializeObject(new { status = "success", msg = "日期未於25號之後" }));
        }

        string checkstr = @"select top 1 [SYSID] from [InSpecation_Dimax].[dbo].[Mission_Case] where Month is not null ";   //WORK_DATE between @START_TIME AND @END_TIME
        string START_TIME = new DateTime(year, month, 1).ToString("yyyy-MM-dd 00:00:00");
        logger.Info(DateTime.DaysInMonth(year, month));
        string END_TIME = new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("yyyy-MM-dd 23:59:59");
        logger.Info("TIME : " + START_TIME + END_TIME);
        ClassTemplate template = new ClassTemplate()
        {
            START_TIME = START_TIME,
            END_TIME = END_TIME
        };

        var chk = DBTool.Query<String>(checkstr, template);
        if (chk.Any())
        {
            logger.Info("已經有新增過資料了" + START_TIME + END_TIME);
            context.Response.Write(JsonConvert.SerializeObject(new { status = "success", msg = "已經有新增過資料了，起始時間：" + START_TIME + "，終止時間：" + END_TIME }));
            return;
        };

        logger.Info("開始新增資料" + START_TIME + END_TIME);

        try
        {
            ClassScheduleRepository.GenClassClassSchedule(year, month);
            logger.Info("新增完成" + year + month);
            context.Response.Write(JsonConvert.SerializeObject(new { status = "success", msg = string.Empty + "，新增 Mission_Case 資料完成，起始時間：" + START_TIME + "，終止時間：" + END_TIME }));
        }
        catch (Exception err)
        {
            logger.Info("ERROE：" + err);
            context.Response.Write(JsonConvert.SerializeObject(new { status = "error", msg = err.Message + Environment.NewLine + err.StackTrace }));
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}