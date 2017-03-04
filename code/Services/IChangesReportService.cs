using System.Collections.Generic;
using Sitecore.Data.Items;
using UserAuditReport.Models;
using UserAuditReport.ViewModels;

namespace UserAuditReport.Services
{
    public interface IChangesReportService
    {
        void AddOrUpdateSavings(Item oldItem, Item newItem);

        void AddOrUpdateRemovals(Item item);

        void AddOrUpdateCopies(Item item);

        void AddOrUpdateMoves(Item item);

        void AddOrUpdateClones(Item item);

        List<UserChange> GetAll();

        IEnumerable<UserViewModel> GetUsersOverview(int dateRange);

        UserViewModel GetUserOverview(string username, int dateRange);

        UserDetailsViewModel GetUserDetails(string username, int dateRange);
    }
}