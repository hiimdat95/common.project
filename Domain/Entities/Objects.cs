using Domain.Implements;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Objects : AuditedEntity, ISoftDelete
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}