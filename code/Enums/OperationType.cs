using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserAuditReport.Enums
{
    public enum OperationType
    {
        Saving = 0,
        Deleting = 1,
        Coping = 2,
        Moving = 3,
        Cloning = 4
    }
}