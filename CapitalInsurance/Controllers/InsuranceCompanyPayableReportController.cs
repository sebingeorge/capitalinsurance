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
    public class InsuranceCompanyPayableReportController : BaseController
    {
        // GET: InsuranceCompanyPayableReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InsuranceCompanyPayableReport()
        {
            var model = new ReportRepository().GetInsuranceCompanyPayable();
            Session["BussData"] = model;
            return PartialView("_InsuranceCompanyPayableReport", model);
            //return PartialView("_InsuranceCompanyPayableReport", new ReportRepository().GetInsuranceCompanyPayable());

        }
        public ActionResult PolicyDetailsPopUp(int id = 0)
        {
            if (id != 0)
                return PartialView("_PolicyDetailsPopup", new ReportRepository().GetPolicyDetailsforPayablePopUp(id));
            return View();
        }


        public ActionResult InsuranceCompanyPayable()
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


                sb.AppendFormat("<td style={0}font-weight:bold;{0}>INSURANCE COMPANY</td>", (Char)34);
                sb.AppendFormat("<td style={0}font-weight:bold;{0}>PAYABLE</td>", (Char)34);
            

            sb.Append("</tr>");

            foreach (var item in model)
            {
                sb.Append("<tr>");
                //sb.AppendFormat("<td></td>", (Char)34);



                sb.AppendFormat("<td>{1}</td>", (Char)34, item.InsCmpName);
               
                    sb.AppendFormat("<td>{1}</td>", (Char)34, item.CommittedAmt);
              



                sb.Append("</tr>");




            }
            sb.Append("</Table>");
            string ExcelFileName = "InsuranceCompanyPayable.xls";
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