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
    public class PolicyRenewalSummaryReportController : BaseController
    {
        // GET: PolicyRenewalSummaryReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PolicyRenewalSummary()
        {
            return PartialView("_PolicyRenewalSummary", new ReportRepository().GetPolicyRenewalSummary());
        }

        public ActionResult PolicyDetailsPopup(int id = 0)
        {
            if (id != 0)
                return PartialView("_PolicyDetailsPopup", new ReportRepository().GetPolicyDetails(id));
            return View();
        }
        //public ActionResult PolicyDetailsPopup(int id = 0)
        //{
        //    var List = new ReportRepository().GetPolicyDetails(id);
        //    return View(List);
        //}
        //public ActionResult PolicyDetailsPopup(int id = 0)
        //{
        //    List<RenewalSummary> objRenewal = (new ReportRepository()).GetPolicyDetails(id);
        //    return View(objRenewal);
        //}

        public ActionResult ExportPolicyRenewalSummary()
        {

            List<PolicyIssue> model = new ReportRepository().GetPolicyDetailsComplite();
            //string[] tags = (string[])TempData["Tags"];
            //if (TempData["Tags"] == null)
            //{
            //    TempData.Add("Tags", tags);
            //}
            //TempData.Keep("Tags");
            //ViewBag.tags = tags;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<Table border={0}1{0}>", (Char)34);
            sb.Append("<tr>");




            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Transaction No.</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Policy No.</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Client</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Contact Name</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Insured Name</td>", (Char)34); ;
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Insurance Company</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Product</td>", (Char)34);
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
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.RenewalDate);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.SalesMgName);
           



                sb.Append("</tr>");




            }
            sb.Append("</Table>");
            string ExcelFileName = "PolicyRenewalSummary.xls";
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