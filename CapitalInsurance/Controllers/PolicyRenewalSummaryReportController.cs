using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class PolicyRenewalSummaryReportController : BaseController
    {
        // GET: PolicyRenewalSummaryReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PolicyRenewalSummary()
        {
            return PartialView("_PolicyRenewalSummary", new ReportRepository().GetPolicyRenewalSummary());
        }
        public ActionResult PolicyDetailsPopup(int id=0)
        {
            if (id != 0)
            return PartialView("_PolicyDetailsPopup", new ReportRepository().GetPolicyDetails(id));
            return View();
        }

    }
}