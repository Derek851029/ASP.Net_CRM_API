using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using log4net;
using log4net.Config;

/// <summary>
/// Agent 的摘要描述
/// </summary>
public class Agent : System.Web.UI.Page
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public Agent()
    {

    }
    public List<String> Agent_Team(string Agent_Company, bool isdistinct)
    {
        string sqlstr = "select Agent_Team from DispatchSystem where Agent_Status = '在職'";
        if (!string.IsNullOrEmpty(Agent_Company))
        {
            sqlstr = string.Format("{0} AND Agent_Company=@Agent_Company", sqlstr);
        }
        sqlstr = sqlstr + "  order by Agent_Team";
        logger.Info("sqstrl = " + sqlstr);
        var result = DBTool.Query<string>(sqlstr, new { Agent_Company = Agent_Company });

        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<String> Agent_Name(string Agent_Company, string Agent_Team, bool isdistinct)
    {
        if (string.IsNullOrEmpty(Agent_Company))
            throw new Exception("Agent_Company can't empty or null");
        string sqlstr = "select Agent_Name from DispatchSystem where Agent_Status = '在職' AND Agent_Company=@Agent_Company";
        if (!string.IsNullOrEmpty(Agent_Team))
        {
            sqlstr = string.Format("{0} AND Agent_Team=@Agent_Team", sqlstr);
        }
        var result = DBTool.Query<string>(sqlstr, new { Agent_Company = Agent_Company, Agent_Team = Agent_Team });

        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<AgentItem> AgentItemList(string Agent_Company, string Agent_Team, bool isdistinct)
    {
        if (string.IsNullOrEmpty(Agent_Company))
            throw new Exception("Agent_Company can't empty or null");
        string sqlstr = "select Agent_ID,Agent_Name,Agent_Phone from DispatchSystem where Agent_Status = '在職' AND Agent_Company=@Agent_Company";
        if (!string.IsNullOrEmpty(Agent_Team))
        {
            sqlstr = string.Format("{0} AND Agent_Team=@Agent_Team", sqlstr);
        }
        var result = DBTool.Query<AgentItem>(sqlstr, new { Agent_Company = Agent_Company, Agent_Team = Agent_Team });

        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<string> Agent_Company(bool isdistinct = true)
    {
        string sqlstr = @"SELECT [Agent_Company] FROM [DispatchSystem] where Agent_Status = '在職' AND Agent_ID != '' ";
        var result = DBTool.Query<string>(sqlstr);
        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<string> Labor_Team(bool isdistinct = true)
    {
        string sqlstr = @"SELECT [Labor_Team] FROM [Labor_System] where Labor_ID != '' ";
        var result = DBTool.Query<string>(sqlstr);
        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<string> Cust_FullName(bool isdistinct = true)
    {
        string sqlstr = @"SELECT [Cust_FullName] FROM [Labor_System] where Labor_ID != '' ";
        var result = DBTool.Query<string>(sqlstr);
        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<string> Cust_FullNameList(string Labor_Team, bool isdistinct)
    {
        string sqlstr = "select Cust_FullName from Labor_System ";
        if (!string.IsNullOrEmpty(Labor_Team))
        {
            sqlstr = string.Format("{0} where Labor_Team=@Labor_Team", sqlstr);
        }
        var result = DBTool.Query<string>(sqlstr, new { Labor_Team = Labor_Team });

        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<AgentItem> Select_Labor_Name(string Labor_Team, string Cust_Name, bool isdistinct)
    {
        string sqlstr = @"SELECT [Labor_ID] ,[Labor_CName] FROM [Labor_System] where Labor_ID != '' ";
        if (!string.IsNullOrEmpty(Cust_Name))
        {
            sqlstr = string.Format("{0} AND Cust_FullName=@Cust_FullName", sqlstr);
        }
        else if (!string.IsNullOrEmpty(Labor_Team))
        {
            sqlstr = string.Format("{0} AND Labor_Team=@Labor_Team", sqlstr);
        }
        else if (!string.IsNullOrEmpty(Cust_Name) && !string.IsNullOrEmpty(Cust_Name))
        {
            sqlstr = string.Format("{0} AND Labor_Team=@Labor_Team AND Cust_FullName=@Cust_FullName", sqlstr);
        }
        var result = DBTool.Query<AgentItem>(sqlstr, new { Cust_FullName = Cust_Name, Labor_Team = Labor_Team });
        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public static ClassTemplate Select_Labor_Template(string Labor_ID)
    {
        string sqlstr = @"select [Labor_Team],[Cust_FullName],[Labor_Country],[Labor_Phone],[Labor_Address],[Labor_PID],[Labor_RID],[Labor_EID] from Labor_System
                          where Labor_ID=@Labor_ID";
        var list = DBTool.Query<ClassTemplate>(sqlstr, new { Labor_ID = Labor_ID });
        if (list.Count() > 0)
            return list.First();
        else
            return null;
    }

    // ======================= 0040010002 =======================

    public List<string> Agent_Team_0040010002_View()
    {
        string sqlstr = @"SELECT [Agent_Team] FROM [View_WorkDate] group by Agent_Team";
        var result = DBTool.Query<string>(sqlstr);
        return result.Distinct().ToList();
    }

    public List<AgentItem> Agent_Name_0040010002_View(string Agent_Team)
    {
        string sqlstr = @"SELECT [Agent_ID], [Agent_Name] FROM [View_WorkDate] where Agent_Team = @Agent_Team group by Agent_ID,Agent_Name";

        var result = DBTool.Query<AgentItem>(sqlstr, new { Agent_Team = Agent_Team });
        return result.Distinct().ToList();
    }

    public static ClassTemplate Agent_Working_0040010002_View(string time, string Agent_Team, string Agent_ID)
    {
        string sqlstr = @"SELECT [Working], [WorkOff] FROM [View_WorkDate] 
where CONVERT(varchar(10), Work_Day,111) = CONVERT(varchar(10), @time,111) 
AND Agent_Team = @Agent_Team 
AND Agent_ID = @Agent_ID";

        Convert.ToDateTime(time);
        var list = DBTool.Query<ClassTemplate>(sqlstr, new { time = time, Agent_Team = Agent_Team, Agent_ID = Agent_ID });
        if (list.Count() > 0)
            return list.First();
        else
            return null;
    }


    // ======================= 0040010002 =======================

    public class AgentItem
    {
        public string Agent_Phone;
        public string Agent_ID;
        public string Agent_Name;
        public string Cust_Name;
        public string Labor_ID;
        public string Labor_CName;
        public string Labor_Country;
    }
}