using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserAuditReport.Services
{
    interface IUserService
    {
        bool IsUserInRole(string username);
    }
}