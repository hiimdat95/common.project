using cm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace cm.Infrastructure.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
         

            base.OnModelCreating(modelBuilder);
        }
    }
}