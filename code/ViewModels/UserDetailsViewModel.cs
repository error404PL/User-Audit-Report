using System.Collections.Generic;

namespace UserAuditReport.ViewModels
{
    public class UserDetailsViewModel
    {
        public UserDetailsViewModel()
        {
            ItemsSaved = new List<ItemChangesViewModel>();
            ItemsDeleted = new List<ItemChangesViewModel>();
            ItemsCopied = new List<ItemChangesViewModel>();
            ItemsMoved = new List<ItemChangesViewModel>();
            ItemsCloned = new List<ItemChangesViewModel>();
        }

        public string UserName { get; set; }
        public List<ItemChangesViewModel> ItemsSaved { get; set; }
        public List<ItemChangesViewModel> ItemsDeleted { get; set; }
        public List<ItemChangesViewModel> ItemsCopied { get; set; }
        public List<ItemChangesViewModel> ItemsMoved { get; set; }
        public List<ItemChangesViewModel> ItemsCloned { get; set; }
    }
}