using System.Collections.Generic;

namespace UserAuditReport.Models
{
    public class ReportSettings
    {
        public IEnumerable<string> TrackedRoles { get; set; }
    }
}