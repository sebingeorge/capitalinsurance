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
            //List<InsuranceProduct> lstInsurProducts = (new InsuranceProductRepository()).GetInsuranceProduct();
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
            if (res.Value)
            {
                TempData["Success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int Id)
        {
            ViewBag.Title = "Edit";
            FillDropdowns();
            InsuranceProduct objProd = new InsuranceProductRepository().GetInsuranceProduct(Id);
            objProd.ProductParameters = new InsuranceProductRepository().GetProductionParameter(Id);
            return View("Create", objProd);

        }
        [HttpPost]
        public ActionResult Edit(InsuranceProduct model)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new InsuranceProductRepository().Update(model);


            if (res.Value)
            {
                TempData["Success"] = "Updated Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int Id)
        {
            ViewBag.Title = "Delete";
            FillDropdowns();
            InsuranceProduct objProd = new InsuranceProductRepository().GetInsuranceProduct(Id);
            objProd.ProductParameters = new InsuranceProductRepository().GetProductionParameter(Id);
            return View("Create", objProd);
        }
        [HttpPost]
        public ActionResult Delete(InsuranceProduct model)
        {
            Result res = new InsuranceProductRepository().Delete(model);
            if (res.Value)
            {
                TempData["Success"] = "Deleted Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult ProductList(string Product = "")
        {

            return PartialView("_InsuranceProductGrid", new InsuranceProductRepository().GetInsuranceProduct(Product));
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