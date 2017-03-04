using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        IEnumerable<UserViewModel> GetUsersOverview();

        UserViewModel GetUserOverview(string username);

        UserDetailsViewModel GetUserDetails(string username);
    }
}