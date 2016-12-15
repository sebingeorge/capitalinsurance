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
        public DateTime? DailyActivityDate { get; set; }
        public string SalesMgName { get; set; }
        public string DsgName { get; set; }
        public List<DailyActivityItem> DailyActivityItems { get; set; }
    }
    public class DailyActivityItem
    {
        public int Slno { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Company { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public IEnumerable<SelectListItem> TypeList
        {
            get
            {
                yield return new SelectListItem { Text = "CC", Value = "CC" };
                yield return new SelectListItem { Text = "FC", Value = "FC" };
                yield return new SelectListItem { Text = "FA", Value = "FA" };
                yield return new SelectListItem { Text = "SC", Value = "SC" };
            }
        }
        public string Remarks { get; set; }

    }
}