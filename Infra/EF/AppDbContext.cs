using Domain.Configurations;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

#nullable disable

namespace Infrastructure.EF
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
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryItem> CategoryItems { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<IdenticationProfile> IdenticationProfiles { get; set; }
        public DbSet<Files> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Configure using Fluent API
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new QueryMigrationConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryItemConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileConfiguration());
            modelBuilder.ApplyConfiguration(new FileConfiguration());

            modelBuilder.Ignore<IdentityUserLogin<Guid>>();
            modelBuilder.Ignore<IdentityUserClaim<Guid>>();
            modelBuilder.Ignore<IdentityUserRole<Guid>>();
            modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
            modelBuilder.Ignore<IdentityUserToken<Guid>>();
        }
    }
}