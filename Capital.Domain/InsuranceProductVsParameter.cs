using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
    public class InsuranceProductVsParameter
    {
        public int? InsParamRowId { get; set; }
        public int? InsPrdId { get; set; }
        public int? InsPrdParamId { get; set; }
        public string InsParamValue { get; set; }
        public string InsPrdParamName { get; set; }
    }
}
