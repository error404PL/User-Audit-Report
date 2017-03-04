using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserAuditReport.DTO;

namespace UserAuditReport.Controllers
{
    public class UserAuditReportController : Controller
    {
        public ActionResult GetUsers()
        {
            List<UserChangeDto> list = new List<UserChangeDto>();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}