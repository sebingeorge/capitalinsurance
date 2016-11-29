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
        public int CusId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage="Customer Name is Required")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Customer Name")]
        public string CusName { get; set; }
        
        [System.ComponentModel.DataAnnotations.Display(Name = "Customer Short Name")]
        public string CusShortName { get; set; }
        public string CusPrefix  { get; set; }
        public string PoBox  { get; set; }
        public string CrNo { get; set; }
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public int? SalesMgId { get; set; }
        public string SalesMgName { get; set; }
        public int? CusCatId { get; set; }
        public string CusCategory { get; set; }
        public int? EmployeeNo { get; set; }
        public int? PremisNo { get; set; }
        public string ContactName { get; set; }
        public string Designation { get; set; }
        public string OfficeNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int? CreditPeriod { get; set; }
        public int? CreditPeriod2 { get; set; }
        public int? CreditPeriod3 { get; set; }
        public int? CreditPeriod4 { get; set; }
        public decimal? CreditAmount { get; set; }
    }
}