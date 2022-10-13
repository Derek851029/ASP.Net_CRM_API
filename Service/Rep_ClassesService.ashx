<%@ WebHandler Language="C#" Class="Rep_ClassesService" %>
using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
public class Rep_ClassesService : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.Charset = "utf-8";
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        #region 判斷式
        if (context.Request.Params["year"] != null
                && !int.TryParse(context.Request.Params["year"].ToString(), out year))
            context.Response.Write(JsonConvert.SerializeObject(
                new { status = "error", msg = "參數year只限輸入數字" }));

        if (context.Request.Params["month"] != null
                && !int.TryParse(context.Request.Params["month"].ToString(), out month))
            context.Response.Write(JsonConvert.SerializeObject(
                new { status = "error", msg = "參數month只限輸入數字" }));
        #endregion


        try
        {
            ReportClassRepository report = new ReportClassRepository();
            report.Create(year, month);
            context.Response.Write(JsonConvert.SerializeObject(new { status = "success", msg = string.Empty }));
        }
        catch (Exception err)
        {
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