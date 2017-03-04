using Sitecore.Mvc.Controllers;
using System.Web.Mvc;
using UserAuditReport.Services;

namespace UserAuditReport.Controllers
{
    public class UserAuditReportController : SitecoreController
    {
        private readonly IChangesReportService _userChangesReportService;

        public UserAuditReportController()
        {
            _userChangesReportService = new ChangesReportService();
        }

        public ActionResult GetUserOverview(string username, int dateRange)
        {
            var user = _userChangesReportService.GetUserOverview(username, dateRange);
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserDetails(string username, int dateRange)
        {
            var user = _userChangesReportService.GetUserDetails(username, dateRange);
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUsersOverview(string username, int dateRange)
        {
            var users = _userChangesReportService.GetUsersOverview(dateRange, username);
            return Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}