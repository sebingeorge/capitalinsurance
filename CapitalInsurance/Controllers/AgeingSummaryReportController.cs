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
    public class AgeingSummaryReportController : BaseController
    {
        // GET: AgeingSummaryReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AgeingSummary(string Client = "")
        {
            var list = new ReportRepository().GetAgeingSummaryBasedCommittedDate(Client);

            Session["ageingdata"] = list;

            return PartialView("_AgeingSummary", list);
        }

        public ActionResult DetailsPopup(int id)
        {
            var popup = new ReportRepository().GetAgeingSummaryBasedDetailed(id);

            Session["ageingdatadetail"] = popup;
            return PartialView("_AgeingSummaryDetailed", popup);
        }


        public ActionResult ExportAgeingSummaryPopup(string cusname)
        {

            List<AgeingSummary> model = (List<AgeingSummary>)Session["ageingdatadetail"];
            //string[] tags = (string[])TempData["Tags"];
            //if (TempData["Tags"] == null)
            //{
            //    TempData.Add("Tags", tags);
            //}
            //TempData.Keep("Tags");
            //ViewBag.tags = tags;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<Table border={0}1{0}>", (Char)34);
            sb.Append("</tr><td colspan='6'><b><h3>"+cusname+"</h3></b></td></tr>");
            sb.Append("<tr>");




            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Policy No</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Date</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Coverage</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Committed Date</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Committed Amount</td>", (Char)34); ;
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Paid</td>", (Char)34);




            sb.Append("</tr>");

            //decimal Debit = 0;
            //decimal Credit = 0;
            foreach (var item in model)
            {
                sb.Append("<tr>");



                sb.AppendFormat("<td>{1}</td>", (Char)34, item.PolicyNo);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.Date);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.Coverage);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.CommittedDate);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.CommittedAmount);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.Paid);




                sb.Append("</tr>");




            }
            sb.Append("</tr><td colspan='4' align='right'><b>Total Receivables</b></td><td><b>" + model[0].netamount + "</b></td><td></td></tr>");
            sb.Append("</Table>");
            string ExcelFileName = "AgeingSummaryCustomerwise.xls";
            Response.Clear();
            Response.Charset = "";
            Response.ContentType = "application/excel";
            Response.AddHeader("Content-Disposition", "filename=" + ExcelFileName);
            Response.Write(sb);
            Response.End();
            Response.Flush();
            return View();

        }
        public ActionResult ExportAgeingSummary()
        {

            List<AgeingSummary> model = (List<AgeingSummary>)Session["ageingdata"];
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




            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Customer</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Total Receivable</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>Overdue</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>(0 - 15)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>(15 - 30)</td>", (Char)34); ;
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>(30 - 60)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>(60 - 90)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>(90 - 180) </td>", (Char)34);



            sb.Append("</tr>");

            //decimal Debit = 0;
            //decimal Credit = 0;
            foreach (var item in model)
            {
                sb.Append("<tr>");



                sb.AppendFormat("<td>{1}</td>", (Char)34, item.CusName);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.TotalPremium);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.Overdue);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.Amount1);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.Amount2);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.Amount3);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.Amount4);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.Amount5);



                sb.Append("</tr>");




            }
            sb.Append("</Table>");
            string ExcelFileName = "AgeingSummary.xls";
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