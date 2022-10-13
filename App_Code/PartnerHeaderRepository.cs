using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using log4net;
using log4net.Config;
/// <summary>
/// PartnerHeaderRepository 的摘要描述
/// </summary>
public class PartnerHeaderRepository
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public static List<PartnerHeader> GetList()
    {
        //Disable：是否停用該班次0:啟用; 1:停用
        String Sqlstr = @"select * from PartnerHeader where Disable=0";
        return DBTool.Query<PartnerHeader>(Sqlstr).ToList();
    }

    public static bool Delete(string seqno)
    {
        int iseqno = 0;
        if (!int.TryParse(seqno, out iseqno))
            throw new Exception("來源資料不是數字");
        //Disable：是否停用該班次0:啟用; 1:停用
        String Sqlstr = @"update PartnerHeader Set Disable=1 where SYS_ID=@SYS_ID";
        using (IDbConnection conn = DBTool.GetConn())
        {
            conn.Execute(Sqlstr, new { SYS_ID = seqno });
        }
        return true;
    }

    public static PartnerHeader Get(int seqno)
    {
        String Sqlstr = @"select * from PartnerHeader where SYS_ID=@SYS_ID";
        var list = DBTool.Query<PartnerHeader>(Sqlstr, new { SYS_ID = seqno });
        if (list.Any())
            return list.First();
        else
            throw new Exception("找不到資料");
    }

    public static bool Update(PartnerHeader partnerHeader)
    {
        string sqlstr = @"update [dbo].[PartnerHeader] 
                          Set Partner_Company=@Partner_Company,
                              Partner_Driver=@Partner_Driver,
                              Partner_Phone=@Partner_Phone
                          Where SYS_ID=@SYS_ID";
        using (IDbConnection db = DBTool.GetConn())
            return db.Execute(sqlstr, partnerHeader) > 0;
    }


    //==========  CMS_0020010000 ==========

    public static PartnerHeader CMS_0020010000_Get(int seqno)
    {
        String Sqlstr = @"select * from AgentWorkTemplate where Agent_ID=@Agent_ID";
        var list = DBTool.Query<PartnerHeader>(Sqlstr, new { Agent_ID = seqno });
        if (list.Count() > 0)
            return list.First();
        else
            return null;
    }

    //==========  CMS_0030010000 ==========

    public static List<PartnerHeader> CMS_0030010001_OE_List(string OE_Main, string OE_Detail)
    {
        String Sqlstr = @"select * from OE_Product where Main_Classified = @OE_Main and Detail_Classified = @OE_Detail";
        return DBTool.Query<PartnerHeader>(Sqlstr, new { OE_Main = OE_Main, OE_Detail = OE_Detail }).ToList();
    }

    public static List<PartnerHeader> CMS_0030010001_GetList(string MNo)
    {
        String Sqlstr = @"select * from CASEDetail where MNo = @MNo";
        return DBTool.Query<PartnerHeader>(Sqlstr, new { MNo = MNo }).ToList();
    }

    public static List<ClassTemplate> CMS_0030010001_MNo_Labor(string MNo)
    {
        String Sqlstr = @"SELECT * FROM MNo_Labor WHERE MNo = @MNo";
        return DBTool.Query<ClassTemplate>(Sqlstr, new { MNo = MNo }).ToList();
    }

    public static List<ClassTemplate> CMS_0030010001_Labor()
    {
        string Sqlstr = @"SELECT TOP 25000 " +
            "SYSID, " +
            "Cust_ID, " +
            "Cust_FullName, " +
            "Labor_ID, " +
            "Labor_CName, " +
            "Labor_Country, " +
            "Labor_PID, " +
            "Labor_RID, " +
            "Labor_EID, " +
            "Labor_Phone, " +
            "Labor_Team, " +
            "Labor_Company, " +
            "Labor_Address " +
            "FROM Labor_System";
        return DBTool.Query<ClassTemplate>(Sqlstr).ToList();
    }
    //==========  CMS_0040010000 ==========

    public static List<PartnerHeader> CMS_0040010002_GetList(string start, string end, string Hospital)
    {
        logger.Info("start = " + start + " , end = " + end + " , Hospital = " + Hospital);
        Convert.ToDateTime(start);
        Convert.ToDateTime(end);   //between @startDate AND @ednDate
        String Sqlstr = @"select * from CASEDetail where CONVERT(varchar(10), StartTime,111) between CONVERT(varchar(10), @start,111) and CONVERT(varchar(10), @end,111) and Hospital = @Hospital and CarAgent_Name != '' ";
        return DBTool.Query<PartnerHeader>(Sqlstr, new { start = start, end = end, Hospital = Hospital }).ToList();
    }
    //==========  CMS_0150010000 ==========

    public static List<PartnerHeader> CMS_0150010004_GetList()
    {
        //Disable：是否停用該班次0:啟用; 1:停用
        String Sqlstr = @"select * from ClassTemplate where ClassDisable=0";
        return DBTool.Query<PartnerHeader>(Sqlstr).ToList();
    }

    public static bool CMS_0150010004_Delete(string seqno, string ID, string NAME)
    {
        int iseqno = 0;
        if (!int.TryParse(seqno, out iseqno))
            throw new Exception("來源資料不是數字");
        //Disable：是否停用該班次0:啟用; 1:停用
        String Sqlstr = @"update [dbo].[ClassTemplate] Set ClassDisable=1,UPDATE_NAME=@UPDATE_NAME, UPDATE_ID=@UPDATE_ID, UPDATE_TIME=@UPDATE_TIME where SYS_ID = @SYS_ID";
        using (IDbConnection conn = DBTool.GetConn())
        {
            conn.Execute(Sqlstr, new { SYS_ID = seqno, UPDATE_NAME = NAME, UPDATE_ID = ID, UPDATE_TIME = DateTime.Now });
        }
        return true;
    }

    public static bool Delete_Rep_Classes2016(string seqno)
    {
        int iseqno = 0;
        if (!int.TryParse(seqno, out iseqno))
        {
            throw new Exception("來源資料不是數字");
        }

        DateTime date = DateTime.Now;
        logger.Info("刪除 Rep_Classes2016 今天以後的班次，班次ID：" + seqno);
        logger.Info(date.ToString("yyyyMMdd"));
        String Sqlstr = @"DELETE Rep_Classes2016 WHERE Template_SYS_ID = @SYS_ID AND CONVERT(varchar(100), WORK_DATE, 112) > @DATE ";
        using (IDbConnection conn = DBTool.GetConn())
        {
            conn.Execute(Sqlstr, new { SYS_ID = seqno, DATE = date.ToString("yyyyMMdd") });
        }
        return true;
    }

    public static bool CMS_0150010002_Delete(string seqno)
    {
        int iseqno = 0;
        if (!int.TryParse(seqno, out iseqno))
            throw new Exception("來源資料不是數字");
        //Disable：是否停用該班次0:啟用; 1:停用
        String Sqlstr = @"DELETE FROM Rep_Classes2016 where SYS_ID = @SYS_ID";
        using (IDbConnection conn = DBTool.GetConn())
        {
            conn.Execute(Sqlstr, new { SYS_ID = seqno });
        }
        return true;
    }

    public static PartnerHeader CMS_0150010004_Get(int seqno)
    {
        String Sqlstr = @"select * from ClassTemplate where SYS_ID=@SYS_ID";
        var list = DBTool.Query<PartnerHeader>(Sqlstr, new { SYS_ID = seqno });
        if (list.Count() > 0)
            return list.First();
        else
            return null;
    }

    public static bool Agent_Delete(string SYSID)
    {
        string Sqlstr = @"UPDATE [dbo].[DispatchSystem] SET Agent_Status='離職' where SYSID = @SYSID";
        using (IDbConnection conn = DBTool.GetConn())
        {
            conn.Execute(Sqlstr, new { SYSID = SYSID });
        }
        return true;
    }

    public static List<ClassTemplate> Bootstrap_TEST()
    {
        string Sqlstr = @"SELECT DISTINCT Cust_FullName ,Cust_ID ,Cust_Name FROM Labor_System WHERE Cust_ID !='' ORDER BY Cust_FullName ";
        return DBTool.Query<ClassTemplate>(Sqlstr).ToList();
    }
}