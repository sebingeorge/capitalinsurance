using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
    public class PolicyIssue
    {
        public int? PolicyId { get; set; }
        public string TranPrefix { get; set; }
        public int? TranNumber { get; set; }
        public DateTime? TranDate { get; set; }
        public int? CusId { get; set; }
        public string InsuredName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? InsCmpId { get; set; }
        public int? InsPrdId { get; set; }
        public int? InsSubTypeId { get; set; }
        public DateTime? PolicySubDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal PolicyFee { get; set; }
        public decimal ExtraPremium { get; set; }
        public decimal CommissionPerc { get; set; }
        public decimal CommissionAmount { get; set; }
        public string CustContPersonName { get; set; }
        public string CustContDesignation { get; set; }
        public string CustContEmail { get; set; }
        public string CustContMobile { get; set; }
        public string PaymentOption { get; set; }
        public int? SalesMgId { get; set; }
        public string OperationManager { get; set; }
        public string PolicyNo { get; set; }
        public string FinanceManager { get; set; }
        public string PaymentTo { get; set; }
        public int? PayModeId { get; set; }
        public int? OldPolicyId { get; set; }
        public DateTime? CIBEffectiveDate { get; set; }
        public DateTime? EndorcementDate { get; set; }
        public int? AdditionEmpNo { get; set; }
        public int? DeletionEmpNo { get; set; }
        public int? EndorcementTypeId { get; set; }
        public string TranType { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<PolicyIssueChequeReceived> Cheque { get; set; }
    }
}
