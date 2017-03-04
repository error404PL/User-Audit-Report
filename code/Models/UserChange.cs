using System.Collections.Generic;
using MongoDB.Bson;

namespace UserAuditReport.Models
{
    public class UserChange
    {
        public UserChange(string userName, IEnumerable<string> userRoles, ICollection<ChangedItem> changedItems)
        {
            Id = ObjectId.GenerateNewId();
            UserName = userName;
            UserRoles = userRoles;
            ChangedItems = changedItems;
        }
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public ICollection<ChangedItem> ChangedItems { get; set; }
    }
}