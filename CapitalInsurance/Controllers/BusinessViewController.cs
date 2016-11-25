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
           if(TempData["Tags"] ==  null)
           {
               TempData.Add("Tags", tags);
           }           
           TempData.Keep("Tags");
            return PartialView("_BussinessViewDetails", new BusinessViewRepository().GetBusinessViewDetails(Company, PolicyNo, Client, SalesManager));
        }
       public ActionResult BusinessViewDetailsFilter(string Company = "", string PolicyNo = "", string Client = "", string SalesManager = "")
        {
            string[] tags = (string[])TempData["Tags"];
            if (TempData["Tags"] == null)
            {
                TempData.Add("Tags", tags);
            }   
            TempData.Keep("Tags");
            ViewBag.tags = tags;
            return PartialView("_BussinessViewDetails", new BusinessViewRepository().GetBusinessViewDetails(Company, PolicyNo, Client, SalesManager));
        }
        public ActionResult AddingFields()
        {
            return View();
        }
    }
}