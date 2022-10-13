using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.Configuration;

public class CMS_db : System.Web.UI.Page
{

    public string ConnStr()
    {
        string strConn = null;

        strConn = WebConfigurationManager.ConnectionStrings["SQLString"].ToString() + ";Connection Timeout=600";
        return strConn;

    }

    //'=============== 寫錯誤Log ==============="
    public void WriteErrorLog(Exception exception)
    {
        ILog logger = null;

        logger = LogManager.GetLogger("GeneralLogger");
        logger.Error(exception.Source);
        logger.Error(System.Web.HttpContext.Current.Request.Path);
        logger.Error(exception.Message);
        logger.Error(exception.StackTrace);

    }

    //public string FormatDate(string day)
    //{
    //    string tmp = "";
    //    if (!string.IsNullOrEmpty(day.Trim()))
    //    {
    //        tmp = day.Substring(0, 4) + day.Substring(6, 2) + day.Substring(day.Length - 2);
    //    }
    //    return tmp;
    //}

    //public string ReturnFormatDate(string day)
    //{
    //    string tmp = "";
    //    if (!string.IsNullOrEmpty(day.Trim))
    //    {
    //        tmp = Strings.Left(day, 4) + "/" + Strings.Mid(day, 5, 2) + "/" + Strings.Mid(day, 7, 2);
    //    }
    //    return tmp;
    //}

    //public string ChineseFormatDate(string day)
    //{
    //    string tmp = "";
    //    if (!string.IsNullOrEmpty(day.Trim))
    //    {
    //        tmp = Convert.ToString(Convert.ToInt32(Strings.Left(day, 4)) - 1911) + "/" + Strings.Mid(day, 5, 2) + "/" + Strings.Right(day, 2);
    //    }
    //    return tmp;
    //}

    //public string ChineseFormatDate2(string day)
    //{
    //    string tmp = null;
    //    int tmpa = 0;
    //    if (day.Length == 0)
    //    {
    //        return "";
    //    }
    //    if (day.Length == 6)
    //    {
    //        tmpa = 4;
    //    }
    //    else
    //    {
    //        tmpa = 2;
    //    }
    //    tmp = Convert.ToString(Convert.ToInt32(Strings.Left(day, tmpa)) - 1911) + "/" + Strings.Right(day, day.Length - tmpa);
    //    return tmp;
    //}

    //public bool Check_Back_Forward(string x1, string x2)
    //{

    //    if ((string.IsNullOrEmpty(x1)) | (string.IsNullOrEmpty(x2)))
    //    {
    //        return false;
    //    }
    //    else if ((Strings.InStr(x1, "-") > 0) | (Strings.InStr(x2, "-") > 0))
    //    {
    //        return false;
    //    }
    //    else if (Convert.ToInt32(x1) < 0 | Convert.ToInt32(x2) < 0)
    //    {
    //        return false;
    //    }
    //    else
    //    {
    //        if (Convert.ToInt32(x2) >= Convert.ToInt32(x1))
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //}

}