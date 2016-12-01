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
    public class PolicyIssue
    {
        public int? PolicyId { get; set; }
        public string TranPrefix { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Transaction Number is Required")]
        public string TranNumber { get; set; }
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
         //[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Product Type is Required")]
        public int? InsCoverId { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Policy Sub Date is Required")]
        public DateTime PolicySubDate { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Effective Date is Required")]
        public DateTime EffectiveDate { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Renewal Date is Required")]
        public DateTime? RenewalDate { get; set; }
         public DateTime ExpiryDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal PolicyFee { get; set; }
        public decimal ExtraPremium { get; set; }
        public decimal TotalPremium { get; set; }
        public int CommissionPerc { get; set; }
        public decimal CommissionAmount { get; set; }
        public string CustContPersonName { get; set; }
        public string CustContDesignation { get; set; }
        public string CustContEmail { get; set; }
        public string CustContMobile { get; set; }
        public string CustContOfficeNo { get; set; }
        public string PaymentOption { get; set; }
         [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Sales Manager is Required")]
        public int? SalesMgId { get; set; }
        public string OperationManager { get; set; }
        public string PolicyNo { get; set; }
        public string EndorsementNo { get; set; }
        public string Remarks { get; set; }
        public string FinanceManager { get; set; }
        public string QuickBookRefNo { get; set; }
        public int? PayModeId { get; set; }
        public int? OldPolicyId { get; set; }
        public DateTime? CIBEffectiveDate { get; set; }
        public DateTime? EndorcementDate { get; set; }
        public DateTime? ICActualDate { get; set; }
        public string EndorcementNo { get; set; }
        public int? AdditionEmpNo { get; set; }
        public int? DeletionEmpNo { get; set; }
        public string EndType { get; set; }
        public int? EndorcementTypeId { get; set; }
        public string TranType { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CusName { get; set; }
        public string InsPrdName { get; set; }
        public string InsCoverName { get; set; }
        public string InsCmpName { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Day { get; set; }
        public string SalesMgCode { get; set; }
        public string SalesMgName { get; set; }
        public string OldPolicyNo { get; set; }
        public string OldCompany { get; set; }
        public string OldProductType { get; set; }
        public decimal OldPremiumAmt { get; set; }
        public string OfficeNo{ get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string CusAddress { get; set; }
        public string OfficeEmail { get; set; }
        public string QuatarContactNo { get; set; }
        public int Aging { get; set; }
        //Endorsement
     
        public int EmployeeNo { get; set; }
        public int TotalEmployes { get; set; }
        public List<PolicyIssueChequeReceived> Cheque { get; set; }
        public List<PaymentCommitments> Committed { get; set; }
        public int Type { get; set; }
        public string PaymentTo { get; set; }
        public IEnumerable<SelectListItem> PaymentToList
        {
            get
            {
                yield return new SelectListItem { Text = "CIB", Value = "CIB" };
                yield return new SelectListItem { Text = "Insurance Co", Value = "Insurance Co" };
            }
        }
        public bool SelectStatus { get; set; }
    }
}
