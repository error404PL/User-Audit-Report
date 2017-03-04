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
        //Robert: Change on post if it's needed.
        [HttpGet]
        public JsonResult GetReportSettings()
        {
            var settingService = new ReportSettingsService();
            var reportSettingsDTOResult = settingService.GetReportSettings();

            return Json(reportSettingsDTOResult, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult IsUserInRole(string username)
        {
            var userService = new UserService();
            var isUserTracked = userService.IsUserInRole(username);

            return Json(isUserTracked, JsonRequestBehavior.AllowGet);
        }
    }
}