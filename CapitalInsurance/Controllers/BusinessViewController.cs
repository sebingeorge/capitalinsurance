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
        public ActionResult Index()
        {
            List<PolicyIssue> lstBusinessView = (new BusinessViewRepository()).GetBusinessViewDetails();
            return View(lstBusinessView);
        }
        //public ActionResult BussinessViewDetails()
        //{
        //    List<PolicyIssue> lstBusinessView = (new BusinessViewRepository()).GetBusinessViewDetails();
        //    return View(lstBusinessView);
           
        //}
    }
}