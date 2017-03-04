﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using UserAuditReport.DTO;
using UserAuditReport.Enums;
using UserAuditReport.Repositories;
using UserAuditReport.ViewModels;

namespace UserAuditReport.Services
{
    public class ChangesReportService : IChangesReportService
    {
        private readonly IUserService _userService;
        private readonly IUserAuditReportRepository _userAuditReportReposiotry;

        public ChangesReportService()
        {
            _userAuditReportReposiotry = new UserAuditReportRepository();
            _userService = new UserService();
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

        public List<UserChangeDto> GetAll()
        {
            return _userAuditReportReposiotry.GetAll();
        }

        public IEnumerable<UserViewModel> GetUsersOverview()
        {
            DateTime from = DateTime.MinValue;
            DateTime to = DateTime.MaxValue;
            var usersViewModel = new List<UserViewModel>();

            foreach (var user in _userAuditReportReposiotry.GetAll())
            {
                var allChanges = new List<ChangeDto>();
                foreach (var changes in user.ChangedItems.Select(x => x.Changes))
                {
                    allChanges.AddRange(changes);
                }

                var filtredChanges = allChanges.Where(y => y.Date >= from && y.Date <= to);
                var userViewModel = new UserViewModel()
                {
                    UserName = user.UserName,
                    AllChanges = filtredChanges.Count(),
                    AllFieldsChanged = allChanges.Select(x => x.ChangedFields.Count()).Sum(),
                    ItemsCloned = allChanges.Count(y => y.OperationType == OperationType.Cloning),
                    ItemsCopied = allChanges.Count(y => y.OperationType == OperationType.Coping),
                    ItemsDeleted = allChanges.Count(y => y.OperationType == OperationType.Deleting),
                    ItemsMoved = allChanges.Count(y => y.OperationType == OperationType.Moving),
                    ItemsSaved = allChanges.Count(y => y.OperationType == OperationType.Saving)
                };

                usersViewModel.Add(userViewModel);
            }

            return usersViewModel;
        }

        public UserViewModel GetUserOverview(string username)
        {
            DateTime from = DateTime.MinValue;
            DateTime to = DateTime.MaxValue;
            var user = _userAuditReportReposiotry.GetByUserName(username);
            var allChanges = new List<ChangeDto>();
            foreach (var changes in user.ChangedItems.Select(x => x.Changes))
            {
                allChanges.AddRange(changes);
            }

            var filtredChanges = allChanges.Where(y => y.Date >= from && y.Date <= to);
            var userViewModel = new UserViewModel()
            {
                UserName = user.UserName,
                AllChanges = filtredChanges.Count(),
                AllFieldsChanged = allChanges.Select(x => x.ChangedFields.Count()).Sum(),
                ItemsCloned = allChanges.Count(y => y.OperationType == OperationType.Cloning),
                ItemsCopied = allChanges.Count(y => y.OperationType == OperationType.Coping),
                ItemsDeleted = allChanges.Count(y => y.OperationType == OperationType.Deleting),
                ItemsMoved = allChanges.Count(y => y.OperationType == OperationType.Moving),
                ItemsSaved = allChanges.Count(y => y.OperationType == OperationType.Saving)
            };

            return userViewModel;
        }

        public UserDetailsViewModel GetUserDetails(string username)
        {
            DateTime from = DateTime.MinValue;
            DateTime to = DateTime.MaxValue;
            var user = _userAuditReportReposiotry.GetByUserName(username);
            var userViewModel = new UserDetailsViewModel();

            foreach (var item in user.ChangedItems)
            {
                var changes = item.Changes.Where(x => x.Date >= from && x.Date <= to).ToList();
                var itemViewModel = PrepareChangesDetails(changes.FirstOrDefault(x => x.OperationType == OperationType.Cloning),item);
                if (itemViewModel != null)
                {
                    userViewModel.ItemsCloned.Add(itemViewModel);
                }

                itemViewModel = PrepareChangesDetails(changes.FirstOrDefault(x => x.OperationType == OperationType.Coping), item);
                if (itemViewModel != null)
                {
                    userViewModel.ItemsCopied.Add(itemViewModel);
                }

                itemViewModel = PrepareChangesDetails(changes.FirstOrDefault(x => x.OperationType == OperationType.Deleting), item);
                if (itemViewModel != null)
                {
                    userViewModel.ItemsDeleted.Add(itemViewModel);
                }

                itemViewModel = PrepareChangesDetails(changes.FirstOrDefault(x => x.OperationType == OperationType.Moving), item);
                if (itemViewModel != null)
                {
                    userViewModel.ItemsMoved.Add(itemViewModel);
                }

                itemViewModel = PrepareChangesDetails(changes.FirstOrDefault(x => x.OperationType == OperationType.Saving), item, true);
                if (itemViewModel != null)
                {
                    userViewModel.ItemsSaved.Add(itemViewModel);
                }
            }

            return userViewModel;
        }

        private ItemChangesViewModel PrepareChangesDetails(ChangeDto change, ChangedItemDto item, bool areSavingChanges = false)
        {
            if (change == null)
            {
                return null;
            }

            if (areSavingChanges)
            {
                return new ItemSavedViewModel()
                {
                    Id = item.ItemId,
                    Path = item.ItemPath,
                    Date = change.Date.ToString(),
                    FieldsChanged = change.ChangedFields               
                };
            }
            return new ItemChangesViewModel()
            {
                Id = item.ItemId,
                Path = item.ItemPath,
                Date = change.Date.ToString()
            };
        }

        //private List<ItemSavedViewModel> PrepareSavingChanges(ICollection<ChangeDto> changes, ChangedItemDto item)
        //{
        //    return changes.Select(y => new ItemSavedViewModel()
        //    {
        //        Id = item.ItemId,
        //        Path = item.ItemPath,

        //    }).ToList();
        //}

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
            var userName = item.Statistics.UpdatedBy;
            if (string.IsNullOrEmpty(userName))
            {
                userName = item.Statistics.CreatedBy;
            }

            if (!_userService.IsUserInRole(userName))
            {
                return;
            }

            var user = _userAuditReportReposiotry.GetByUserName(userName);
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