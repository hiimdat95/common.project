using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class FileConfiguration : IEntityTypeConfiguration<Files>
    {
        public void Configure(EntityTypeBuilder<Files> builder)
        {
            builder.ToTable("Files");
            builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Property(x => x.Name).HasColumnType("nvarchar").HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Extension).HasColumnType("varchar").HasMaxLength(20).IsRequired();
            builder.Property(x => x.EntityName).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Path).HasColumnType("nvarchar(MAX)").IsRequired();
            builder.Property(x => x.FileTypeUpload).HasColumnType("varchar(100)").IsRequired();
        }
    }
}