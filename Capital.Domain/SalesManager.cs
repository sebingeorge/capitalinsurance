using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
    public class SalesManager
    {
        public int SalesMgId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Sales Manager Name is Required")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Sales Manager")]
        public string SalesMgName { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Marrital Status is Required")]
        public string MaritalStatus { get; set; }
        public int? DsgId { get; set; }
        public int? CountryId { get; set; }
        public int? DeptId { get; set; }
        public int? LoctId { get; set; }
        public string CurrentAddress1 { get; set; }
        public string CurrentAddress2 { get; set; }
        public string CurrentAddress3 { get; set; }
        public int? StateId { get; set; }
        public string PermanantAddress1 { get; set; }
        public string PermanantAddress2 { get; set; }
        public string PermanantAddress3 { get; set; }
        public string PermanantState { get; set; }
        public int? PermanantCountryId { get; set; }
        public string QuatarContactNo { get; set; }
        public string HomeCountryContactNo { get; set; }
        public string PassportNo { get; set; }
        public DateTime PassportIssueDate { get; set; }
        public DateTime PassportEndDate { get; set; }
        public string VisaOrResId { get; set; }
        public DateTime VisaIssueDate { get; set; }
        public DateTime VisaEndDate { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string OfficeEmail { get; set; }
        public string PersonalEmail { get; set; }

    }
}
