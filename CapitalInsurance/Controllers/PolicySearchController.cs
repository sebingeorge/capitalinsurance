using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class PolicySearchController : BaseController
    {
        // GET: PolicySearch
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PolicySearch()
        {
            return View();
        }
        public ActionResult NewPolicyList(string PolicyNo = "")
        {
                return PartialView("_NewPolicyList", new PolicyIssueRepository().GetPaymentCommittedList(PolicyNo));
        }
        public ActionResult ViewPolicy(int Id)
        {
            ViewBag.Title = "View";
            FillDropdowns();
            PolicyIssue objPolicy = new PolicyIssueRepository().GetNewPolicy(Id);
            //if(objPolicy.PayModeId != 0)
            //{
            //    ViewBag.Type = 3;
            //}
            //else
            //{
            //    ViewBag.Type = 2;
            //}
            objPolicy.Cheque = new PolicyIssueRepository().GetChequeDetails(Id);
            objPolicy.Committed = new List<PaymentCommitments>();
            objPolicy.Committed.Add(new PaymentCommitments());
           
                objPolicy.Committed = new PolicyIssueRepository().GetCommittedDetails(Id);
                if (objPolicy.Committed.Count == 1)
                {
                    objPolicy.Committed.Add(new PaymentCommitments());
                    objPolicy.Committed.Add(new PaymentCommitments());
                    objPolicy.Committed.Add(new PaymentCommitments());
                }
                else if (objPolicy.Committed.Count == 2)
                {
                    objPolicy.Committed.Add(new PaymentCommitments());
                    objPolicy.Committed.Add(new PaymentCommitments());
                }
                else if (objPolicy.Committed.Count == 3)
                {
                    objPolicy.Committed.Add(new PaymentCommitments());
                }
                objPolicy.Committed = new PolicyIssueRepository().GetCommittedDetails(Id);
                objPolicy.Cheque = new PolicyIssueRepository().GetChequeDetails(Id);
                if (objPolicy.Cheque.Count == 0)
                {
                    objPolicy.Cheque.Add(new PolicyIssueChequeReceived());

                }
                return View(objPolicy);
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