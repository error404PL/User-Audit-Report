using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserAuditReport.DTO;

namespace UserAuditReport.Services
{
    public class ReportSettingsService : IReportSettingsService
    {
        public ReportSettingsDTO GetReportSettings()
        {
            var settingsItem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(Common.Constants.ReportsSettings.ReportsSettingsItemId);

            Sitecore.Data.Fields.MultilistField trackedRoles = settingsItem.Fields["Tracked Roles"];
            var trackedRolesList = trackedRoles.Items.ToList();

            var reportSetttingDTO = new ReportSettingsDTO()
            {
                   TrackedRoles = trackedRolesList
            };

            return reportSetttingDTO;
        }
    }
}