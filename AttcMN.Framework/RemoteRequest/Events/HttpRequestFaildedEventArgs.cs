namespace AttcMN.Framework.RemoteRequest;

/// <summary>
/// 餈?霂瑟?憭梯揖鈭辣蝐?
/// </summary>
[SuppressSniffer]
public sealed class HttpRequestFaildedEventArgs : EventArgs
{
    /// <summary>
    /// ???
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <param name="exception"></param>
    public HttpRequestFaildedEventArgs(HttpRequestMessage request, HttpResponseMessage response, Exception exception)
    {
        Request = request;
        Response = response;
        Exception = exception;
    }

    /// <summary>
    /// 霂瑟?撖寡情
    /// </summary>
    public HttpRequestMessage Request { get; internal set; }

    /// <summary>
    /// ??撖寡情
    /// </summary>
    public HttpResponseMessage Response { get; internal set; }

    /// <summary>
    /// 撘虜撖寡情
    /// </summary>
    public Exception Exception { get; internal set; }
}
