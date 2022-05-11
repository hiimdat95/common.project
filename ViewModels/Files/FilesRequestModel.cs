using System;

namespace ViewModels.Files
{
    public class FilesRequestModel
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Extension { get; set; }
        public Decimal Size { get; set; }
        public String Path { get; set; }
        public Guid? EntityId { get; set; }
        public string FileTypeUpload { get; set; }
        public String EntityName { get; set; }
    }
}