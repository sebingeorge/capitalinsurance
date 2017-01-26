using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class InsuranceCompanyPayableReportController : BaseController
    {
        // GET: InsuranceCompanyPayableReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InsuranceCompanyPayableReport()
        {

            return PartialView("_InsuranceCompanyPayableReport");

        }
    }
}