using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class ReceivableReportController : Controller
    {
        // GET: ReceivableReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PendingReceivableReport()
        {
            return View();
        }
        public ActionResult ReceivableReport()
        {
            return View();
        }
        public ActionResult PreviousReceivable()
        {
            return View();
        }
    }
}