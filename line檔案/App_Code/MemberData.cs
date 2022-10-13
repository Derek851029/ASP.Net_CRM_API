using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// MemberData 的摘要描述
/// </summary>
public class MemberData
{
	public string MID { get; set; }
	public string MName { get; set; }
	public string MIdentity { get; set; }
	public string Sex { get; set; }
	public string Phone { get; set; }
	public string Email { get; set; }
	public string MAddress { get; set; }
	public string UserID { get; set; }
	public DateTime CreateDate { get; set; }
	public DateTime UpdateDate { get; set; }
	public string flag { get; set; }


	public MemberData()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
}