using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class adminController : Controller
    {
        // GET: admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Customer()
        {
            return View();
        }
        public ActionResult Employee()
        {
            return View();
        }
        public ActionResult Designation()
        {
            return View();
        }
        public ActionResult Insurance()
        {
            return View();
        }
        public ActionResult InsuranceProduct()
        {
            return View();
        }
        public ActionResult Policy()
        {
            return View();
        }
    }
}