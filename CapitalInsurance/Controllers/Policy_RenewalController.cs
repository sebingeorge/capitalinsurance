using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

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

            var pendingData = new PolicyRenewalRepository().GetNewPolicyForRenewal(FromDate, ToDate, PolicyNo, Client, SalesManager);

            Session["pendingData"] = pendingData;

            return PartialView("_PendingPolicyListGrid", pendingData);


            //return PartialView("_PendingPolicyListGrid", new PolicyRenewalRepository().GetNewPolicyForRenewal(FromDate, ToDate,PolicyNo,Client, SalesManager));
        }

        public ActionResult ExportToExcel()
        {

            List<PolicyIssue> model = (List<PolicyIssue>)Session["pendingData"];
           
            //string[] tags = (string[])TempData["Tags"];
            //if (TempData["Tags"] == null)
            //{
            //    TempData.Add("Tags", tags);
            //}
            //TempData.Keep("Tags");
            //ViewBag.tags = tags;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<Table border={0}1{0}>", (Char)34);
            //sb.Append("</tr><td colspan='6'><b><h3>"+cusname+"</h3></b></td></tr>");
            //sb.Append("<tr>");




            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Transaction No</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Policy No</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Client</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Contact Name</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Insured Name</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Insurance Company</td>", (Char)34); ;
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Coverage</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Total Premium</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Renewal Date</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Sales Manager</td>", (Char)34);




            sb.Append("</tr>");

            //decimal Debit = 0;
            //decimal Credit = 0;
            foreach (var item in model)
            {
                sb.Append("<tr>");



                sb.AppendFormat("<td>{1}</td>", (Char)34, item.StrTranNumber);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.PolicyNo);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.CusName);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.CustContPersonName);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.InsuredName);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.InsCmpName);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.InsPrdName);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.TotalPremium);

              
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.RenewalDate.ToString("MMMM dd,yyyy"));

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.SalesMgName);




                sb.Append("</tr>");




            }
            //sb.Append("</tr><td colspan='4' align='right'><b>Total Receivables</b></td><td><b>" + model[0].netamount + "</b></td><td></td></tr>");
            sb.Append("</Table>");
            string ExcelFileName;
           
           
            ExcelFileName = "PendingPolicyRenewal.xls"; 

            Response.Clear();
            Response.Charset = "";
            Response.ContentType = "application/excel";
            Response.AddHeader("Content-Disposition", "filename=" + ExcelFileName);
            Response.Write(sb);
            Response.End();
            Response.Flush();
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