using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class SalesAchievementReportController : BaseController
    {
        // GET: SalesAchievementReport
        public ActionResult Index()
        {
            FillFinYear();
            return View();
        }
        public ActionResult SalesAchieveReport(int? FyId)
        {
                    
            return PartialView("_SalesAchieveReport", new SalesTargetRepository().GetSalesAchievementReportDetails(FyId));
        }
        void FillFinYear()
        {
            ViewBag.FinYear = new SelectList((new DropdownRepository()).FillFinYear(), "Id", "Name");
        }
     
    }
}