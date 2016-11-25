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
            ViewBag.Fromdate = new PolicyRenewalRepository().GetFromDate();
            ViewBag.Todate = new PolicyRenewalRepository().GetToDate();
            return View();
        }
        public ActionResult Create(int Id)
        {
            FillDropdowns();
            PolicyIssue objPolicy = new PolicyRenewalRepository().GetNewPolicyForRenewal(Id);
            objPolicy.PolicySubDate = DateTime.Now;
            objPolicy.TranType = "RenewPolicy";
            var internalid = PolicyIssueRepository.GetNextDocNo(objPolicy.TranType);
            objPolicy.TranNumber = "CIB/REN/" + internalid;
            objPolicy.Cheque = new PolicyIssueRepository().GetChequeDetails(Id);
            if (objPolicy.Cheque.Count == 0)
            {
                objPolicy.Cheque.Add(new PolicyIssueChequeReceived());
            }
            return View("Create", objPolicy);
        }
        [HttpPost]
        public ActionResult Create(PolicyIssue model)
        {
            model.TranPrefix = "CIB/REN";
            model.TranDate = System.DateTime.Now;
            model.CreatedDate = System.DateTime.Now;
            model.CreatedBy = UserID;
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new PolicyRenewalRepository().Insert(model);
            if (res.Value)
            {
                TempData["Success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult RenewalList()
        {
            return View();
        }
        public ActionResult PendingPolicyList(DateTime? FromDate, DateTime? ToDate,string PolicyNo="" , string Client = "", string SalesManager = "")
        {
            ViewBag.Fromdate = new PolicyRenewalRepository().GetFromDate();
            ViewBag.Todate = new PolicyRenewalRepository().GetToDate();
            FromDate = FromDate ?? ViewBag.Fromdate;
            ToDate = ToDate ?? ViewBag.Todate;
            return PartialView("_PendingPolicyListGrid", new PolicyRenewalRepository().GetNewPolicyForRenewal(FromDate, ToDate,PolicyNo,Client, SalesManager));
        }
        void FillDropdowns()
        {
            FillCustomer();
            FillSalesManager();
            FillCompany();
            FillInsuranceProduct();
            FillProductType();
            FillPaymentMode();
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
        }

}