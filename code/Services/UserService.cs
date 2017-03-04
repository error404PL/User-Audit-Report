using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace UserAuditReport.Services
{
    public class UserService : IUserService
    {
        private ReportSettingsService reportSettingService { get; set; }
        public UserService()
        {
            this.reportSettingService = new ReportSettingsService();
        }

        public bool IsUserInRole(string username)
        {
            var reportSettings = reportSettingService.GetReportSettings();
            var userRoles = Roles.GetRolesForUser(username);

            var isUserInTrackedRoles = reportSettings.TrackedRoles.Intersect(userRoles).Any();

            return isUserInTrackedRoles;
        }
    }
}