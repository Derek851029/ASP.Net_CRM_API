using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// fCeventItem 的摘要描述
/// </summary>
public class fCeventItem
{
    // ===== 用來接行事曆 FullCalendar 的值 =====

    public string id { get; set; }
    public string type { get; set; }
    public string Type_0 { get; set; }
    public string Type_1 { get; set; }
    public string Type_2 { get; set; }
    public string Type_3 { get; set; }
    public string Type_4 { get; set; }
    public string Type_5 { get; set; }
    public string Type_6 { get; set; }
    public string Type_7 { get; set; }
    public string Type_8 { get; set; }
    public string Type_9 { get; set; }
    public string title { get; set; }
    public string value { get; set; }
    [JsonConverter(typeof(LongDateConverter))]
    public DateTime start { get; set; }
}

class LongDateConverter : IsoDateTimeConverter
{
    public LongDateConverter()
    {
        DateTimeFormat = "yyyy-MM-dd";
    }
}