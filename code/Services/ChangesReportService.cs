using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using UserAuditReport.DTO;
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
        public void AddOrUpdateChangesForUser(Item oldItem, Item newItem)
        {
            var changedFields = GetChangedFields(oldItem, newItem);
            if (!changedFields.Any())
            {
                return;
            }

            var user = _userAuditReportReposiotry.GetByUserName(newItem.Statistics.UpdatedBy);
            if (user == null)
            {
                var changes = new List<ChangeDto>
                {
                    new ChangeDto()
                    {
                        Date = DateTime.UtcNow,
                        ChangedFields = changedFields
                    }
                };

                var changedItems = new List<ChangedItemDto>
                {
                    new ChangedItemDto()
                    {
                        ItemPath = newItem.Paths.FullPath,
                        Changes = changes
                    }
                };

                user = new UserChangeDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    UserRoles =
                        RolesInRolesManager.GetRolesForUser(User.FromName(newItem.Statistics.UpdatedBy, false), true)
                            .Select(r => r.Name),
                    UserName = newItem.Statistics.UpdatedBy,
                    ChangedItems = changedItems
                };
                _userAuditReportReposiotry.Add(user);
            }
            else
            {
                var changedItem = user.ChangedItems.FirstOrDefault(i => i.ItemPath.Equals(newItem.Paths.FullPath));
                if (changedItem == null)
                {
                    changedItem = new ChangedItemDto()
                    {
                        ItemPath = newItem.Paths.FullPath,
                        Changes = new List<ChangeDto>()
                    };
                    user.ChangedItems.Add(changedItem);
                }


                var change = new ChangeDto()
                {
                    Date = DateTime.Now,
                    ChangedFields = changedFields
                };
                    

                changedItem.Changes.Add(change);
                _userAuditReportReposiotry.Update(user);
            }
        }

        private ICollection<ChangedFieldDto> GetChangedFields(Item oldItem, Item newItem)
        {
            newItem.Fields.ReadAll();
            oldItem.Fields.ReadAll();
            var changedFields = new List<ChangedFieldDto>();
            foreach (var fieldName in newItem.Fields.Select(f => f.Name).Where(f => !f.StartsWith("_")))
            {
                if (newItem.Fields[fieldName].Value.Equals(oldItem.Fields[fieldName].Value))
                {
                    continue;
                }
                var changedField = new ChangedFieldDto()
                {
                    FieldName = fieldName,
                    OldValue = oldItem.Fields[fieldName].Value,
                    NewValue = newItem.Fields[fieldName].Value,
                };
                changedFields.Add(changedField);

                Sitecore.Diagnostics.Log.Info(
                    $"Field: {fieldName} from item {newItem.Paths.FullPath} was changed by {newItem.Statistics.UpdatedBy}", this);
            }

            return changedFields;
        }
    }
}