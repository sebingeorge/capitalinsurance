using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class SalesIncentiveReportController : BaseController
    {
        // GET: SalesIncentiveReport
        public ActionResult Index()
        {
            FillFinYear();
            return View();
        }
        public ActionResult SalesIncentiveReport(int? FyId)
        {
            return PartialView("_SalesIncentiveReport", new SalesTargetRepository().GetSalesIncentiveReportDetails(UserID,UserRolename,FyId));
        }
        void FillFinYear()
        {
            ViewBag.FinYear = new SelectList((new DropdownRepository()).FillFinYear(), "Id", "Name");
        }
     
    }
}