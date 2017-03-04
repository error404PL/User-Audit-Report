using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace UserAuditReport.DTO
{
    public class ChangedItemDto
    {
        public string ItemPath { get; set; }
        public ICollection<ChangeDto> Changes { get; set; }
    }
}