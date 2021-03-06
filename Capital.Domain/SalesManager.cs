﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
    public class SalesManager
    {
        public int SalesMgId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Sales Manager Code is Required")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Sales Manager Code")]
        public string SalesMgCode { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Sales Manager Name is Required")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Sales Manager")]
        public string SalesMgName { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Gender is Required")]
        public int GenderId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Marrital Status is Required")]
        public int MaritalStatusId { get; set; }
        public int? DsgId { get; set; }
        public string DsgName { get; set; }
        public string CountryId { get; set; }
        public string Deptment { get; set; }
        public string Location { get; set; }
        public string CurrentAddress1 { get; set; }
        public string CurrentAddress2 { get; set; }
        public string CurrentAddress3 { get; set; }
        public string StateId { get; set; }
        public string PermanantAddress1 { get; set; }
        public string PermanantAddress2 { get; set; }
        public string PermanantAddress3 { get; set; }
        public string PermanantState { get; set; }
        public string PermanantCountry { get; set; }
        public string QuatarContactNo { get; set; }
        public string HomeCountryContactNo { get; set; }
        public string PassportNo { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public DateTime? PassportEndDate { get; set; }
        public string VisaOrResId { get; set; }
        public DateTime? VisaIssueDate { get; set; }
        public DateTime? VisaEndDate { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string OfficeEmail { get; set; }
        public string PersonalEmail { get; set; }

        public decimal MonthlySalary { get; set; }

        public decimal IncentivePerc { get; set; }

    }
}
