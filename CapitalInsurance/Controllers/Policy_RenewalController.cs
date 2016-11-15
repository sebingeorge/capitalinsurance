using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class Policy_RenewalController : Controller
    {
        // GET: Policy_Renewal
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Proposal()
        {
            return View();
        }

        public ActionResult RenewalList()
        {
            return View();
        }
                public ActionResult PreviousProposal()
        {
            return View();
        }
        public ActionResult PendingPolicyList()
        {

            List<PolicyIssue> lstNewPolicy = (new PolicyIssueRepository()).GetNewPolicy();
            return View(lstNewPolicy);
        }

    }

}