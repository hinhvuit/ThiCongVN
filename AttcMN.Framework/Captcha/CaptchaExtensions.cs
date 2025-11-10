using Lazy.Captcha.Core;
using AttcMN.Framework;

namespace Microsoft.Extensions.DependencyInjection;

public static class CaptchaExtensions
{
    public static IServiceCollection AddLazyCaptcha(this IServiceCollection services)
    {
        // CaptchaOptions 的配置参考: https://github.com/pojianbing/LazyCaptcha
        var options = App.GetConfig<CaptchaOptions>(nameof(CaptchaOptions), true);

        return services.AddCaptcha();
    }
}
