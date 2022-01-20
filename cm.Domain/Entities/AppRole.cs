using cm.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace cm.Domain.Entities
{
    public class AppRole : IdentityRole<Guid>, IAuditedEntity, IEntity<Guid>
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }
        public bool IsSystemRole { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public int Order { get; set; }
    }
}