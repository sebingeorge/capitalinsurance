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

            return PartialView("_InsuranceCompanyPayableReport", new ReportRepository().GetInsuranceCompanyPayable());

        }
        public ActionResult PolicyDetailsPopUp(int id = 0)
        {
            if (id != 0)
                return PartialView("_PolicyDetailsPopup", new ReportRepository().GetPolicyDetailsforPayablePopUp(id));
            return View();
        }
    }
}