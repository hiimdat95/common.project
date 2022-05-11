using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Utilities.Common;
using Utilities.Constants;
using Utilities.Contracts;
using Utilities.Services;
using ViewModels.Files;

namespace Application.Services
{
    public partial class FilesService : Service, IFilesService
    {
        private readonly IBaseRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger<IFilesService> _logger;
        private readonly IFilesRepository _filesRepository;

        public FilesService(
            IBaseRepository repository,
            IConfiguration config
            , IMapper mapper
            , IUnitOfWork unitOfWork
            , ILogger<IFilesService> logger
            , IFilesRepository filesRepository
            , ICurrentPrincipal currentPrincipal
            , IHttpContextAccessor httpContextAccessor) : base(currentPrincipal, httpContextAccessor)
        {
            _repository = repository;
            _config = config;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _filesRepository = filesRepository;
        }

        public async Task<ServiceResponse> FindByIdFile(Guid id)
        {
            var item = await _repository.FistOrDefaultAsync<Files>(x => x.Id == id);
            return Ok(item);
        }

        public async Task<ServiceResponse> FindAllFiles(string keyword = null)
        {
            var item = await _repository.ListAllAsync<Files>();
            return Ok(item);
        }

        public async Task<ServiceResponse> UpdateFile(FilesRequestModel request)
        {
            var entity = await _repository.FistOrDefaultAsync<Files>(x => x.Id == request.Id);

            entity.Name = request.Name;
            entity.Extension = request.Extension;
            entity.Size = request.Size;
            entity.Path = request.Path;
            entity.FileTypeUpload = request.FileTypeUpload;
            entity.EntityName = request.EntityName;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Ok(entity);
        }

        public async Task<ServiceResponse> DeleteFile(Guid id)
        {
            var file = await _repository.FistOrDefaultAsync<Files>(x => x.Id == id);
            string targetPath = Path.Combine(_config.GetSection(SystemConstants.AppSettings.UPLOAD_FOLDER).Value, file.EntityName, file.Path);
            if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }
            _repository.Delete<Files>(file);
            await _unitOfWork.SaveChangesAsync();
            return Ok("", "", "Xóa dữ liệu thành công");
        }

        public async Task<ServiceResponse> DeleteMultiple(List<Files> files)
        {
            foreach (var item in files)
            {
                string targetPath = Path.Combine(_config.GetSection(SystemConstants.AppSettings.UPLOAD_FOLDER).Value, item.EntityName, item.Path);
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
            }
            _repository.DeleteMany<Files>(files);
            await _unitOfWork.SaveChangesAsync();
            return Ok("Xóa thành công");
        }

        public async Task<List<Guid>> SaveFilesAsync(IList<IFormFile> files, Guid? entityId, string fileTypeUpload, string entityName, bool isPrivate)
        {
            List<Guid> result = new List<Guid>();
            string targetPath = Path.Combine(_config.GetSection(SystemConstants.AppSettings.UPLOAD_FOLDER).Value, entityName);
            Directory.CreateDirectory(targetPath);
            if (files != null)
            {
                foreach (IFormFile file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        string fileName = file.FileName;
                        string filePath = Path.Combine(targetPath, fileName);
                        string fileNameWithoutEx = Path.GetFileNameWithoutExtension(filePath);

                        var tmpGuid = Guid.NewGuid();
                        filePath = filePath.Replace(fileNameWithoutEx, $"{fileNameWithoutEx}-{tmpGuid}");

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        var entity = new Files
                        {
                            Id = Guid.NewGuid(),
                            EntityId = entityId,
                            Size = file.Length,
                            FileTypeUpload = fileTypeUpload,
                            Path = filePath.Replace(_config.GetSection(SystemConstants.AppSettings.UPLOAD_FOLDER).Value, "").Replace("\\", "/"),
                            Name = file.FileName,
                            Extension = Path.GetExtension(file.FileName),
                            EntityName = entityName,
                            IsPrivate = isPrivate
                        };
                        await _repository.AddAsync(entity);
                        result.Add(entity.Id);
                    }
                }
            }
            return result;
        }

        public async Task<ServiceResponse> CreateFiles(IList<IFormFile> files, Guid? entityId, string fileTypeUpload, string entityName, bool isPrivate)
        {
            try
            {
                if (files != null && files.Any() && files.Sum(x => x.Length) > SystemConstants.MaxValueFiles)
                {
                    return Forbidden("", SystemConstants.MessageResponse.MessageUploadFile);
                }

                List<Guid> result;
                if (!string.IsNullOrEmpty(entityName))
                {
                    result = (await SaveFilesAsync(files, entityId, fileTypeUpload, entityName, isPrivate));
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    return BadRequest("", "Bạn phải chọn loại đối tượng file cần thêm.");
                }

                return Ok(result, "", "Upload thành công!");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return ServerError("", e.Message);
            }
        }

        public async Task<ServiceResponse> CreateMultiFileType(UploadRequestModel uploadRequest)
        {
            try
            {
                if (!uploadRequest.Files.Any())
                {
                    return BadRequest("", "Bạn chưa đính kèm file upload.");
                }
                var lstUploadInfoModel = JsonConvert.DeserializeObject<List<UploadRequestInfoModel>>(uploadRequest.JsonLstUploadRequestInfo);
                if (!lstUploadInfoModel.Any())
                {
                    return BadRequest("", "Bạn chưa điền thông tin file upload.");
                }
                foreach (var item in lstUploadInfoModel)
                {
                    var file = uploadRequest.Files.Where(x => x.FileName == item.FileName).FirstOrDefault();
                    if (file != null)
                    {
                        await SaveFilesAsync(new List<IFormFile> { file }, item.EntityId, item.FileTypeUpload, item.EntityName, item.IsPrivate);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                return Ok("", "Upload thành công!");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return ServerError("", e.Message);
            }
        }

        public async Task<ServiceResponse> DeleteFileUpload(List<DeleteFileUploadModel> param)
        {
            var result = await _filesRepository.DeleteFileUpload(param);

            if (!result)
                return BadRequest(StatusCodes.Status404NotFound.ToString(),
                    Messages.DELETE_FAIL);

            return Ok(true, StatusCodes.Status200OK.ToString(), Messages.DELETE_SUCCESSFULL);
        }

        public FileContentResult GetFile(Guid fileId)
        {
            var file = _repository.FistOrDefaultAsync<Files>(x => x.Id == fileId).Result;
            if (file == null)
            {
                return null;
            }
            if (file.IsPrivate && (_principal == null || _principal.UserId != file.CreatedBy))
            {
                return null;
            }
            string folderUploadPath = _config.GetSection(SystemConstants.AppSettings.UPLOAD_FOLDER).Value;
            string fileFullPath = folderUploadPath + file.Path;
            if (!File.Exists(fileFullPath))
            {
                return null;
            }
            FileContentResult fileResult = new FileContentResult(File.ReadAllBytes(fileFullPath), GetMimeTypeByFileExtension(file.Extension));
            fileResult.FileDownloadName = file.Name;
            return fileResult;
        }

        private string GetMimeTypeByFileExtension(string fileExt)
        {
            fileExt = fileExt.ToLower().Replace(".", "");
            switch (fileExt)
            {
                case "doc":
                    return "application/msword";

                case "docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

                case "xls":
                    return "application/vnd.ms-excel";

                case "xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                case "jpg":
                    return "image/jpeg";

                case "png":
                    return "image/png";

                default:
                    return "application/pdf";
            }
        }

        //public HttpResponseMessage DownloadFile(Guid fileId)
        //{
        //    var itemFile = _repository.FistOrDefaultAsync<Files>(x => x.Id == fileId);
        //    if (itemFile != null)
        //    {
        //        string folderUploadPath = _config.GetSection(SystemConstants.AppSettings.UPLOAD_FOLDER).Value;
        //        string fileFullPath = folderUploadPath + itemFile.Result.Path;
        //        if (File.Exists(fileFullPath))
        //        {
        //            var buffer = File.ReadAllBytes(fileFullPath);
        //            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK)
        //            {
        //                Content = new ByteArrayContent(buffer)
        //            };
        //            result.Content.Headers.ContentDisposition =
        //                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //                {
        //                    FileName = itemFile.Result.Name
        //                };
        //            result.Content.Headers.ContentType = new MediaTypeHeaderValue(GetMimeTypeByFileExtension(itemFile.Result.Extension));

        //            return result;
        //        }
        //        else
        //        {
        //            return new HttpResponseMessage(HttpStatusCode.NotFound);
        //        }
        //    }
        //    else
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.NotFound);
        //    }
        //}
    }
}