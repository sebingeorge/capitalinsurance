using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class Sales_TargetController : Controller
    {
        // GET: Sales_Target
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sales_Target()
        {
            return View();
        }
        public ActionResult Create()
        {
            FillFinYear();
            return View();
        }
        public ActionResult SalesTarget()
        {

            return PartialView("_SalesTargetGrid", new SalesTargetRepository().GetEmployees());
        }
        void FillFinYear()
        {
            ViewBag.FinYear = new SelectList((new DropdownRepository()).FillFinYear(), "Id", "Name");
        }
    }
}