using Domain.Implements;
using System;

namespace Domain.Entities
{
    public class QueryMigration : Entity<Guid>
    {
        public string Name { get; set; }
        public DateTime MigrationDate { get; set; }
    }
}