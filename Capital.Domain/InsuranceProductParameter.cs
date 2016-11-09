using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
    public class InsuranceProductParameter
    {
        public int? InsPrdParamId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Product Parameter Name is Required")]
        public string InsPrdParamName { get; set; }
        public string InsPrdParamDecription { get; set; }
    }
}
