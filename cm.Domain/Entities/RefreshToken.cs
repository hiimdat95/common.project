using cm.Domain.Implements;
using System;

namespace cm.Domain.Entities
{
    public class RefreshToken : Entity
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public Guid UserId { get; set; }
        public string RemoteIpAddress { get; set; }
        public bool Active => DateTime.UtcNow <= Expires;

        public void Deactivate() => Expires = DateTime.UtcNow;
    }
}