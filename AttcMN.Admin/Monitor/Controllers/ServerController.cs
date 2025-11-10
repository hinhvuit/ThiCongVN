using Microsoft.Extensions.Logging;
using AttcMN.Framework;
using AttcMN.System.Services;

namespace AttcMN.System.Controllers
{
    /// <summary>
    /// ??函???
    /// </summary>
    [Route("monitor/server")]
    [ApiDescriptionSettings("Monitor")]
    public class ServerController : ControllerBase
    {
        private readonly ILogger<SysOperLogController> _logger;
        private readonly ServerService _serverService;

        public ServerController(ILogger<SysOperLogController> logger, ServerService serverService)
        {
            _logger = logger;
            _serverService = serverService;
        }

        /// <summary>
        /// ??函???
        /// </summary>
        [HttpGet("")]
        [AppAuthorize("monitor:server:list")]
        public AjaxResult GetServerInfo()
        {
            var server = _serverService.GetServerInfo();
            return AjaxResult.Success(server);
        }
    }
}
