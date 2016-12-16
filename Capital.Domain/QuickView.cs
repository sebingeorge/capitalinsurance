using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
  public class QuickView
    {
        public bool NewPolicy { get; set; }
        public int NoOfNewPolicy { get; set; }
        public bool RenewalPolicy { get; set; }
        public int NoOfRenewalPolicy { get; set; }
        public bool EndorsePolicy { get; set; }
        public int NoOfEndorsePolicy { get; set; }
        public bool SalesTarget { get; set; }
        public int NoOfSalesTarget { get; set; }
        public bool DailyActivity { get; set; }
        public int NoOfDailyActivity { get; set; }

    }
}
