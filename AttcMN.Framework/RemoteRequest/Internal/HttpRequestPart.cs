using AttcMN.Framework.JsonSerialization;
using System.Text;

namespace AttcMN.Framework.RemoteRequest;

/// <summary>
/// Http 霂瑟?撖寡情蝏??其辣
/// </summary>
public sealed partial class HttpRequestPart
{
    /// <summary>
    /// ?撩?窈瘙隞?
    /// </summary>
    public static HttpRequestPart Default()
    {
        return new();
    }

    /// <summary>
    /// 霂瑟??啣?
    /// </summary>
    public string RequestUrl { get; private set; }

    /// <summary>
    /// Url ?啣?璅⊥
    /// </summary>
    public IDictionary<string, object> Templates { get; private set; }

    /// <summary>
    /// 霂瑟??孵?
    /// </summary>
    public HttpMethod Method { get; private set; }

    /// <summary>
    /// 霂瑟??交?憭?
    /// </summary>
    public IDictionary<string, object> Headers { get; private set; }

    /// <summary>
    /// ?亥砭?
    /// </summary>
    public IDictionary<string, object> Queries { get; private set; }

    /// <summary>
    /// 敹賜?潔蛹 null ??Url ?
    /// </summary>
    public bool IgnoreNullValueQueries { get; private set; }

    /// <summary>
    /// 摰Ｘ蝡臬?蝘?
    /// </summary>
    public string ClientName { get; private set; }

    /// <summary>
    /// 摰Ｘ蝡舀?靘?
    /// </summary>
    public Func<HttpClient> ClientProvider { get; private set; }

    /// <summary>
    /// 霂瑟??交? Body ?
    /// </summary>
    public object Body { get; private set; }

    /// <summary>
    /// 霂瑟??交? Body ?捆蝐餃?
    /// </summary>
    public string ContentType { get; private set; } = "application/json";

    /// <summary>
    /// ?捆蝻?
    /// </summary>
    public Encoding ContentEncoding { get; private set; } = Encoding.UTF8;

    /// <summary>
    /// 銝??辣
    /// </summary>
    public List<HttpFile> Files { get; private set; } = new();

    /// <summary>
    /// JSON 摨???靘
    /// </summary>
    public Type JsonSerializerProvider { get; private set; } = typeof(SystemTextJsonSerializerProvider);

    /// <summary>
    /// JSON 摨???蝵桅★
    /// </summary>
    public object JsonSerializerOptions { get; private set; }

    /// <summary>
    /// ?臬?舐璅∪?撉?
    /// </summary>
    public (bool Enabled, bool IncludeNull) ValidationState { get; private set; } = (false, false);

    /// <summary>
    /// ?遣霂瑟?撖寡情?行??
    /// </summary>
    public List<Action<HttpClient, HttpRequestMessage>> RequestInterceptors { get; private set; } = new List<Action<HttpClient, HttpRequestMessage>>();

    /// <summary>
    /// ?遣摰Ｘ蝡臬笆鞊⊥?芸
    /// </summary>
    public List<Action<HttpClient>> HttpClientInterceptors { get; private set; } = new List<Action<HttpClient>>();

    /// <summary>
    /// 霂瑟????行??
    /// </summary>
    public List<Action<HttpClient, HttpResponseMessage>> ResponseInterceptors { get; private set; } = new List<Action<HttpClient, HttpResponseMessage>>();

    /// <summary>
    /// 霂瑟?撘虜?行??
    /// </summary>
    public List<Action<HttpClient, HttpResponseMessage, string>> ExceptionInterceptors { get; private set; } = new List<Action<HttpClient, HttpResponseMessage, string>>();

    /// <summary>
    /// 霈曄蔭霂瑟?雿??
    /// </summary>
    public IServiceProvider RequestScoped { get; private set; }

    /// <summary>
    /// 霈曄蔭??蝑
    /// </summary>
    public (int NumRetries, int RetryTimeout)? RetryPolicy { get; private set; }

    /// <summary>
    /// ?舀? GZip ?憬/??蝻?
    /// </summary>
    public bool SupportGZip { get; private set; } = false;

    /// <summary>
    /// ?臬撖?Url 餈? Uri.EscapeDataString
    /// </summary>
    public bool EncodeUrl { get; private set; } = true;

    /// <summary>
    /// 霈曄蔭 Http 霂瑟??
    /// </summary>
    public string HttpVersion { get; private set; } = "1.1";
}
