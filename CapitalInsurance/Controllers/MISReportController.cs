using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class MISReportController : BaseController
    {
        // GET: MISReport
        public ActionResult Index()
        {

            Dashboard dashboard = new Dashboard();
            MisReportRepository repo = new MisReportRepository();


            dashboard.MonthlyAcheivementcoveragewise = repo.GetmonthlycoverageAchivement();
            dashboard.MonthlySales = repo.GetmonthlySales();
            dashboard.EmployeeAchievementVsTraget = repo.GetEmployeeTargetAchivement(UserID);
            dashboard.CoverageVsSales = repo.GetCoverageVsSales();

            return View(dashboard);
  
        }
    }
}