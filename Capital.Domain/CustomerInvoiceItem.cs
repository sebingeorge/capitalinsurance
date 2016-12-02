using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
  public  class CustomerInvoiceItem
    {


      public int CusInvoiceItemId { get; set; }
      public int PolicyId { get; set; }
      public int Qty { get; set; }
      public decimal Rate { get; set; }
      public decimal Discount { get; set; }
      public decimal Amount { get; set; }
      public string Unit { get; set; }
      public DateTime? EffectiveDate { get; set; }
      public string TranType { get; set; }
      public string InsCmpName { get; set; }
      public string InsuredName { get; set; }
      public string PolicyNo { get; set; }
    }
}
