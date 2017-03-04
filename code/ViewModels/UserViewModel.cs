using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserAuditReport.ViewModels
{
        //    Saving = 0,
        //Deleting = 1,
        //Coping = 2,
        //Moving = 3,
        //Cloning = 4
    public class UserViewModel
    {
        public string UserName { get; set; }

        public int AllFieldsChanged { get; set; }
        public int AllChanges { get; set; }

        public int ItemsSaved { get; set; }

        public int ItemsDeleted { get; set; }

        public int ItemsCopied { get; set; }
        public int ItemsMoved { get; set; }

        public int ItemsCloned { get; set; }
    }
}