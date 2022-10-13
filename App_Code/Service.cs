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
public class Service
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public Service()
    {

    }

    //=========== 服務項目選單 ===========

    public List<string> ServiceNameList(string Service, bool isdistinct)
    {
        if (string.IsNullOrEmpty(Service)) ;
        string sqlstr = "select ServiceName from Data where Service = @Service AND Open_Flag != '0' AND Service !='外包商' ";
        var result = DBTool.Query<string>(sqlstr, new { Service = Service });

        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<ServiceItem> ServiceNameList_View(string Service, bool isdistinct)
    {
        if (string.IsNullOrEmpty(Service)) ;
        string sqlstr = "select ServiceName,Service_ID from Data where Service = @Service AND Open_Flag != '0' AND Service !='外包商' ";
        var result = DBTool.Query<ServiceItem>(sqlstr, new { Service = Service });

        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<string> DropService(bool isdistinct = true)
    {
        string sqlstr = @"SELECT [Service] FROM [Data] WHERE Open_Flag != '0'";
        var result = DBTool.Query<string>(sqlstr);
        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }
       
    //=========== 車輛服務選單 ===========

    public List<ServiceItem> CarNameList(string Agent_Name, string Agent_Team, string CarName, bool isdistinct)
    {
        string sqlstr = "select CarName, CarNumber, Agent_Name, Agent_Team from DataCar where Agent_Team = @Agent_Team AND Agent_Name=@Agent_Name  AND CarName=@CarName";
        var result = DBTool.Query<ServiceItem>(sqlstr, new { Agent_Name = Agent_Name, Agent_Team = Agent_Team, CarName = CarName });

        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<string> DropAgent_Name(bool isdistinct = true)
    {
        string sqlstr = @"SELECT [Agent_Name] FROM [DataCar]";
        var result = DBTool.Query<string>(sqlstr);
        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<string> CarAgent_Team(bool isdistinct = true)
    {
        string sqlstr = @"SELECT [Agent_Team] FROM [DataCar]";

        var result = DBTool.Query<string>(sqlstr);
        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<string> CarAgent_Name(string Agent_Team, bool isdistinct = true)
    {
        string sqlstr = @"SELECT [Agent_Name]  FROM [DataCar] where Agent_Team = @Agent_Team";

        var result = DBTool.Query<string>(sqlstr, new { Agent_Team = Agent_Team});
        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public List<string> CarName(string Agent_Name, bool isdistinct = true)
    {
        string sqlstr = @"SELECT [CarName]  FROM [DataCar] where Agent_Name = @Agent_Name";

        var result = DBTool.Query<string>(sqlstr, new { Agent_Name = Agent_Name });
        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    //=========== 醫療院所選單 ===========

    public List<string> DropHospitalName(bool isdistinct = true)
    {
        string sqlstr = @"SELECT [HospitalName] FROM [DataHospital]";
        var result = DBTool.Query<string>(sqlstr);
        if (isdistinct)
            result = result.Distinct();
        return result.ToList();
    }

    public class ServiceItem
    {
        public string Service;
        public string Service_ID;
        public string ServiceName;
        public string CarName;
        public string CarNumber;
        public string Agent_Name;
        public string Agent_Team;
    }
}