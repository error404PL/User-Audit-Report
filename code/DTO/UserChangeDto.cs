using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace UserAuditReport.DTO
{
    public class UserChangeDto
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public ICollection<ChangedItemDto> ChangedItems { get; set; }
    }
}