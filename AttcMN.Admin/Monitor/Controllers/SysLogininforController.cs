using Microsoft.Extensions.Logging;
using AttcMN.Common.Enums;
using AttcMN.Common.Utils;
using AttcMN.Data.Dtos;
using AttcMN.System.Services;
using SqlSugar;
using System.IO;

namespace AttcMN.System.Controllers
{
    /// <summary>
    /// 蝟餌?霈輸霈啣?
    /// </summary>
    [ApiDescriptionSettings("Monitor")]
    [Route("monitor/logininfor")]
    public class SysLogininforController : ControllerBase
    {
        private readonly ILogger<SysLogininforController> _logger;
        private readonly SysLogininforService _sysLogininforService;
                
        public SysLogininforController(ILogger<SysLogininforController> logger,
            SysLogininforService sysLogininforService)
        {
            _logger = logger;
            _sysLogininforService = sysLogininforService;
        }

        /// <summary>
        /// ?亥砭蝟餌?霈輸霈啣??”
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("system:logininfor:list")]
        public async Task<SqlSugarPagedList<SysLogininforDto>> GetSysLogininforList([FromQuery] SysLogininforDto dto)
        {
            return await _sysLogininforService.GetDtoPagedListAsync(dto);
        }

        /// <summary>
        /// ?瑕? 蝟餌?霈輸霈啣? 霂衣?靽⊥
        /// </summary>
        [HttpGet("")]
        [HttpGet("{id}")]
        [AppAuthorize("system:logininfor:query")]
        public async Task<AjaxResult> Get(long id)
        {
            var data = await _sysLogininforService.GetDtoAsync(id);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// ?啣? 蝟餌?霈輸霈啣?
        /// </summary>
        [HttpPost("")]
        [AppAuthorize("system:logininfor:add")]
        [TypeFilter(typeof(AttcMN.Framework.DataValidation.DataValidationFilter))]
        [AttcMN.System.Log(Title = "蝟餌?霈輸霈啣?", BusinessType = BusinessType.INSERT)]
        public async Task<AjaxResult> Add([FromBody] SysLogininforDto dto)
        {
            var data = await _sysLogininforService.InsertAsync(dto);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 靽格 蝟餌?霈輸霈啣?
        /// </summary>
        [HttpPut("")]
        [AppAuthorize("system:logininfor:edit")]
        [TypeFilter(typeof(AttcMN.Framework.DataValidation.DataValidationFilter))]
        [AttcMN.System.Log(Title = "蝟餌?霈輸霈啣?", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> Edit([FromBody] SysLogininforDto dto)
        {
            var data = await _sysLogininforService.UpdateAsync(dto);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// ? 蝟餌?霈輸霈啣?
        /// </summary>
        [HttpDelete("{ids}")]
        [AppAuthorize("system:logininfor:remove")]
        [AttcMN.System.Log(Title = "蝟餌?霈輸霈啣?", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove([ModelBinder] long[] ids)
        {
            var data = await _sysLogininforService.DeleteAsync(ids);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 撖澆 蝟餌?霈輸霈啣?
        /// </summary>
        [HttpPost("import")]
        [AppAuthorize("system:logininfor:import")]
        [AttcMN.System.Log(Title = "蝟餌?霈輸霈啣?", BusinessType = BusinessType.IMPORT)]
        public async Task Import([Required] IFormFile file)
        {
            var stream = new MemoryStream();
            file.CopyTo(stream);
            var list = await ExcelUtils.ImportAsync<SysLogininforDto>(stream);
            await _sysLogininforService.ImportDtoBatchAsync(list);
        }

        /// <summary>
        /// 撖澆 蝟餌?霈輸霈啣?
        /// </summary>
        [HttpPost("export")]
        [AppAuthorize("system:logininfor:export")]
        [AttcMN.System.Log(Title = "蝟餌?霈輸霈啣?", BusinessType = BusinessType.EXPORT)]
        public async Task Export(SysLogininforDto dto)
        {
            var list = await _sysLogininforService.GetDtoListAsync(dto);
            await ExcelUtils.ExportAsync(App.HttpContext.Response, list);
        }
    }
}
