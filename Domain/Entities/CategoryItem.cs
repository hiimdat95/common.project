using Domain.Implements;
using Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class CategoryItem : AuditedEntity, ISoftDelete
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public Guid? ParentId { get; set; }
        public int Order { get; set; } = 0;
        public bool IsDeleted { get; set; }

        public virtual ICollection<IdenticationProfile> IdenticationProfiles { get; set; }
    }
}