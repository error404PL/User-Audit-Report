using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserAuditReport.Models;

namespace UserAuditReport.Services
{
    interface IReportSettingsService
    {
        ReportSettings GetReportSettings();
    }
}