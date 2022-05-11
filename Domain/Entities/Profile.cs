using Domain.Implements;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Profile : AuditedEntity, ISoftDelete
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual AppUser AppUser { get; set; }

        public string UserName { get; set; }
        public string ProfileCode { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string HomePhoneNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<IdenticationProfile> IdenticationProfiles { get; set; }
    }
}