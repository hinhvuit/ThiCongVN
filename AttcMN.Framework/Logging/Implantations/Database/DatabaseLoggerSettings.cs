using Microsoft.Extensions.Logging;

namespace AttcMN.Framework.Logging;

/// <summary>
/// ?唳摨敹?蝵桃掩
/// </summary>
[SuppressSniffer]
public sealed class DatabaseLoggerSettings
{
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
    /// 敹賜?亙?敺芰颲
    /// </summary>
    /// <remarks>撖寞扯??霈詨蔣??/remarks>
    public bool IgnoreReferenceLoop { get; set; } = true;

    /// <summary>
    /// ?曄內頝葵/霂瑟? Id
    /// </summary>
    public bool WithTraceId { get; set; } = false;

    /// <summary>
    /// ?曄內??獢嚗?摨??瘜倌??
    /// </summary>
    public bool WithStackFrame { get; set; } = false;
}
