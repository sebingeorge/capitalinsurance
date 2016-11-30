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
        public ActionResult Index(int? type)
        {
            ViewBag.Type = type;
            ViewBag.Fromdate = FYStartdate;
            return View();
        }
        public ActionResult Create(int? type)
        {
            ViewBag.Type = type;
            FillDropdowns();
            PolicyIssue model = new PolicyIssue();
            model.TranType = "NewPolicy";
            var internalid = PolicyIssueRepository.GetNextDocNo(model.TranType);
            model.TranNumber = "CIB/NEW/" + internalid;
            model.Cheque = new List<PolicyIssueChequeReceived>();
            model.Cheque.Add(new PolicyIssueChequeReceived());
            model.PolicySubDate = DateTime.Now;
            model.EffectiveDate = DateTime.Now;
            model.RenewalDate = DateTime.Now;
            model.ICActualDate = null;
          
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(PolicyIssue model)
        {
            model.TranPrefix = model.TranNumber.Substring(0,model.TranNumber.LastIndexOf('/'));
            model.TranDate = System.DateTime.Now;
            model.CreatedDate = System.DateTime.Now;
            model.CreatedBy = UserID;
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new PolicyIssueRepository().Insert(model);
            if (res.Value)
            {
                TempData["Success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index", new { type=1});
        }
        public ActionResult Edit(int?type,int Id)
          
        {
          
            ViewBag.Type = type;
            ViewBag.Title = "Edit";
            FillDropdowns();
         
            PolicyIssue objPolicy = new PolicyIssueRepository().GetNewPolicy(Id);
            objPolicy.Cheque = new PolicyIssueRepository().GetChequeDetails(Id);
            objPolicy.Committed = new List<PaymentCommitments>();
            objPolicy.Committed.Add(new PaymentCommitments());
            if(type==2)
            {
                objPolicy.Committed = new PolicyIssueRepository().GetCommittedDetails(Id);
                return View("PaymentCommitments", objPolicy);
            }
            if (type == 3)
            {
                objPolicy.Committed = new PolicyIssueRepository().GetCommittedDetails(Id);
                objPolicy.Cheque = new PolicyIssueRepository().GetChequeDetails(Id);
                return View("PaymentCommitments", objPolicy);
            }
            return View("Create", objPolicy);
            

        }
        [HttpPost]
        public ActionResult Edit(PolicyIssue model)
        {
            model.TranDate = System.DateTime.Now;
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new PolicyIssueRepository().Update(model);


            if (res.Value)
            {
                TempData["Success"] = "Updated Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index", new { type = 1 });
        }

        public ActionResult PaymentCommitments(int Id, int? type)
        {

            ViewBag.Type = type;
            ViewBag.Title = "Edit";
            FillDropdowns();

            PolicyIssue objPolicy = new PolicyIssueRepository().GetNewPolicy(Id);
            objPolicy.Cheque = new List<PolicyIssueChequeReceived>();
            objPolicy.Cheque.Add(new PolicyIssueChequeReceived());
            if(type==2)
            {
                objPolicy.Committed = new List<PaymentCommitments>();
                objPolicy.Committed.Add(new PaymentCommitments());
                objPolicy.Committed.Add(new PaymentCommitments());
                objPolicy.Committed.Add(new PaymentCommitments());
                objPolicy.Committed.Add(new PaymentCommitments());
            }
            else
            {
                objPolicy.Committed = new PolicyIssueRepository().GetCommittedDetails(Id);
            }
           
            return View(objPolicy);

        }
        public ActionResult PaymentDetails(int Id, int? type)
        {

            ViewBag.Type = type;
            ViewBag.Title = "Edit";
            FillDropdowns();

            PolicyIssue objPolicy = new PolicyIssueRepository().GetNewPolicy(Id);
            objPolicy.Cheque = new PolicyIssueRepository().GetChequeDetails(Id);
            objPolicy.Committed = new List<PaymentCommitments>();
            objPolicy.Committed.Add(new PaymentCommitments());

            return View("PaymentCommitments", objPolicy);

        }
        public ActionResult Delete(int Id)
        {
            ViewBag.Title = "Delete";
            FillDropdowns();
            PolicyIssue objPolicy = new PolicyIssueRepository().GetNewPolicy(Id);
            objPolicy.Cheque = new PolicyIssueRepository().GetChequeDetails(Id);
            return View("Create", objPolicy);
        }
        [HttpPost]
        public ActionResult Delete(PolicyIssue model)
        {
            Result res = new PolicyIssueRepository().Delete(model);
            if (res.Value)
            {
                TempData["Success"] = "Deleted Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public JsonResult GetCustomerContactDetailsByKey(int Id)
        {
            var details = (new PolicyIssueRepository()).GetCustomerContactDetails(Id);
            return Json(new { Success = true, ContactName = details.ContactName, Designation = details.Designation, EmailId = details.EmailId, MobileNo = details.MobileNo, OfficeNo = details.OfficeNo }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTransNum(string Id)
        {
            var internalid = PolicyIssueRepository.GetNextDocNo(Id);
            string  prefix;
            if (Id=="RenewPolicy")
            {
                 prefix="CIB/REN/";
            }
            else
            {
                 prefix = "CIB/NEW/";
            }

            return Json(new { Success = true, internalid = prefix + internalid }, JsonRequestBehavior.AllowGet);
        }
      
        public ActionResult NewPolicyList(int? type,DateTime? FromDate, DateTime? ToDate,string PolicyNo="", string Client = "", string SalesManager = "")
        {
            ViewBag.Type = type;
            FromDate = FromDate ?? FYStartdate;
            ToDate = ToDate ?? DateTime.Now;
            if (type == 2)
            {
                return PartialView("_NewPolicyListGrid", new PolicyIssueRepository().GetPaymentCommittedList(FromDate, ToDate, PolicyNo, Client, SalesManager));
            }
            else if (type == 3)
            {
                return PartialView("_NewPolicyListGrid", new PolicyIssueRepository().GetPaymentCollectionList(FromDate, ToDate, PolicyNo, Client, SalesManager));
            }
            return PartialView("_NewPolicyListGrid", new PolicyIssueRepository().GetNewPolicy(FromDate, ToDate, PolicyNo,Client,SalesManager));
        }
        public ActionResult PendingPolicyForCommitments()
        {
            return View(new PolicyIssueRepository().GetNewPolicyForCommitments());
        }
        public ActionResult PendingPolicyForPaymentDetails()
        {
            return View("PendingPolicyForCommitments", new PolicyIssueRepository().GetNewPolicyForPaymentCollection());
        }
        public ActionResult UpdateCommitments(PolicyIssue model)
        {
            Result res = new PolicyIssueRepository().UpdatePaymentCommitments(model);
            if (res.Value)
            {
                TempData["Success"] = "Updated Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index", new { type = 1 });
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