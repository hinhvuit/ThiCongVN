using Microsoft.AspNetCore.Http;
using AttcMN.Framework;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// 摨銝剝隞嗆?撅掩嚗獢?靚嚗?
/// </summary>
[SuppressSniffer]
public static class AppApplicationBuilderExtensions
{
    /// <summary>
    /// 瘜典?箇?銝剝隞塚?撣吁wagger嚗?
    /// </summary>
    /// <param name="app"></param>
    /// <param name="routePrefix">蝛箏?蝚虫葡撠蛹擐△</param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseInject(this IApplicationBuilder app, string routePrefix = default, Action<UseInjectOptions> configure = null)
    {
        // 頧賢銝剝隞園?蝵桅★
        var configureOptions = new UseInjectOptions();
        configure?.Invoke(configureOptions);

        app.UseSpecificationDocuments(routePrefix, UseInjectOptions.SwaggerConfigure, UseInjectOptions.SwaggerUIConfigure);

        return app;
    }

    /// <summary>
    /// 瘜典?箇?銝剝隞塚?撣吁wagger嚗?
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseInject(this IApplicationBuilder app, Action<UseInjectOptions> configure)
    {
        return app.UseInject(default, configure: configure);
    }

    /// <summary>
    /// 瘜典?箇?銝剝隞?
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseInjectBase(this IApplicationBuilder app)
    {
        return app;
    }

    /// <summary>
    /// 閫? .NET6 WebApplication 璅∪?銝?蝥扯??敶?霂舫憸?
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder MapRouteControllers(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }

    /// <summary>
    /// ?舐 Body ??霂餃???
    /// </summary>
    /// <remarks>憿餃 app.UseRouting() 銋?瘜典?</remarks>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder EnableBuffering(this IApplicationBuilder app)
    {
        return app.Use(next => context =>
        {
            context.Request.EnableBuffering();
            return next(context);
        });
    }

    /// <summary>
    /// 瘛餃?摨銝剝隞?
    /// </summary>
    /// <param name="app">摨?遣??/param>
    /// <param name="configure">摨?蔭</param>
    /// <returns>摨?遣??/returns>
    internal static IApplicationBuilder UseApp(this IApplicationBuilder app, Action<IApplicationBuilder> configure = null)
    {
        // 靚?芸?銋???
        configure?.Invoke(app);
        return app;
    }
}
