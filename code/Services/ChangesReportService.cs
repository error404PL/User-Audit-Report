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
        private readonly IUserAuditReportReposiotry _userAuditReportReposiotry;

        private readonly string[] _skippedFields =
        {
            "__Created by",
            "__Updated by",
            "__Updated",
            "__Revision",
            "__Owner"
        };

        public ChangesReportService()
        {
            _userAuditReportReposiotry = new UserAuditReportReposiotry();
        }
        public void AddOrUpdateChangesForUser(Item oldItem, Item newItem)
        {
            var operation = OperationType.Saving;
            var changedFields = GetChangedFields(oldItem, newItem);
            if (changedFields.Any(x => x.FieldName.Equals("__Name")))
            {
                operation = OperationType.Renaming;
            } else if(changedFields.Any(x => x.FieldName.Equals("__Sortorder")))
            {
                operation = OperationType.Moving;
            } else if (changedFields.Any(x => x.FieldName.Equals("__Created")))
            {
                operation = OperationType.Creating;
            }
            if (!changedFields.Any())
            {
                return;
            }
            var changedFieldsWithoutBaseFields = changedFields.Where(x => !x.FieldName.StartsWith("__"));

            var user = _userAuditReportReposiotry.GetByUserName(newItem.Statistics.UpdatedBy);
            if (user == null)
            {
                var changes = new List<ChangeDto>
                {
                    new ChangeDto(changedFieldsWithoutBaseFields, operation)
                };

                var changedItems = new List<ChangedItemDto>
                {
                    new ChangedItemDto(newItem.ID.ToString() , newItem.Paths.FullPath, newItem.Language.Name, changes)
                };
                var userRoles = RolesInRolesManager.GetRolesForUser(User.FromName(newItem.Statistics.UpdatedBy, false),
                    true).Select(r => r.Name);
                user = new UserChangeDto(newItem.Statistics.UpdatedBy, userRoles, changedItems);

                _userAuditReportReposiotry.Add(user);
            }
            else
            {
                var changedItem = user.ChangedItems.FirstOrDefault(i => i.ItemId.Equals(newItem.ID.ToString()) && i.Language.Equals(newItem.Language.Name));
                if (changedItem == null)
                {
                    changedItem = new ChangedItemDto(newItem.ID.ToString(), newItem.Paths.FullPath, newItem.Language.Name);
                    user.ChangedItems.Add(changedItem);
                }
                changedItem.ItemPath = newItem.Paths.FullPath;
                var change = new ChangeDto(changedFieldsWithoutBaseFields, operation);                 

                changedItem.Changes.Add(change);
                _userAuditReportReposiotry.Update(user);
            }
        }

        private ICollection<ChangedFieldDto> GetChangedFields(Item oldItem, Item newItem)
        {
            newItem.Fields.ReadAll();
            oldItem.Fields.ReadAll();
            var changedFields = new List<ChangedFieldDto>();
            foreach (var fieldName in newItem.Fields.Select(f => f.Name).Where(f => !_skippedFields.Contains(f)))
            {
                if (newItem.Fields[fieldName].Value.Equals(oldItem.Fields[fieldName].Value))
                {
                    continue;
                }

                changedFields.Add(new ChangedFieldDto(fieldName, oldItem.Fields[fieldName].Value, newItem.Fields[fieldName].Value));               
            }
            if (!oldItem.Name.Equals(newItem.Name))
            {
                changedFields.Add(new ChangedFieldDto("__Name", oldItem.Name, newItem.Name));
            }

            if (!oldItem.Paths.FullPath.Equals(newItem.Paths.FullPath))
            {
                changedFields.Add(new ChangedFieldDto("__Full Path", oldItem.Paths.FullPath, newItem.Paths.FullPath));
            }


            return changedFields;
        }
    }
}