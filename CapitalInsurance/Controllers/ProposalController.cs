using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class ProposalController : Controller
    {
        // GET: Proposal
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateProposal()
        {
            return View();
        }
    }
}