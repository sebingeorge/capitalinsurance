using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class Policy_EndorsementController : BaseController
    {
        // GET: Endorsement
        public ActionResult Index()
        {
            ViewBag.Fromdate = FYStartdate;
            return View();
        }
        public ActionResult Create(int Id)
        {
            FillDropdowns();
            PolicyIssue objPolicy = new PolicyEndorsementRepository().GetNewPolicyForEndorse(Id);
            objPolicy.PolicySubDate = DateTime.Now;
            objPolicy.EndorcementDate = DateTime.Now;
            objPolicy.ICActualDate = DateTime.Now;
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
            model.TranPrefix = "CIB/END";
            model.TranDate = System.DateTime.Now;
            model.CreatedDate = System.DateTime.Now;
            model.CreatedBy = UserID;
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new PolicyEndorsementRepository().Insert(model);
            if (res.Value)
            {
                TempData["Success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult PendingPolicyList(DateTime? FromDate, DateTime? ToDate, string PolicyNo = "", string Client = "", string SalesManager = "")
        {
            FromDate = FromDate ?? FYStartdate;
            ToDate = ToDate ?? DateTime.Now;
            return PartialView("_PendingPolicyListGrid", new PolicyEndorsementRepository().GetNewPolicyForEndorse(FromDate, ToDate, PolicyNo, Client, SalesManager));
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
