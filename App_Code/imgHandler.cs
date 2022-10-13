using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

/// <summary>
/// 處理盜連圖片、img 404
/// </summary>
public class imgHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        bool imgExist = (File.Exists(context.Request.PhysicalPath)) ? true : false;//檔案存不存在
        bool access = (context.Request.UrlReferrer.Host == "localhost") ? true : false;//是否為本應用程式的Request

        //如果"檔案不存在"或"盜連"其中一種情況，都輸出404圖片，否則輸出正常的圖片
        string filePath = (imgExist && access) ? context.Request.PhysicalPath : Path.Combine(context.Request.PhysicalApplicationPath, @"images\404.jpg");

        context.Response.Expires = 0;
        context.Response.Clear();
        context.Response.ContentType = "image/png";

        context.Response.WriteFile(filePath);//輸出圖片
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}