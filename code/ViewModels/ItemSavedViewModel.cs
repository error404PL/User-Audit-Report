using System.Collections.Generic;
using UserAuditReport.Models;

namespace UserAuditReport.ViewModels
{
    public class ItemSavedViewModel : ItemChangesViewModel
    {
        public IEnumerable<ChangedField> FieldsChanged { get; set; }
    }
}