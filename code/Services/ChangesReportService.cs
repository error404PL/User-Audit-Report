using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using UserAuditReport.DTO;
using UserAuditReport.Enums;
using UserAuditReport.Repositories;

namespace UserAuditReport.Services
{
    public class ChangesReportService : IChangesReportService
    {
        private readonly IUserAuditReportRepository _userAuditReportReposiotry;

        public ChangesReportService()
        {
            _userAuditReportReposiotry = new UserAuditReportRepository();
        }
        public void AddOrUpdateSavings(Item oldItem, Item newItem)
        {
            var operation = OperationType.Saving;
            var changedFields = GetChangedFields(oldItem, newItem);
          
            if (!changedFields.Any())
            {
                return;
            }
            AddOrUpdate(newItem, operation, changedFields);
        }

        public void AddOrUpdateRemovals(Item item)
        {
            AddOrUpdate(item, OperationType.Deleting, new List<ChangedFieldDto>());
        }

        public void AddOrUpdateCopies(Item item)
        {
            AddOrUpdate(item, OperationType.Coping, new List<ChangedFieldDto>());
        }

        public void AddOrUpdateMoves(Item item)
        {
            AddOrUpdate(item, OperationType.Moving, new List<ChangedFieldDto>());
        }

        public void AddOrUpdateClones(Item item)
        {
            AddOrUpdate(item, OperationType.Cloning, new List<ChangedFieldDto>());
        }

        private ICollection<ChangedFieldDto> GetChangedFields(Item oldItem, Item newItem)
        {
            newItem.Fields.ReadAll();
            oldItem.Fields.ReadAll();
            var changedFields = new List<ChangedFieldDto>();
            foreach (var fieldName in newItem.Fields.Select(f => f.Name).Where(x => !x.StartsWith("__")))
            {
                if (newItem.Fields[fieldName].Value.Equals(oldItem.Fields[fieldName].Value))
                {
                    continue;
                }

                changedFields.Add(new ChangedFieldDto(fieldName, oldItem.Fields[fieldName].Value, newItem.Fields[fieldName].Value));               
            }

            return changedFields;
        }

        private void AddOrUpdate(Item item, OperationType operation, IEnumerable<ChangedFieldDto> changedFields)
        {
            var user = _userAuditReportReposiotry.GetByUserName(item.Statistics.UpdatedBy);
            if (user == null)
            {
                var changes = new List<ChangeDto>
                {
                    new ChangeDto(changedFields, operation)
                };

                var changedItems = new List<ChangedItemDto>
                {
                    new ChangedItemDto(item.ID.ToString() , item.Paths.FullPath, item.Language.Name, changes)
                };

                var userName = item.Statistics.UpdatedBy;
                if (string.IsNullOrEmpty(userName))
                {
                    userName = item.Statistics.CreatedBy;
                }

                var userRoles = RolesInRolesManager.GetRolesForUser(User.FromName(userName, false),
                    true).Select(r => r.Name);
                user = new UserChangeDto(userName, userRoles, changedItems);

                _userAuditReportReposiotry.Add(user);
            }
            else
            {
                var changedItem = user.ChangedItems.FirstOrDefault(i => i.ItemId.Equals(item.ID.ToString()) && i.Language.Equals(item.Language.Name));
                if (changedItem == null)
                {
                    changedItem = new ChangedItemDto(item.ID.ToString(), item.Paths.FullPath, item.Language.Name);
                    user.ChangedItems.Add(changedItem);
                }
                changedItem.ItemPath = item.Paths.FullPath;
                var change = new ChangeDto(changedFields, operation);

                changedItem.Changes.Add(change);
                _userAuditReportReposiotry.Update(user);
            }
        }
    }
}