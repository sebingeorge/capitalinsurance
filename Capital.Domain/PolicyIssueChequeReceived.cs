using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
    public class PolicyIssueChequeReceived
    {
        public int? InsChqRowId { get; set; }
        public int? PolicyId { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public decimal ChequeAmt { get; set; }
        public string CusName { get; set; }
        public decimal TotalPremium { get; set; }
        public string PayModeName { get; set; }
        public string QuickBookRefNo { get; set; }

    }
}
