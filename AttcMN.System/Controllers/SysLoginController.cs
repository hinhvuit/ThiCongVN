using AttcMN.Common.Utils;
using AttcMN.Data.Models;
using AttcMN.System.Services;

namespace AttcMN.Admin
{
    /// <summary>
    /// 登录验证
    /// </summary>
    [ApiDescriptionSettings("System")]
    public class SysLoginController : ControllerBase
    {
        private readonly ILogger<SysLoginController> _logger;
        private readonly TokenService _tokenService;

        private readonly SysLoginService _sysLoginService;
        private readonly SysPermissionService _sysPermissionService;
        private readonly SysMenuService _sysMenuService;
        private readonly SysLogininforService _sysLogininforService;
        private readonly FactoryService _factoryService;

        public SysLoginController(ILogger<SysLoginController> logger,
            TokenService tokenService,
            SysLoginService sysLoginService,
            SysPermissionService sysPermissionService,
            SysMenuService sysMenuService,
            SysLogininforService sysLogininforService,
            FactoryService factoryService)
        {
            _logger = logger;
            _tokenService = tokenService;
            _sysLoginService = sysLoginService;
            _sysPermissionService = sysPermissionService;
            _sysMenuService = sysMenuService;
            _sysLogininforService = sysLogininforService;
            _factoryService = factoryService;
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <returns></returns>
        [HttpPost("/login")]
        public async Task<AjaxResult> Login([FromBody]LoginBody loginBody)
        {
            AjaxResult ajax = AjaxResult.Success();
            // 生成令牌
            string token = await _sysLoginService.LoginAsync(loginBody.Username, loginBody.Password, loginBody.Code, loginBody.Uuid);
            ajax.Add(Constants.TOKEN, token);
            return ajax;
        }

        /// <summary>
        /// 退出
        /// </summary>
        [HttpPost("/logout")]
        public AjaxResult Logout()
        {
            LoginUser loginUser = _tokenService.GetLoginUser(App.HttpContext.Request);
            if (loginUser != null)
            {
                string userName = loginUser.UserName;
                // 删除用户缓存记录
                _tokenService.DelLoginUser(loginUser.Token);
                // 记录用户退出日志
                _ = Task.Factory.StartNew(async () =>
                {
                    await _sysLogininforService.AddAsync(userName, Constants.LOGOUT, "退出成功");
                });
            }
            return AjaxResult.Success("退出成功");
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        [HttpGet("/getInfo")]
        public async Task<AjaxResult> GetInfo()
        {
            SysUserDto user = SecurityUtils.GetLoginUser().User;
            // 角色集合
            List<string> roles = await _sysPermissionService.GetRolePermissionAsync(user);
            // 权限集合
            List<string> permissions = _sysPermissionService.GetMenuPermission(user);
            List<Factory> factories = await _factoryService.GetFactoriesByUserIdAsync(user);

            AjaxResult ajax = AjaxResult.Success();
            ajax.Add("user", user);
            ajax.Add("roles", roles);
            ajax.Add("permissions", permissions);
            ajax.Add("factories", factories);

            return ajax;
        }

        /// <summary>
        /// 获取路由信息
        /// </summary>
        [HttpGet("/getRouters")]
        public AjaxResult GetRouters()
        {
            long userId = SecurityUtils.GetUserId();
            List<SysMenu> menus = _sysMenuService.SelectMenuTreeByUserId(userId);
            var treeMenus = _sysMenuService.BuildMenus(menus);
            return AjaxResult.Success(treeMenus);
        }
    }
}