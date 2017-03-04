using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserAuditReport.DTO;

namespace UserAuditReport.ViewModels
{
    public class ItemSavedViewModel : ItemChangesViewModel
    {
        public IEnumerable<ChangedFieldDto> FieldsChanged { get; set; }
    }
}