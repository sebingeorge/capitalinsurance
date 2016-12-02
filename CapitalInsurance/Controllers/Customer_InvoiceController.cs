using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class Customer_InvoiceController : BaseController
    {
        // GET: Customer_Invoice
        public ActionResult Index()
        {
            FillCustomer();
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
        public ActionResult PendingPolicyForCustomerInvoice(int ClientId = 0)
        {

            return PartialView("_PendingPolicyForCustomerInvoice", new CustomerInvoiceRepository().GetPendingPoilcyforInvoice(ClientId));
        }
        public ActionResult Create(IList<PolicyIssue>PendingPolicySelected)
        {
            return View();
        }
        public ActionResult PreviousCustomerInvoice()
        {
            return View();
        }
        void FillCustomer()
        {
            ViewBag.Customer = new SelectList((new DropdownRepository()).GetCustomerFrmPolicy(), "Id", "Name");
        }
    }
}