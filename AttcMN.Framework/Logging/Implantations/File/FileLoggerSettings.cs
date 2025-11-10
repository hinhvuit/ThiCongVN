using Microsoft.Extensions.Logging;

namespace AttcMN.Framework.Logging;

/// <summary>
/// ?辣?亙??蔭蝐?
/// </summary>
[SuppressSniffer]
public sealed class FileLoggerSettings
{
    /// <summary>
    /// ?亙??辣摰頝臬???隞嗅?嚗??.log 雿蛹????
    /// </summary>
    public string FileName { get; set; } = "application.log";

    /// <summary>
    /// 餈賢??啣歇摮?亙??辣????隞?
    /// </summary>
    public bool Append { get; set; } = true;

    /// <summary>
    /// ?批瘥?銝芣敹?隞嗆?憭批??典之撠?暺恕???塚?????B嚗?撠望 1024 ??鈭?1KB
    /// </summary>
    /// <remarks>憒???鈭砲?潘?????亙??辣憭批?頞鈭砲?蔭撠曹??遣?敹?隞塚??啣?撱箇??亙??辣?賢?閫?嚗?隞嗅?+[??摨].log</remarks>
    public long FileSizeLimitBytes { get; set; } = 0;

    /// <summary>
    /// ?批?憭批?撱箇??亙??辣?圈?嚗?霈斗??嚗???<see cref="FileSizeLimitBytes"/> 雿輻
    /// </summary>
    /// <remarks>憒???鈭砲?潘????頞霂亙澆?隞??敹?隞嗡葉隞仍?閬?</remarks>
    public int MaxRollingFiles { get; set; } = 0;

    /// <summary>
    /// ?雿敹扇敶漣??
    /// </summary>
    public LogLevel MinimumLevel { get; set; } = LogLevel.Trace;

    /// <summary>
    /// ?臬雿輻 UTC ?園?喉?暺恕 false
    /// </summary>
    public bool UseUtcTimestamp { get; set; }

    /// <summary>
    /// ?交??澆???
    /// </summary>
    public string DateFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fffffff zzz dddd";

    /// <summary>
    /// ?臬?舐?亙?銝???
    /// </summary>
    public bool IncludeScopes { get; set; } = true;

    /// <summary>
    /// ?曄內頝葵/霂瑟? Id
    /// </summary>
    public bool WithTraceId { get; set; } = false;

    /// <summary>
    /// ?曄內??獢嚗?摨??瘜倌??
    /// </summary>
    public bool WithStackFrame { get; set; } = false;
}
