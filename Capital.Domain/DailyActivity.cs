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
    public class DailyActivity
    {
        public DateTime TranDate { get; set; }
        public string SalesMgName { get; set; }
        public int SalesMgId { get; set; }
        public string DsgName { get; set; }
        public int CreatedBy { get; set; }
        public List<DailyActivityItem> DailyActivityItems { get; set; }
    }
    public class DailyActivityItem
    {
        public DateTime? DailyActivityDate { get; set; }
        public string DailyActivityTime { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Sales Manager is Required")]
        public string DailyActivityCompany { get; set; }
        public string DailyActivityContactPerson { get; set; }
        public string DailyActivityContactNo { get; set; }
        public string DailyActivityEmail { get; set; }
        public string DailyActivityType { get; set; }
        public string DailyActivityRemarks { get; set; }
      
        public IEnumerable<SelectListItem> TypeList
        {
            get
            {
                yield return new SelectListItem { Text = "Select Type", Value = "" };
                yield return new SelectListItem { Text = "CC", Value = "CC" };
                yield return new SelectListItem { Text = "FC", Value = "FC" };
                yield return new SelectListItem { Text = "FA", Value = "FA" };
                yield return new SelectListItem { Text = "SC", Value = "SC" };
            }
        }
    

    }
}