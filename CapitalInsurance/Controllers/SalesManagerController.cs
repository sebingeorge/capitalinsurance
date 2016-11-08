using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class SalesManagerController : Controller
    {
        // GET: SalesManager
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View(new SalesManager());
        }
    }
}