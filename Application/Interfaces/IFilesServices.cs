using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities.Contracts;
using ViewModels.Files;

namespace Application.Interfaces
{
    public partial interface IFilesService
    {
        Task<ServiceResponse> FindByIdFile(Guid id);

        Task<ServiceResponse> FindAllFiles(string keyword = null);

        Task<ServiceResponse> UpdateFile(FilesRequestModel request);

        Task<ServiceResponse> CreateFiles(IList<IFormFile> files, Guid? entityId, string fileTypeUpload, string entityName, bool isPrivate);

        Task<ServiceResponse> DeleteFile(Guid id);

        Task<ServiceResponse> DeleteMultiple(List<Files> files);

        Task<List<Guid>> SaveFilesAsync(IList<IFormFile> files, Guid? entityId, string fileTypeUpload, string entityName, bool isPrivate);

        Task<ServiceResponse> CreateMultiFileType(UploadRequestModel uploadRequest);

        Task<ServiceResponse> DeleteFileUpload(List<DeleteFileUploadModel> param);

        FileContentResult GetFile(Guid fileId);

        //HttpResponseMessage DownloadFile(Guid fileId);
    }
}