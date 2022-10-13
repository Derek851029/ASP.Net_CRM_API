<%@ WebHandler Language="C#" Class="AgentTeam" %>

using System;
using System.Web;
using Newtonsoft.Json;

public class AgentTeam : IHttpHandler
{
    /// <summary>
    /// 連動組別
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.Charset = "utf-8";
        string company= context.Request.Params["company"]==null?null:context.Request.Params["company"].ToString();
        if (string.IsNullOrEmpty(company)) {
            context.Response.StatusCode = 404;
            context.Response.StatusDescription = "no paramer Company";
            return;
        }
        Agent agent = new Agent();
        context.Response.Write(JsonConvert.SerializeObject(agent.Agent_Team(company, true)));
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

}