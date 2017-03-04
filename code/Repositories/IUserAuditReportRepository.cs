using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserAuditReport.DTO;

namespace UserAuditReport.Repositories
{
    public interface IUserAuditReportRepository
    {
        bool Add(UserChangeDto changesInfo);
        UserChangeDto GetByUserName(string userName);
        bool Update(UserChangeDto changesInfo);
    }
}