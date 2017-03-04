using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserAuditReport.Enums
{
    public enum OperationType
    {
        Saving,
        Creating,
        Deleting,
        Cloning,
        Renaming,
        Moving
    }
}