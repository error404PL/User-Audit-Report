using System;
using System.Collections.Generic;
using UserAuditReport.Enums;

namespace UserAuditReport.Models
{
    public class Change
    {
        public Change(IEnumerable<ChangedField> changedFields, OperationType operationType)
        {
            ChangedFields = changedFields;
            OperationType = operationType;
            Date = DateTime.UtcNow;
        }

        public Change(OperationType operationType)
        {
            ChangedFields = new List<ChangedField>();
            OperationType = operationType;
            Date = DateTime.UtcNow;
        }

        public DateTime Date { get; set; }
        public OperationType OperationType { get; set; }
        public IEnumerable<ChangedField> ChangedFields { get; set; }
    }
}