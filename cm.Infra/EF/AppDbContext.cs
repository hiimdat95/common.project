using cm.Domain.Configurations;
using cm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

#nullable disable

namespace cm.Infrastructure.EF
{
    public class AppDbContext : AppDbContext<AppUser, AppRole>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }

    public class AppDbContext<TUser, TRole> : IdentityDbContext<TUser, TRole, Guid>
        where TUser : AppUser
        where TRole : AppRole
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<QueryMigration> QueryMigrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Configure using Fluent API
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());

            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new QueryMigrationConfiguration());

            modelBuilder.Ignore<IdentityUserLogin<Guid>>();
            modelBuilder.Ignore<IdentityUserClaim<Guid>>();
            modelBuilder.Ignore<IdentityUserRole<Guid>>();
            modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
            modelBuilder.Ignore<IdentityUserToken<Guid>>();
        }
    }
}