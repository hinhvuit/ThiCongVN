using Microsoft.Extensions.Logging;
using AttcMN.Framework;

namespace AttcMN.System.Controllers
{
    /// <summary>
    /// ?唳?(??甇文???
    /// </summary>
    [Route("monitor/druid")]
    [ApiDescriptionSettings("Monitor")]
    public class DruidController : ControllerBase
    {
        private readonly ILogger<SysOperLogController> _logger;

        public DruidController(ILogger<SysOperLogController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// ?唳?
        /// </summary>
        [HttpGet("")]
        [AppAuthorize("monitor:druid:list")]
        public AjaxResult GetDruidInfo()
        {
            return AjaxResult.Success();
        }
    }
}
