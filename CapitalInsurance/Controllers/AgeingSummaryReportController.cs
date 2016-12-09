using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CapitalInsurance.Controllers
{
    public class AgeingSummaryReportController : BaseController
    {
        // GET: AgeingSummaryReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AgeingSummary(string Client="")
        {

            return PartialView("_AgeingSummary", new ReportRepository().GetAgeingSummaryBasedCommittedDate(Client));
        }


    }
}