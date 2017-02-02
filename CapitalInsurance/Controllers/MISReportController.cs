using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class MISReportController : Controller
    {
        // GET: MISReport
        public ActionResult Index()
        {

            Dashboard dashboard = new Dashboard();
            MisReportRepository repo = new MisReportRepository();


            dashboard.MonthlyAcheivementcoveragewise = repo.GetmonthlycoverageAchivement();
            dashboard.MonthlySales = repo.GetmonthlySales();
            dashboard.EmployeeAchievementVsTraget = repo.GetEmployeeTargetAchivement();

            return View(dashboard);
  
        }
    }
}