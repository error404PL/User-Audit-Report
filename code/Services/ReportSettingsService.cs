using System.Linq;
using UserAuditReport.Models;

namespace UserAuditReport.Services
{
    public class ReportSettingsService : IReportSettingsService
    {
        public ReportSettings GetReportSettings()
        {
            var settingsItem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(Common.Constants.ReportsSettings.ReportsSettingsItemId);

            Sitecore.Data.Fields.MultilistField trackedRoles = settingsItem.Fields["Tracked Roles"];
            var trackedRolesList = trackedRoles.Items.ToList();

            var reportSettting = new ReportSettings()
            {
                   TrackedRoles = trackedRolesList
            };

            return reportSettting;
        }
    }
}