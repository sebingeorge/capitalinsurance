using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class Policy_RenewalController : BaseController
    {
        // GET: Policy_Renewal
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(int Id)
        {
            FillDropdowns();
            PolicyIssue objPolicy = new PolicyIssueRepository().GetNewPolicy(Id);
            objPolicy.Cheque = new PolicyIssueRepository().GetChequeDetails(Id);
            return View("Create", objPolicy);
        }

        public ActionResult RenewalList()
        {
            return View();
        }
                public ActionResult PreviousProposal()
        {
            return View();
        }
        public ActionResult PendingPolicyList()
        {

            List<PolicyIssue> lstNewPolicy = (new PolicyIssueRepository()).GetNewPolicy();
            return View(lstNewPolicy);
        }
        void FillDropdowns()
        {
            FillCustomer();
            FillSalesManager();
            FillCompany();
            FillInsuranceProduct();
            FillProductType();
            FillPaymentMode();
            FillPaymentTo();
        }
        void FillSalesManager()
        {
            ViewBag.SalesManager = new SelectList((new SalesManagerRepository()).GetSalesManagers(), "SalesMgId", "SalesMgName");
        }
        void FillCustomer()
        {
            ViewBag.Customer = new SelectList((new DropdownRepository()).GetCustomer(), "Id", "Name");
        }
        void FillCompany()
        {
            ViewBag.Company = new SelectList((new DropdownRepository()).GetInsuranceCompany(), "Id", "Name");
        }
        void FillInsuranceProduct()
        {
            ViewBag.InsuranceProduct = new SelectList((new DropdownRepository()).GetInsuranceProduct(), "Id", "Name");
        }
        void FillProductType()
        {
            ViewBag.ProductType = new SelectList((new DropdownRepository()).GetProductType(), "Id", "Name");
        }
        void FillPaymentMode()
        {
            ViewBag.PaymentMode = new SelectList((new DropdownRepository()).GetPaymentMode(), "Id", "Name");
        }
        void FillPaymentTo()
        {
            List<Dropdown> types = new List<Dropdown>();
            types.Add(new Dropdown { Id = 1, Name = "CIB" });
            types.Add(new Dropdown { Id = 2, Name = "Insurance Co" });
            ViewBag.Payment = new SelectList(types, "Id", "Name");
        }
    }

}