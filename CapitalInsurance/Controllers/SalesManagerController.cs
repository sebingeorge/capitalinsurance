using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class SalesManagerController : BaseController
    {
        // GET: SalesManager
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            FillDropdowns();
            return View(new SalesManager());
        }
        void FillDropdowns()
        {
            FillDesignation();
            FillState();
            FillCountry();
        }
        [HttpPost]
        public ActionResult Create(SalesManager model)
        {

            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new SalesManagerRepository().Insert(model);
            return RedirectToAction("Create");
        }
         void FillDesignation()
        {

            ViewBag.Designation = new SelectList((new DropdownRepository()).GetDesignation(), "Id", "Name");
          
        }
        void FillState()
        {
            ViewBag.State = new SelectList((new StateRepository()).GetState(), "StateId", "StateName");
        }
        void FillCountry()
        {
            ViewBag.Country = new SelectList((new CountryRepository()).GetCountry(), "CountryId", "CountryName");
        }
        public ActionResult DepartmentPopup()
        {
            var List = new SalesManagerRepository().FillDepartmentList();
            return View(List);
        }
    }
}