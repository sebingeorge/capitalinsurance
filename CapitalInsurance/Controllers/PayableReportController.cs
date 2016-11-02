using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class PayableReportController : Controller
    {
        // GET: PayableReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }
        public ActionResult PayableReport()
        {
            return View();
        }
        public ActionResult PreviousPayable()
        {
            return View();
        }
    }
}