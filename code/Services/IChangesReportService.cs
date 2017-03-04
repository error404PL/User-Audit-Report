using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace UserAuditReport.Services
{
    public interface IChangesReportService
    {
        void AddOrUpdateSavings(Item oldItem, Item newItem);

        void AddOrUpdateRemovals(Item item);

        void AddOrUpdateCopies(Item item);
        void AddOrUpdateMoves(Item item);
        void AddOrUpdateClones(Item item);
    }
}