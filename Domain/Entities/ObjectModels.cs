using Domain.Implements;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class ObjectModels : AuditedEntity, ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}