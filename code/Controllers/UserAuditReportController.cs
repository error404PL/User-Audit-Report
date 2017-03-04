using Sitecore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UserAuditReport.Services;

namespace UserAuditReport.Controllers
{
    public class UserAuditReportController : SitecoreController
    {
        private readonly IChangesReportService _userChangeService;

        private readonly IUserService _userService;

        private readonly IReportSettingsService _settingService;

        public UserAuditReportController()
        {
            _userChangeService = new ChangesReportService();
            _userService = new UserService();
            _settingService = new ReportSettingsService();
        }

        [HttpGet]
        public ActionResult GetUsers()
        {
            var list = _userChangeService.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //Robert: Change on post if it's needed.
        [HttpGet]
        public JsonResult GetReportSettings()
        {
            var reportSettingsDTOResult = _settingService.GetReportSettings();
            return Json(reportSettingsDTOResult, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult IsUserInRole(string username)
        {
            var isUserTracked = _userService.IsUserInRole(username);
            return Json(isUserTracked, JsonRequestBehavior.AllowGet);
        }
    }
}