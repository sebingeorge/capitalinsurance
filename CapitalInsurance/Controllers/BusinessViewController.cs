using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class BusinessViewController : BaseController
    {
        // GET: BusinessView
        [HttpPost]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BusinessViewDetails(string[] tags, string Company = "", string PolicyNo = "", string Client = "", string SalesManager = "")
        {
            ViewBag.tags = tags;
            if (TempData["Tags"] == null)
            {
                TempData.Add("Tags", tags);
            }
            TempData.Keep("Tags");
            var model = new BusinessViewRepository().GetBusinessViewDetails(UserID, Company, PolicyNo, Client, SalesManager);
            Session["BussData"] = model;
            return PartialView("_BussinessViewDetails", model);
        }
        public ActionResult BusinessViewDetailsFilter(string Company = "", string PolicyNo = "", string Client = "", string SalesManager = "")
        {
            string[] tags = (string[])TempData["Tags"];
            if (TempData["Tags"] == null)
            {
                TempData.Add("Tags", tags);
            }
            TempData.Keep("Tags");
            ViewBag.tags = tags;
            var model = new BusinessViewRepository().GetBusinessViewDetails(UserID,Company, PolicyNo, Client, SalesManager);
            Session["BussData"] = model;
            return PartialView("_BussinessViewDetails", model);
        }
        public ActionResult AddingFields()
        {
            return View();
        }

        public ActionResult ExportBussnessView()
        {
            
                List<PolicyIssue> model = (List<PolicyIssue>)Session["BussData"];
                string[] tags = (string[])TempData["Tags"];
                if (TempData["Tags"] == null)
                {
                    TempData.Add("Tags", tags);
                }
                TempData.Keep("Tags");
                ViewBag.tags = tags;

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("<Table border={0}1{0}>", (Char)34);
                sb.Append("<tr>");


                if (tags.Contains("Model.Year"))
                {

                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>YEAR</td>", (Char)34);
                }
                if (tags.Contains("Model.Month"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>MONTH</td>", (Char)34);
                }
                //if (tags.Contains("Model.Day"))
                //{
                //    columns.Add(m => m.Day).Titled("DAY").SetWidth(5);
                //}
                if (tags.Contains("Model.SalesMgCode"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>EMP NO</td>", (Char)34);
                }
                if (tags.Contains("Model.SalesMgName"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>MANAGER NAME</td>", (Char)34);
                }
                if (tags.Contains("Model.InsuredName"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>INSURED NAME</td>", (Char)34); ;
                }
                if (tags.Contains("Model.CusName"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>CUSTOMER NAME</td>", (Char)34);
                }
                if (tags.Contains("Model.CustContPersonName"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>CONTACT PERSON</td>", (Char)34);
                }
                if (tags.Contains("Model.CustContDesignation"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>DESIGNATION</td>", (Char)34);
                }
                if (tags.Contains("Model.MobileNo"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>MOBILE NO</td>", (Char)34);
                }
                if (tags.Contains("Model.OfficeNo"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>BOARD NO</td>", (Char)34);
                }
                if (tags.Contains("Model.EmailId"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>EMAIL</td>", (Char)34);
                }
                if (tags.Contains("Model.TranType"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>TYPE OF BIZ</td>", (Char)34);
                }
                //if (tags.Contains("Model.InsCoverName"))
                //{
                //    columns.Add(m => m.InsCoverName).Titled("TYPE OF COVER").SetWidth(10);
                //}
                if (tags.Contains("Model.InsCmpName"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>INS COMPANY</td>", (Char)34);
                }
                if (tags.Contains("Model.InsPrdName"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>COVERAGE</td>", (Char)34);
                }
                if (tags.Contains("Model.PolicyNo"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>POLICY NO</td>", (Char)34);
                }
                if (tags.Contains("Model.EffectiveDate"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCEPTION DATE</td>", (Char)34);
                }
                if (tags.Contains("Model.ExpiryDate"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>EXPIRY DATE</td>", (Char)34);
                }
                if (tags.Contains("Model.RenewalDate"))
                {

                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>RENEWAL DATE</td>", (Char)34);
                }
                if (tags.Contains("Model.EndorcementDate"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>End date_CIB COMM CALC</td>", (Char)34);
                }
                if (tags.Contains("Model.ICActualDate"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>End Date_IC ACTUALS</td>", (Char)34);
                }
                if (tags.Contains("Model.EndType"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>END TYPE</td>", (Char)34);
                }
                if (tags.Contains("Model.EndorcementNo"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>END NUMBER</td>", (Char)34);
                }
                if (tags.Contains("Model.AdditionEmpNo"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>NO OF MEMBERS ADDED</td>", (Char)34);
                }
                if (tags.Contains("Model.DeletionEmpNo"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>NO OF MEMBERS DELETED</td>", (Char)34);
                }
                if (tags.Contains("Model.PremiumAmount"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>BASIC PREMIUM</td>", (Char)34);
                }
                if (tags.Contains("Model.PolicyFee"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>POLICY FEE</td>", (Char)34);
                }
                if (tags.Contains("Model.CommissionPerc"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>COMMISSION %</td>", (Char)34);
                }
                if (tags.Contains("Model.ExtraPremium"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>EXTRA PREMIUM</td>", (Char)34);
                }
                if (tags.Contains("Model.TotalPremium"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOTAL PREMIUM</td>", (Char)34);
                }


                if (tags.Contains("Model.cibpaid"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>CIB PAID</td>", (Char)34);
                }
                if (tags.Contains("Model.BalanceRecivable"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>BALANCE RECEIVABLE</td>", (Char)34);
                }
                if (tags.Contains("Model.InsCompPaid"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>INS COMP PAID</td>", (Char)34);
                }



                if (tags.Contains("Model.CommissionAmount"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOTAL COMMISSION</td>", (Char)34);
                }
                if (tags.Contains("Model.PaymentTo"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>PAID TO</td>", (Char)34);
                }
                if (tags.Contains("Model.Aging"))
                {
                    sb.AppendFormat("<td style={0}font-weight:bold;{0}>AGEING</td>", (Char)34);
                }

                sb.Append("</tr>");
       
            //decimal Debit = 0;
            //decimal Credit = 0;
            foreach (var item in model)
            {
                sb.Append("<tr>");
                //sb.AppendFormat("<td></td>", (Char)34);

                if (tags.Contains("Model.Year"))
                {

                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.Year);
                }

                if (tags.Contains("Model.Month"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.Month);
                }

                if (tags.Contains("Model.SalesMgCode"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.SalesMgCode);
                }
                if (tags.Contains("Model.SalesMgName"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.SalesMgName);
                }
                if (tags.Contains("Model.InsuredName"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.InsuredName);
                }
                if (tags.Contains("Model.CusName"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.CusName);
                }
                if (tags.Contains("Model.CustContPersonName"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.CustContPersonName);
                }
                if (tags.Contains("Model.CustContDesignation"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.CustContDesignation);
                }
                if (tags.Contains("Model.MobileNo"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.MobileNo);
                }
                if (tags.Contains("Model.OfficeNo"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.OfficeNo);
                }
                if (tags.Contains("Model.EmailId"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.EmailId);
                }
                if (tags.Contains("Model.TranType"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.TranType);
                }

                if (tags.Contains("Model.InsCmpName"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.InsCmpName);
                }
                if (tags.Contains("Model.InsPrdName"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.InsPrdName);
                }
                if (tags.Contains("Model.PolicyNo"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.PolicyNo);
                }
                if (tags.Contains("Model.EffectiveDate"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.EffectiveDate.ToString("dd-MMM-yyyy"));
                }
                if (tags.Contains("Model.ExpiryDate"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.ExpiryDate.ToString("dd-MMM-yyyy"));
                }
                if (tags.Contains("Model.RenewalDate"))
                {

                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.RenewalDate.ToString("dd-MMM-yyyy"));
                }
                if (tags.Contains("Model.EndorcementDate"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.EndorcementDate);
                }
                if (tags.Contains("Model.ICActualDate"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.ICActualDate);
                }
                if (tags.Contains("Model.EndType"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.EndType);
                }
                if (tags.Contains("Model.EndorcementNo"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.EndorcementNo);
                }
                if (tags.Contains("Model.AdditionEmpNo"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.AdditionEmpNo);
                }
                if (tags.Contains("Model.DeletionEmpNo"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.DeletionEmpNo);
                }
                if (tags.Contains("Model.PremiumAmount"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.PremiumAmount);
                }
                if (tags.Contains("Model.PolicyFee"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.PolicyFee);
                }
                if (tags.Contains("Model.CommissionPerc"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.CommissionPerc);
                }
                if (tags.Contains("Model.ExtraPremium"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.ExtraPremium);
                }
                if (tags.Contains("Model.TotalPremium"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.TotalPremium);
                }

                if (tags.Contains("Model.cibpaid"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.cibpaid);
                }
                if (tags.Contains("Model.BalanceRecivable"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.BalanceRecivable);
                }
                if (tags.Contains("Model.InsCompPaid"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.InsCompPaid);
                }


                if (tags.Contains("Model.CommissionAmount"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.CommissionAmount);
                }
                if (tags.Contains("Model.PaymentTo"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.PaymentTo);
                }
                if (tags.Contains("Model.Aging"))
                {
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.Aging);
                }



                sb.Append("</tr>");

                


            }
            sb.Append("</Table>");
            string ExcelFileName = "BussinessView.xls";
            Response.Clear();
            Response.Charset = "";
            Response.ContentType = "application/excel";
            Response.AddHeader("Content-Disposition", "filename=" + ExcelFileName);
            Response.Write(sb);
            Response.End();
            Response.Flush();
            return View();

        }
    }

}
