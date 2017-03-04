using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserAuditReport.DTO
{
    public class ReportSettingsDTO
    {
        public IEnumerable<string> TrackedRoles { get; set; }
    }
}