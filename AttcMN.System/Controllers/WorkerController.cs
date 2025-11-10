using AttcMN.Common.Enums;
using AttcMN.Common.Utils;
using AttcMN.System.Services;
using AttcMN.System.Slave.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.System.Controllers
{
    [ApiDescriptionSettings("System")]
    [Route("construction/workers")]
    public class WorkerController : ControllerBase
    {
        private readonly ILogger<WorkerController> _logger;
        private readonly WorkerService _workerService;
        public WorkerController(ILogger<WorkerController> logger,
            WorkerService workerService)
        {
            _logger = logger;
            _workerService = workerService;
        }
        /// <summary>
        /// Retrieves a paginated list of workers based on the specified query parameters.
        /// </summary>
        /// <remarks>This method processes the query parameters provided in the <paramref name="dto"/>
        /// object  to retrieve a filtered and paginated list of workers. The result includes metadata such as  the
        /// total number of items and pages.</remarks>
        /// <param name="dto">The query parameters used to filter and paginate the worker list.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a  <see
        /// cref="SqlSugarPagedList{Worker}"/> object with the paginated list of workers.</returns>
        [HttpGet("list")]
        [AppAuthorize("construction:workers:list")]
        public async Task<SqlSugarPagedList<Worker>> GetUserList([FromQuery] WorkerDto dto)
        {
            return await _workerService.GetPagedUserListAsync(dto);
        }
        /// <summary>
        /// Retrieves information about a worker based on the specified worker ID.
        /// </summary>
        /// <remarks>This method supports two routes: one without parameters to retrieve default worker
        /// information, and one with a worker ID to retrieve specific worker details.</remarks>
        /// <param name="workerid">The ID of the worker to retrieve. If null, retrieves default worker information.</param>
        /// <returns>An <see cref="AjaxResult"/> containing the worker information. The result includes the worker data under the
        /// <see cref="AjaxResult.DATA_TAG"/> key.</returns>
        [HttpGet("")]
        [HttpGet("{workid}")]
        public async Task<AjaxResult> GetInfo(long? workerid)
        {
            AjaxResult ajax = AjaxResult.Success();
            var worker = await _workerService.GetAsync(workerid);
            ajax.Add(AjaxResult.DATA_TAG, worker);
            return ajax;
        }
        /// <summary>
        /// Adds a new worker to the system.
        /// </summary>
        /// <remarks>This method validates the worker's data before attempting to add it. If the worker's
        /// name or identification number conflicts with existing records, an error is returned.</remarks>
        /// <param name="worker">The worker data to be added, provided as a <see cref="WorkerDto"/> object.</param>
        /// <returns>An <see cref="AjaxResult"/> indicating the outcome of the operation. Returns an error result if the worker's
        /// name or identification number already exists; otherwise, returns a success result with the operation data.</returns>
        [HttpPost]
        [AppAuthorize("construction:workers:add")]
        [TypeFilter(typeof(Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "Worker", BusinessType = BusinessType.INSERT)]
        public async Task<AjaxResult> Add([FromBody] WorkerDto worker)
        {
            if (!string.IsNullOrEmpty(worker.EmpName))
            {
                return AjaxResult.Error("新增用户'" + worker.EmpName + "'失败，手机号码已存在");
            }
            else if (!string.IsNullOrEmpty(worker.Idno))
            {
                return AjaxResult.Error("新增用户'" + worker.Idno + "'失败，邮箱账号已存在");
            }
            var data = _workerService.InsertWorkerAsync(worker);
            return AjaxResult.Success(data);
        }
        /// <summary>
        /// Exports worker data to an Excel file based on the specified criteria.
        /// </summary>
        /// <remarks>This method retrieves a list of workers matching the specified criteria, converts the
        /// data to DTOs, and writes the result to the HTTP response as an Excel file. The response is sent directly to
        /// the client.</remarks>
        /// <param name="dto">The data transfer object containing the criteria for filtering the worker data to be exported.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("export")]
        [AppAuthorize("construction:workers:export")]
        [Log(Title = "Worker", BusinessType = BusinessType.EXPORT)]
        public async Task Export(WorkerDto dto)
        {
            var list = await _workerService.GetWorkerListAsync(dto);
            var dtos = _workerService.ToDtos(list);
            await ExcelUtils.ExportAsync(App.HttpContext.Response, dtos);
        }

        /// <summary>
        /// Import danh sách nhân viên từ file Excel
        /// </summary>
        /// <param name="isUpdateSupport">Có hỗ trợ cập nhật nếu đã tồn tại không</param>
        /// <returns></returns>
        [HttpPost("import")]
        [AppAuthorize("construction:workers:import")]
        [Log(Title = "Worker", BusinessType = BusinessType.IMPORT)]
        public async Task<AjaxResult> Import([FromQuery] bool isUpdateSupport = false)
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                if (file == null || file.Length == 0)
                {
                    return AjaxResult.Error("请选择要导入的文件");
                }

                // Kiểm tra định dạng file
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (fileExtension != ".xlsx" && fileExtension != ".xls")
                {
                    return AjaxResult.Error("只支持 .xlsx 或 .xls 格式的文件");
                }

                // Đọc dữ liệu từ file Excel
                List<WorkerDto> workers;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Quan trọng: reset position về đầu
                    
                    var resultim = await ExcelUtils.ImportAsync<WorkerDto>(memoryStream);
                       workers = resultim.ToList();
                }

                if (workers == null || !workers.Any())
                {
                    return AjaxResult.Error("文件中没有数据");
                }

                // Lấy tên người dùng hiện tại
                var operName = SecurityUtils.GetUsername() ?? "System";

                // Import dữ liệu
                var result = await _workerService.ImportDtosAsync(workers, isUpdateSupport, operName);

                return AjaxResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "导入用户数据失败");
                return AjaxResult.Error($"导入失败：{ex.Message}");
            }
        }

        [HttpPut]
        [AppAuthorize("construction:workers:edit")]
        [TypeFilter(typeof(Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "Worker", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> Edit([FromBody] WorkerDto worker)
        {
            if (!string.IsNullOrEmpty(worker.EmpName))
            {
                return AjaxResult.Error("修改用户'" + worker.EmpName + "'失败，手机号码已存在");
            }
            else if (!string.IsNullOrEmpty(worker.Idno))
            {
                return AjaxResult.Error("修改用户'" + worker.Idno + "'失败，邮箱账号已存在");
            }
            var data = await _workerService.UpdateWorkerAsync(worker);
            return AjaxResult.Success(data);
        }
        [HttpDelete("{workid}")]
        [AppAuthorize("construction:workers:remove")]
        [Log(Title = "Worker", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove(long workid)
        {
            var data = await _workerService.DeleteWorkerAsync(workid);
            return AjaxResult.Success(data);

        }
        [HttpPost("template")]
        public async Task DownloadImportTemplate()
        {
            await ExcelUtils.GetImportTemplateAsync<WorkerDto>(App.HttpContext.Response, "用户数据");
        }
    }
}
