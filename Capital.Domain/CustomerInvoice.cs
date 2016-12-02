using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Capital.Domain
{
  public  class CustomerInvoice
    {
        public int CusInvoiceId { get; set; }
        public string CusInvoiceRefNo { get; set; }
        public DateTime? CusInvoiceDate { get; set; }
        public int PolicyId { get; set; }
        public string SpecialRemarks { get; set; }
        public string PaymentTerms { get; set; }
        public string Addition { get; set; }
        public string Deduction { get; set; }
        public string AdditionRemarks { get; set; }
        public string DeductionRemarks { get; set; }
        public DateTime? SalesInvoiceDueDate { get; set; }
        public string CusName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public List<CustomerInvoiceItem>Items { get; set; }
  
  }
}
