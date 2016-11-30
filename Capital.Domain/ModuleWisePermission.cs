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
}
