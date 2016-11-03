using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class Policy_EndorsementController : Controller
    {
        // GET: Endorsement
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Proposal()
        {
            return View();
        }
        public ActionResult PendingProposal()
        {
            return View();
        }

                public ActionResult PreviousProposal()
        {
            return View();
        }

    }
    }
