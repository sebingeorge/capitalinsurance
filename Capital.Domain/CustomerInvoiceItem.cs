using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
  public  class CustomerInvoiceItem
    {

      public int CusInvoiceId { get; set; }
      public int CusInvoiceItemId { get; set; }
      public int PolicyId { get; set; }
      public int Qty { get; set; }
      public decimal TotalPremium { get; set; }
      public DateTime? EffectiveDate { get; set; }
      public string TranType { get; set; }
      public string InsCmpName { get; set; }
      public string InsuredName { get; set; }
      public string PolicyNo { get; set; }
      public string Remarks { get; set; }
    
    }
}
