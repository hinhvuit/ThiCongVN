using AttcMN.Common.Files;
using AttcMN.Data.Dtos;
using AttcMN.Framework.Exceptions;
using AttcMN.System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;

namespace AttcMN.System.Controllers
{
    [ApiDescriptionSettings("System")]
    [Route("system/file")]
    public class StorageController : ControllerBase
    {
        private readonly ILogger<StorageController> _logger;
        private readonly StorageService _storageService;
        private readonly FileUploadUtils.SftpOptions _defaultSftpOptions;

        public StorageController(ILogger<StorageController> logger,
            StorageService storageService,
            IConfiguration configuration)
        {
            _logger = logger;
            _storageService = storageService;

            // Bind SFTP config from appsettings: "RuoYiConfig:Sftp"
            _defaultSftpOptions = configuration
                .GetSection("RuoYiConfig:Sftp")
                .Get<FileUploadUtils.SftpOptions>() ?? new FileUploadUtils.SftpOptions();
        }

        [HttpGet("{fileId}")]
        public async Task<AjaxResult> GetFileByFileId(Guid fileId)
        {
            var data = await _storageService.GetByFileIdAsync(fileId);
            return AjaxResult.Success(data);
        }

        [HttpGet("list")]
        public async Task<AjaxResult> GetStorageList([FromQuery] StorageDto dto)
        {
            var data = await _storageService.GetStorageListAsync(dto);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// Downloads a file from the server using its unique identifier.
        /// </summary>
        /// <remarks>This method retrieves the file metadata using the provided <paramref name="fileId"/>
        /// and downloads the file from an SFTP server. If the file metadata is not found, a 404 Not Found response is
        /// returned. If an error occurs during the download process, a 400 Bad Request response is returned with an
        /// error message.</remarks>
        /// <param name="fileId">The unique identifier of the file to be downloaded.</param>
        /// <returns>An <see cref="IActionResult"/> containing the file as a binary stream with the appropriate content type and
        /// file name, or an error response if the file cannot be found or the operation fails.</returns>
        [HttpPost("download/byid/{fileId}")]
        public async Task<IActionResult> DownloadByFileId(Guid fileId)
        {
            try
            {
                var storage = await _storageService.GetByFileIdAsync(fileId);
                if (storage == null)
                    return NotFound("File metadata not found.");
                var data = await FileUploadUtils.DownloadFromSftpAsync(storage.FilePath!, _defaultSftpOptions);
                var name = storage.OldName ?? Path.GetFileName(storage.FilePath!);
                return File(data, "application/octet-stream", name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File download by ID failed");
                return BadRequest(AjaxResult.Error("File download failed: " + ex.Message));
            }
        }

        /// <summary>
        /// Uploads a file to an SFTP server and persists its metadata in storage.
        /// Returns the saved StorageDto including ViewUrl.
        /// </summary>
        [HttpPost("upload")]
        public async Task<AjaxResult> UploadFileToSftp(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return AjaxResult.Error("No file uploaded.");
            try
            {
                var remotePath = await FileUploadUtils.UploadToSftpAsync(file, _defaultSftpOptions);
                // Persist storage metadata
                var storageDto = new StorageDto
                {
                    FileId = Guid.NewGuid(),
                    OldName = file.FileName,
                    FilePath = remotePath,
                    FileSize = file.Length,
                    ContentType = file.ContentType,
                    IsDeleted = false
                };
                var insertResult = await _storageService.InsertAsync(storageDto);
                if (insertResult)
                {
                    // Build view URL (absolute when possible, otherwise relative)
                    string relativeView = $"/system/file/view/{storageDto.FileId}";
                    if (Request?.Host.HasValue == true)
                    {
                        storageDto.ViewUrl = $"{Request.Scheme}://{Request.Host}{relativeView}";
                    }
                    else
                    {
                        storageDto.ViewUrl = relativeView;
                    }

                    return AjaxResult.Success(storageDto);
                }
                else
                {
                    return AjaxResult.Error("Failed to save file metadata.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SFTP upload failed");
                return AjaxResult.Error("SFTP upload failed: " + ex.Message);
            }
        }

        /// <summary>
        /// View file inline by file id (preview in browser when supported).
        /// - Sets Content-Disposition to inline so browsers will render images, PDFs, etc.
        /// - Falls back to application/octet-stream if ContentType not available.
        /// </summary>
        [HttpGet("view/{fileId}")]
        public async Task<IActionResult> ViewByFileId(Guid fileId)
        {
            try
            {
                var storage = await _storageService.GetByFileIdAsync(fileId);
                if (storage == null)
                    return NotFound(AjaxResult.Error("File metadata not found."));

                if (string.IsNullOrWhiteSpace(storage.FilePath))
                    return BadRequest(AjaxResult.Error("File path is empty."));

                // Download bytes from SFTP
                var bytes = await FileUploadUtils.DownloadFromSftpAsync(storage.FilePath!, _defaultSftpOptions);

                var name = storage.OldName ?? Path.GetFileName(storage.FilePath!);
                var contentType = string.IsNullOrWhiteSpace(storage.ContentType) ? "application/octet-stream" : storage.ContentType;

                // Prefer inline display; set header to inline. Browser will render if it supports the content type.
                Response.Headers[HeaderNames.ContentDisposition] = $"inline; filename=\"{Uri.EscapeDataString(name)}\"";

                return File(bytes, contentType);
            }
            catch (ServiceException sx)
            {
                _logger.LogWarning(sx, "SFTP file view failed");
                return BadRequest(AjaxResult.Error("File view failed: " + sx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File view by ID failed");
                return BadRequest(AjaxResult.Error("File view failed: " + ex.Message));
            }
        }
    }
}
