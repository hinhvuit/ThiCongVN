using AttcMN.Common.Enums;
using AttcMN.System.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.System.Controllers
{
    /// <summary>
    /// Provides endpoints for managing system configuration settings.
    /// </summary>
    /// <remarks>This controller is part of the "System" API and is used to handle operations related to
    /// system configuration.</remarks>
    [ApiDescriptionSettings("System")]
    [Route("system/factory")]
    public class FactoryController: ControllerBase
    {
        private readonly ILogger<FactoryController> _logger;
        private readonly FactoryService _factoryService;

        public FactoryController(ILogger<FactoryController> logger,
            FactoryService factoryConfigService)
        {
            _logger = logger;
            _factoryService = factoryConfigService;
        }
        /// <summary>
        /// 新增 部门表
        /// </summary>
        /// 
        [HttpPost("")]
        [AppAuthorize("system:factory:add")]
        [TypeFilter(typeof(Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "factory_add", BusinessType = BusinessType.INSERT)]
        public async Task<AjaxResult> AddFactory([FromBody] FactoryDto factory)
        {
            var data = await _factoryService.InsertFactoryAsync(factory);
            return AjaxResult.Success(data);
        }
        /// <summary>
        /// Retrieves a list of factories based on the specified query parameters.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the factory data. The result
        /// is wrapped in an  <see cref="AjaxResult"/> object, which indicates the success of the operation and includes
        /// the retrieved data.</remarks>
        /// <param name="dto">The query parameters used to filter the factory list.</param>
        /// <returns>An <see cref="AjaxResult"/> containing the list of factories that match the specified criteria.</returns>
        [HttpGet("list")]
        public async Task<AjaxResult> GetFactoryList([FromQuery] FactoryDto dto)
        {
            var data = await _factoryService.GetFactoryListAsync(dto);
            return AjaxResult.Success(data);
        }
        /// <summary>
        /// Retrieves the details of a factory based on its unique identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the factory details. If the
        /// factory with the specified identifier does not exist, the result may indicate an empty or null data
        /// payload.</remarks>
        /// <param name="facId">The unique identifier of the factory to retrieve. Must be a positive integer.</param>
        /// <returns>An <see cref="AjaxResult"/> containing the factory details if found. The <see cref="AjaxResult.Data"/>
        /// property holds the factory information.</returns>
        [HttpGet("{facId:int}")]
        public async Task<AjaxResult> GetFactoryById(int facId)
        {
            var data = await _factoryService.GetFactoryByIdAsync(facId);
            return AjaxResult.Success(data);
        }
        /// <summary>
        /// Retrieves a list of all factories.
        /// </summary>
        /// <remarks>This method fetches all factory records asynchronously and returns them in a
        /// successful response.</remarks>
        /// <returns>An <see cref="AjaxResult"/> containing the list of factories if the operation is successful.</returns>
        [HttpGet("all")]
        public async Task<AjaxResult> GetAllFactories()
        {
            var data = await _factoryService.GetAllFactoriesAsync();
            return AjaxResult.Success(data);
        }
        /// <summary>
        /// Updates the details of an existing factory.
        /// </summary>
        /// <remarks>This method requires the caller to have the "system:factory:edit" permission. It
        /// applies data validation to the input and logs the operation as a factory update.</remarks>
        /// <param name="factory">The <see cref="Factory"/> object containing the updated factory details. Cannot be null.</param>
        /// <returns>An <see cref="AjaxResult"/> indicating the outcome of the operation. The result contains the updated factory
        /// data if the operation is successful.</returns>
        [HttpPut("")]
        [AppAuthorize("system:factory:edit")]
        [TypeFilter(typeof(Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "factory_edit", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> EditFactory([FromBody] Factory factory)
        {
            var data = await _factoryService.UpdateFactoryAsync(factory);
            return AjaxResult.Success(data);
        }
        /// <summary>
        /// Deletes a factory with the specified identifier.
        /// </summary>
        /// <remarks>This operation requires the "system:factory:remove" permission. Ensure the user has
        /// the appropriate authorization before invoking this method.</remarks>
        /// <param name="facId">The unique identifier of the factory to be deleted.</param>
        /// <returns>An <see cref="AjaxResult"/> indicating the success of the operation, including any relevant data.</returns>
        [HttpDelete("{facId}")]
        [AppAuthorize("system:factory:remove")]
        [Log(Title = "factory_delete", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> RemoveFactory(int facId)
        {
            var data = await _factoryService.DeleteFactoryAsync(facId);
            return AjaxResult.Success(data);
        }
    }

}
