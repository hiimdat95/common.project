using System;

namespace cm.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int? Order { get; set; }
        public bool InUsed { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}