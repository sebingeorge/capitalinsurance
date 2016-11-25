using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class BusinessViewController :  BaseController
    {
        // GET: BusinessView
       [HttpPost]
        public ActionResult Index()
        {
            return View();
        }

       public ActionResult BusinessViewDetails(string[] tags, string Company = "", string PolicyNo = "", string Client = "", string SalesManager = "")
        {
            ViewBag.tags = tags;
            return PartialView("_BussinessViewDetails", new BusinessViewRepository().GetBusinessViewDetails(Company, PolicyNo, Client, SalesManager));
        }
       public ActionResult BusinessViewDetailsFilter(string Company = "", string PolicyNo = "", string Client = "", string SalesManager = "")
        {

            return PartialView("_BusinessViewDetailsFilter", new BusinessViewRepository().GetBusinessViewDetails(Company, PolicyNo, Client, SalesManager));
        }
        public ActionResult AddingFields()
        {
            return View();
        }
    }
}