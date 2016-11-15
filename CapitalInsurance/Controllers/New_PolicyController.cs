using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CapitalInsurance.Controllers
{
    public class New_PolicyController : BaseController
    {
        // GET: Proposal
        public ActionResult Index()
        {

            List<PolicyIssue> lstNewPolicy = (new PolicyIssueRepository()).GetNewPolicy();
            return View(lstNewPolicy);
        }
        public ActionResult Create()
        {
            FillDropdowns();
            PolicyIssue model = new PolicyIssue();
            model.Cheque = new List<PolicyIssueChequeReceived>();
            model.Cheque.Add(new PolicyIssueChequeReceived());
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(PolicyIssue model)
        {
            model.TranPrefix = "PSF";
            model.TranDate = System.DateTime.Now;
            model.CreatedDate = System.DateTime.Now;
            model.CreatedBy = UserID;
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new PolicyIssueRepository().Insert(model);
            return RedirectToAction("Create");
        }

        public ActionResult PolicyList()
        {
            return View();
        }

        public ActionResult PreviousProposal()
        {
            return View();
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
            ViewBag.PaymentTo = new SelectList(types, "Id", "Name");
        }

    }
}