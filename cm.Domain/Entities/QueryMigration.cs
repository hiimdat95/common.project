using cm.Domain.Implements;
using System;
using System.Collections.Generic;
using System.Text;

namespace cm.Domain.Entities
{
    public class QueryMigration : Entity<Guid>
    {
        public string Name { get; set; }
        public DateTime MigrationDate { get; set; }
    }
}