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

            var data = new MonthlyReportRepository().MonthlyPaymentCommittment(Client);
            return PartialView("MonthlyPaymentCommittment", data);
        }

    }
}