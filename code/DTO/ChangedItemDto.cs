using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace UserAuditReport.DTO
{
    public class ChangedItemDto
    {
        public ChangedItemDto(string itemId, string itemPath, string language)
        {
            ItemId = itemId;
            ItemPath = itemPath;
            Language = language;
            Changes = new List<ChangeDto>();
        }

        public ChangedItemDto(string itemId, string itemPath, string language, ICollection<ChangeDto> changes)
        {
            ItemId = itemId;
            ItemPath = itemPath;
            Language = language;
            Changes = changes;
        }

        public string ItemId { get; set; }
        public string ItemPath { get; set; }
        public string Language { get; set; }
        public ICollection<ChangeDto> Changes { get; set; }
    }
}