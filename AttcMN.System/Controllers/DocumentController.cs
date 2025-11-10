using AttcMN.Common.Enums;
using AttcMN.Common.Files;
using AttcMN.Common.Utils;
using AttcMN.System.Services;
using Microsoft.Extensions.Configuration;

namespace AttcMN.System.Controllers
{
    /// <summary>
    /// Document management controller
    /// </summary>
    [ApiDescriptionSettings("System")]
    [Route("records/document")]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly DocumentService _documentService;
        private readonly StorageService _storageService;
        private readonly FileUploadUtils.SftpOptions _defaultSftpOptions;

        public DocumentController(
            ILogger<DocumentController> logger,
            DocumentService documentService,
            StorageService storageService,
            IConfiguration configuration)
        {
            _logger = logger;
            _documentService = documentService;
            _storageService = storageService;

            _defaultSftpOptions = configuration
                .GetSection("RuoYiConfig:Sftp")
                .Get<FileUploadUtils.SftpOptions>() ?? new FileUploadUtils.SftpOptions();
        }

        /// <summary>
        /// Get document list with filters
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("records:document:list")]
        public async Task<AjaxResult> GetList([FromQuery] DocumentDto dto)
        {
            var list = await _documentService.GetDtoPagedListAsync(dto);
            return AjaxResult.Success(list);
        }

        /// <summary>
        /// Get document types
        /// </summary>
        [HttpGet("types")]
        [AppAuthorize("records:document:list")]
        public AjaxResult GetDocumentTypes()
        {
            var types = _documentService.GetDocumentTypeList();
            return AjaxResult.Success(types);
        }

        /// <summary>
        /// Get documents by factory id
        /// </summary>
        [HttpGet("factory/{facId:int}")]
        [AppAuthorize("records:document:list")]
        public async Task<AjaxResult> GetByFactoryId(int facId)
        {
            var list = await _documentService.GetListByFacIdAsync(facId);
            return AjaxResult.Success(list);
        }

        /// <summary>
        /// Get documents created by current user
        /// </summary>
        [HttpGet("myDocuments")]
        [AppAuthorize("records:document:list")]
        public async Task<AjaxResult> GetMyDocuments()
        {
            var userId = SecurityUtils.GetUserId();
            var list = await _documentService.GetListByCreateByAsync(userId);
            return AjaxResult.Success(list);
        }

        /// <summary>
        /// Get document by id
        /// </summary>
        [HttpGet("{docId:long}")]
        [AppAuthorize("records:document:query")]
        public async Task<AjaxResult> GetInfo(long docId)
        {
            var entity = await _documentService.GetAsync(docId);
            return AjaxResult.Success(entity);
        }

        /// <summary>
        /// Create new document
        /// </summary>
        [HttpPost("")]
        [AppAuthorize("records:document:add")]
        [TypeFilter(typeof(Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "Document", BusinessType = BusinessType.INSERT)]
        public async Task<AjaxResult> Add([FromBody] DocumentDto dto)
        {
            if (SecurityUtils.GetUsername() != null)
            {
                dto.CreateBy = SecurityUtils.GetUsername();
            }
            var result = await _documentService.InsertDocumentAsync(dto);
            
            return AjaxResult.Success(result);
        }

        /// <summary>
        /// Upload file and attach to existing document
        /// </summary>
        [HttpPost("upload/{docId:long}")]
        [AppAuthorize("records:document:edit")]
        [Log(Title = "Document Upload", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> UploadFile(long docId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return AjaxResult.Error("No file provided");

            try
            {
                // Upload to SFTP
                var remotePath = await FileUploadUtils.UploadToSftpAsync(file, _defaultSftpOptions);

                // Create storage record
                var storageDto = new StorageDto
                {
                    FileId = Guid.NewGuid(),
                    OldName = file.FileName,
                    FilePath = remotePath,
                    FileSize = file.Length,
                    ContentType = file.ContentType,
                    IsDeleted = false
                };

                var inserted = await _storageService.InsertAsync(storageDto);
                if (!inserted)
                {
                    return AjaxResult.Error("Failed to save file metadata");
                }

                // Attach file to document
                var attached = await _documentService.UpdateDocumentFileAsync(storageDto.FileId, docId);
                if (!attached)
                {
                    return AjaxResult.Error("Failed to attach file to document");
                }

                var result = new
                {
                    fileId = storageDto.FileId,
                    fileName = storageDto.OldName,
                    downloadUrl = $"/system/file/download/byid/{storageDto.FileId}",
                    viewUrl = $"/system/file/view/{storageDto.FileId}"
                };

                return AjaxResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload file failed for document {DocId}", docId);
                return AjaxResult.Error("Upload failed: " + ex.Message);
            }
        }

        /// <summary>
        /// Export documents to Excel
        /// </summary>
        [HttpPost("export")]
        [AppAuthorize("records:document:export")]
        [Log(Title = "Document", BusinessType = BusinessType.EXPORT)]
        public async Task Export([FromBody] DocumentDto dto)
        {
            var list = await _documentService.GetlistAsync(dto);
            await ExcelUtils.ExportAsync(App.HttpContext.Response, list);
        }

        /// <summary>
        /// Update document metadata
        /// </summary>
        [HttpPut("")]
        [AppAuthorize("records:document:edit")]
        [TypeFilter(typeof(Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "Document", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> Edit([FromBody] DocumentDto dto)
        {
            var result = await _documentService.UpdateDocumentAsync(dto);
            return AjaxResult.Success(result);
        }

        /// <summary>
        /// Delete document by id
        /// </summary>
        [HttpDelete("{docId:long}")]
        [AppAuthorize("records:document:remove")]
        [Log(Title = "Document", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove(long docId)
        {
            var result = await _documentService.DeleteByDocIDAsync(docId);
            return AjaxResult.Success(result);
        }
    }
}
