﻿using System.Collections.Generic;

namespace Capital.Domain
{
    public class Customer
    {
        public Customer()
        {
           
        }
        public int CustomerId { get; set; }
        public string CusName { get; set; }
        public string CusShortName { get; set; }
        public int RegionId { get; set; }
        public int SalesMgId { get; set; }
        public string IsProspectiveClient { get; set; }
        public string ContactName { get; set; }
        public string Designation { get; set; }
        public string OfficeNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string StateId { get; set; }
        public int CountryId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int CreditPeriod { get; set; }
        public decimal CreditAmount { get; set; }
    }
}