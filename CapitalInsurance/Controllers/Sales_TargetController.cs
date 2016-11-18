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
        public ActionResult Create()
        {
            FillFinYear();
            return View();
        }
        public ActionResult SalesTarget(int? FyId)
        {
            SalesTarget Model = new SalesTarget();
            if (FyId == null || FyId == 0)
            {
                Model.SalesTargetItems = new SalesTargetRepository().GetEmployees();
            }
            else
            {
                Model.SalesTargetItems = new SalesTargetRepository().GetEmployees(FyId);
            }
            if (Model.SalesTargetItems.Count == 0)
                Model.SalesTargetItems = new SalesTargetRepository().GetEmployees();
            
            return PartialView("_SalesTargetGrid", Model);
        }
        [HttpPost]
        public ActionResult Create(SalesTarget model)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new SalesTargetRepository().Insert(model);
            if (res.Value)
            {
                TempData["Success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Create");
        }
        void FillFinYear()
        {
            ViewBag.FinYear = new SelectList((new DropdownRepository()).FillFinYear(), "Id", "Name");
        }
    }
}