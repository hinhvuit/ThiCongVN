using AttcMN.Common.Enums;
using AttcMN.Data;
using AttcMN.Data.Models;
using AttcMN.Framework.Cache;
using AttcMN.Framework.Utils;
using AttcMN.System.Services;
using SqlSugar;

namespace AttcMN.System.Controllers
{
    /// <summary>
    /// ?函瑪?冽?
    /// </summary>
    [Route("monitor/online")]
    [ApiDescriptionSettings("Monitor")]
    public class SysUserOnlineController : ControllerBase
    {
        private readonly ILogger<SysOperLogController> _logger;
        private readonly ICache _cache;
        private readonly SysUserOnlineService _sysUserOnlineService;

        public SysUserOnlineController(ILogger<SysOperLogController> logger,
            ICache cache,
            SysUserOnlineService sysUserOnlineService)
        {
            _logger = logger;
            _cache = cache;
            _sysUserOnlineService = sysUserOnlineService;
        }

        /// <summary>
        /// ?亥砭???亙?霈啣??”
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("monitor:online:list")]
        public async Task<SqlSugarPagedList<SysUserOnline>> GetSysOperLogList([FromQuery] string ipaddr, [FromQuery] string userName)
        {
            var keys = _cache.GetDbKeys(CacheConstants.LOGIN_TOKEN_KEY + "*", 10000); // ??銝? ??函瑪?冽摨砲銝?頞?1銝?

            List<SysUserOnline> userOnlineList = new List<SysUserOnline>();
            foreach (var key in keys)
            {
                LoginUser user = await _cache.GetAsync<LoginUser>(key);
                if (StringUtils.IsNotEmpty(ipaddr) && StringUtils.IsNotEmpty(userName))
                {
                    userOnlineList.Add(_sysUserOnlineService.GetOnlineByInfo(ipaddr, userName, user));
                }
                else if (StringUtils.IsNotEmpty(ipaddr))
                {
                    userOnlineList.Add(_sysUserOnlineService.GetOnlineByIpaddr(ipaddr, user));
                }
                else if (StringUtils.IsNotEmpty(userName) && user.User != null)
                {
                    userOnlineList.Add(_sysUserOnlineService.GetOnlineByUserName(userName, user));
                }
                else
                {
                    userOnlineList.Add(_sysUserOnlineService.LoginUserToUserOnline(user));
                }
            }
            userOnlineList = userOnlineList.Where(u => u != null).ToList();

            return new SqlSugarPagedList<SysUserOnline>
            {
                Rows = userOnlineList,
                Total = userOnlineList.Count
            };
        }

        /// <summary>
        /// 撘粹?冽
        /// </summary>
        [HttpDelete("{tokenId}")]
        [AppAuthorize("monitor:online:forceLogout")]
        [AttcMN.System.Log(Title = "?函瑪?冽", BusinessType = BusinessType.FORCE)]
        public AjaxResult ForceLogout(string tokenId)
        {
            _cache.Remove(CacheConstants.LOGIN_TOKEN_KEY + tokenId);
            return AjaxResult.Success();
        }
    }
}
