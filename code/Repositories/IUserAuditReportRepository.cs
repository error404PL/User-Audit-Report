using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserAuditReport.Models;

namespace UserAuditReport.Repositories
{
    public interface IUserAuditReportRepository
    {
        bool Add(UserChange changesInfo);
        
        bool Update(UserChange changesInfo);

        List<UserChange> GetAll();

        UserChange GetByUserName(string userName);
    }
}