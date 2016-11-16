using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Capital.Domain
{
    public class PolicyIssue
    {
        public int? PolicyId { get; set; }
        public string TranPrefix { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Transaction Number is Required")]
        public int? TranNumber { get; set; }
         public string StrTranNumber { get; set; }
        public DateTime? TranDate { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Client Name is Required")]
        public int? CusId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Insured Name is Required")]
        public string InsuredName { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Address1 is Required")]
        public string Address1 { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Address2 is Required")]
        public string Address2 { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Company is Required")]
        public int? InsCmpId { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Product is Required")]
        public int? InsPrdId { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Product Type is Required")]
        public int? InsCoverId { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Policy Sub Date is Required")]
        public DateTime PolicySubDate { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Effective Date is Required")]
        public DateTime EffectiveDate { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Renewal Date is Required")]
        public DateTime RenewalDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal PolicyFee { get; set; }
        public decimal ExtraPremium { get; set; }
        public decimal TotalPremium { get; set; }
        public decimal CommissionPerc { get; set; }
        public decimal CommissionAmount { get; set; }
        public string CustContPersonName { get; set; }
        public string CustContDesignation { get; set; }
        public string CustContEmail { get; set; }
        public string CustContMobile { get; set; }
        public string PaymentOption { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Sales Manager is Required")]
        public int? SalesMgId { get; set; }
        public string OperationManager { get; set; }
        public string PolicyNo { get; set; }
        public string Remarks { get; set; }
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
        public string CusName { get; set; }
        public string InsPrdName { get; set; }
        public string InsCoverName { get; set; }
        public string InsCmpName { get; set; }
        public string SalesMgName { get; set; }
        public List<PolicyIssueChequeReceived> Cheque { get; set; }
    }
}
