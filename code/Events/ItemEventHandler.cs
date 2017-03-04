using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.SecurityModel;

namespace UserAuditReport.Events
{
    public class ItemEventHandler
    {
        public void OnItemSaved(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item savedItem = Event.ExtractParameter(args, 0) as Item;

            //// Allow only non null items and allow only items from the master database
            //if (savedItem != null && savedItem.Database.Name.ToLower() == "master")
            //{
            //    // Do some kind of template validation to limit only the items you actually want

            //    if (savedItem.TemplateID == ID.Parse("{00000000-0000-0000-0000-000000000000}"))
            //    {
            //        // Get the data that you need to populate here

            //        // Start Editing the Item

            //        using (new SecurityDisabler())
            //        {
            //            savedItem.Editing.BeginEdit();

            //            // Do your edits here

            //            savedItem.Editing.EndEdit();
            //        }
            //    }
            //}
        }
    }
}