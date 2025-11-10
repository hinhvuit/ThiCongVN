using Microsoft.AspNetCore.Hosting;
using AttcMN.Framework;
using AttcMN.Framework.Extensions;
using AttcMN.Framework.Reflection;

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// 銝餅?遣?冽?撅掩
/// </summary>
[SuppressSniffer]
public static class HostBuilderExtensions
{
    /// <summary>
    /// Web 銝餅瘜典
    /// </summary>
    /// <param name="hostBuilder">Web銝餅?遣??/param>
    /// <param name="configure"></param>
    /// <returns>IWebHostBuilder</returns>
    public static IWebHostBuilder Inject(this IWebHostBuilder hostBuilder, Action<IWebHostBuilder, InjectOptions> configure = default)
    {
        // 頧賢??蔭?★
        var configureOptions = new InjectOptions();
        configure?.Invoke(hostBuilder, configureOptions);

        // ?瑕?暺恕蝔???蝘?
        var defaultAssemblyName = configureOptions.AssemblyName ?? Reflect.GetAssemblyName(typeof(HostBuilderExtensions));

        //  ?瑕??臬??? ASPNETCORE_HOSTINGSTARTUPASSEMBLIES ?蔭
        var environmentVariables = Environment.GetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUPASSEMBLIES");
        var combineAssembliesName = $"{defaultAssemblyName};{environmentVariables}".ClearStringAffixes(1, ";");

        hostBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, combineAssembliesName);

        // 摰?? Starup嚗圾?單??蜓?箏?券憸?
        hostBuilder.UseStartup<FakeStartup>();
        return hostBuilder;
    }

    /// <summary>
    /// 瘜?銝餅瘜典
    /// </summary>
    /// <param name="hostBuilder">瘜?銝餅瘜典?遣??/param>
    /// <param name="configure"></param>
    /// <returns>IHostBuilder</returns>
    public static IHostBuilder Inject(this IHostBuilder hostBuilder, Action<IHostBuilder, InjectOptions> configure = default)
    {
        // 頧賢??蔭?★
        var configureOptions = new InjectOptions();
        configure?.Invoke(hostBuilder, configureOptions);

        InternalApp.ConfigureApplication(hostBuilder, configureOptions.AutoRegisterBackgroundService);

        return hostBuilder;
    }

    /// <summary>
    /// 瘜典? IWebHostBuilder 靘?蝏辣
    /// </summary>
    /// <typeparam name="TComponent">瘣曄???<see cref="IWebComponent"/></typeparam>
    /// <param name="hostBuilder">Web摨?遣??/param>
    /// <param name="options">蝏辣?</param>
    /// <returns><see cref="IWebHostBuilder"/></returns>
    public static IWebHostBuilder AddWebComponent<TComponent>(this IWebHostBuilder hostBuilder, object options = default)
        where TComponent : class, IWebComponent, new()
    {
        hostBuilder.AddWebComponent<TComponent>(options);

        return hostBuilder;
    }

    /// <summary>
    /// 瘜典? IWebHostBuilder 靘?蝏辣
    /// </summary>
    /// <typeparam name="TComponent">瘣曄???<see cref="IWebComponent"/></typeparam>
    /// <typeparam name="TComponentOptions">蝏辣?</typeparam>
    /// <param name="hostBuilder">Web摨?遣??/param>
    /// <param name="options">蝏辣?</param>
    /// <returns><see cref="IWebHostBuilder"/></returns>
    public static IWebHostBuilder AddWebComponent<TComponent, TComponentOptions>(this IWebHostBuilder hostBuilder, TComponentOptions options = default)
        where TComponent : class, IWebComponent, new()
    {
        hostBuilder.AddWebComponent<TComponent, TComponentOptions>(options);

        return hostBuilder;
    }

    /// <summary>
    /// 瘜典? IWebHostBuilder 靘?蝏辣
    /// </summary>
    /// <param name="hostBuilder"><see cref="IWebHostBuilder"/></param>
    /// <param name="componentType">蝏辣蝐餃?</param>
    /// <param name="options">蝏辣?</param>
    /// <returns><see cref="IWebHostBuilder"/></returns>
    public static IWebHostBuilder AddWebComponent(this IWebHostBuilder hostBuilder, Type componentType, object options = default)
    {
        return hostBuilder;
    }
}
