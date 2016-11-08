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
            return View(new SalesManager());
        }
        [HttpPost]
        public ActionResult Create(SalesManager model)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            return RedirectToAction("Create");
        }
    }
}