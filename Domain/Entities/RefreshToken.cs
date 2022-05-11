using Domain.Implements;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class RefreshToken : Entity
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual AppUser AppUser { get; set; }

        public string RemoteIpAddress { get; set; }
        public bool Active => DateTime.UtcNow <= Expires;

        public void Deactivate() => Expires = DateTime.UtcNow;
    }
}