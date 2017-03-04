using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserAuditReport.DTO
{
    public class ChangeDto
    {
        public DateTime Date { get; set; }
        public IEnumerable<ChangedFieldDto> ChangedFields { get; set; }
    }
}