using cm.Domain.Implements;
using System;

namespace cm.Domain.Entities
{
    public class LogActions : AuditedEntity<Guid>
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string IpAddress { get; set; }
        public string Browse { get; set; }
        public string Os { get; set; }
        public string Action { get; set; }
        public string Subsystem { get; set; }
        public string OleValue { get; set; }
        public string NewValue { get; set; }
     
    }
}