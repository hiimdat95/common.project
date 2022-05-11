using System;

namespace ViewModels.Files
{
    public class FileAttachResponseModel
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}