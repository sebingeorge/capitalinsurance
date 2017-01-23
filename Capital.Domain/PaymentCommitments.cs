using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Capital.Domain
{
    public class PaymentCommitments
    {
        public int? CommRowId { get; set; }
        public int? PolicyId { get; set; }
        public DateTime? CommittedDate { get; set; }
        public decimal CommittedAmt { get; set; }

        public Boolean paid { get; set; }
        public Boolean InsPaid { get; set; }
    }
}
