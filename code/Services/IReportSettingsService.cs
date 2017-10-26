using UserAuditReport.Models;

namespace UserAuditReport.Services
{
    interface IReportSettingsService
    {
        ReportSettings GetReportSettings();
    }
}