using cm.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cm.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>, IEntity<Guid>, IAuditedEntity, ISoftDelete
    {
        public bool IsSystemUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public bool HasValidRefreshToken(string refreshToken) => RefreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);

        public void AddRefreshToken(string token, string remoteIpAddress, double daysToExpire = 7)
        {
            RefreshTokens.Add(new RefreshToken
            {
                Token = token,
                Expires = DateTime.UtcNow.AddDays(daysToExpire),
                UserId = Id,
                RemoteIpAddress = remoteIpAddress
            });
        }

        public void DeactiveActiveRefreshTokens()
        {
            foreach (var active in RefreshTokens.Where(t => t.Active))
            {
                active.Deactivate();
            }
        }

        public void DeactivateRefreshToken(string refreshToken) => RefreshTokens.First(t => t.Token == refreshToken).Deactivate();
    }
}