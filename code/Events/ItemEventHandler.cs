using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using Sitecore.SecurityModel;
using UserAuditReport.Repositories;
using UserAuditReport.Services;

namespace UserAuditReport.Events
{
    public class ItemEventHandler
    {
        private readonly IChangesReportService _changesReportService;

        public ItemEventHandler()
        {
            _changesReportService = new ChangesReportService();
        }

        public void OnItemSaving(object sender, EventArgs args)
        {
            Item item = Event.ExtractParameter(args, 0) as Item;

                if (item != null && item.Database.Name.ToLower().Equals("master") 
                    && item.Paths.IsContentItem)
                {
                    Item originalItem = item.Database.GetItem(item.ID, item.Language, item.Version);
                    
                   _changesReportService.AddOrUpdateSavings(originalItem, item);
                }           
        }

        public void OnItemCopied(object sender, EventArgs args)
        {
            Item item = Event.ExtractParameter(args, 0) as Item;

            if (item != null && item.Database.Name.ToLower().Equals("master")
                && item.Paths.IsContentItem)
            {
                _changesReportService.AddOrUpdateCopies(item);
            }
        }

        public void OnItemDeleting(object sender, EventArgs args)
        {
            Item item = Event.ExtractParameter(args, 0) as Item;

            if (item != null && item.Database.Name.ToLower().Equals("master")
                && item.Paths.IsContentItem)
            {
                _changesReportService.AddOrUpdateRemovals(item);
            }
        }

        public void OnItemMoved(object sender, EventArgs args)
        {
            Item item = Event.ExtractParameter(args, 0) as Item;

            if (item != null && item.Database.Name.ToLower().Equals("master")
                && item.Paths.IsContentItem)
            {
                _changesReportService.AddOrUpdateMoves(item);
            }
        }

        public void OnItemCloned(object sender, EventArgs args)
        {
            Item item = Event.ExtractParameter(args, 0) as Item;

            if (item != null && item.Database.Name.ToLower().Equals("master")
                && item.Paths.IsContentItem)
            {
                _changesReportService.AddOrUpdateClones(item);
            }
        }
    }
}