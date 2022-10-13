using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using log4net;
using log4net.Config;

/// <summary>
/// JASON 的摘要描述
/// </summary>
public class JASON
{
    static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public JASON()
    {

    }

    //===========================================================
    public static string Encryption(string Password) //密碼加密
    {
        string back = "";
        string key = "ACMEACME";
        string iv = "70472615";

        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        des.Key = Encoding.ASCII.GetBytes(key);
        des.IV = Encoding.ASCII.GetBytes(iv);
        byte[] s = Encoding.ASCII.GetBytes(Password);
        ICryptoTransform desencrypt = des.CreateEncryptor();
        back = BitConverter.ToString(desencrypt.TransformFinalBlock(s, 0, s.Length)).Replace("-", string.Empty);
        return back;
    }

    public static string Decrypted(string Password) //密碼解密
    {
        string back = "";
        string key = "ACMEACME";
        string iv = "70472615";

        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        des.Key = Encoding.ASCII.GetBytes(key);
        des.IV = Encoding.ASCII.GetBytes(iv);
        byte[] s = new byte[Password.Length / 2];
        int j = 0;
        for (int i = 0; i < Password.Length / 2; i++)
        {
            s[i] = Byte.Parse(Password[j].ToString() + Password[j + 1].ToString(), System.Globalization.NumberStyles.HexNumber);
            j += 2;
        }
        ICryptoTransform desencrypt = des.CreateDecryptor();
        back = Encoding.ASCII.GetString(desencrypt.TransformFinalBlock(s, 0, s.Length));
        return back;
    }

    //================== Check_ID 檢查有沒有權限訪問該頁面 ==================

    public static string Check_ID(string URL)
    {
        try
        {
            string Role_ID = HttpContext.Current.Session["RoleID"].ToString();
            string sqlstr = @"SELECT TOP 1 Role_ID, NAVIGATE_URL FROM ROLEPROG as A LEFT JOIN PROGLIST as B on A.TREE_ID = B.TREE_ID " +
                "WHERE Role_ID=@Role_ID AND NAVIGATE_URL LIKE '%'+@URL+'%' ";
            var chk = DBTool.Query<ClassTemplate>(sqlstr, new { Role_ID = Role_ID, URL = URL });
            if (!chk.Any())
            {
                return "NO";
            }
            else
            {
                return "YES";
            }
        }
        catch
        {
            System.Web.HttpContext.Current.Response.Redirect("~/Logout.aspx");
            return "ERROR";
        }
    }

    //===========================================================

    ///CheckXSS
    public static string Check_XSS(string str_JSON)
    {
        var array_JSON = JsonConvert.DeserializeObject<List<XXS>>(str_JSON);
        foreach (var obj in array_JSON)
        {
            int int_len = 0;
            string value = "";

            //====================================================

            if (obj.MiniLen == obj.MaxLen)
            {
                if (obj.URL_ID.Length > obj.MaxLen || obj.URL_ID.Length < obj.MiniLen)
                {
                    return "【" + obj.Alert_Name + "】不足" + obj.MiniLen + "個字元或超過" + obj.MaxLen + "個字元。";
                }
            }
            else if (obj.MiniLen == 1 && obj.MaxLen != 0)
            {
                if (obj.URL_ID.Length > obj.MaxLen || obj.URL_ID.Length < obj.MiniLen)
                {
                    return "【" + obj.Alert_Name + "】不能空白或超過" + obj.MaxLen + "個字元。";
                }
            }
            else if (obj.MiniLen == 0)
            {
                if (obj.URL_ID.Length > obj.MaxLen)
                {
                    return "【" + obj.Alert_Name + "】不能超過" + obj.MaxLen + "個字元。";
                }
            }
            else if (obj.MiniLen == 1 && obj.MaxLen == 0)
            {
                if (obj.URL_ID.Length < obj.MiniLen)
                {
                    return "請選擇【" + obj.Alert_Name + "】。";
                }
            }

            //======================================================

            if (obj.URL_ID.Length > 0)
            {
                if (obj.URL_Type == "int")
                {
                    if (JASON.IsInt(obj.URL_ID) != true)
                    {
                        return "【" + obj.Alert_Name + "】只能輸入數字。";
                    }
                }
                else if (obj.URL_Type == "txt")
                {
                    int_len = obj.URL_ID.Length;
                    value = HttpUtility.HtmlEncode(obj.URL_ID);
                    if (value.Length != int_len)
                    {
                        return "【" + obj.Alert_Name + "】內容包含系統不允許的字元。";
                    }
                }
                else if (obj.URL_Type == "tel")
                {
                    if (JASON.IsTelephone(obj.URL_ID) != true)
                    {
                        return "【" + obj.Alert_Name + "】格式不正確。";
                    }
                }
            }

            //=====================================================
        }
        return "";
    }

    public static bool SYSID_Check(string ID)
    {
        if (ID.Length > 6 || ID.Length < 1) 
        {
            return false;
        }
        return Regex.IsMatch(ID, RegularExp.Numeric);
    }

    public static bool Alphabet_Check(string ID)
    {
        return Regex.IsMatch(ID, RegularExp.Alphabet);
    }

    //========================== 正則運算式 ==========================
    /// <summary>
    /// 判斷字串是否與指定正則運算式匹配
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <param name="regularExp">正則運算式</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsMatch(string input, string regularExp)
    {
        return Regex.IsMatch(input, regularExp);
    }

    /// <summary>
    /// 驗證非負整數（正整數 + 0）
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsUnMinusInt(string input)
    {
        return Regex.IsMatch(input, RegularExp.UnMinusInteger);
    }

    /// <summary>
    /// 驗證正整數
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsPlusInt(string input)
    {
        return Regex.IsMatch(input, RegularExp.PlusInteger);
    }

    /// <summary>
    /// 驗證非正整數（負整數 + 0）
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsUnPlusInt(string input)
    {
        return Regex.IsMatch(input, RegularExp.UnPlusInteger);
    }

    /// <summary>
    /// 驗證負整數
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsMinusInt(string input)
    {
        return Regex.IsMatch(input, RegularExp.MinusInteger);
    }

    /// <summary>
    /// 驗證整數
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsInt(string input)
    {
        return Regex.IsMatch(input, RegularExp.Integer);
    }

    /// <summary>
    /// 驗證非負浮點數（正浮點數 + 0）
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsUnMinusFloat(string input)
    {
        return Regex.IsMatch(input, RegularExp.UnMinusFloat);
    }

    /// <summary>
    /// 驗證正浮點數
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsPlusFloat(string input)
    {
        return Regex.IsMatch(input, RegularExp.PlusFloat);
    }

    /// <summary>
    /// 驗證非正浮點數（負浮點數 + 0）
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsUnPlusFloat(string input)
    {
        return Regex.IsMatch(input, RegularExp.UnPlusFloat);
    }

    /// <summary>
    /// 驗證負浮點數
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsMinusFloat(string input)
    {
        return Regex.IsMatch(input, RegularExp.MinusFloat);
    }

    /// <summary>
    /// 驗證浮點數
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsFloat(string input)
    {
        return Regex.IsMatch(input, RegularExp.Float);
    }

    /// <summary>
    /// 驗證由26個英文字母組成的字串
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsLetter(string input)
    {
        return Regex.IsMatch(input, RegularExp.Letter);
    }

    /// <summary>
    /// 驗證由中文組成的字串
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsChinese(string input)
    {
        return Regex.IsMatch(input, RegularExp.Chinese);
    }

    /// <summary>
    /// 驗證由26個英文字母的大寫組成的字串
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsUpperLetter(string input)
    {
        return Regex.IsMatch(input, RegularExp.UpperLetter);
    }

    /// <summary>
    /// 驗證由26個英文字母的小寫組成的字串
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsLowerLetter(string input)
    {
        return Regex.IsMatch(input, RegularExp.LowerLetter);
    }

    /// <summary>
    /// 驗證由數位和26個英文字母組成的字串
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsNumericOrLetter(string input)
    {
        return Regex.IsMatch(input, RegularExp.NumericOrLetter);
    }

    /// <summary>
    /// 驗證由數位組成的字串
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsNumeric(string input)
    {
        return Regex.IsMatch(input, RegularExp.Numeric);
    }
    /// <summary>
    /// 驗證由數位和26個英文字母或中文組成的字串
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsNumericOrLetterOrChinese(string input)
    {
        return Regex.IsMatch(input, RegularExp.NumbericOrLetterOrChinese);
    }

    /// <summary>
    /// 驗證由數位、26個英文字母或者下劃線組成的字串
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsNumericOrLetterOrUnderline(string input)
    {
        return Regex.IsMatch(input, RegularExp.NumericOrLetterOrUnderline);
    }

    /// <summary>
    /// 驗證email地址
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsEmail(string input)
    {
        return Regex.IsMatch(input, RegularExp.Email);
    }

    /// <summary>
    /// 驗證URL
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsUrl(string input)
    {
        return Regex.IsMatch(input, RegularExp.Url);
    }

    /// <summary>
    /// 驗證電話號碼
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsTelephone(string input)
    {
        return Regex.IsMatch(input, RegularExp.Telephone);
    }

    /// <summary>
    /// 驗證手機號碼
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsMobile(string input)
    {
        return Regex.IsMatch(input, RegularExp.Mobile);
    }

    /// <summary>
    /// 通過檔副檔名驗證圖像格式
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsImageFormat(string input)
    {
        return Regex.IsMatch(input, RegularExp.ImageFormat);
    }

    /// <summary>
    /// 驗證IP
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsIP(string input)
    {
        return Regex.IsMatch(input, RegularExp.IP);
    }

    /// <summary>
    /// 驗證日期（YYYY-MM-DD）
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsDate(string input)
    {
        return Regex.IsMatch(input, RegularExp.Date);
    }

    /// <summary>
    /// 驗證日期和時間（YYYY-MM-DD HH:MM:SS）
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsDateTime(string input)
    {
        return Regex.IsMatch(input, RegularExp.DateTime);
    }

    /// <summary>
    /// 驗證顏色（#ff0000）
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsColor(string input)
    {
        return Regex.IsMatch(input, RegularExp.Color);
    }

    /// <summary>
    /// 驗證車牌號碼
    /// </summary>
    /// <param name="input">要驗證的字串</param>
    /// <returns>驗證通過返回true</returns>
    public static bool IsCar(string input)
    {
        return Regex.IsMatch(input, RegularExp.Car);
    }
}

public class XXS
{
    public string URL_ID;
    public string Alert_Name;
    public int MiniLen;
    public int MaxLen;
    public string URL_Type;
}

public struct RegularExp
{
    public const string Alphabet = "^[A-L]{1}";
    public const string Car = @"^[a-zA-Z0-9]+-[a-zA-Z0-9]+$";
    public const string Chinese = @"^[\u4E00-\u9FA5\uF900-\uFA2D]+$";
    public const string Color = "^#[a-fA-F0-9]{6}";
    public const string Date = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
    public const string DateTime = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";
    public const string Email = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
    public const string Float = @"^(-?\d+)(\.\d+)?$";
    public const string ImageFormat = @"\.(?i:jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$";
    public const string Integer = @"^-?\d+$";
    public const string IP = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
    public const string Letter = "^[A-Za-z]+$";
    public const string LowerLetter = "^[a-z]+$";
    public const string MinusFloat = @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$";
    public const string MinusInteger = "^-[0-9]*[1-9][0-9]*$";
    public const string Mobile = "^0{0,1}13[0-9]{9}$";
    public const string NumbericOrLetterOrChinese = @"^[A-Za-z0-9\u4E00-\u9FA5\uF900-\uFA2D]+$";
    public const string Numeric = "^[0-9]+$";
    public const string NumericOrLetter = "^[A-Za-z0-9]+$";
    public const string NumericOrLetterOrUnderline = @"^\w+$";
    public const string PlusFloat = @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
    public const string PlusInteger = "^[0-9]*[1-9][0-9]*$";
    public const string Telephone = @"(\d+-)?(^\d{2,4}-?\d{7,8}$|^\d{2,4}-?\d{7,8}#?\d{1,6}$|^\d{7,8}|\d{4}-?\d{3}-?\d{3}$)(-\d+)?";
    public const string UnMinusFloat = @"^\d+(\.\d+)?$";
    public const string UnMinusInteger = @"\d+$";
    public const string UnPlusFloat = @"^((-\d+(\.\d+)?)|(0+(\.0+)?))$";
    public const string UnPlusInteger = @"^((-\d+)|(0+))$";
    public const string UpperLetter = "^[A-Z]+$";
    public const string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
}