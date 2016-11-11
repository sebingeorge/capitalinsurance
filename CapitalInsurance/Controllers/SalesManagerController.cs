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
            FillMaritalStatus();
            FillGender();
        }
        [HttpPost]
        public ActionResult Create(SalesManager model)
        {

            model.DateOfBirth = System.DateTime.Now;
            model.DateOfJoining = System.DateTime.Now;
            model.PassportEndDate = System.DateTime.Now;
            model.PassportIssueDate = System.DateTime.Now;
            model.VisaEndDate = System.DateTime.Now;
            model.VisaIssueDate = System.DateTime.Now;

            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                FillDropdowns();
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
        void FillMaritalStatus()
        {
            List<Dropdown> types = new List<Dropdown>();
            types.Add(new Dropdown { Id = 1, Name = "Single" });
            types.Add(new Dropdown { Id = 2, Name = "Married" });
            ViewBag.MaritalStatus = new SelectList(types, "Id", "Name");
        }
        void FillGender()
        {
            List<Dropdown> types = new List<Dropdown>();
            types.Add(new Dropdown { Id = 1, Name = "Male" });
            types.Add(new Dropdown { Id = 2, Name = "Female" });
            ViewBag.Gender = new SelectList(types, "Id", "Name");
        }

        public ActionResult DepartmentPopup()
        {
            var List = new SalesManagerRepository().FillDepartmentList();
            return View(List);
        }
        public ActionResult LocationPopup()
        {
            var List = new SalesManagerRepository().FillLocationList();
            return View(List);
        }
    }
}