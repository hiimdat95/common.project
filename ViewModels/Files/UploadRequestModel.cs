using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ViewModels.Files
{
    public class UploadRequestInfoModel
    {
        public Guid? EntityId { get; set; }
        public string FileTypeUpload { get; set; }
        public string EntityName { get; set; }
        public string FileName { get; set; }
        public bool IsPrivate { get; set; } = false;
    }

    public class UploadRequestModel
    {
        public IList<IFormFile> Files { get; set; }
        public string JsonLstUploadRequestInfo { get; set; }
    }
}