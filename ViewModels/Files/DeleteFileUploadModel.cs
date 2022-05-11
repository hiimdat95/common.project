using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.Files
{
    public class DeleteFileUploadModel
    {
        public Guid EntityId { get; set; }
        public string EntityType { get; set; }
        public string FilePath { get; set; }
    }
}
