using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using Sitecore.SecurityModel;
using UserAuditReport.DTO;
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
                    
                   _changesReportService.AddOrUpdateChangesForUser(originalItem, item);
                }           
        }
    }
}