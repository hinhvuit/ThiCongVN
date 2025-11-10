using AttcMN.Framework;
using Microsoft.AspNetCore.Cors.Infrastructure;
using AttcMN.Framework.CorsAccessor;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 頝典?霈輸???蝐?
/// </summary>
[SuppressSniffer]
public static class CorsAccessorServiceCollectionExtensions
{
    /// <summary>
    /// ?蔭頝典?
    /// </summary>
    /// <param name="services">???</param>
    /// <param name="corsOptionsHandler"></param>
    /// <param name="corsPolicyBuilderHandler"></param>
    /// <returns>???</returns>
    public static IServiceCollection AddCorsAccessor(this IServiceCollection services, Action<CorsOptions> corsOptionsHandler = default, Action<CorsPolicyBuilder> corsPolicyBuilderHandler = default)
    {
        // 瘛餃?頝典??蔭?★
        services.AddConfigurableOptions<CorsAccessorSettingsOptions>();

        // ?瑕??★
        var corsAccessorSettings = App.GetConfig<CorsAccessorSettingsOptions>("CorsAccessorSettings", true);

        // 瘛餃?頝典??
        services.AddCors(options =>
        {
            // 瘛餃?蝑頝典?
            options.AddPolicy(corsAccessorSettings.PolicyName, builder =>
            {
                // 霈曄蔭頝典?蝑
                Penetrates.SetCorsPolicy(builder, corsAccessorSettings);

                // 瘛餃??芸?銋?蝵?
                corsPolicyBuilderHandler?.Invoke(builder);
            });

            // 瘛餃??芸?銋?蝵?
            corsOptionsHandler?.Invoke(options);
        });

        return services;
    }
}
