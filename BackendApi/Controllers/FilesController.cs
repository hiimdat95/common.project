using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Contracts;
using ViewModels.Files;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class FilesController : ControllerBase
    {
        private readonly IFilesService _itemService;

        public FilesController(
            IFilesService itemService
            )
        {
            _itemService = itemService;
        }

        [HttpDelete("{id}")]
        public async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            var result = await _itemService.DeleteFile(id);
            return result;
        }

        [HttpPost]
        public async Task<ServiceResponse> CreateAsync(IList<IFormFile> files, Guid? entityId, string fileTypeUpload, string entityName, bool isPrivate = false)
        {
            var result = await _itemService.CreateFiles(files, entityId, fileTypeUpload, entityName, isPrivate);
            return result;
        }

        [HttpPost("create-multi-file")]
        public async Task<ServiceResponse> CreateMultiFileType([FromForm] UploadRequestModel lstUploadRequest)
        {
            var result = await _itemService.CreateMultiFileType(lstUploadRequest);
            return result;
        }

        /// <summary>
        /// Xóa tất cả file upload thảo mãn các tham số truyền vào
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("xoa-fileupload")]
        public async Task<ServiceResponse> DeleteFileUpload(List<DeleteFileUploadModel> param)
        {
            return await _itemService.DeleteFileUpload(param);
        }

        /// <summary>
        /// Tải file
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(string id)
        {
            return string.IsNullOrEmpty(id) ? null : await Task.FromResult(_itemService.GetFile(Guid.Parse(id)));
        }
    }
}