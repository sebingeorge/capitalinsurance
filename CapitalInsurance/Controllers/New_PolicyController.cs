using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class New_PolicyController : Controller
    {
        // GET: Proposal
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Proposal()
        {
            return View();
        }
        public ActionResult Issue()
        {
            return View();
        }
        public ActionResult PolicyList()
        {
            return View();
        }
        public ActionResult Pendingissue()
        {
            return View();
        }
        public ActionResult PreviousProposal()
        {
            return View();
        }
        public ActionResult PreviousIssue()
        {
            return View();
        }
    }
}