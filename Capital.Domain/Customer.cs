using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Capital.Domain
{
    public class Customer
    {
        public Customer()
        {
           
        }
        public int CustomerId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage="Customer Name is Required")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Customer Name")]
        public string CusName { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Customer Short Name is Required")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Customer Short Name")]
        public string CusShortName { get; set; }
        public int? RegionId { get; set; }
        public int? SalesMgId { get; set; }
        public int? CusCatId { get; set; }
        public int? EmployeeNo { get; set; }
        public int? PremisNo { get; set; }
        public string ContactName { get; set; }
        public string Designation { get; set; }
        public string OfficeNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string StateId { get; set; }
        public int? CountryId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int? CreditPeriod { get; set; }
        public decimal? CreditAmount { get; set; }
    }
}