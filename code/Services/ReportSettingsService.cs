using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            var reportSetttingDTO = new ReportSettings()
            {
                   TrackedRoles = trackedRolesList
            };

            return reportSetttingDTO;
        }
    }
}