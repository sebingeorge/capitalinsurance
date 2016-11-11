﻿using System;
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
            return RedirectToAction("Create");
        }
    }
}