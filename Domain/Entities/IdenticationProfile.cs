using Domain.Implements;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("IdenticationProfiles")]
    public class IdenticationProfile : AuditedEntity
    {
        public Guid TypeIdentificationId { get; set; }

        [ForeignKey(nameof(TypeIdentificationId))]
        public virtual CategoryItem TypeIdentification { get; set; }

        public Guid ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        public virtual Profile Profile { get; set; }
    }
}