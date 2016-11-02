using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class Customer_InvoiceController : Controller
    {
        // GET: Customer_Invoice
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Customer_Invoice()
        {
            return View();
        }
        public ActionResult PendingCustomerInvoice()
        {
            return View();
        }
        public ActionResult PreviousCustomerInvoice()
        {
            return View();
        }
    }
}