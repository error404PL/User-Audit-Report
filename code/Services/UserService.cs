using System.Linq;
using System.Web.Security;

namespace UserAuditReport.Services
{
    public class UserService : IUserService
    {
        private readonly ReportSettingsService _reportSettingService;
        public UserService()
        {
            _reportSettingService = new ReportSettingsService();
        }

        public bool IsUserInRole(string username)
        {
            var reportSettings = _reportSettingService.GetReportSettings();
            var userRoles = Roles.GetRolesForUser(username);

            var isUserInTrackedRoles = reportSettings.TrackedRoles.Intersect(userRoles).Any();

            return isUserInTrackedRoles;
        }
    }
}