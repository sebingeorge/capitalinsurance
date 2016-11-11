using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class InsuranceProductController : BaseController
    {
        // GET: InsuranceProduct
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            FillDropdowns();
            InsuranceProduct objProd = new InsuranceProduct();
            objProd.ProductParameters = new InsuranceProductRepository().GetProductionParameterList();
            return View(objProd);
        }

        [HttpPost]
        public ActionResult Create(InsuranceProduct model)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new InsuranceProductRepository().Insert(model);
            return RedirectToAction("Create");
        }



        void FillDropdowns()
        {
            FillInsuranceType();
            FillInsuranceCompany();
        }

        void FillInsuranceType()
        {

            ViewBag.InsuranceType = new SelectList((new DropdownRepository()).GetInsuranceType(), "Id", "Name");

        }
        void FillInsuranceCompany()
        {

            ViewBag.InsuranceCompany = new SelectList((new DropdownRepository()).GetInsuranceCompany(), "Id", "Name");

        }

    }
}