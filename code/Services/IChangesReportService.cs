using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace UserAuditReport.Services
{
    public interface IChangesReportService
    {
        void AddOrUpdateChangesForUser(Item oldItem, Item newItem);
    }
}