using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace UserAuditReport.DTO
{
    public class UserChangeDto
    {
        public UserChangeDto(string userName, IEnumerable<string> userRoles, ICollection<ChangedItemDto> changedItems)
        {
            Id = ObjectId.GenerateNewId();
            UserName = userName;
            UserRoles = userRoles;
            ChangedItems = changedItems;
        }
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public ICollection<ChangedItemDto> ChangedItems { get; set; }
    }
}