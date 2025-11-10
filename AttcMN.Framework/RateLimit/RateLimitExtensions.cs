using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using AttcMN.Framework.Cache.Redis;
using AttcMN.Framework.RateLimit.Memory;
using AttcMN.Framework.RateLimit.Redis;

namespace AttcMN.Framework.RateLimit
{
    /// <summary>
    /// 并发控制, 参考
    /// https://learn.microsoft.com/zh-cn/aspnet/core/performance/rate-limit?view=aspnetcore-7.0
    /// https://github.com/cristipufu/aspnetcore-redis-rate-limiting/tree/master
    /// </summary>
    public static class RateLimitExtensions
    {
        public static IServiceCollection AddConcurrencyLimiter(this IServiceCollection services)
        {
            var cacheConfig = App.GetConfig<CacheConfig>("CacheConfig");
            if (cacheConfig.CacheType == Cache.Enums.CacheType.Memory)
            {
                services.AddRateLimiter(options =>
                {
                    // 全局 并发限流
                    //options.AddPolicy<string, MemoryGlobalRateLimiterPolicy>(LimitType.Default);

                    // ip 并发限流
                    options.AddPolicy<string, MemoryIpRateLimiterPolicy>(LimitType.IP);
                });
            }
            else
            {
                services.AddRateLimiter(options =>
                {
                    // 全局 并发限流
                    options.AddPolicy<string, GlobalRateLimiterPolicy>(LimitType.Default);

                    // ip 并发限流
                    options.AddPolicy<string, IpRateLimiterPolicy>(LimitType.IP);
                });
            }

            return services;
        }
    }
}

