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
            return View();
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
            view.NewPolicy = true;
            view.RenewalPolicy = true;
            view.EndorsePolicy = true;
            view.SalesTarget = true;

            if (view.NewPolicy)
            {
                DateTime? FromDate=null;
                DateTime? ToDate = null;
                var res = new PolicyIssueRepository().GetNewPolicy(FromDate ?? FYStartdate, ToDate ?? DateTime.Now, "", "", "");
                view.NoOfNewPolicy = res.Count;
            }
            if (view.RenewalPolicy)
            {
                DateTime FromDate = new PolicyRenewalRepository().GetFromDate(); ;
                DateTime ToDate = new PolicyRenewalRepository().GetToDate();
                var res = new PolicyRenewalRepository().GetNewPolicyForRenewal(FromDate,ToDate , "", "", "");
                view.NoOfRenewalPolicy = res.Count;
            }
            if (view.EndorsePolicy)
            {
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                var res = new PolicyEndorsementRepository().GetNewPolicyForEndorse(FromDate ?? FYStartdate, ToDate ?? DateTime.Now, "", "", "");
                view.NoOfEndorsePolicy = res.Count;
            }
            if (view.DailyActivity)
            {
                var res = new DailyActivityRepository().GetDailyActivityDetails(0);
                view.NoOfDailyActivity = res.Count;
            }
            if (view.SalesTarget)
            {
                var res = new SalesTargetRepository().GetEmployees();
                view.NoOfSalesTarget = res.Count;
            }
           
            return PartialView("_LoginPartial", view);
        }
    }
}