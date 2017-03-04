using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserAuditReport.Models;

namespace UserAuditReport.ViewModels
{
    public class ItemSavedViewModel : ItemChangesViewModel
    {
        public IEnumerable<ChangedField> FieldsChanged { get; set; }
    }
}