using System.Collections.Generic;

namespace UserAuditReport.Models
{
    public class ChangedItem
    {
        public ChangedItem(string itemId, string itemPath, string language)
        {
            ItemId = itemId;
            ItemPath = itemPath;
            Language = language;
            Changes = new List<Change>();
        }

        public ChangedItem(string itemId, string itemPath, string language, ICollection<Change> changes)
        {
            ItemId = itemId;
            ItemPath = itemPath;
            Language = language;
            Changes = changes;
        }

        public string ItemId { get; set; }
        public string ItemPath { get; set; }
        public string Language { get; set; }
        public ICollection<Change> Changes { get; set; }
    }
}