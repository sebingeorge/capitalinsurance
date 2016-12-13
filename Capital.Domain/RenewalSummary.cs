using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
  public  class RenewalSummary
    {
      public int MonthCode { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public int CountofExpired { get; set; }
        public int CountofRenewed { get; set; }
    }
}
