using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("Profiles");
            builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Property(x => x.FirstName).HasColumnType("nvarchar").HasMaxLength(200).IsRequired();
            builder.Property(x => x.MiddleName).HasColumnType("nvarchar").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Name).HasColumnType("nvarchar").HasMaxLength(200).IsRequired();
            builder.Property(x => x.FullName).HasColumnType("nvarchar").IsRequired();
            builder.Property(x => x.ProfileCode).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(x => x.MobileNumber).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.HomePhoneNumber).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.OfficePhoneNumber).HasColumnType("varchar").HasMaxLength(100);
        }
    }
}