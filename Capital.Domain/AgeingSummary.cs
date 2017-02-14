using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
  public  class AgeingSummary
    {
      public string CusName { get; set; }
      public int CusId { get; set; }   
      public decimal TotalPremium { get; set; }
      public decimal Overdue { get; set; }
      public decimal Amount1 { get; set; }
      public decimal Amount2 { get; set; }
      public decimal Amount3 { get; set; }
      public decimal Amount4{ get; set; }
      public decimal Amount5 { get; set; }
      public string PolicyNo { get; set; }
      public DateTime Date { get; set; }
      public string Coverage { get; set; }
      public DateTime CommittedDate { get; set; }
      public string CommittedAmount { get; set; }
      public string Paid { get; set; }
      public decimal netamount { get; set; }
      public string TranType { get; set; }
    }
}
