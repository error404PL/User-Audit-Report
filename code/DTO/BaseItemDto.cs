using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserAuditReport.Enums;

namespace UserAuditReport.DTO
{
    public class BaseItemDto
    {
        public string ItemId { get; set; }
        public string ItemPath { get; set; }
        public string Language { get; set; }
    }
}