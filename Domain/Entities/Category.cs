using Domain.Implements;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Category : AuditedEntity, ISoftDelete
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int Order { get; set; } = 0;
        public bool IsDeleted { get; set; }
    }
}