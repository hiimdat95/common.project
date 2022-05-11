using System;

namespace ViewModels.Files
{
    public class FilesResponeModel
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Extension { get; set; }
        public Decimal Size { get; set; }
        public String Path { get; set; }
        public Guid? EntityId { get; set; }
        public int? EntityId_DM { get; set; }
        public Int32 EntityType { get; set; }
        public String EntityName { get; set; }
    }
}