using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserAuditReport.DTO
{
    public class ChangedFieldDto
    {
        public ChangedFieldDto(string fieldName, string oldValue, string newValue)
        {
            FieldName = fieldName;
            OldValue = oldValue;
            NewValue = newValue;
        }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}