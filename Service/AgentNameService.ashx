<%@ WebHandler Language="C#" Class="AgentName" %>

using System;
using System.Web;
using Newtonsoft.Json;

public class AgentName : IHttpHandler
{
    /// <summary>
    /// 連動組別
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.Charset = "utf-8";
        string company= context.Request.Params["Company"]==null?null:context.Request.Params["Company"].ToString();
        string team= context.Request.Params["team"]==null?null:context.Request.Params["team"].ToString();
        if (string.IsNullOrEmpty(company)) {
            context.Response.StatusCode = 404;
            context.Response.StatusDescription = "no paramer company";
            return;
        }
        Agent agent = new Agent();
        context.Response.Write(JsonConvert.SerializeObject(agent.AgentItemList(company,team, true)));
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

}