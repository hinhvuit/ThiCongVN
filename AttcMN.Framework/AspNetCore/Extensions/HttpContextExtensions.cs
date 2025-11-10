using Microsoft.AspNetCore.Mvc.Controllers;
using AttcMN.Framework;
using System.Text;

namespace Microsoft.AspNetCore.Http;

/// <summary>
/// Http ??蝐?
/// </summary>
[SuppressSniffer]
public static class HttpContextExtensions
{
    /// <summary>
    /// ?瑕? Action ?寞?
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static TAttribute GetMetadata<TAttribute>(this HttpContext httpContext)
        where TAttribute : class
    {
        return httpContext.GetEndpoint()?.Metadata?.GetMetadata<TAttribute>();
    }

    /// <summary>
    /// ?瑕? ?批??Action ?膩??
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static ControllerActionDescriptor GetControllerActionDescriptor(this HttpContext httpContext)
    {
        return httpContext.GetEndpoint()?.Metadata?.FirstOrDefault(u => u is ControllerActionDescriptor) as ControllerActionDescriptor;
    }

    /// <summary>
    /// 霈曄蔭閫???獢??函敶?
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="accessToken"></param>
    public static void SigninToSwagger(this HttpContext httpContext, string accessToken)
    {
        // 霈曄蔭 Swagger ?瑟?芸??
        httpContext.Response.Headers["access-token"] = accessToken;
    }

    /// <summary>
    /// 霈曄蔭閫???獢??箇敶?
    /// </summary>
    /// <param name="httpContext"></param>
    public static void SignoutToSwagger(this HttpContext httpContext)
    {
        httpContext.Response.Headers["access-token"] = "invalid_token";
    }

    /// <summary>
    /// 霈曄蔭??憭?Tokens
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="accessToken"></param>
    /// <param name="refreshToken"></param>
    public static void SetTokensOfResponseHeaders(this HttpContext httpContext, string accessToken, string refreshToken = null)
    {
        httpContext.Response.Headers["access-token"] = accessToken;
        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            httpContext.Response.Headers["x-access-token"] = refreshToken;
        }
    }

    /// <summary>
    /// ?瑕??祆 IPv4?啣?
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetLocalIpAddressToIPv4(this HttpContext context)
    {
        return context.Connection.LocalIpAddress?.MapToIPv4()?.ToString();
    }

    /// <summary>
    /// ?瑕??祆 IPv6?啣?
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetLocalIpAddressToIPv6(this HttpContext context)
    {
        return context.Connection.LocalIpAddress?.MapToIPv6()?.ToString();
    }

    /// <summary>
    /// ?瑕?餈? IPv4?啣?
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetRemoteIpAddressToIPv4(this HttpContext context)
    {
        return context.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
    }

    /// <summary>
    /// ?瑕?餈? IPv6?啣?
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetRemoteIpAddressToIPv6(this HttpContext context)
    {
        return context.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
    }

    /// <summary>
    /// ?瑕?摰霂瑟??啣?
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static string GetRequestUrlAddress(this HttpRequest request)
    {
        return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
    }

    /// <summary>
    /// ?瑕??交??啣?
    /// </summary>
    /// <param name="request"></param>
    /// <param name="refererHeaderKey"></param>
    /// <returns></returns>
    public static string GetRefererUrlAddress(this HttpRequest request, string refererHeaderKey = "Referer")
    {
        return request.Headers[refererHeaderKey].ToString();
    }

    /// <summary>
    /// 霂餃? Body ?捆
    /// </summary>
    /// <param name="httpContext"></param>
    /// <remarks>?? Startup ??Configure 銝剜釣??app.EnableBuffering()</remarks>
    /// <returns></returns>
    public static async Task<string> ReadBodyContentAsync(this HttpContext httpContext)
    {
        if (httpContext == null) return default;
        return await httpContext.Request.ReadBodyContentAsync();
    }

    /// <summary>
    /// 霂餃? Body ?捆
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>?? Startup ??Configure 銝剜釣??app.EnableBuffering()</remarks>
    /// <returns></returns>
    public static async Task<string> ReadBodyContentAsync(this HttpRequest request)
    {
        request.Body.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
        var body = await reader.ReadToEndAsync();

        // ?憿園
        request.Body.Seek(0, SeekOrigin.Begin);
        return body;
    }

    /// <summary>
    /// 撠?<see cref="BadPageResult"/> ???瘚葉
    /// </summary>
    /// <param name="httpResponse"><see cref="HttpResponse"/></param>
    /// <param name="badPageResult"><see cref="BadPageResult"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    public static async ValueTask WriteAsync(this HttpResponse httpResponse, BadPageResult badPageResult, CancellationToken cancellationToken = default)
    {
        await httpResponse.Body.WriteAsync(badPageResult.ToByteArray(), cancellationToken);
    }

    /// <summary>
    /// ?斗?臬??WebSocket 霂瑟?
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static bool IsWebSocketRequest(this HttpContext context)
    {
        return context.WebSockets.IsWebSocketRequest || context.Request.Path == "/ws";
    }
}
