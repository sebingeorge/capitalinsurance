using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CapitalInsurance.Controllers
{
    public class TotalSalesTargetReportController : BaseController
    {
        // GET: TotalSalesTargetReport
        public ActionResult Index()
        {
            FillFinYear();
            return View();
        }
        void FillFinYear()
        {
            ViewBag.FinYear = new SelectList((new DropdownRepository()).FillFinYear(), "Id", "Name");
        }
              public ActionResult TotalSalesTargetReport(int? FyId)
        {

            return PartialView("_TotalSalesTargetReport", new SalesTargetRepository().GetTotalSalesTargetReport(FyId));
        }
       
    }
}