using Dapper;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class _0060010001 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Check();
        }
    }

    public static void Check()
    {
        
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string GetPartnerList()
    {
        Check();
        string sqlstr = @"SELECT * FROM ROLELIST WHERE Del_Flag='N'";
        var a = DBTool.Query<CMS_0060010000>(sqlstr).ToList().Select(p => new
        {
            ROLE_ID = p.ROLE_ID,
            ROLE_NAME = p.ROLE_NAME,
            UpDateUser = p.UpDateUser,
            UpDateDate = p.UpDateDate
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string List_PROGLIST(string ROLE_ID)
    {
        Check();
        string sqlstr = "";
        string outputJson = "";

        //============= 驗證 權限代碼有無被竄改 =============
        sqlstr = @"SELECT TOP 1 * FROM ROLELIST WHERE ROLE_ID=@ROLE_ID";
        var chk = DBTool.Query<CMS_0060010000>(sqlstr, new { ROLE_ID = ROLE_ID });
        if (!chk.Any())
        {
            outputJson = "[" + JsonConvert.SerializeObject(new { TREE_ID = "NULL" }) + "]"; // 組合JSON 格式
            return outputJson;
        }
        //============= 驗證 權限代碼有無被竄改 =============

        sqlstr = @"select case when c.TREE_Name is null then a.TREE_NAME else c.TREE_NAME end  as M_TREE_NAME,a.TREE_ID , a.TREE_NAME ," +
           " Case when b.Tree_ID is not null then '1' else '0' end as NowStatus ,d.Agent_Name , b.UpDateDate " +
           " FROM ( select * from PROGLIST where NAVIGATE_URL is not null and LTRIM(NAVIGATE_URL) <> '' and IS_OPEN='Y') as a " +
           " Left join ROLEPROG b on b.Role_ID = @Role_ID and a.TREE_ID = b.TREE_ID and IS_OPEN = 'Y' " +
           " Left join (select * from PROGLIST where (PARENT_ID is not null and LTRIM(PARENT_ID) <> '') " +
           " and (NAVIGATE_URL is  null or LTRIM(NAVIGATE_URL) = '') ) as c on a.PARENT_ID = c.TREE_ID " +
           " Left join DispatchSystem as d on d.Agent_ID = b.UpDateUser " +
           " order by c.sort_id , a.sort_id ";
        var a = DBTool.Query<CMS_0060010000>(sqlstr, new { ROLE_ID = ROLE_ID }).ToList().Select(p => new
        {
            TREE_ID = p.TREE_ID,
            M_TREE_NAME = p.M_TREE_NAME,
            TREE_NAME = p.TREE_NAME,
            Agent_Name = p.Agent_Name,
            UpDateDate = p.UpDateDate,
            NowStatus = p.NowStatus
        });
        outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Check_Menu(string Flag, string TREE_ID, string ROLE_ID)
    {
        Check();
        string Agent_ID = HttpContext.Current.Session["UserID"].ToString();
        if (Flag.Length > 1)
        {
            return JsonConvert.SerializeObject(new { status = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。" });
        }

        if (TREE_ID.Length > 10)
        {
            return JsonConvert.SerializeObject(new { status = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。" });
        }

        if (ROLE_ID.Length > 10)
        {
            return JsonConvert.SerializeObject(new { status = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。" });
        }
        string sqlstr = "";

        //============= 驗證 權限代碼有無被竄改 =============
        sqlstr = @"SELECT TOP 1 * FROM ROLELIST WHERE ROLE_ID=@ROLE_ID";
        var chk = DBTool.Query<CMS_0060010000>(sqlstr, new { ROLE_ID = ROLE_ID });
        if (!chk.Any())
        {
            return JsonConvert.SerializeObject(new { status = "無此系統參數，請再嘗試或詢問管理人員，謝謝。" });
        }
        //============= 驗證 權限代碼有無被竄改 =============

        //============= 驗證 選單編號有無被竄改 =============
        sqlstr = @"SELECT TOP 1 * FROM PROGLIST WHERE TREE_ID=@TREE_ID";
        chk = DBTool.Query<CMS_0060010000>(sqlstr, new { TREE_ID = TREE_ID });
        if (!chk.Any())
        {
            return JsonConvert.SerializeObject(new { status = "無此系統參數，請再嘗試或詢問管理人員，謝謝。" });
        }
        //============= 驗證 選單編號有無被竄改 =============

        if (Flag == "1")
        {
            sqlstr = @"DELETE FROM ROLEPROG WHERE Role_ID=@ROLE_ID AND TREE_ID=@TREE_ID";
            Flag = "系統選單關閉完成。";
        }
        else
        {
            sqlstr = @"INSERT INTO ROLEPROG ( Role_ID, TREE_ID, UpDateUser, UpDateDate ) VALUES(@ROLE_ID, @TREE_ID, @Agent_ID, @DateTime)";
            Flag = "系統選單開啟完成。";
        }

        try
        {
            using (IDbConnection conn = DBTool.GetConn())
            {
                conn.Execute(sqlstr, new { TREE_ID = TREE_ID, ROLE_ID = ROLE_ID, Agent_ID = Agent_ID, DateTime = DateTime.Now });
            }
            return JsonConvert.SerializeObject(new { status = Flag });
        }
        catch (Exception err)
        {
            return JsonConvert.SerializeObject(new { status = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。" });
        }
    }

    //============= 新增【使用者權限】=============
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string New(int Flag, string ROLE_ID, string ROLE_NAME)
    {
        Check();
        //=========================================================================

        if (ROLE_ID.Length < 1 || ROLE_ID.Length > 3)
        {
            return JsonConvert.SerializeObject(new { status = "【權限代碼】必須為大於１００或小於９９９的數字。" });
        }
        else
        {
            try
            {
                int i = Convert.ToInt32(ROLE_ID);
                if (Convert.ToInt32(i) > 999 || Convert.ToInt32(i) < 99)
                {
                    return JsonConvert.SerializeObject(new { status = "【權限代碼】必須為大於１００或小於９９９的數字。" });
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new { status = "【權限代碼】必須為大於１００或小於９９９的數字。" });
            }
        }

        if (ROLE_NAME.Length < 1 || ROLE_NAME.Length > 10)
        {
            return JsonConvert.SerializeObject(new { status = "【權限名稱】不能空白或超過１０個字元。" });
        }
        else
        {
            int i_1 = ROLE_NAME.IndexOf("<");
            int i_2 = ROLE_NAME.IndexOf(">");
            if (i_2 - i_1 > 1)
            {
                return JsonConvert.SerializeObject(new { status = "【權限名稱】含有不正確的關鍵字 ' < ' 與 ' > ' 。" });
            }
        }

        //=========================================================================

        string status;
        string Sqlstr = "";
        if (Flag == 0)
        {
            Sqlstr = @"SELECT TOP 1 ROLE_ID FROM ROLELIST WHERE ROLE_ID=@ROLE_ID OR ROLE_NAME=@ROLE_NAME";
            var a = DBTool.Query<ClassTemplate>(Sqlstr, new { ROLE_ID = ROLE_ID, ROLE_NAME = ROLE_NAME });
            if (a.Any())
            {
                return JsonConvert.SerializeObject(new { status = "已有相同的【權限代碼】或【權限名稱】。" });
            };

            Sqlstr = @"INSERT INTO ROLELIST (ROLE_ID, ROLE_NAME, UpDateUser, UpDateDate) " +
           " VALUES(@ROLE_ID, @ROLE_NAME, @UpDateUser, @UpDateDate)";
            status = "new";
        }
        else if (Flag == 1)
        {
            Sqlstr = @"SELECT TOP 1 ROLE_ID FROM ROLELIST WHERE ROLE_NAME=@ROLE_NAME";
            var a = DBTool.Query<ClassTemplate>(Sqlstr, new { ROLE_NAME = ROLE_NAME });
            if (a.Any())
            {
                return JsonConvert.SerializeObject(new { status = "已有相同的【權限名稱】。" });
            };

            Sqlstr = @"UPDATE ROLELIST SET ROLE_NAME=@ROLE_NAME, UpDateUser=@UpDateUser, " +
           "UpDateDate=@UpDateDate WHERE ROLE_ID=@ROLE_ID";
            status = "update";
        }
        else
        {
            return JsonConvert.SerializeObject(new { status = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。" });
        }

        try
        {
            var a = DBTool.Query<ClassTemplate>(Sqlstr, new
            {
                ROLE_ID = ROLE_ID,
                ROLE_NAME = ROLE_NAME,
                UpDateUser = HttpContext.Current.Session["UserIDNAME"].ToString(),
                UpDateDate = DateTime.Now
            });
        }
        catch
        {
            return JsonConvert.SerializeObject(new { status = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。" });
        }

        return JsonConvert.SerializeObject(new { status = status });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Load_ROLELIST(string ROLE_ID)
    {
        Check();
        string Sqlstr = @"SELECT ROLE_ID, ROLE_NAME FROM ROLELIST WHERE ROLE_ID = @ROLE_ID AND OPEN_DEL = 'Y' ";
        var a = DBTool.Query<ClassTemplate>(Sqlstr, new { ROLE_ID = ROLE_ID }).ToList().Select(p => new
        {
            ROLE_ID = p.Role_ID,
            ROLE_NAME = p.ROLE_NAME
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }
}
