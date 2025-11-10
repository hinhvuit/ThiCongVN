using AttcMN.Common.Enums;
using AttcMN.Common.Utils;
using AttcMN.Data.Dtos;
using AttcMN.System.Services;
using SqlSugar;

namespace AttcMN.System.Controllers
{
    /// <summary>
    /// ???亙?霈啣?
    /// </summary>
    [Route("monitor/operlog")]
    [ApiDescriptionSettings("Monitor")]
    public class SysOperLogController : ControllerBase
    {
        private readonly ILogger<SysOperLogController> _logger;
        private readonly SysOperLogService _sysOperLogService;

        public SysOperLogController(ILogger<SysOperLogController> logger,
            SysOperLogService sysOperLogService)
        {
            _logger = logger;
            _sysOperLogService = sysOperLogService;
        }

        /// <summary>
        /// ?亥砭???亙?霈啣??”
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("system:log:list")]
        public async Task<SqlSugarPagedList<SysOperLogDto>> GetSysOperLogList([FromQuery] SysOperLogDto dto)
        {
            return await _sysOperLogService.GetDtoPagedListAsync(dto);
        }

        /// <summary>
        /// ? ???亙?霈啣?
        /// </summary>
        [HttpDelete("{ids}")]
        [AppAuthorize("system:log:remove")]
        [AttcMN.System.Log(Title = "???亙?", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove([ModelBinder] long[] ids)
        {
            var data = await _sysOperLogService.DeleteAsync(ids);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// ? ???亙?霈啣?
        /// </summary>
        [HttpDelete("clean")]
        [AppAuthorize("system:log:remove")]
        [AttcMN.System.Log(Title = "???亙?", BusinessType = BusinessType.CLEAN)]
        public AjaxResult Clean()
        {
            _sysOperLogService.Clean();
            return AjaxResult.Success();
        }

        /// <summary>
        /// 撖澆 ???亙?霈啣?
        /// </summary>
        [HttpPost("export")]
        [AppAuthorize("system:log:export")]
        [AttcMN.System.Log(Title = "???亙?", BusinessType = BusinessType.EXPORT)]
        public async Task Export(SysOperLogDto dto)
        {
            var list = await _sysOperLogService.GetDtoListAsync(dto);
            await ExcelUtils.ExportAsync(App.HttpContext.Response, list);
        }
    }
}
