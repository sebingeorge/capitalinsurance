﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
    public class InsuranceCompany
    {
        public int? InsCmpId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Insurance Company Name is Required")]
        public string InsCmpName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNo { get; set; }
        public string Designation { get; set; }
        public string ContactPerson2  { get; set; }
        public string ContactPerson3  { get; set; }
        public string Designation2  { get; set; }
        public string Designation3  { get; set; }
        public string ContactNo2  { get; set; }
        public string ContactNo3  { get; set; }
        public string Email2  { get; set; }
        public string Email3  { get; set; }
        public string PoBox  { get; set; }
        public string CrNo { get; set; }
    }
}
