using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class MonthlyReportController :  BaseController
    {
        // GET: MonthlyReport
        public ActionResult MonthlyPaymentCommittmentIndex()
        {
            return View();
        }
        public ActionResult MonthlyPaymentCommittment(string Client = "")
        {

            return PartialView("MonthlyPaymentCommittment", new MonthlyReportRepository().MonthlyPaymentCommittment(Client));
        }

    }
}