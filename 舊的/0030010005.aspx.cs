using Dapper;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using System.Threading;

public partial class _0030010005 : System.Web.UI.Page
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    string sql_txt;

    protected string opinionsubject = "";
    protected string opinioncontent_txt = "456";
    protected string test = "123";

    protected string seqno = "";
    //protected string str_title = "新增";
    protected string str_type = " 存檔後系統自動編號";
    protected string new_mno = "";

    //protected string opinionsubject = "意見主旨";
    //protected string opinioncontent_txt = "意見內容";

    protected void Page_Load(object sender, EventArgs e)            //判斷是否前往 10000/10003.aspx  或回到 10000/10002.aspx
    {
        if (!IsPostBack)
        {
            Check();
            seqno = Request.Params["seqno"];
            if (string.IsNullOrEmpty(seqno))
            {
                Response.Redirect("/Default.aspx");  //如果 seqno 沒有任何值 則回到 Default.aspx
            }
            else
                Session["SYSID"] = seqno;

            if (seqno != "0")
            {
                Session["OE_Case_ID"] = seqno;
                sql_txt = @"SELECT TOP 1 OE_Case_ID FROM OE_Data WHERE OE_Case_ID=@OE_Case_ID";          //  LaborTemplate > CaseData, MNo > Case_ID

                var chk = DBTool.Query<ClassTemplate>(sql_txt, new { OE_Case_ID = seqno });                 // MNo > Case_ID               001
                if (!chk.Any())
                {
                    //Response.Redirect("/0030010000/0030010004.aspx");
                    Response.Write("<script>alert('查無【" + seqno + "】OE單編號'); location.href='/0030010000/0030010004.aspx'; </script>");
                }
                else
                {
                    ClassTemplate schedule = ClassScheduleRepository._0030010001_View_Case_ID(seqno);       // To 730
                    string url = "/0030010000/0030010003.aspx?seqno=" + Request.Params["seqno"];
/*                    if (schedule.Type_Value == "1")
                    {
                        //str_title = "修改";
                        str_type = "尚未審核";
                        opinionsubject = @"SELECT TOP 1 OpinionSubject as OS FROM CaseData WHERE Case_ID=@Case_ID";     //意見主旨內容

                        if (Session["Agent_LV"].ToString() != "30")         // 權限 = 30 ?
                        {
                            if (Session["Agent_Team"].ToString() != schedule.Type_Value)
                            {
                                Response.Write("<script>alert('您沒有訪問此頁面的權限，此需求單非您部門的需求單。'); location.href='/0030010000/0030010002.aspx'; </script>");
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect(url);
                    }   //*/
                };
            }
            else
            {
                //【創造母單編號】
                new_mno = DateTime.Now.ToString("yyMMddHHmmssfff");    //   ("yyMMddHHmmssfff");
                Session["Case_ID"] = new_mno; // 增單時自動產生的編號 要F5 才會有新編號
            }
        }
    }

    //============= 帶入【填單人】資訊 =============
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Load() 
    {
        Check();
        string Agent_ID = HttpContext.Current.Session["UserID"].ToString();
        string Sqlstr = @"SELECT TOP 1 * FROM DispatchSystem WHERE Agent_ID = @Agent_ID AND Agent_Status != '離職'";      // 經銷商名單內且未離職
        var a = DBTool.Query<ClassTemplate>(Sqlstr, new { Agent_ID = Agent_ID }).ToList().Select(p => new
        {
            Agent_Company = p.Agent_Company,                      // 所屬部門
            Agent_Name = p.Agent_Name,                     // 人名
            Agent_Phone_2 = p.Agent_Phone_2,            // 個人電話
            //Agent_Phone_3 = p.Agent_Phone_3,            // 分機
            Agent_Co_TEL = p.Agent_Co_TEL,                  // 公司電話+分機
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Dealer()                   // 出現在 21, 208 行
    {
        //Check();
        string Sqlstr = @"SELECT *  FROM Dealer " ;      //整理出可選擇的客戶列表

        var a = DBTool.Query<T_0030010005_2>(Sqlstr).ToList().Select(p => new
        {
            A = p.D_Name.Trim(),
            B = p.ID,
            C = p.PID
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Client_Code_Search()                   // 出現在 21, 208 行
    {
        //Check();
        string Sqlstr = @"SELECT PID,BUSINESSNAME,ID  FROM BusinessData where Type = '保留' " + //group by PID,BUSINESSNAME,ID
            "  order by PID ";      //整理出可選擇的客戶列表

        var a = DBTool.Query<ClassTemplate>(Sqlstr).ToList().Select(p => new
        {
            A = p.BUSINESSNAME,
            B = p.ID,
            C = p.PID
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Client_Code_Search2(string PID)                   // 缺個檢查 無子公司
    {
        //Check();
        PID = PID.Trim();
        string Sqlstr = @"SELECT PNumber, Name  FROM Business_Sub where PID = @PID ";      //整理出子公司列表 '108'

        var a = DBTool.Query<ClassTemplate>(Sqlstr, new { PID = PID }).ToList().Select(p => new
        {
            A = p.Name,
            B = p.PNumber
            //C = p.PID
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]       //OE_Main() 主分類選項
    public static string OE_Main()
    {
        //Check();
        string sqlstr = @"SELECT DISTINCT Main_Classified FROM OE_Product ";
        var a = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new
        {
            Main_Classified = p.Main_Classified
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]       //OE_Main() 主分類選項
    public static string OE_Detail(string Main)
    {
        //Check();
        string sqlstr ="";
        if (Main == "all")
            sqlstr = @"SELECT DISTINCT Detail_Classified FROM OE_Product";
        else
            sqlstr = @"SELECT DISTINCT Detail_Classified FROM OE_Product where Main_Classified = @Main ";
        var a = DBTool.Query<ClassTemplate>(sqlstr, new { Main = Main }).ToList().Select(p => new
        {
            Detail_Classified = p.Detail_Classified
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string OE_List(string PID)                                                       // 新增 讀BusinessData 案件資料
    {
        //Check(); 
        string Agent_Team = HttpContext.Current.Session["Agent_Team"].ToString();
        string Agent_Company1 = HttpContext.Current.Session["Agent_Company"].ToString();
        string sqlstr = "";
        PID = PID.Trim();
        if (Agent_Company1 == "Pre-sale" || Agent_Company1 == "Operation" )
        {
            sqlstr = @"SELECT * FROM OE_Data  WHERE PID=@PID";
        }    //string outputJson = "";
        else if (Agent_Company1 == "Engineer" && Agent_Team == "Leader")
            sqlstr = @"SELECT * FROM OE_Data  WHERE PID=@PID and Con_Company in( 'Engineer', 'MA_Sales')";
        else
            sqlstr = @"SELECT * FROM OE_Data  WHERE PID=@PID and Con_Company = @Agent_Company";

        var lcd = DBTool.Query<T_0030010005_2>(sqlstr, new { PID = PID, Agent_Company = Agent_Company1 }).ToList().Select(p => new
            {
                OE_Case_ID = Value(p.OE_Case_ID),
                BUSINESSNAME = Value(p.BUSINESSNAME),
                Name = Value(p.Name),
                Warranty_Date = Test(Value2(p.Warranty_Date)),
                Warr_Time = Value(p.Warr_Time),

            });
        string outputJson = JsonConvert.SerializeObject(lcd);
        return outputJson;  //*/

        /*string sqlstr2 = @"SELECT * FROM OE_Data  WHERE PID=@PID  ";   //and Con_Company = @Agent_Company2
        //string outputJson = "";
        var lcd2 = DBTool.Query<T_0030010005_2>(sqlstr2, new { PID = PID,Agent_Company2= Agent_Company1 }).ToList().Select(p => new
        {
            OE_Case_ID = Value(p.OE_Case_ID),
            BUSINESSNAME = Value(p.BUSINESSNAME),
            Name = Value(p.Name),
            Warranty_Date = Test(Value2(p.Warranty_Date)),
            Warr_Time = Value(p.Warr_Time),
        });
        string outputJson2 = JsonConvert.SerializeObject(lcd2);
        return outputJson2; //*/
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string URL(string mno)
    {
        //Check();
        mno = mno.Trim();
        string error = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。";
        if (JASON.IsInt(mno) != true)
        {
            return JsonConvert.SerializeObject(new { status = error + "_1" });
        }

        if (mno.Length > 16 || mno.Length < 1)
        {
            return JsonConvert.SerializeObject(new { status = error + "_2" });
        }

        if (mno != "0")
        {
            string sqlstr = @"SELECT TOP 1 OE_Case_ID FROM [DimaxCallcenter].[dbo].[OE_Data] WHERE OE_Case_ID=@Case_ID ";
            var a = DBTool.Query<ClassTemplate>(sqlstr, new { Case_ID = mno });

            if (!a.Any())
            {
                return JsonConvert.SerializeObject(new { status = "查無【" + mno + "】需求單編號。", type = "no" });
            };
        };  //*/

        string str_url = "../0030010015.aspx?seqno=" + mno;         //打開10099 並放入同Case_ID號的資料
        return JsonConvert.SerializeObject(new { status = str_url, type = "ok" });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string URL4(string mno)
    {
        //Check();
        mno = mno.Trim();
        string error = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。";
        if (JASON.IsInt(mno) != true)
        {
            return JsonConvert.SerializeObject(new { status = error + "_1  " + mno });
        }

        if (mno.Length > 16 || mno.Length < 1)
        {
            return JsonConvert.SerializeObject(new { status = error + "_2 序號長度錯誤" });
        }

        if (mno != "0")
        {
            string sqlstr = @"SELECT TOP 1 OE_Case_ID FROM [DimaxCallcenter].[dbo].[OE_Data] WHERE OE_Case_ID=@Case_ID ";
            var a = DBTool.Query<ClassTemplate>(sqlstr, new { Case_ID = mno });

            if (!a.Any())
            {
                return JsonConvert.SerializeObject(new { status = "查無【" + mno + "】需求單編號。", type = "no" });
            };
        };  //*/

        //string str_url = "http://118.163.27.22:5888/OE_File/" + mno + (char)92;         // 極緻用
        string str_url = "http://123.51.218.13:5888/OE_File/" + mno + (char)92;         // 德瑪用
        return JsonConvert.SerializeObject(new { status = str_url, type = "ok" });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]       //OE商品列表
    public static string GetOEList(string Detail)       //案件列表程式
    {
        string sqlstr2 = "";
        if (Detail =="all")
            sqlstr2 = @" select * From [DimaxCallcenter].[dbo].[OE_Product] ";
        else
            sqlstr2 = @" select * From [DimaxCallcenter].[dbo].[OE_Product] where Detail_Classified = @Detail ";
        var a = DBTool.Query<T_0030010005_2>(sqlstr2, new
        {
            Detail = Detail
        });
        var b = a.ToList().Select(p => new
        {
            ID = p.OE_ID,
            Product_Name = p.Product_Name,
            Main_Classified = p.Main_Classified,
            Detail_Classified = p.Detail_Classified,
            Price = p.Unit_Price,
        });
        return JsonConvert.SerializeObject(b);
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Delete(string OE_O_ID)
    {
        //Check();
        OE_O_ID = OE_O_ID.Trim();
        //string sqlstr = @"delete FROM OE_Order where OE_O_ID = @OE_O_ID ";
        string sqlstr = @"Update OE_Order set Delete_Date = getdate() WHERE OE_O_ID = @OE_O_ID";
        DBTool.Query<T_0030010005_2>(sqlstr, new { OE_O_ID = OE_O_ID });  //, UpdateTime = DateTime.Now
        return JsonConvert.SerializeObject(new { status = "success" });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]       //OE商品列表
    public static string GetOEOrder(string OE_Case_ID)       //案件列表程式
    {
        OE_Case_ID = OE_Case_ID.Trim();
        string sqlstr2 = @" select * From OE_Order where OE_Case_ID = @OE_Case_ID and Delete_Date is null";
        var a = DBTool.Query<T_0030010005_2>(sqlstr2, new
        {
            OE_Case_ID = OE_Case_ID
        });
        var b = a.ToList().Select(p => new
        {
            OE_O_ID = p.OE_O_ID,
            OE_ID = p.OE_ID,
            OE_Case_ID = p.OE_Case_ID,
            Product_Name = p.Product_Name,
            Quantity = p.Quantity,
            Price = p.Unit_Price,
            T_Price = p.T_Price,
            //Main_Classified = p.Main_Classified,
            //Detail_Classified = p.Detail_Classified,
        });
        return JsonConvert.SerializeObject(b);
    }
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]       //OE_Main() 主分類選項
    public static string OE_Total_Price(string OE_Case_ID)
    {
        //Check();
        OE_Case_ID = OE_Case_ID.Trim();
        string sqlstr = @"SELECT SUM([Unit_Price]*[Quantity]) as Total_Price FROM OE_Order where OE_Case_ID = @OE_Case_ID ";
        var a = DBTool.Query<T_0030010005_2>(sqlstr, new { OE_Case_ID = OE_Case_ID }).ToList().Select(p => new
        {
            Total_Price = p.Total_Price
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

/*    [WebMethod(EnableSession = true)]//或[WebMethod(true)]           訂貨儲存
    public static int ALL(string ID)
    {
        string sqlstr = "";
        int i = 0;
        sqlstr = "select [Unit_Price]*[Quantity] as a FROM [DimaxCallcenter].[dbo].[OE_Order]  where [OE_Case_ID] = '170630152407413'";
        var Team_Array = DBTool.Query<T_0030010005_2>(sqlstr, new { OE_Case_ID = ID });
        if (Team_Array.Count() > 0)
        {
            foreach (var var in Team_Array)
            {
                i += var.a;
            }
        }
        return i;
    }   //*/

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]           訂貨儲存
    public static string Order(string OE_Case_ID, string ID, string Main, string Price,
        string Product_Name, string Main_Classified, string Detail_Classified, string T_Price)
    {        
        ID = ID.Trim();
        string status;
        string Sqlstr = "";
 //       string error = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。";
 //       if (JASON.IsInt(ID) != true)
 //       {
 //           return JsonConvert.SerializeObject(new { status = error + "_1" });
 //       }
        Sqlstr = @"INSERT INTO OE_Order (OE_Case_ID, OE_ID, Unit_Price, Quantity, Product_Name, Main_Classified, Detail_Classified, T_Price ) " +
                " VALUES (@OE_Case_ID, @OE_ID, @Unit_Price, @Quantity, @Product_Name, @Main_Classified, @Detail_Classified, @T_Price ) ";
        //共50個    
        status = "new";

        var a = DBTool.Query<T_0030010005_2>(Sqlstr, new
        {
            OE_Case_ID = OE_Case_ID,
            OE_ID = ID,
            Quantity = Main,
            Unit_Price = Price,
            Product_Name = Product_Name,
            Main_Classified = Main_Classified,
            Detail_Classified = Detail_Classified,
            T_Price = T_Price,
        });
        return JsonConvert.SerializeObject(new { status = status });
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Show_Client_Data(string value)                     // 叫出商業資料  234行
    {
        Check();
        string Sqlstr = @"SELECT * FROM BusinessData WHERE PID=@value ";                                                                //  ID=@value ?
        var a = DBTool.Query<T_0030010005_2>(Sqlstr, new { value = value }).ToList().Select(p => new
        {
            A = p.PID.Trim(),
            B = p.ID.Trim(),
            C = p.BUSINESSNAME.Trim(),         // 公司名
            D = p.APPNAME.Trim(),                  // 聯絡人
            E = p.CONTACT_ADDR.Trim(),
            F = p.APP_FTEL.Trim(),
            G = p.APP_MTEL.Trim(),                 // 手機
            H = p.HardWare.Trim(),      // 硬體
            I = p.SoftwareLoad.Trim()
            //J = p.SERVICEITEM,              // 合約
            //E = p.APP_OTEL_AREA,
            //F = p.APP_OTEL,                  // 電話
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Show_Client_Data2(string value_2)                     // 叫出商業資料  234行
    {
        Check();
        string Sqlstr = @"SELECT * FROM Business_Sub WHERE PNumber=@value_2 ";                                                                //  ID=@value ?
        var a = DBTool.Query<T_0030010005_2>(Sqlstr, new { value_2 = value_2 }).ToList().Select(p => new
        {
            B = p.ID,
            C = p.Name,         // 公司名
            D = p.APPNAME,                  // 聯絡人
            E = p.Contact_ADDR,
            F = p.APP_FTEL,
            G = p.APP_MTEL,                 // 手機
            H = p.HardWare.Trim(),      // 硬體
            I = p.SoftwareLoad
            //J = p.SERVICEITEM,              // 合約
            //E = p.APP_OTEL_AREA,
            //F = p.APP_OTEL,                  // 電話
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Select_Telecomm()          //服務電信清單
    {
        Check();
        string Sqlstr = @"SELECT DISTINCT Telecomm_Name,Telecomm_ID FROM Telecomm ";            // Telecomm 目前空的 只有測試資料
        var b = DBTool.Query<ClassTemplate>(Sqlstr).ToList().Select(p => new
        {
            A = p.Telecomm_Name,
            B = p.Telecomm_ID
        });
        string outputJson = JsonConvert.SerializeObject(b);
        return outputJson;
    }
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]

    public static string Case_Save(string sysid, string OE_PS, string Con_Name, string Con_Company, string Con_Phone, //string SetupDate,
        string UpdateDate, string Type_Value, string Type, string Dealer, string PID, string BUSINESSNAME, 
        string APPNAME, string CONTACT_ADDR, string APP_FTEL, string APP_MTEL, string HardWare,
        string SoftwareLoad,
        //string PNumber, string Name, string S_APPNAME, string S_CONTACT_ADDR,   //子公司
        //string S_APP_FTEL, string S_APP_MTEL, string S_HardWare, string S_SoftwareLoad, //子公司
        string Final_Price, string Warranty_Date, string Warr_Time, string Protect_Date, string Prot_Time,
        string Receipt_Date, string Receipt_PS, string Close_Out_Date, string Close_Out_PS)
    // 新單子 數據登錄
    {   
        //string SetupDate = UpdateDate;
        //DateTime time_02 = DateTime.Parse(esti_fin_time);
        //DateTime time = DateTime.Parse(login_time);
        string Sqlstr;
        Sqlstr = @"SELECT TOP 1 OE_Case_ID FROM OE_Data WHERE OE_Case_ID=@OE_Case_ID";
        var a = DBTool.Query<T_0030010005_2>(Sqlstr, new
        {
            OE_Case_ID = sysid,
        });
        if (a.Any())
        {
            return JsonConvert.SerializeObject(new { status = "已建立同單號OE單。" });
        };

        Sqlstr = @"INSERT INTO OE_Data (OE_Case_ID, OE_PS, Con_Name, Con_Company, Con_Phone,  " + //UpdateDate,
           "  SetupDate, Type_Value, Type, Dealer, PID, BUSINESSNAME, " +
           " APPNAME, CONTACT_ADDR, APP_FTEL, APP_MTEL, HardWare, " +
           "  SoftwareLoad, " +   //PNumber, Name, S_APPNAME, S_CONTACT_ADDR, 
           "   " +//S_APP_FTEL, S_APP_MTEL, S_HardWare, S_SoftwareLoad, 
           " Final_Price, Warr_Time, Prot_Time,  " +
           " Receipt_PS, Close_Out_PS ) " +

           " VALUES(@OE_Case_ID, @OE_PS, @Con_Name, @Con_Company, @Con_Phone, " +    //@UpdateDate, 
           "  @SetupDate, @Type_Value, @Type, @Dealer, @PID, @BUSINESSNAME, " +
            " @APPNAME, @CONTACT_ADDR, @APP_FTEL, @APP_MTEL, @HardWare, " +
           "  @SoftwareLoad, " +  //@PNumber, @Name, @S_APPNAME, @S_CONTACT_ADDR,   //子公司
           "  " +   //@S_APP_FTEL, @S_APP_MTEL, @S_HardWare, @S_SoftwareLoad,   //子公司
           " @Final_Price, @Warr_Time, @Prot_Time, " +
           " @Receipt_PS, @Close_Out_PS ) ";
        // 增加資料 依前後排序一一寫入CaseData中
        string status = "登單完成";
        a = DBTool.Query<T_0030010005_2>(Sqlstr, new
        {
            OE_Case_ID = sysid,
            OE_PS = OE_PS,
            Con_Name = Con_Name,
            Con_Company = Con_Company,
            Con_Phone = Con_Phone,
            SetupDate = UpdateDate,
            //UpdateDate = UpdateDate,
            Type_Value = Type_Value,
            Type = Type,
            Dealer = Dealer,

            PID = PID,
            BUSINESSNAME = BUSINESSNAME,
            APPNAME = APPNAME,
            CONTACT_ADDR = CONTACT_ADDR,
            APP_FTEL = APP_FTEL,
            APP_MTEL = APP_MTEL,
            HardWare = HardWare,
            SoftwareLoad = SoftwareLoad,

            /*PNumber = PNumber,    //子公司
            Name = Name,
            S_APPNAME = S_APPNAME,
            S_CONTACT_ADDR = S_CONTACT_ADDR,
            S_APP_FTEL = S_APP_FTEL,
            S_APP_MTEL = S_APP_MTEL,
            S_HardWare = S_HardWare,
            S_SoftwareLoad = S_SoftwareLoad,    //*/

            Final_Price = Final_Price,
            Warranty_Date = Warranty_Date,
            Warr_Time = Warr_Time,
            Protect_Date = Protect_Date,
            Prot_Time = Prot_Time,
            Receipt_Date = Receipt_Date,
            Receipt_PS = Receipt_PS,
            Close_Out_Date = Close_Out_Date,
            Close_Out_PS = Close_Out_PS
        });

        if (!string.IsNullOrEmpty(Warranty_Date))   // 當 Warranty_Date 非null或Empty時 update
        {
            Sqlstr = @"UPDATE OE_Data SET Warranty_Date = @Warranty_Date " +
                "where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new
            {
                OE_Case_ID = sysid,
                Warranty_Date = Test(Warranty_Date),
            });
        }
        else    //將 Warranty_Date 改成 null
        {
            Sqlstr = @"UPDATE OE_Data SET Warranty_Date = null where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new { OE_Case_ID = sysid });
        }
        if (!string.IsNullOrEmpty(Protect_Date))   // 當值為null時跳過  非 null 時 update
        {
            Sqlstr = @"UPDATE OE_Data SET Protect_Date = @Protect_Date " +
                "where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new
            {
                OE_Case_ID = sysid,
                Protect_Date = Test(Protect_Date),
            });
        }
        else
        {
            Sqlstr = @"UPDATE OE_Data SET Protect_Date = null where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new { OE_Case_ID = sysid });
        }
        if (!string.IsNullOrEmpty(Close_Out_Date))   // 當值為null時跳過  非 null 時 update
        {
            Sqlstr = @"UPDATE OE_Data SET Close_Out_Date = @Close_Out_Date " +
                "where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new
            {
                OE_Case_ID = sysid,
                Close_Out_Date = Test(Close_Out_Date),
            });
        }
        else
        {
            Sqlstr = @"UPDATE OE_Data SET Close_Out_Date = null where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new { OE_Case_ID = sysid });
        }
        if (!string.IsNullOrEmpty(Receipt_Date))   // 當值為null時跳過  非 null 時 update
        {
            Sqlstr = @"UPDATE OE_Data SET Receipt_Date = @Receipt_Date " +
                "where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new
            {
                OE_Case_ID = sysid,
                Receipt_Date = Test(Receipt_Date),
            });
        }
        else
        {
            Sqlstr = @"UPDATE OE_Data SET Receipt_Date = null where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new { OE_Case_ID = sysid });
        }


        string AgentName = HttpContext.Current.Session["UserIDNAME"].ToString(); //部門
        string Agent_Company = HttpContext.Current.Session["Agent_Company"].ToString(); //部門
        //string Agent_Team = HttpContext.Current.Session["Agent_Team"].ToString();   //職位

        string URL = System.Web.Configuration.WebConfigurationManager.AppSettings["EMMA"].ToString();
        string EMMA = URL + "CheckLogin.aspx?seqno=" + sysid + "&page=1&login=";
        //page 1  OE （審查） by CheckLogin
        string sqlstr = "";

        if (Agent_Company == "Sales 1")
            sqlstr = @"SELECT Top 1 Agent_Name, Agent_Mail, UserID FROM DispatchSystem where Agent_Status != '離職' and Agent_Company ='Sales 1' and Agent_Team ='Leader'  ";
        else if (Agent_Company == "Sales 2")
            sqlstr = @"SELECT Top 1 Agent_Name, Agent_Mail, UserID FROM DispatchSystem where Agent_Status != '離職' and Agent_Company ='Sales 2' and Agent_Team ='Leader'  ";
        var list = DBTool.Query<ClassTemplate>(sqlstr );
        if (list.Count() > 0)
        {
            string sql = @"INSERT INTO [dbo].[tblAssign] " +
                "( [AssignNo], [E_MAIL], [ConnURL] ) " +
                "VALUES ( @AssignNo, @E_MAIL, @ConnURL )";
            foreach (var q in list)
            {
                ClassTemplate emma = new ClassTemplate()
                {
                    AssignNo = "【OE審核】【" + BUSINESSNAME + "】",   // + q.Agent_Name 
                    ConnURL = EMMA + JASON.Encryption(q.UserID),
                    //ConnURL = EMMA + JASON.Encryption("admin"),   //測試用
                    E_MAIL = q.Agent_Mail
                };
                using (IDbConnection db = DBTool.GetConn())
                    db.Execute(sql, emma);
            }
        }   //Sales 1/2 存簡訊

        return JsonConvert.SerializeObject(new { status = status });
    }
    //============修改
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]

    public static string Case_Save2(string Flag, string sysid, string OE_PS, string Con_Name, string Con_Company, string Con_Phone, //string SetupDate,
        string UpdateDate, string Type_Value, string Type, string Dealer, string PID, string BUSINESSNAME,
        string APPNAME, string CONTACT_ADDR, string APP_FTEL, string APP_MTEL, string HardWare,
        string SoftwareLoad,
        //string PNumber, string Name, string S_APPNAME, string S_CONTACT_ADDR,   //子公司
        //string S_APP_FTEL, string S_APP_MTEL, string S_HardWare, string S_SoftwareLoad,
        string Final_Price, string Warranty_Date, string Warr_Time, string Protect_Date, string Prot_Time,
        string Receipt_Date, string Receipt_PS, string Close_Out_Date, string Close_Out_PS)
    // 新單子 數據登錄
    {
        string status = "update";
        if (Flag == "1")
        {
            Type = "已經結案";
            status = "closed";
        }
        //DateTime time = DateTime.Parse(login_time);
        string Sqlstr = @"UPDATE OE_Data SET " +
            "OE_PS = @OE_PS, " +
            "Con_Name = @Con_Name, " +
            "Con_Phone = @Con_Phone, " +
            "UpdateDate = @UpdateDate, " +
            "Type_Value = @Type_Value, " + //填單部門
            "Type = @Type, " +
            "Dealer = @Dealer, " +

            "PID = @PID, " +
            "BUSINESSNAME = @BUSINESSNAME, " +
            "APPNAME = @APPNAME, " +
            "CONTACT_ADDR = @CONTACT_ADDR, " +
            "APP_FTEL = @APP_FTEL, " +
            "APP_MTEL = @APP_MTEL, " +
            "HardWare = @HardWare, " +
            "SoftwareLoad = @SoftwareLoad, " +

            /*"PNumber = @PNumber, " +  //子公司
            "Name = @Name, " +
            "S_APPNAME = @S_APPNAME, " +
            "S_CONTACT_ADDR = @S_CONTACT_ADDR, " +
            "S_APP_FTEL = @S_APP_FTEL, " +
            "S_APP_MTEL = @S_APP_MTEL, " +
            "S_HardWare = @S_HardWare, " +
            "S_SoftwareLoad = @S_SoftwareLoad, " +  //*/

            "Final_Price = @Final_Price, " +
            //"Warranty_Date = @Warranty_Date, " +
            "Warr_Time = @Warr_Time, " +
            //"Protect_Date = @Protect_Date, " +
            "Prot_Time = @Prot_Time, " +
            //"Receipt_Date = @Receipt_Date, " +
            "Receipt_PS = @Receipt_PS, " +
            //"Close_Out_Date = @Close_Out_Date, " +
            "Close_Out_PS = @Close_Out_PS " +   //*/

            "WHERE OE_Case_ID = @OE_Case_ID";
        //status = "update";
        var a = DBTool.Query<T_0030010005_2>(Sqlstr, new
        {
            OE_Case_ID = sysid,
            OE_PS = OE_PS,
            Con_Name = Con_Name,
            Con_Phone = Con_Phone,
            //SetupDate = SetupDate,
            UpdateDate = UpdateDate,
            Type_Value = Type_Value,    //填單部門
            Type = Type,
            Dealer = Dealer, 

            PID = PID,
            BUSINESSNAME = BUSINESSNAME,
            APPNAME = APPNAME,
            CONTACT_ADDR = CONTACT_ADDR,
            APP_FTEL = APP_FTEL,
            APP_MTEL = APP_MTEL,
            HardWare = HardWare,
            SoftwareLoad = SoftwareLoad,

            /*PNumber = PNumber,  //子公司
            Name = Name,
            S_APPNAME = S_APPNAME,
            S_CONTACT_ADDR = S_CONTACT_ADDR,
            S_APP_FTEL = S_APP_FTEL,
            S_APP_MTEL = S_APP_MTEL,
            S_HardWare = S_HardWare,
            S_SoftwareLoad = S_SoftwareLoad,    //*/

            Final_Price = Final_Price,
            Warranty_Date = Warranty_Date,
            Warr_Time = Warr_Time,
            Protect_Date = Protect_Date,
            Prot_Time = Prot_Time,
            Receipt_Date = Receipt_Date,
            Receipt_PS = Receipt_PS,
            Close_Out_Date = Close_Out_Date,
            Close_Out_PS = Close_Out_PS
        });
        if (!string.IsNullOrEmpty(Warranty_Date))   // 當值為null時跳過  非 null 時 update
        {
            Sqlstr = @"UPDATE OE_Data SET Warranty_Date = @Warranty_Date " +
                "where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new
            {
                OE_Case_ID = sysid,
                Warranty_Date = Test(Warranty_Date),
            });
        }
        else
        {
            Sqlstr = @"UPDATE OE_Data SET Warranty_Date = null where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new { OE_Case_ID = sysid });
        }
        if (!string.IsNullOrEmpty(Protect_Date))   // 當值為null時跳過  非 null 時 update
        {
            Sqlstr = @"UPDATE OE_Data SET Protect_Date = @Protect_Date " +
                "where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new
            {
                OE_Case_ID = sysid,
                Protect_Date = Test(Protect_Date),
            });
        }
        else
        {
            Sqlstr = @"UPDATE OE_Data SET Protect_Date = null where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new { OE_Case_ID = sysid });
        }
        if (!string.IsNullOrEmpty(Close_Out_Date))   // 當值為null時跳過  非 null 時 update
        {
            Sqlstr = @"UPDATE OE_Data SET Close_Out_Date = @Close_Out_Date " +
                "where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new
            {
                OE_Case_ID = sysid,
                Close_Out_Date = Test(Close_Out_Date),
            });
        }
        else
        {
            Sqlstr = @"UPDATE OE_Data SET Close_Out_Date = null where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new { OE_Case_ID = sysid });
        }
        if (!string.IsNullOrEmpty(Receipt_Date))   // 當值為null時跳過  非 null 時 update
        {
            Sqlstr = @"UPDATE OE_Data SET Receipt_Date = @Receipt_Date " +
                "where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new
            {
                OE_Case_ID = sysid,
                Receipt_Date = Test(Receipt_Date),
            });
        }
        else
        {
            Sqlstr = @"UPDATE OE_Data SET Receipt_Date = null where OE_Case_ID = @OE_Case_ID ";
            a = DBTool.Query<T_0030010005_2>(Sqlstr, new { OE_Case_ID = sysid });
        }
                
        return JsonConvert.SerializeObject(new { status = status });
    }

    //============= 帶入【服務分類】資訊 =============
    /*    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
        public static string Service_List()             // 顯示服務類別
        {
            Check();
            string Sqlstr = @"SELECT DISTINCT Service FROM Data WHERE Open_Flag = '1' AND Service !='外包商' ";        //舊的  無用
            var a = DBTool.Query<ClassTemplate>(Sqlstr).ToList().Select(p => new
            {
                Service = p.Service
            });
            string outputJson = JsonConvert.SerializeObject(a);
            return outputJson;
        }   //*/

    //============= 帶入【服務分類】資訊 =============
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Owner_List()           // 從勞工系統找資料
    {
        Check();
        string Sqlstr = @"SELECT DISTINCT Cust_ID, Cust_FullName FROM Labor_System WHERE Cust_ID != '' AND Labor_Type = '任用' ORDER BY Cust_FullName";
        // 舊的  無用
        var a = DBTool.Query<ClassTemplate>(Sqlstr).ToList().Select(p => new
        {
            Cust_ID = p.Cust_ID,
            Cust_FullName = p.Cust_FullName
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    //============= 帶入【醫療院所】資訊 =============            無用
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Hospital_List()
    {
        Check();
        string sqlstr = @"SELECT HospitalName FROM DataHospital WHERE Flag = '1'";                   // 舊的 
        var a = DBTool.Query<ClassTemplate>(sqlstr).ToList().Select(p => new
        {
            HospitalName = p.HospitalName
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    //============= 帶入【服務內容】資訊 =============            舊的  列出非外包的服務處清單  無用
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string ServiceName_List(string service)
    {
        Check();
        string Sqlstr = @"SELECT Service_ID, ServiceName FROM Data WHERE Open_Flag = '1' AND Service = @Service AND Service !='外包商' ";           // 舊的 待改?
        var a = DBTool.Query<ClassTemplate>(Sqlstr, new { Service = service }).ToList().Select(p => new
        {
            ServiceName = p.ServiceName,
            Service_ID = p.Service_ID
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string GetCNoList()                                                       // 舊的 勞工相關  無用
    {
        Check();
        string MNo = HttpContext.Current.Session["MNo"].ToString();
        var a = PartnerHeaderRepository.CMS_0030010001_MNo_Labor(MNo)
            .Select(p => new
            {
                SYS_ID = p.SYS_ID,
                Cust_FullName = p.Cust_FullName,//客戶全名
                Labor_CName = p.Labor_CName,//外勞中文姓名
                Labor_ID = p.Labor_ID,//外勞編號               
                Labor_PID = p.Labor_PID,//護照號碼        
                Labor_RID = p.Labor_RID,//居留證號
                Labor_EID = p.Labor_EID,//職工編號
                Labor_Phone = p.Labor_Phone,//手機號碼
                Labor_Country = p.Labor_Country,//國籍
                Labor_Valid = p.Labor_Valid//國籍
            });
        return JsonConvert.SerializeObject(a, Formatting.Indented);
    }

    //=============== 帶入【已選擇的雇主或外勞（瀏覽）】資訊 ===============
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Btn_Add_Click(string Cust_ID, string Labor_ID)             // 勞工系統 無用
    {
        Check();
        string sql_txt = "";
        string add_txt = "";
        string back = "勞工或雇主只能擇一選擇";
        string MNo = HttpContext.Current.Session["MNo"].ToString();
        if (!string.IsNullOrEmpty(Cust_ID))
        {
            if (!string.IsNullOrEmpty(Labor_ID))
            {
                sql_txt = @"SELECT TOP 1 SYS_ID FROM MNo_Labor WHERE MNo=@MNo AND Labor_ID='' AND Cust_ID=@Cust_ID";             // 舊的 待改?
                var Labor_chk = DBTool.Query<String>(sql_txt, new { MNo = MNo, Labor_ID = Labor_ID, Cust_ID = Cust_ID });
                if (Labor_chk.Any())
                {
                    return JsonConvert.SerializeObject(new { status = back });
                }
                else
                {
                    sql_txt = @"SELECT TOP 1 SYS_ID FROM MNo_Labor WHERE MNo=@MNo AND Labor_ID=@Labor_ID";
                    add_txt = @"INSERT INTO MNo_Labor " +
                   "(" +
                   "[MNo], [Cust_ID], [Cust_Name], [Cust_FullName], [Labor_ID], [Labor_CName], [Labor_EName], [Labor_Country], " +
                   "[Labor_PID], [Labor_RID], [Labor_EID], [Labor_Phone], [Labor_Team], [Labor_Company], [Labor_Address], [Labor_Address2], [Labor_Valid] " +
                   ") " +
                   " SELECT TOP 1 " +
                   "@MNO, [Cust_ID], [Cust_Name], [Cust_FullName], [Labor_ID], [Labor_CName], [Labor_EName], [Labor_Country], " +
                   "[Labor_PID], [Labor_RID], [Labor_EID], [Labor_Phone], [Labor_Team], [Labor_Company], [Labor_Address], [Labor_Address2], [Labor_Valid] " +
                   "FROM Labor_System WHERE Cust_ID=@Cust_ID AND Labor_ID=@Labor_ID ";
                }
            }
            else
            {
                sql_txt = @"SELECT TOP 1 SYS_ID FROM MNo_Labor WHERE MNo=@MNo AND Labor_ID != '' AND Cust_ID=@Cust_ID";
                var Labor_chk = DBTool.Query<String>(sql_txt, new { MNo = MNo, Labor_ID = Labor_ID, Cust_ID = Cust_ID });
                if (Labor_chk.Any())
                {
                    return JsonConvert.SerializeObject(new { status = back });
                }
                else
                {
                    sql_txt = @"SELECT TOP 1 SYS_ID FROM MNo_Labor WHERE MNo=@MNo AND Cust_ID=@Cust_ID";
                    add_txt = @"INSERT INTO MNo_Labor " +
                       "(" +
                       "[MNo], [Cust_ID], [Cust_Name], [Cust_FullName] " +
                       ") " +
                       " SELECT TOP 1 " +
                       "@MNO, [Cust_ID], [Cust_Name], [Cust_FullName] " +
                       "FROM Labor_System WHERE Cust_ID=@Cust_ID ";
                }
            }

            var chk = DBTool.Query<String>(sql_txt, new { MNo = MNo, Labor_ID = Labor_ID, Cust_ID = Cust_ID });
            if (chk.Any())
            {
                return JsonConvert.SerializeObject(new { status = "已有重複的勞工或雇主。" });
            }
            else
            {
                using (IDbConnection db = DBTool.GetConn())
                {
                    db.Execute(add_txt, new { MNo = MNo, Labor_ID = Labor_ID, Cust_ID = Cust_ID });
                    db.Close();
                }
                return JsonConvert.SerializeObject(new { status = "新增雇主或外勞完成。" });
            }
        }
        else
        {
            return JsonConvert.SerializeObject(new { status = "請選擇雇主或外勞。" });
        };
    }

    //===============

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Labor_Delete(string SYS_ID)                            // 舊的 刪除用
    {
        Check();
        string Sqlstr = @"DELETE FROM MNo_Labor WHERE SYS_ID = @SYS_ID";
        using (IDbConnection conn = DBTool.GetConn())
        {
            conn.Execute(Sqlstr, new { SYS_ID = SYS_ID });
            conn.Close();
        }
        return JsonConvert.SerializeObject(new { status = "success" });
    }

    //===============

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Labor_Value(string SYSID, string Flag)             //      勞工的 無用
    {
        Check();
        string outputJson = "";
        string sqlstr = "";
        if (Flag == "2")
        {
            sqlstr = @"SELECT TOP 1 * FROM Labor_System WHERE SYSID = @SYSID";
            var a = DBTool.Query<ClassTemplate>(sqlstr, new { SYSID = SYSID });
            if (a.Any())
            {
                ClassTemplate schedule = a.First();
                var language = schedule.Labor_Country.Trim();
                switch (language)
                {
                    case "南非":
                        language = "英語";
                        break;
                    case "泰國":
                        language = "泰語";
                        break;
                    case "越南":
                        language = "越南語";
                        break;
                    case "印尼":
                        language = "印尼語";
                        break;
                    case "菲律賓":
                        language = "菲律賓語";
                        break;
                    default:
                        language = "";
                        break;
                }
                var b = a.ToList().Select(p => new
               {
                   SYSID = p.SYSID,
                   Cust_ID = p.Cust_ID,
                   Cust_FullName = p.Cust_FullName,
                   Labor_Team = p.Labor_Team,
                   Labor_CName = p.Labor_CName,
                   Labor_Country = p.Labor_Country.Trim(),
                   Labor_ID = p.Labor_ID,
                   Labor_PID = p.Labor_PID,
                   Labor_RID = p.Labor_RID,
                   Labor_EID = p.Labor_EID,
                   Labor_Phone = p.Labor_Phone,
                   Labor_Address = p.Labor_Address,
                   Labor_Address2 = p.Labor_Address2,
                   Labor_Valid = p.Labor_Valid,
                   Language = language
               });
                outputJson = JsonConvert.SerializeObject(b);
            }
            else
            {
                outputJson = JsonConvert.SerializeObject(new { status = "null" });
            }
            return outputJson;
        }
        else
        {
            sqlstr = @"SELECT TOP 1 * FROM Labor_System WHERE SYSID = @SYSID";
            var a = DBTool.Query<ClassTemplate>(sqlstr, new { SYSID = SYSID });
            if (a.Any())
            {
                var b = a.ToList().Select(p => new
                {
                    SYSID = p.SYSID,
                    Cust_ID = p.Cust_ID,
                    Cust_FullName = p.Cust_FullName,
                    Labor_Team = "",
                    Labor_CName = "",
                    Labor_Country = "",
                    Labor_ID = "",
                    Labor_PID = "",
                    Labor_RID = "",
                    Labor_EID = "",
                    Labor_Phone = "",
                    Labor_Address = "",
                    Labor_Address2 = "",
                    Labor_Valid = "",
                    Language = ""
                });
                outputJson = JsonConvert.SerializeObject(b);
            }
            else
            {
                outputJson = JsonConvert.SerializeObject(new { status = "null" });
            }
            return outputJson;
        }
    }

    //=============【取消需求單】=============
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Btn_Cancel_Click(string txt_cancel)                                    // 勞工服務 取消用
    {
        //Check();
        string error = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。";
        string MNo = HttpContext.Current.Session["MNo"].ToString();
        string UserID = HttpContext.Current.Session["UserID"].ToString();
        string UserIDNAME = HttpContext.Current.Session["UserIDNAME"].ToString();
        int i = 0;
        if (!String.IsNullOrEmpty(txt_cancel))
        {
            i = txt_cancel.Length;
            if (txt_cancel.Length > 250)
            {
                return JsonConvert.SerializeObject(new { status = "【取消原因】不能超過２５０個字元。" });
            }
            else
            {
                txt_cancel = HttpUtility.HtmlEncode(txt_cancel.Trim());
                if (txt_cancel.Length != i)
                {
                    return JsonConvert.SerializeObject(new { status = error });
                }
            }
        }
        else
        {
            return JsonConvert.SerializeObject(new { status = "請填寫【取消原因】" });
        }

        //  類型  1：尚未審核  2：尚未派工  3：尚未結案    4：已經結案    5：退單    0：取消        
        ClassTemplate template = new ClassTemplate()
        {
            MNo = MNo,
            Type_Value = "0",
            Type = "取消",
            Question = txt_cancel.Trim(),
            Cancel_ID = UserID,
            Cancel_Name = UserIDNAME,
            Cancel_Time = DateTime.Now
        };

        string sqlstr = @"UPDATE LaborTemplate SET " +
        "Type_Value=@Type_Value, " +
        "Type=@Type, " +
        "Cancel_ID=@Cancel_ID, " +
        "Cancel_Name=@Cancel_Name, " +
        "Cancel_Time=@Cancel_Time, " +
        "Question2=@Question " +
        "WHERE MNo=@MNo";

        using (IDbConnection db = DBTool.GetConn())
        {
            db.Execute(sqlstr, template);
            db.Close();
        };

        return JsonConvert.SerializeObject(new { status = "success", mno = MNo });
    }

    //=============【帶入需求單 母單內容】=============
    /*    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
        public static string Load_MNo(string MNo)                                                       // 勞工 案件資料
        {
            Check();
            string Agent_LV = HttpContext.Current.Session["Agent_LV"].ToString();
            string Agent_Team = HttpContext.Current.Session["Agent_Team"].ToString();
            string sqlstr = @"SELECT TOP 1 * FROM LaborTemplate WHERE MNo=@MNo";
            string outputJson = "";
            var chk = DBTool.Query<ClassTemplate>(sqlstr, new { MNo = MNo });
            if (!chk.Any())
            {
                outputJson = "[" + JsonConvert.SerializeObject(new { MNo = "NULL" }) + "]"; // 組合JSON 格式
                return outputJson;
            }
            else
            {
                if (Agent_LV != "30")
                {
                    foreach (var q in chk)
                    {
                        if (q.Create_Team != Agent_Team)
                        {
                            outputJson = "[" + JsonConvert.SerializeObject(new { MNo = "NO" }) + "]"; // 組合JSON 格式
                            return outputJson;
                        }
                    }
                }

                outputJson = JsonConvert.SerializeObject(
                    chk.ToList().Select(p => new
                    {
                        Create_Name = p.Create_Name,
                        Create_Team = p.Create_Team,
                        Service = p.Service,
                        Time_01 = p.Time_01,
                        Time_02 = p.Time_02,
                        Location = p.Location,
                        LocationStart = p.LocationStart,
                        LocationEnd = p.LocationEnd,
                        CarSeat = p.CarSeat,
                        ContactName = p.ContactName,
                        ContactPhone2 = p.ContactPhone2,
                        ContactPhone3 = p.ContactPhone3,
                        Contact_Co_TEL = p.Contact_Co_TEL,
                        HospitalClass = p.HospitalClass,
                        Question = p.Question,
                        //==========================
                        Cust_FullName = p.Cust_Name,
                        Labor_Team = p.Labor_Team,
                        Labor_CName = p.Labor_CName,
                        Labor_Country = p.Labor_Country,
                        Labor_Language = p.Labor_Language,
                        Labor_ID = p.Labor_ID,
                        Labor_PID = p.Labor_PID,
                        Labor_RID = p.Labor_RID,
                        Labor_EID = p.Labor_EID,
                        Labor_Phone = p.Labor_Phone,
                        Labor_Address = p.Labor_Address,
                        Labor_Address2 = p.Labor_Address2,
                        Labor_Valid = p.Labor_Valid
                    })
                  );
                return outputJson;
            }

        }   */

    //============= 驗證資料 ==============                                     // 勞工 填單 驗證用 583~793
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Check_Form(string Flag, string Service_ID, string Time_01, string Time_02, string PostCode, string Location, string LocationStart,
        string LocationEnd, string CarSeat, string ContactName, string ContactPhone2, string ContactPhone3, string Contact_Co_TEL,
        string Hospital, string HospitalClass, string Question)
    {
        Check();
        DateTime Date;
        string MNo = HttpContext.Current.Session["MNo"].ToString();
        string error = "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。";
        string Service = "";
        string ServiceName = "";
        string sql_check = "";

        // 驗證 Service_ID 服務內容
        if (!String.IsNullOrEmpty(Service_ID))
        {
            sql_check = @"SELECT TOP 1 SYS_ID, Service, ServiceName FROM Data WHERE Service_ID=@Service_ID AND Service !='外包商' ";
            var check = DBTool.Query<ClassTemplate>(sql_check, new { Service_ID = Service_ID });
            if (!check.Any())
            {
                return JsonConvert.SerializeObject(new { status = "沒有此【服務內容】名稱" });
            }
            else
            {
                ClassTemplate schedule = check.First();
                Service = schedule.Service;
                ServiceName = schedule.ServiceName;
            }
        }
        else
        {
            return JsonConvert.SerializeObject(new { status = "請選擇【服務內容】" });
        }

        // 驗證 Time_01 預定起始時間是否為時間 是否空白
        if (!String.IsNullOrEmpty(Time_01))
        {
            if (!DateTime.TryParse(Time_01, out Date))
            {
                return JsonConvert.SerializeObject(new { status = error });
            }
        }
        else
        {
            return JsonConvert.SerializeObject(new { status = "請填寫【預定起始時間】" });
        }

        // 驗證 Time_02 預定終止時間是否為時間 是否空白
        if (!String.IsNullOrEmpty(Time_02))
        {
            if (!DateTime.TryParse(Time_02, out Date))
            {
                return JsonConvert.SerializeObject(new { status = error });
            }
        }
        else
        {
            return JsonConvert.SerializeObject(new { status = "請填寫【預定終止時間】" });
        }

        // 驗證 【預定起始時間】是否小於【現在時間】
        if (Convert.ToDateTime(Time_01) < DateTime.Now.AddDays(-14))
        {
            return JsonConvert.SerializeObject(new { status = "【預定起始時間】不能小於【" + DateTime.Now.AddDays(-14).ToString("yyyy/MM/dd hh:mm") + "】" });
        }

        // 驗證 【預定起始時間】是否小於【預定終止時間】
        if (Convert.ToDateTime(Time_01) > Convert.ToDateTime(Time_02))
        {
            return JsonConvert.SerializeObject(new { status = "【預定起始時間】不能大於【預定終止時間】" });
        }

        // 驗證 PostCode 郵遞區號是否為數字 是否不超過五碼
        if (!String.IsNullOrEmpty(PostCode))
        {
            if (PostCode.Length > 5)
            {
                return JsonConvert.SerializeObject(new { status = error });
            }
            else
            {
                try
                {
                    int i = Convert.ToInt32(PostCode);
                }
                catch
                {
                    return JsonConvert.SerializeObject(new { status = error });
                }
            }
        }

        //======================================================================
        string back = "";
        List<XXS> check_value = new List<XXS>();
        check_value.Add(new XXS { URL_ID = Location, MiniLen = 0, MaxLen = 100, Alert_Name = "地址與郵遞區號", URL_Type = "txt" });
        check_value.Add(new XXS { URL_ID = LocationStart, MiniLen = 0, MaxLen = 100, Alert_Name = "行程起點", URL_Type = "txt" });
        check_value.Add(new XXS { URL_ID = LocationEnd, MiniLen = 0, MaxLen = 100, Alert_Name = "行程終點", URL_Type = "txt" });
        check_value.Add(new XXS { URL_ID = ContactPhone2, MiniLen = 0, MaxLen = 10, Alert_Name = "公司手機號碼", URL_Type = "int" });
        check_value.Add(new XXS { URL_ID = ContactPhone3, MiniLen = 0, MaxLen = 10, Alert_Name = "手機簡碼", URL_Type = "int" });
        check_value.Add(new XXS { URL_ID = Question, MiniLen = 1, MaxLen = 250, Alert_Name = "狀況說明", URL_Type = "txt" });
        JavaScriptSerializer Serializer = new JavaScriptSerializer();
        string outputJson = Serializer.Serialize(check_value);
        back = JASON.Check_XSS(outputJson);
        if (back != "")
        {
            return JsonConvert.SerializeObject(new { status = back });
        };
        //=======================================================================

        //驗證 CarSeat 搭車人數是否為數字
        try
        {
            if (Convert.ToInt32(CarSeat) > 10 || Convert.ToInt32(CarSeat) < 0)
            {
                return JsonConvert.SerializeObject(new { status = error });
            }
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new { status = error });
        }

        //驗證 ContactName 【聯絡人】
        if (!String.IsNullOrEmpty(ContactName))
        {
            if (ContactName.Length > 10)
            {
                return JsonConvert.SerializeObject(new { status = "【聯絡人】不能超過１０個字元。" });
            }
            else
            {
                if (JASON.IsNumericOrLetterOrChinese(ContactName) != true)
                {
                    return JsonConvert.SerializeObject(new { status = "【聯絡人】只能輸入數字、英文或中文。" });
                }
            }
        }

        //驗證 Contact_Co_TEL 【公司電話】
        Contact_Co_TEL = Contact_Co_TEL.Trim();
        if (!String.IsNullOrEmpty(Contact_Co_TEL))
        {
            if (Contact_Co_TEL.Length > 20)
            {
                return JsonConvert.SerializeObject(new { status = "【公司電話】不能超過２０個字元。" });
            }
            else
            {
                if (JASON.IsTelephone(Contact_Co_TEL) != true)
                {
                    return JsonConvert.SerializeObject(new { status = "【公司電話】格式不正確。" });
                }
            }
        }

        //驗證 Hospital 【醫療院所】與 HospitalClass【就醫類型】
        if (Service == "醫療")
        {
            //================ 驗證 Hospital 【醫療院所】 ================
            if (!String.IsNullOrEmpty(Hospital))
            {
                sql_check = @"SELECT HospitalName FROM DataHospital WHERE HospitalName = @HospitalName AND Flag = '1'";
                var check = DBTool.Query<ClassTemplate>(sql_check, new { HospitalName = Hospital });
                if (!check.Any())
                {
                    return JsonConvert.SerializeObject(new { status = "沒有此【醫療院所】名稱" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { status = "請選擇【醫療院所】" });
            }

            //================ 驗證 HospitalClass【就醫類型】 ================
            if (!String.IsNullOrEmpty(HospitalClass))
            {
                if (HospitalClass != "上班就醫" && HospitalClass != "上班就醫 ( 需診斷書 )" && HospitalClass != "下班就醫" && HospitalClass != "下班就醫 ( 需診斷書 )" && HospitalClass != "")
                {
                    return JsonConvert.SerializeObject(new { status = "沒有此【就醫類型】名稱" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { status = "請選擇【就醫類型】" });
            }
        }

        if (Flag == "1")
        {
            update_MNo(Service_ID, ServiceName, Time_01, Time_02, LocationStart, LocationEnd, CarSeat, ContactName, ContactPhone2,
                   ContactPhone3, Contact_Co_TEL, Hospital, HospitalClass, Question, PostCode, Location);
            return JsonConvert.SerializeObject(new { status = "update" });
        }
        else
        {
            sql_check = @"SELECT SYS_ID FROM MNo_Labor WHERE MNo=@MNo";
            var chk = DBTool.Query<ClassTemplate>(sql_check, new { MNo = MNo });
            if (chk.Count() > 0)
            {
                back = new_MNo(Service_ID, ServiceName, Time_01, Time_02, LocationStart, LocationEnd, CarSeat, ContactName, ContactPhone2,
                    ContactPhone3, Contact_Co_TEL, Hospital, HospitalClass, Question, PostCode, Location);
                return JsonConvert.SerializeObject(new { status = back });
            }
            else
            {
                return JsonConvert.SerializeObject(new { status = "請選擇【雇主】或【外勞】" });
            }
        }
    }

    //============= 新增需求單 =============                         勞工 795~986
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string new_MNo(string Service_ID, string ServiceName, string Time_01, string Time_02, string LocationStart, string LocationEnd,
        string CarSeat, string ContactName, string ContactPhone2, string ContactPhone3, string Contact_Co_TEL, string Hospital, string HospitalClass,
        string Question, string PostCode, string Location)
    {
        Check();
        string MNo = HttpContext.Current.Session["MNo"].ToString();
        string UserID = HttpContext.Current.Session["UserID"].ToString();
        string UserIDNAME = HttpContext.Current.Session["UserIDNAME"].ToString();
        string Agent_Team = HttpContext.Current.Session["Agent_Team"].ToString();
        string language = "";
        string EMMA_URL;
        string sql;
        string f_MNo;

        //↓↓↓↓↓ 檢查 [Faremma].[dbo].[Data] 服務項目 需不需要審核 ↓↓↓↓↓
        string EMMA = System.Web.Configuration.WebConfigurationManager.AppSettings["EMMA"].ToString();
        string sql_txt = @"SELECT TOP 1 SYS_ID FROM Data WHERE Service_ID=@Service_ID AND Flag = '1' AND Service !='外包商' ";
        string f_type = "尚未派工";
        string f_value = "2";  //  類型  1：尚未審核  2：尚未派工  3：尚未結案    4：已經結案    5：退單
        string f_page = "2";  // 1.  需求單審核    2.   員工派工管理  3.  個人派工及結案管理
        string f_no = "派工";   // "審核"  "派工"  "接單"  "暫結案"  "結案"    
        var chk = DBTool.Query<ClassTemplate>(sql_txt, new { Service_ID = Service_ID });
        if (chk.Any())
        {
            f_type = "尚未審核";
            f_value = "1";
            f_page = "1";
            f_no = "審核";
        }
        //↑↑↑↑↑ 檢查 [Faremma].[dbo].[Data] 服務項目 需不需要審核 ↑↑↑↑↑
        Thread.Sleep(100);

        //============== 檢查 新增多筆勞工資訊（瀏覽） ==============
        #region
        sql_txt = @"SELECT * FROM MNo_Labor WHERE MNo=@MNo";
        chk = DBTool.Query<ClassTemplate>(sql_txt, new { MNo = MNo });
        Thread.Sleep(100);
        if (chk.Count() > 0)
        {
            #region
            //sql_txt = @"INSERT INTO LaborTemplate" +
            //    "(MNo, Type_Value, Type, Cust_Name, Cust_ID, Labor_ID, Labor_Team, Labor_Phone, Labor_Address, Labor_Address2, " +
            //    "Labor_Language, Service_ID, Time_01, Time_02, LocationStart, LocationEnd, CarSeat, ContactName, " +
            //    "ContactPhone2, ContactPhone3, Contact_Co_TEL, Hospital, HospitalClass, Question, Create_ID, PostCode, Location) " +
            //    "VALUES" +
            //    "(@MNo,@Type_Value,@Type,@Cust_Name, @Cust_ID, @Labor_ID,@Labor_Team,@Labor_Phone,@Labor_Address,@Labor_Address2, " +
            //    "@Labor_Language, @Service_ID, @Time_01, @Time_02, @LocationStart, @LocationEnd,@CarSeat, @ContactName, " +
            //     "@ContactPhone2, @ContactPhone3, @Contact_Co_TEL, @Hospital, @HospitalClass, @Question, @Create_ID, @PostCode, @Location)";
            #endregion
            sql_txt = "Declare @temp1 table ( [Labor_ID] [nvarchar](50) NULL " +
                " , [Service_ID] [varchar](2) NULL " +
                " , [Create_ID] [nvarchar](50) NULL )";

            sql_txt += "insert into @temp1 " +
                "([Labor_ID], [Service_ID], [Create_ID]) values ( @Labor_ID, @Service_ID, @Create_ID) ";

            sql_txt += "insert into LaborTemplate ([MNo], [Type_Value], [Type], [Cust_ID], [Cust_Name], [Labor_Team], [Labor_Company], [Labor_ID] " +
                ", [Labor_EName], [Labor_CName], [Labor_Country], [Labor_PID], [Labor_RID], [Labor_EID], [Labor_Phone], [Labor_Language] " +
                ", [Labor_Address], [Labor_Address2], [Labor_Valid], [Service_ID], [Service], [ServiceName], [Time_01], [Time_02], [CarSeat], [LocationStart], [LocationEnd] " +
                ", [Location], [PostCode], [ContactName], [ContactPhone2], [ContactPhone3], [Contact_Co_TEL], [Hospital], [HospitalClass], [Question], [Create_ID] " +
                ", [Create_Team], [Create_Name], [Create_Company], [Agent_Mail], [UserID] ) ";

            sql_txt += "select TOP 1 @MNo, @Type_Value, @Type, @Cust_ID, @Cust_Name, b.[Labor_Team], b.[Labor_Company], a.[Labor_ID] " +
                ", b.[Labor_EName], b.[Labor_CName], b.[Labor_Country], b.[Labor_PID], b.[Labor_RID], b.[Labor_EID], b.[Labor_Phone], @Labor_Language " +
                ", @Labor_Address, @Labor_Address2, b.[Labor_Valid], a.[Service_ID], c.[Service], c.[ServiceName], @Time_01, @Time_02, @CarSeat, @LocationStart, @LocationEnd " +
                ", @Location, @PostCode, @ContactName, @ContactPhone2, @ContactPhone3, @Contact_Co_TEL, @Hospital, @HospitalClass, @Question, a.[Create_ID] " +
                ", [Agent_Team] as [Create_Team]" +
                ", [Agent_Name] as [Create_Name]" +
                ", [Agent_Company] as [Create_Company]" +
                ", [Agent_Mail], [UserID] ";

            sql_txt += "from @temp1 as a " +
                "left join [Labor_System] as b on a.[Labor_ID]=b.[Labor_ID] " +
                "left join [Data] as c on a.[Service_ID]=c.[Service_ID] " +
                "left join [DispatchSystem] as d on a.[Create_ID]=d.[Agent_ID]";

            Thread.Sleep(100);
            foreach (var q in chk)
            {
                f_MNo = "1" + DateTime.Now.ToString("yyMMdd") + q.SYS_ID.ToString("000000");
                switch (q.Labor_Country.Trim())
                {
                    case "南非":
                        language = "英語";
                        break;
                    case "泰國":
                        language = "泰語";
                        break;
                    case "越南":
                        language = "越南語";
                        break;
                    case "印尼":
                        language = "印尼語";
                        break;
                    case "菲律賓":
                        language = "菲律賓語";
                        break;
                    default:
                        language = "";
                        break;
                }
                ClassTemplate template = new ClassTemplate()
                {
                    MNo = f_MNo,
                    Type = f_type,
                    Type_Value = f_value,  //  類型  1：尚未審核  2：尚未派工  3：尚未結案    4：已經結案    5：退單
                    Cust_Name = q.Cust_FullName,
                    Cust_ID = q.Cust_ID,
                    Labor_ID = q.Labor_ID,
                    Labor_Team = q.Labor_Team,
                    Labor_Phone = q.Labor_Phone,
                    Labor_Address = q.Labor_Address,
                    Labor_Address2 = q.Labor_Address2,
                    Labor_Language = language,
                    Service_ID = Service_ID,
                    Time_01 = Convert.ToDateTime(Time_01),
                    Time_02 = Convert.ToDateTime(Time_02),
                    LocationStart = LocationStart.Trim(),
                    LocationEnd = LocationEnd.Trim(),
                    CarSeat = CarSeat,
                    ContactName = ContactName,
                    ContactPhone2 = ContactPhone2,
                    ContactPhone3 = ContactPhone3,
                    Contact_Co_TEL = Contact_Co_TEL,
                    Hospital = Hospital,
                    HospitalClass = HospitalClass,
                    Question = Question.Trim(),
                    Create_ID = UserID,
                    PostCode = PostCode,
                    Location = Location
                };
                Thread.Sleep(100);

                try
                {
                    using (IDbConnection db = DBTool.GetConn())
                    {
                        db.Execute(sql_txt, template);
                        db.Close();
                    }
                }
                catch
                {
                    logger.Info("需求單新增錯誤：編號：" + f_MNo + "，Service_ID：" + Service_ID + "，Create_ID：" + UserID + "，Labor_ID：" + q.Labor_ID);
                    logger.Info("需求單新增錯誤：SQL：" + sql_txt);
                    return "傳送系統參數錯誤，請再嘗試或詢問管理人員，謝謝。";
                }

                Thread.Sleep(100);
                //=============  EMMA =============
                #region EMMA

                EMMA_URL = EMMA + "CheckLogin.aspx?seqno=" + f_MNo + "&page=" + f_page + "&login=";
                sql = @"SELECT * FROM Master_DATA WHERE Agent_Team=@Agent_Team AND Agent_LV = '15' ";
                var list = DBTool.Query<EMMA>(sql, new { Agent_Team = Agent_Team });
                //logger.Info("需求單編號：【" + f_MNo + "】EMMA Function 開始，部門【" + Agent_Team + "】");
                if (list.Count() > 0)
                {
                    sql = @"INSERT INTO tblAssign (AssignNo, E_MAIL, ConnURL) VALUES(@AssignNo ,@E_MAIL ,@ConnURL)";
                    logger.Info("===== EMMA 部門迴圈 開始=====");
                    foreach (var r in list)
                    {
                        ClassTemplate emma = new ClassTemplate()
                        {
                            AssignNo = "【" + f_no + "通知】【" + q.Cust_FullName + "：" + ServiceName + "】",    // "審核"  "派工"  "接單"  "暫結案"  "結案"
                            ConnURL = EMMA_URL + JASON.Encryption(r.UserID),
                            E_MAIL = r.E_Mail
                        };
                        using (IDbConnection db = DBTool.GetConn())
                        {
                            db.Execute(sql, emma);
                            db.Close();
                        }
                    }
                    logger.Info("===== EMMA 部門迴圈 結束=====");
                }
                logger.Info("===== EMMA Function 結束=====");

                #endregion
                //=============  EMMA =============
                Thread.Sleep(100);
            }
            return "new";
        }
        else
        {
            return "請選擇【雇主】或【外勞】";
        }
        #endregion
    }

    //============= 修改需求單 =============                     // 勞工   988~1048    SQL 資料 Update
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string update_MNo(string Service_ID, string ServiceName, string Time_01, string Time_02, string LocationStart, string LocationEnd,
        string CarSeat, string ContactName, string ContactPhone2, string ContactPhone3, string Contact_Co_TEL, string Hospital, string HospitalClass,
        string Question, string PostCode, string Location)
    {
        Check();
        string MNo = HttpContext.Current.Session["MNo"].ToString();
        string UserID = HttpContext.Current.Session["UserID"].ToString();
        string UserIDNAME = HttpContext.Current.Session["UserIDNAME"].ToString();
        string Agent_Team = HttpContext.Current.Session["Agent_Team"].ToString();
        string sql_txt = @"UPDATE LaborTemplate SET " +
            "Service_ID=@Service_ID, " +
            "Time_01=@Time_01, " +
            "Time_02=@Time_02, " +
            "Location=@Location, " +
            "PostCode=@PostCode, " +
            "LocationStart=@LocationStart, " +
            "LocationEnd=@LocationEnd, " +
            "CarSeat=@CarSeat, " +
            "ContactName=@ContactName, " +
            "ContactPhone2=@ContactPhone2, " +
            "ContactPhone3=@ContactPhone3, " +
            "Contact_Co_TEL=@Contact_Co_TEL, " +
            "Hospital=@Hospital, " +
            "HospitalClass=@HospitalClass, " +
            "Question=@Question, " +
            "UPDATE_ID=@UPDATE_ID, " +
            "UPDATE_Name=@UPDATE_Name, " +
            "UPDATE_TIME=@UPDATE_TIME " +
            "WHERE MNo = @MNo";

        ClassTemplate template = new ClassTemplate()
        {
            MNo = MNo,
            Service_ID = Service_ID,
            Time_01 = Convert.ToDateTime(Time_01),
            Time_02 = Convert.ToDateTime(Time_02),
            Location = Location,
            PostCode = PostCode,
            LocationStart = LocationStart,
            LocationEnd = LocationEnd,
            CarSeat = CarSeat,
            ContactName = ContactName,
            ContactPhone2 = ContactPhone2,
            ContactPhone3 = ContactPhone3,
            Contact_Co_TEL = Contact_Co_TEL,
            Hospital = Hospital,
            HospitalClass = HospitalClass,
            Question = Question,
            UPDATE_ID = UserID,
            UPDATE_Name = UserIDNAME,
            UPDATE_TIME = DateTime.Now,
        };
        using (IDbConnection db = DBTool.GetConn())
        {
            db.Execute(sql_txt, template);
            db.Close();
        }
        return JsonConvert.SerializeObject(new { status = "success" });
    }

    //============= 讀取機關地址 =============                    // 勞工  調出各機關地址及資料
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Location_List(string Type)                                     // 762 行    
    {
        Check();
        string Sqlstr = @"SELECT SYS_ID, Name, Location, Postcode, Address, TEL FROM HRM2_Location WHERE Type = @Type ";
        var a = DBTool.Query<Location_str>(Sqlstr, new { Type = Type }).ToList().Select(p => new
        {
            SYS_ID = p.SYS_ID,
            Name = p.Name,
            Location = p.Location,
            Postcode = p.Postcode,
            Address = p.Address,
            TEL = p.TEL
        });
        string outputJson = JsonConvert.SerializeObject(a);
        return outputJson;
    }

    //============= 地址欄位帶入地址 =============
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Add_Location(string ID, string Flag)               // 819 行
    {
        Check();
        string sqlstr = "";
        if (Flag == "0")
        {
            sqlstr = @"SELECT Name, Location, Postcode, Address FROM HRM2_Location WHERE SYS_ID = @SYS_ID ";
            var a = DBTool.Query<Location_str>(sqlstr, new { SYS_ID = ID }).ToList().Select(p => new
            {
                Name = p.Name,
                Location = p.Location,
                Postcode = p.Postcode,
                Address = p.Address
            });
            string outputJson = JsonConvert.SerializeObject(a);
            return outputJson;
        }
        else
        {
            sqlstr = @"SELECT Cust_FullName, Cust_Postcode, Cust_Address_ZH FROM HRM2_Cust_System WHERE SYS_ID = @SYS_ID ";
            var a = DBTool.Query<Location_str>(sqlstr, new { SYS_ID = ID }).ToList().Select(p => new
            {
                Name = p.Cust_FullName,
                Location = "",
                Postcode = p.Cust_Postcode,
                Address = p.Cust_Address_ZH
            });
            string outputJson = JsonConvert.SerializeObject(a);
            return outputJson;
        }
    }

    //=============== 判斷【地址與郵遞區號】Flag ===============       // 勞工
    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string ServiceName_Flag(string Service_ID)
    {
        Check();
        if (!string.IsNullOrEmpty(Service_ID))
        {
            string sql_txt = "";
            sql_txt = @"SELECT TOP 1 Flag_Add, Service FROM Data WHERE Service_ID=@Service_ID AND Open_Flag = '1' AND Service !='外包商' ";

            var Labor_chk = DBTool.Query<Location_str>(sql_txt, new { Service_ID = Service_ID });
            if (Labor_chk.Any())
            {
                foreach (var q in Labor_chk)
                {
                    if (q.Flag_Add == "1")
                    {
                        if (q.Service == "醫療")
                        {
                            // B 為 開啟【單位地址】選擇按鈕 但內容選擇全為醫療
                            return JsonConvert.SerializeObject(new { status = "B" });
                        }
                        else
                        {
                            // A 為 開啟【單位地址】選擇按鈕
                            return JsonConvert.SerializeObject(new { status = "A" });
                        }
                    }
                }
                // C 為 開啟【客戶地址】選擇按鈕
                return JsonConvert.SerializeObject(new { status = "C" });
            }
        }
        return JsonConvert.SerializeObject(new { status = "NULL" });
    }

    //===============  

    public static void Check()
    {
        string Check = JASON.Check_ID("0030010002.aspx");
        if (Check == "NO")
        {
            //       System.Web.HttpContext.Current.Response.Redirect("~/Default.aspx");
        }
    }

    //===============  

    [WebMethod(EnableSession = true)]//或[WebMethod(true)]
    public static string Load_OEData(string OE_Case_ID)                                                       // 新增 讀CaseData 案件資料
    {
        OE_Case_ID = OE_Case_ID.Trim();
        //Check();       
        //str_title = "修改";        
        string sqlstr = @"SELECT * FROM OE_Data WHERE OE_Case_ID=@OE_Case_ID";
        var lcd = DBTool.Query<T_0030010005_2>(sqlstr, new { OE_Case_ID = OE_Case_ID }).ToList().Select(p => new
        {
            OE_D_ID = Value(p.OE_D_ID)
            ,OE_Case_ID = Value(p. OE_Case_ID)
            ,OE_PS = Value(p.OE_PS)
            ,Con_Name = Value(p.Con_Name)
            ,Con_Company = Value(p.Con_Company)
            ,Con_Phone = Value(p.Con_Phone)
            ,SetupDate = Value2(p. SetupDate)
            ,UpdateDate = Value2(p.UpdateDate)
            ,Total_Price = Value(p.Total_Price)
            ,BUSINESSNAME = Value(p.BUSINESSNAME)
            ,PID = Value(p.PID)
            ,Type_Value = Value(p.Type_Value)
            ,Type = Value(p.Type)
            ,Dealer = Value(p.Dealer)
            ,APPNAME = Value(p.APPNAME)
            ,CONTACT_ADDR = Value(p.CONTACT_ADDR)
            ,APP_FTEL = Value(p.APP_FTEL)
            ,APP_MTEL = Value(p.APP_MTEL)
            ,HardWare = Value(p.HardWare)
            ,SoftwareLoad = Value(p.SoftwareLoad)
            ,Name = Value(p.Name)
            ,PNumber = Value(p.PNumber)
            ,S_APPNAME = Value(p.S_APPNAME)
            ,S_CONTACT_ADDR = Value(p.S_CONTACT_ADDR)
            ,S_APP_FTEL = Value(p.S_APP_FTEL)
            ,S_APP_MTEL = Value(p.S_APP_MTEL)
            ,S_HardWare = Value(p.S_HardWare)
            ,S_SoftwareLoad = Value(p.S_SoftwareLoad)
            ,RFQ = Value(p.RFQ)
            ,Final_Price = Value(p.Final_Price)
            ,Warranty_Date = Test(Value2(p.Warranty_Date))
            ,Warr_Time = Value(p.Warr_Time)
            ,Protect_Date = Test(Value2(p.Protect_Date))
            ,Prot_Time = Value(p.Prot_Time)
            ,Receipt_Date = Test(Value2(p.Receipt_Date))
            ,Receipt_PS = Value(p.Receipt_PS)
            ,Close_Out_Date = Test(Value2(p.Close_Out_Date))
            ,Close_Out_PS = Value(p.Close_Out_PS)            //*/
        }); 
        string outputJson = JsonConvert.SerializeObject(lcd);
        return outputJson;
    }

    //[WebMethod(EnableSession = true)]//或[WebMethod(true)]
    protected void btnUpload_Click(object sender, EventArgs e)  //可以上傳 
    {
        string ID = HttpContext.Current.Session["SYSID"].ToString();
        string seqno = Request.Params["seqno"];
        //string ID = Request.Params["seqno"];
        //string name = ID + "_" + DateTime.Now.ToString("HHmmss");
        try
        {
            if (savefile(FileUpload1) == true) ;
            Response.Write("檔案上傳 成功");
            Thread.Sleep(500);  //延遲 好像沒作用
            System.Web.HttpContext.Current.Response.Redirect("~/0030010005.aspx?seqno=" + seqno);
        }
        catch (Exception ex)
        {
            Response.Write("error code : " + ex.Message);
        }
    }
    //[WebMethod(EnableSession = true)]//或[WebMethod(true)]
    bool savefile(FileUpload FileUpload1)
    {
        string FullFileName = Path.GetFileName(FileUpload1.FileName);    //抓完整檔名
        string seqno = Request.Params["seqno"];

        string A = @"C:\OE_File\" + seqno + (char)92;       //上傳的路徑加檔名.副檔名
        string Address = A + FullFileName;

        if (!Directory.Exists(A))
            Directory.CreateDirectory(A);
        if (FileUpload1.HasFile == true)    //判斷上傳物件否存在
        {
            File.Delete(Address);            //重覆檔名無法上傳 , 所以使用System.IO 將檔案刪除
            FileUpload1.SaveAs(Address);    //上傳檔案,回傳boolin
            string ID = HttpContext.Current.Session["SYSID"].ToString();
            //string sql = "UPDATE InSpecation.dbo.Mission_Title SET File_name=@name WHERE SYSID=@ID "; //儲存SQL紀錄
            //var a = DBTool.Query<T_0030010005_2>(sql, new { ID = ID, name = FullFileName });
            return true;
        }
        else
        {
            return false;
        }
    }

    public static string Value(string value)        // 當值為null時跳過  非 null 時去除後方空白
    {
        if (!string.IsNullOrEmpty(value))
        {
            value = value.Trim();
        }
        return value;
    }
    public static string Value2(string value)        // 當值為null時跳過  非 null 時改時間格式
    {
        if (!string.IsNullOrEmpty(value))
        {
            value = DateTime.Parse(value).ToString("yyyy/MM/dd hh:mm");
        }
        return value;
    }
    public static string Test(string value)        // 當值為null時跳過  非 null 時去除後方空白
    {
        if (!string.IsNullOrEmpty(value))
        {
            value = value.Trim();
            if (value == "1900/01/01 12:00")
            {
                value = "";
            }
        }
        return value;
    }
    /*     //=============== 
        public static string Show_CaseData(string value)                     // 叫出商業資料  234行
        {
            Check();
            string Sqlstr = @"SELECT BUSINESSNAME,APPNAME,APP_OTEL_AREA,APP_OTEL,APP_MTEL,HardWare, " +
                " SoftwareLoad,SERVICEITEM FROM BusinessData WHERE ID=@value ";                                                                //  ID=@value ?
            var a = DBTool.Query<ClassTemplate>(Sqlstr, new { value = value }).ToList().Select(p => new
            {
                C = p.BUSINESSNAME,         // 公司名
                D = p.APPNAME,                  // 聯絡人
                E = p.APP_OTEL_AREA,
                F = p.APP_OTEL,                  // 電話
                G = p.APP_MTEL,                 // 手機
                H = p.HardWare.Trim(),      // 硬體
                I = p.SoftwareLoad,
                J = p.SERVICEITEM              // 合約
            });
            string outputJson = JsonConvert.SerializeObject(a);

            return outputJson;
        }           */

    //===============  
    public class Location_str
    {
        public string SYS_ID { get; set; }
        public string Service { get; set; }
        public string Flag_Add { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Postcode { get; set; }
        public string Address { get; set; }
        public string TEL { get; set; }
        public string Cust_Address_ZH { get; set; }
        public string Cust_FullName { get; set; }
        public string Cust_Postcode { get; set; }
    }
    public class T_0030010005_2
    {
        public int a { get; set; }
        public string ID { get; set; }
        public string OE_ID { get; set; }
        public string Product_Name { get; set; }
        public string Product_ID { get; set; }
        public string Main_Classified { get; set; }
        public string Detail_Classified { get; set; }
        public string Unit_Price { get; set; }
        public string Contact_ADDR { get; set; }

        public string OE_O_ID { get; set; }
        public string OE_Case_ID { get; set; }
        public string Quantity { get; set; }
        public string T_Price { get; set; }

        public string OE_D_ID { get; set; }   
        public string Con_Name { get; set; }
        public string Con_Company { get; set; }      
        public string Con_Phone { get; set; }      
        public string SetupDate { get; set; }
        public string UpdateDate { get; set; }
        public string Total_Price { get; set; }
        public string BUSINESSNAME { get; set; }
        public string PID { get; set; }
        public string Type_Value { get; set; }
        public string Type { get; set; }
        public string APPNAME { get; set; }
        public string CONTACT_ADDR { get; set; }
        public string APP_FTEL { get; set; }
        public string APP_MTEL { get; set; }
        public string HardWare { get; set; }
        public string SoftwareLoad { get; set; }
        public string Name { get; set; }
        public string PNumber { get; set; }
        public string S_APPNAME { get; set; }
        public string S_CONTACT_ADDR { get; set; }
        public string S_APP_FTEL { get; set; }
        public string S_APP_MTEL { get; set; }
        public string S_HardWare { get; set; }
        public string S_SoftwareLoad { get; set; }
        public string RFQ { get; set; }
        public string Warranty_Date { get; set; }
        public string Warr_Time { get; set; }
        public string Protect_Date { get; set; }
        public string Prot_Time { get; set; }
        public string Receipt_Date { get; set; }
        public string Receipt_PS { get; set; }
        public string Close_Out_Date { get; set; }
        public string Close_Out_PS { get; set; }
        public string OE_PS { get; set; }
        public string Final_Price { get; set; }
        public string str_sysid { get; set; }
        public string D_ID { get; set; }
        public string D_Name { get; set; }
        public string D_PS { get; set; }
        public string Dealer { get; set; }
        //public DateTime EstimatedFinishTime { get; set; }
    }
}






//==================== LINQ SQL 迴圈====================
//      string sqlstr = @"select * from EMMA_System
//                          where Service=@Service";
//        var list = DBTool.Query<ClassTemplate>(sqlstr, new { Service = "郵寄" });
//        logger.Info("=====================================");
//        if (list.Count() > 0)
//        {
//            foreach (var q in list)
//            {
//                logger.Info("Name = \"{0}\", Score = {1}" + q.SYS_ID + q.ServiceName);
//            }
//        }
//==================================================