using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserAuditReport.DTO;

namespace UserAuditReport.Services
{
    interface IReportSettingsService
    {
        ReportSettingsDTO GetReportSettings();
    }
}