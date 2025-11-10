using AttcMN.Framework.RemoteRequest;

namespace AttcMN.Framework.RemoteRequest;

/// <summary>
/// 餈?霂瑟??掩
/// </summary>
[SuppressSniffer]
public static class Http
{
    /// <summary>
    /// ?瑕?餈?霂瑟?隞??
    /// </summary>
    /// <typeparam name="THttpDispatchProxy">餈?霂瑟?隞??撖寡情</typeparam>
    /// <returns><see cref="GetHttpProxy{THttpDispatchProxy}"/></returns>
    public static THttpDispatchProxy GetHttpProxy<THttpDispatchProxy>()
        where THttpDispatchProxy : class, IHttpDispatchProxy
    {
        return App.GetService<THttpDispatchProxy>(App.RootServices);
    }
}
