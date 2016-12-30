using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
    public class InsuranceProduct
    {
        public int? InsPrdId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Product Name is Required")]
        public string InsPrdName { get; set; }
        public string InsPrdShortName { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Product Type is Required")]
        public int InsTypeId { get; set; }
        //[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Product Company is Required")]

        public string insTypeName { get; set; }
        public DateTime? InsActiveDate{get;set;}
        public List<InsuranceProductVsParameter> ProductParameters { get; set; }
    }
}
