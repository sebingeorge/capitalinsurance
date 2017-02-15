using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Data;
using System.Text;


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
            //ViewBag.Type = type;
            FillDropdowns();
            PolicyIssue model = new PolicyIssue();
            model.TranType = "NewPolicy";
            model.PolicyStage = type;
            var internalid = PolicyIssueRepository.GetNextDocNo(model.TranType);
            model.TranNumber = "CIB/NEW/" + internalid;
            model.Cheque = new List<PolicyIssueChequeReceived>();
            model.Cheque.Add(new PolicyIssueChequeReceived());
            model.PolicySubDate = DateTime.Now;
            model.EffectiveDate = DateTime.Now;
            model.RenewalDate = DateTime.Now.AddMonths(6);
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
                FillDropdowns();
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
            return RedirectToAction("Index", new { type = model.PolicyStage});
        }
        public ActionResult Edit(int?type,int Id)
          
        {
          
            ViewBag.Type = type;
            ViewBag.Title = "Edit";
            FillDropdowns();
         
            PolicyIssue objPolicy = new PolicyIssueRepository().GetNewPolicy(Id);
            //objPolicy.PolicyStage = type;
            objPolicy.Cheque = new PolicyIssueRepository().GetChequeDetails(Id);
            objPolicy.Committed = new List<PaymentCommitments>();
            objPolicy.Committed.Add(new PaymentCommitments());
            if(type==2)
            {
                objPolicy.Committed = new PolicyIssueRepository().GetCommittedDetails(Id);
                if (objPolicy.Committed.Count == 1)
                {
                    objPolicy.Committed.Add(new PaymentCommitments());
                    objPolicy.Committed.Add(new PaymentCommitments());
                    objPolicy.Committed.Add(new PaymentCommitments());
                }
                else if(objPolicy.Committed.Count == 2)
                {
                    objPolicy.Committed.Add(new PaymentCommitments());
                    objPolicy.Committed.Add(new PaymentCommitments());
                }
                else if (objPolicy.Committed.Count == 3)
                {
                    objPolicy.Committed.Add(new PaymentCommitments());
                }
                objPolicy.PolicyStage = 2;
                return View("PaymentCommitments", objPolicy);
            }
            if (type == 3)
            {
                objPolicy.Committed = new PolicyIssueRepository().GetCommittedDetails(Id);
                objPolicy.Cheque = new PolicyIssueRepository().GetChequeDetails(Id);
                if (objPolicy.Cheque.Count == 0)
                {
                    objPolicy.Cheque.Add(new PolicyIssueChequeReceived());
                  
                }
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
            objPolicy.PolicyStage = type;
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
                objPolicy.Cheque = new PolicyIssueRepository().GetChequeDetails(Id);
                objPolicy.Committed = new PolicyIssueRepository().GetCommittedDetails(Id);
            }
           
            return View(objPolicy);

        }
        public ActionResult CreatePaymentCollection(int Id)
        {
            ViewBag.Type = 3;
            ViewBag.Title = "Edit";
            FillDropdowns();
            PolicyIssue Model = new PolicyIssueRepository().GetNewPolicy(Id);
            Model.PolicyStage = 3;

            Model.Cheque = new PolicyIssueRepository().GetChequeDetails(Id);
            if (Model.Cheque.Count==0)
            {
            Model.Cheque = new List<PolicyIssueChequeReceived>();
            Model.Cheque.Add(new PolicyIssueChequeReceived());
            }
           


            Model.Committed = new PolicyIssueRepository().GetCommittedDetails(Id);
            return View("PaymentCommitments", Model);
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
            return RedirectToAction("Index", new { type = model.PolicyStage });
        }
        [HttpGet]
        public JsonResult GetCustomerContactDetailsByKey(int Id)
        {
            var details = (new PolicyIssueRepository()).GetCustomerContactDetails(Id);
            return Json(new { Success = true, ContactName = details.ContactName, Designation = details.Designation, EmailId = details.EmailId, MobileNo = details.MobileNo, OfficeNo = details.OfficeNo, SalesMgId = details.SalesMgId, Address1 = details.Address1, Address2 = details.Address2 }, JsonRequestBehavior.AllowGet);
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

            //if (type == 2)
            //{
            //    return PartialView("_NewPolicyListGrid", new PolicyIssueRepository().GetPaymentCommittedList(UserID, FromDate, ToDate, PolicyNo, Client, SalesManager));
            //}
            //else if (type == 3)
            //{
            //    return PartialView("_NewPolicyListGrid", new PolicyIssueRepository().GetPaymentCollectionList(FromDate, ToDate, PolicyNo, Client, SalesManager));
            //}

            List<PolicyIssue> model = new PolicyIssueRepository().GetNewPolicy(UserID, UserRolename, FromDate, ToDate, PolicyNo, Client, SalesManager);

            return PartialView("_NewPolicyListGrid", model);
        }
        public ActionResult PendingPolicyForCommitments()
        {
          
            return View();

        }
        public ActionResult PendingPolicyForCommitmentsGrid(string trnno = "", string client = "", string insuredname = "", string insuredComp = "", string coverage = "")
        {
            //return PartialView("_PendingPolicyForCommitments", new PolicyIssueRepository().GetNewPolicyForCommitments(trnno , client ,insuredname , insuredComp , coverage));

            var griddata = new PolicyIssueRepository().GetNewPolicyForCommitments(UserID,UserRolename,trnno, client, insuredname, insuredComp, coverage);

            Session["GridDate"] = griddata;
            Session["excelname"]="Commitments";
            ViewBag.type = 2;
            return PartialView("_PendingPolicyForCommitments", griddata);
        }
        public ActionResult PendingPolicyForPaymentDetails()
        {
            return View("PendingPolicyForCommitments");
        }
        public ActionResult PendingPolicyForPaymentDetailsGrid(string trnno = "", string client = "", string insuredname = "", string insuredComp = "", string coverage = "")

        {
            var griddata = new PolicyIssueRepository().GetNewPolicyForPaymentCollection(trnno, client, insuredname, insuredComp, coverage);
            Session["GridDate"] = griddata;
            Session["excelname"]="PaymentDetails";
            return PartialView("_PendingPolicyForPaymentDetails", griddata);
        }
        public ActionResult UpdateCommitments(PolicyIssue model)
        {
           Result res = new PolicyIssueRepository().UpdatePaymentCommitments(model);
            if (res.Value)
            {
                TempData["Success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index", new { type = model.PolicyStage });
        }
        public ActionResult Print(int Id)
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "ReceiptVoucher.rpt"));

            DataSet ds = new DataSet();
            ds.Tables.Add("Head");
            ds.Tables.Add("Items");

            //    //-------HEAD


            ds.Tables["Head"].Columns.Add("VoucherNo");
            ds.Tables["Head"].Columns.Add("DocumentDate");
            ds.Tables["Head"].Columns.Add("TotalPremium");
            ds.Tables["Head"].Columns.Add("Customer");
            ds.Tables["Head"].Columns.Add("Company");
            ds.Tables["Head"].Columns.Add("Product");
            ds.Tables["Head"].Columns.Add("PaymentTo");
            //-------DT
            ds.Tables["Items"].Columns.Add("ReceiptType");
            ds.Tables["Items"].Columns.Add("ReceiveFrom");
            ds.Tables["Items"].Columns.Add("Amount");
            ds.Tables["Items"].Columns.Add("ChequeNo");
            ds.Tables["Items"].Columns.Add("Date");
            ds.Tables["Items"].Columns.Add("Bank");

            var Head = new PolicyIssueRepository().GetReceiptHdForPrint(Id);
            DataRow dr = ds.Tables["Head"].NewRow();
            dr["VoucherNo"] = Head.StrTranNumber;
            dr["DocumentDate"] = Head.TranDate.Value.ToString("dd-MMM-yyyy");
            dr["TotalPremium"] = Head.TotalPremium;
            dr["Customer"] = Head.CusName;
            dr["Company"] = Head.InsCmpName;
            dr["Product"] = Head.InsPrdName;
            dr["PaymentTo"] = Head.PaymentTo;


            ds.Tables["Head"].Rows.Add(dr);

            var Items = new PolicyIssueRepository().GetReceiptChequeDetailsforPrint(Id);

            foreach (var item in Items)
            {
                var ReceiptDetails = new PolicyIssueChequeReceived
                {
                    CusName = item.CusName,
                    PayModeName = item.PayModeName,
                    TotalPremium = item.TotalPremium,
                    ChequeNo = item.ChequeNo,
                    ChequeDate = item.ChequeDate,
                    BankBranch = item.BankBranch,
                    BankName = item.BankName,
                    ChequeAmt=item.ChequeAmt
                };

                DataRow dri = ds.Tables["Items"].NewRow();
                dri["ReceiptType"] = ReceiptDetails.PayModeName;
                dri["ReceiveFrom"] = ReceiptDetails.CusName;
                dri["Amount"] = ReceiptDetails.TotalPremium;
                dri["ChequeNo"] = ReceiptDetails.ChequeNo;
                dri["Date"] = ReceiptDetails.ChequeDate.Value.ToString("dd-MMM-yyyy");
                dri["Bank"] = ReceiptDetails.BankName;
                ds.Tables["Items"].Rows.Add(dri);
            }

            ds.WriteXml(Path.Combine(Server.MapPath("~/XML"), "ReceiptVoucher.xml"), XmlWriteMode.WriteSchema);

            rd.SetDataSource(ds);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ActionResult ExportToExcel()
        {

              List<PolicyIssue> model = (List<PolicyIssue>)Session["GridDate"];
            string excelname= Session["excelname"].ToString();
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
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Client</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Contact Name</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Insured Name</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Insurance Company</td>", (Char)34); ;
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Coverage</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Total Premium</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Balance</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Renewal Date</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Sales Manager</td>", (Char)34);




            sb.Append("</tr>");

            //decimal Debit = 0;
            //decimal Credit = 0;
            foreach (var item in model)
            {
                sb.Append("<tr>");



                sb.AppendFormat("<td>{1}</td>", (Char)34, item.StrTranNumber);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.CusName);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.CustContPersonName);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.InsuredName);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.InsCmpName);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.InsPrdName);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.TotalPremium);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.BalanceAmount);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.RenewalDate.ToString("MMMM dd,yyyy"));

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.SalesMgName);




                sb.Append("</tr>");




            }
            //sb.Append("</tr><td colspan='4' align='right'><b>Total Receivables</b></td><td><b>" + model[0].netamount + "</b></td><td></td></tr>");
            sb.Append("</Table>");
            string ExcelFileName;
            if (excelname=="Commitments")
            { ExcelFileName = "PendingPolicyForCommitments.xls"; }
            else
            { ExcelFileName = "PendingPolicyForPaymentDetails.xls"; }
            
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