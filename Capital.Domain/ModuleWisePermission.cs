using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
    public class ModuleWisePermission
    {
        public bool Admin { get; set; }
        public bool Documentation { get; set; }
        public bool Sales { get; set; }
        public bool Finance { get; set; }
        public bool MISReports { get; set; }
    }
    public class FormPermission
    {
        public int FormId { get; set; }
        public string ControllerName { get; set; }
        public string Action { get; set; }
        public string FormName { get; set; }
        public int ModuleId { get; set; }
    }
}
