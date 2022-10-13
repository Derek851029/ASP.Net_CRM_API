using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class DispatchSystemRepository
{
    public static string GetAgentPhone(string MASTER_ID)
    {
        string sqlstr = @"SELECT Agent_Phone_2 " +
                          "FROM DispatchSystem " +
                          "WHERE Agent_ID=@Agent_ID "+
                          "ORDER BY Agent_Status";

        var phone = DBTool.Query<AgentItem>(sqlstr, new { Agent_ID = MASTER_ID });
        if (phone.Any())
        {
            return phone.First().Agent_Phone_2;
        }
        else
            return string.Empty;
    }
}
public class AgentItem
{
    public string Agent_ID { get; set; }
    public string Agent_Name { get; set; }
    public string Agent_Status { get; set; }
    public string Agent_Company { get; set; }
    public string Agent_Team { get; set; }
    public string Agent_Mail { get; set; }
    public string Agent_Phone_2 { get; set; }
}