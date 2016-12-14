using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class MonthlyCommittedReportController :  BaseController
    {
        // GET: MonthlyReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MonthlyCommitment(string Client = "")
        {
            var data = new MonthlyReportRepository().MonthlyPaymentCommittment(Client);
            return PartialView("_MonthlyCommitment", data);
        }

    }
}