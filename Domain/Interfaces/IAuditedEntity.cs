using System;

namespace Domain.Interfaces
{
    public interface IAuditedEntity
    {
        DateTime CreatedAt { get; set; }
        Guid CreatedBy { get; set; }
        DateTime UpdatedAt { get; set; }
        Guid UpdatedBy { get; set; }
    }
}