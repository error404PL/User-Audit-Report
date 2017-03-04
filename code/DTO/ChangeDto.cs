using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserAuditReport.Enums;

namespace UserAuditReport.DTO
{
    public class ChangeDto
    {
        public ChangeDto(IEnumerable<ChangedFieldDto> changedFields, OperationType operationType)
        {
            ChangedFields = changedFields;
            OperationType = operationType;
            Date = DateTime.UtcNow;
        }

        public ChangeDto(OperationType operationType)
        {
            ChangedFields = new List<ChangedFieldDto>();
            OperationType = operationType;
            Date = DateTime.UtcNow;
        }
        public DateTime Date { get; set; }
        public OperationType OperationType { get; set; }
        public IEnumerable<ChangedFieldDto> ChangedFields { get; set; }
    }
}