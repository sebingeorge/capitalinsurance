using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capital.DAL;
using Capital.Domain;


namespace CapitalInsurance.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {

            Dashboard dashboard = new Dashboard();
            MisReportRepository repo = new MisReportRepository();


            dashboard.MonthlyAcheivementcoveragewise = repo.GetmonthlycoverageAchivement();
            dashboard.MonthlySales = repo.GetmonthlySales();
            dashboard.EmployeeAchievementVsTraget = repo.GetEmployeeTargetAchivement(UserID);
            dashboard.CoverageVsSales = repo.GetCoverageVsSales();

            return View(dashboard);
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult LoadQuickView()
        {
            QuickView view = new QuickView();
          
            view.RenewalPolicy = true;

            if (view.RenewalPolicy)
            {
                DateTime FromDate = new PolicyRenewalRepository().GetFromDate(); ;
                DateTime ToDate = new PolicyRenewalRepository().GetToDate();
                var res = new PolicyRenewalRepository().GetNewPolicyForRenewal(FromDate,ToDate , "", "", "");
                view.NoOfRenewalPolicy = res.Count;
            }
          
            return PartialView("_LoginPartial", view);
        }
        public ActionResult ChangePassword()
        {

            return PartialView("_ChangePassword");
        }
    }
}