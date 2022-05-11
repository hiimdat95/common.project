using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("AppRoles");
            builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Property(x => x.DisplayName).HasColumnType("nvarchar").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasColumnType("nvarchar").HasMaxLength(200).IsRequired();
        }
    }
}