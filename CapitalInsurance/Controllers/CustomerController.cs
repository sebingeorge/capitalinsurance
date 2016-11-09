using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class CustomerController : BaseController
    {
        // GET: Customer
        public ActionResult Index()
        {
            List<Customer> lstCustomer = (new CustomerRepository()).GetCustomer();
            return View(lstCustomer);
        }
        public ActionResult Create()
        {
            FillDropdowns();
            return View(new Customer());
        }
        public ActionResult Edit(int Id)
        {
            ViewBag.Title = "Edit";
            FillDropdowns();
            Customer objCustomer = new CustomerRepository().GetCustomer(Id);
            return View("Create", objCustomer);
           
        }

        void FillDropdowns()
        {
            FillRegion();
            FillSalesManager();
            FillState();
            FillCustomerCategory();
            FillCountry();
        }
        void FillRegion()
        {
            ViewBag.Regions = new SelectList((new RegionRepository()).GetRegions(), "RegionId", "RegionName");
        }
        void FillSalesManager()
        {
            ViewBag.SalesManager = new SelectList((new SalesManagerRepository()).GetSalesManagers(), "SalesMgId", "SalesMgName");
        }
        void FillState()
        {
            ViewBag.State = new SelectList((new StateRepository()).GetState(), "StateId", "StateName");
        }
        void FillCustomerCategory()
        {
            ViewBag.CustomerCategory = new SelectList((new CustomerCategoryRepository()).GetCustomerCategory(), "CusCatId", "CusCategory");
        }
        void FillCountry()
        {
            ViewBag.Country = new SelectList((new CountryRepository()).GetCountry(), "CountryId", "CountryName");
        }
        [HttpPost]
        public ActionResult Create(Customer model)
        {
            if(!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new CustomerRepository().Insert(model);
            return RedirectToAction("Create");
        }
        [HttpPost]
        public ActionResult Edit(Customer model)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new CustomerRepository().Update(model);
            return RedirectToAction("Index");
        }
    }
}