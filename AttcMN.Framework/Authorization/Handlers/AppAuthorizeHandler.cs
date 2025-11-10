using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace AttcMN.Framework.Authorization;

/// <summary>
/// ??蝑?扯?蝔?
/// </summary>
[SuppressSniffer]
public abstract class AppAuthorizeHandler : IAuthorizationHandler
{
    /// <summary>
    /// ?瑟 Token 頨思遢??
    /// </summary>
    private readonly string[] _refreshTokenClaims = new[] { "f", "e", "s", "l", "k" };

    /// <summary>
    /// ??撉??詨??寞?
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual async Task HandleAsync(AuthorizationHandlerContext context)
    {
        // ?斗?臬??
        var isAuthenticated = context.User.Identity.IsAuthenticated;
        if (isAuthenticated)
        {
            // 蝳迫雿輻?瑟 Token 餈???⊿?
            if (_refreshTokenClaims.All(k => context.User.Claims.Any(c => c.Type == k)))
            {
                context.Fail();
                return;
            }

            await AuthorizeHandleAsync(context);
        }
        else context.GetCurrentHttpContext()?.SignoutToSwagger();    // ??搴wagger?餃?
    }

    /// <summary>
    /// 撉?蝞⊿?
    /// </summary>
    /// <param name="context"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public virtual Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// 蝑撉?蝞⊿?
    /// </summary>
    /// <param name="context"></param>
    /// <param name="httpContext"></param>
    /// <param name="requirement"></param>
    /// <returns></returns>
    public virtual Task<bool> PolicyPipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext, IAuthorizationRequirement requirement)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// ??憭?
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected async Task AuthorizeHandleAsync(AuthorizationHandlerContext context)
    {
        // ?瑕?????撉???瘙?
        var pendingRequirements = context.PendingRequirements;

        // ?瑕? HttpContext 銝???
        var httpContext = context.GetCurrentHttpContext();

        // 靚摮掩蝞⊿?
        var pipeline = await PipelineAsync(context, httpContext);
        if (pipeline)
        {
            // ????撉?
            foreach (var requirement in pendingRequirements)
            {
                // 撉?蝑蝞⊿?
                var policyPipeline = await PolicyPipelineAsync(context, httpContext, requirement);
                if (policyPipeline) context.Succeed(requirement);
                else context.Fail();
            }
        }
        else context.Fail();
    }
}
