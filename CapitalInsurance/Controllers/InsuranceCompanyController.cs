using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capital.Domain;
using Capital.DAL;

namespace CapitalInsurance.Controllers
{
    public class InsuranceCompanyController : BaseController
    {
        // GET: InsuranceCompany
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View(new InsuranceCompany());
        }
        [HttpPost]
        public ActionResult Create(InsuranceCompany model)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new InsuranceCompanyRepository().Insert(model);
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
            InsuranceCompany objCompany = new InsuranceCompanyRepository().GetCompany(Id);
            return View("Create", objCompany);

        }
        [HttpPost]
        public ActionResult Edit(InsuranceCompany model)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new InsuranceCompanyRepository().Update(model);


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
            InsuranceCompany objCompany = new InsuranceCompanyRepository().GetCompany(Id);
            return View("Create", objCompany);
        }
        [HttpPost]
        public ActionResult Delete(InsuranceCompany model)
        {
            Result res = new InsuranceCompanyRepository().Delete(model);
            if (res.Value)
            {
                TempData["Success"] = "Deleted Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult CompanyList(string Company = "")
        {

            return PartialView("_InsuranceCmpyGrid", new InsuranceCompanyRepository().GetCompany(Company));
        }
    }
}