using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using log4net;
using log4net.Config;

public class CMS_0060010001
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public static List<CMS_0060010000> GetList()
    {
        //Disable：是否停用該班次0:啟用; 1:停用
        String Sqlstr = @"select * from ROLELIST where Del_Flag='N'";
        return DBTool.Query<CMS_0060010000>(Sqlstr).ToList();
    }

    public static bool Delete(string seqno)
    {
        int iseqno = 0;
        if (!int.TryParse(seqno, out iseqno))
            throw new Exception("來源資料不是數字");
        //Disable：是否停用該班次0:啟用; 1:停用
        String Sqlstr = @"update ROLELIST Set Del_Flag='Y' where ROLE_ID=@ROLE_ID";
        using (IDbConnection conn = DBTool.GetConn())
        {
            conn.Execute(Sqlstr, new { ROLE_ID = seqno });
        }
        return true;
    }

    public static CMS_0060010000 Get(int seqno)
    {
        String Sqlstr = @"select * from ROLELIST where ROLE_ID=@ROLE_ID";
        var list = DBTool.Query<CMS_0060010000>(Sqlstr, new { ROLE_ID = seqno });
        if (list.Any())
            return list.First();
        else
            throw new Exception("找不到資料");
    }

    public static bool Update(CMS_0060010000 partnerHeader)
    {
        string sqlstr = @"update [dbo].[PartnerHeader] 
                          Set Partner_Company=@Partner_Company,
                              Partner_Driver=@Partner_Driver,
                              Partner_Phone=@Partner_Phone
                          Where SYS_ID=@SYS_ID";
        using (IDbConnection db = DBTool.GetConn())
            return db.Execute(sqlstr, partnerHeader) > 0;
    }

    public static List<CMS_0060010000> EMMA_List()
    {
        String Sqlstr = @"SELECT [Service],[ServiceName],[Service_ID] FROM Data WHERE Open_Flag = 1 AND Service !='外包商' ";
        return DBTool.Query<CMS_0060010000>(Sqlstr).ToList();
    }

    public static List<CMS_0060010000> EMMA_Service_List(string Service_ID)
    {
        string sqlstr = @"select [Service_ID] ,[Service] ,[ServiceName] ,[UserID] ,[MASTER_ID] ,[MASTER_NAME] ,[MASTER_MAIL] ,[Flag_1] ,[Flag_2] 
                                        from EMMA WHERE Service_ID = @Service_ID AND Del_Flag = 'N'
                                        group by [Service_ID] ,[Service] ,[ServiceName] ,[UserID] ,[MASTER_ID] ,[MASTER_NAME] ,[MASTER_MAIL] ,[Flag_1] ,[Flag_2]";
        return DBTool.Query<CMS_0060010000>(sqlstr, new { Service_ID = Service_ID }).ToList();
    }

    public static List<CMS_0060010000> EMMA_TEAM_List(string Service_ID, string MASTER_ID)
    {
        string sqlstr = @"SELECT * FROM EMMA WHERE Service_ID = @Service_ID AND MASTER_ID = @MASTER_ID AND Del_Flag = 'N'";
        return DBTool.Query<CMS_0060010000>(sqlstr, new { Service_ID = Service_ID, MASTER_ID = MASTER_ID }).ToList();
    }

    public List<string> Agent_Company()
    {
        string sqlstr = @"SELECT [Agent_Company] FROM [DispatchSystem] where Agent_ID !='' AND Agent_Status = '在職'";
        var result = DBTool.Query<string>(sqlstr);
        result = result.Distinct();
        return result.ToList();
    }

    public List<string> Agent_Team(string Agent_Company)
    {
        string sqlstr = @"SELECT [Agent_Team] FROM [DispatchSystem] where Agent_ID !='' AND Agent_Status = '在職' AND Agent_Company = @Agent_Company order by Agent_Team";
        var result = DBTool.Query<string>(sqlstr, new { Agent_Company = Agent_Company });
        result = result.Distinct();
        return result.ToList();
    }

    public List<AgentItem> Agent_Name(string Agent_Company, string Agent_Team)
    {
        string sqlstr = @"SELECT [Agent_ID], [Agent_Name] FROM [DispatchSystem] 
                                   where Agent_ID !='' 
                                   AND Agent_Status = '在職' 
                                   AND Agent_Company = @Agent_Company 
                                   AND Agent_Team = @Agent_Team 
                                   order by Agent_Name";
        var result = DBTool.Query<AgentItem>(sqlstr, new { Agent_Company = Agent_Company, Agent_Team = Agent_Team });
        result = result.Distinct();
        return result.ToList();
    }

    public static CMS_0060010000 AgentData(string Agent_ID)
    {
        string sqlstr = @"select * from DispatchSystem
                          where Agent_ID=@Agent_ID AND Agent_Status = '在職'";
        var list = DBTool.Query<CMS_0060010000>(sqlstr, new { Agent_ID = Agent_ID });
        if (list.Count() > 0)
            return list.First();
        else
            return null;
    }

    public static List<CMS_0060010000> PROGLIST(string Role_ID)
    {
        //Disable：是否停用該班次0:啟用; 1:停用
        String Sqlstr = @"select case when c.TREE_Name is null then a.TREE_NAME else c.TREE_NAME end  as M_TREE_NAME,a.TREE_ID , a.TREE_NAME ,
                                         Case when b.Tree_ID is not null then '1' else '0' end as NowStatus ,d.Agent_Name , b.UpDateDate
                                         FROM (SELECT * FROM [Faremma].[dbo].[PROGLIST] where NAVIGATE_URL is not null and LTRIM(NAVIGATE_URL) <> '' and IS_OPEN='Y') a 
                                         Left join [Faremma].[dbo].[ROLEPROG] b on b.Role_ID = @Role_ID and a.TREE_ID = b.TREE_ID and IS_OPEN = 'Y' 
                                         Left join (select * from [Faremma].[dbo].[PROGLIST] where (PARENT_ID is not null and LTRIM(PARENT_ID) <> '') 
                                         and (NAVIGATE_URL is  null or LTRIM(NAVIGATE_URL) = '') ) c on a.PARENT_ID = c.TREE_ID 
                                         Left join  [Faremma].[dbo].[DispatchSystem] d on d.Agent_ID = b.UpDateUser 
                                         order by c.sort_id , a.sort_id ";
        return DBTool.Query<CMS_0060010000>(Sqlstr, new { Role_ID = Role_ID }).ToList();
    }

    public static List<CMS_0060010000> Service_List()
    {
        String Sqlstr = @"SELECT * FROM Data";
        return DBTool.Query<CMS_0060010000>(Sqlstr).ToList();
    }

    public static bool Service_Flag(string SYS_ID, string ID, string NAME, string Flag)
    {
        string TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        String Sqlstr = @"update Data Set UPDATE_ID=@ID,UPDATE_NAME=@NAME,UpDateDate=@TIME,Flag=@Flag where SYS_ID = @SYS_ID";
        using (IDbConnection conn = DBTool.GetConn())
        {
            conn.Execute(Sqlstr, new { SYS_ID = SYS_ID, ID = ID, NAME = NAME, TIME = TIME, Flag = Flag });
            conn.Close();
        }
        return true;
    }

    public static bool Service_Open_Flag(string SYS_ID, string ID, string NAME, string Flag)
    {
        string TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        String Sqlstr = @"update Data Set UPDATE_ID=@ID,UPDATE_NAME=@NAME,UpDateDate=@TIME,Open_Flag=@Flag where SYS_ID = @SYS_ID";
        using (IDbConnection conn = DBTool.GetConn())
        {
            conn.Execute(Sqlstr, new { SYS_ID = SYS_ID, ID = ID, NAME = NAME, TIME = TIME, Flag = Flag });
            conn.Close();
        }
        return true;
    }

    public static bool EMMA_Flag(string ID, string SID, string NAME, string Flag)
    {
        string TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        string Sqlstr = @"update EMMA Set UPDATE_ID=@UPDATE_ID,TIME=@TIME,";
        switch (Flag)
        {
            case "1":
                Sqlstr += "Flag_1 = '1'  where Service_ID = @Service_ID AND MASTER_ID = @MASTER_ID";
                break;
            case "2":
                Sqlstr += "Flag_1 = '0'  where Service_ID = @Service_ID AND MASTER_ID = @MASTER_ID";
                break;
            case "3":
                Sqlstr += "Flag_2 = '1'  where Service_ID = @Service_ID AND MASTER_ID = @MASTER_ID";
                break;
            case "4":
                Sqlstr += "Flag_2 = '0'  where Service_ID = @Service_ID AND MASTER_ID = @MASTER_ID";
                break;
            case "5":
                Sqlstr += "Flag_3 = '1'  where SYS_ID = @MASTER_ID";
                break;
            case "6":
                Sqlstr += "Flag_3 = '0'  where SYS_ID = @MASTER_ID";
                break;
        }
        using (IDbConnection conn = DBTool.GetConn())
        {
            conn.Execute(Sqlstr, new { MASTER_ID = ID, Service_ID = SID, UPDATE_ID = NAME, TIME = TIME });
            conn.Close();
        }
        return true;
    }

    public class AgentItem
    {
        public string Agent_ID;
        public string Agent_Name;
    }
}