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
    public class SalesIncentiveReportController : BaseController
    {
        // GET: SalesIncentiveReport
        public ActionResult Index()
        {
            FillFinYear();
            return View();
        }
        public ActionResult SalesIncentiveReport(int? FyId)
        {

            var model = new SalesTargetRepository().GetSalesIncentiveReportDetails(UserID, UserRolename, FyId);
            Session["BussData"] = model;
            return PartialView("_SalesIncentiveReport", model);
            //return PartialView("_SalesIncentiveReport", new SalesTargetRepository().GetSalesIncentiveReportDetails(UserID,UserRolename,FyId));
        }
        void FillFinYear()
        {
            ViewBag.FinYear = new SelectList((new DropdownRepository()).FillFinYear(), "Id", "Name");
        }


        public ActionResult ExportSalesIncentive(string FinYear)
        {

            List<SalesIncentive> model = (List<SalesIncentive>)Session["BussData"];
            //string[] tags = (string[])TempData["Tags"];
            ////if (TempData["Tags"] == null)
            //{
            //    TempData.Add("Tags", tags);
            //}
            //TempData.Keep("Tags");
            //ViewBag.tags = tags;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<Table border={0}1{0}>", (Char)34);
            sb.Append("</tr><td colspan='3'><b><h3> SALESINCENTIVE -" + FinYear + "</h3></b></td></tr>");
            sb.Append("<tr>");


            sb.AppendFormat("<td style={0}font-weight:bold;{0}>CODE</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>EMPLOYEE</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>BENCHMARK</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE %</td>", (Char)34);

            sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOT COMMITION(JAN)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE AMOUNT(JAN)</td>", (Char)34);

            sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOT COMMITION(FEB)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE AMOUNT(FEB)</td>", (Char)34);

            sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOT COMMITION(MAR)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE AMOUNT(MAR)</td>", (Char)34);

            sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOT COMMITION(APR)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE AMOUNT(APR)</td>", (Char)34);

            sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOT COMMITION(MAY)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE AMOUNT(MAY)</td>", (Char)34);

            sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOT COMMITION(JUN)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE AMOUNT(JUN)</td>", (Char)34);

            sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOT COMMITION(JUL)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE AMOUNT(JUL)</td>", (Char)34);

            sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOT COMMITION(AUG)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE AMOUNT(AUG)</td>", (Char)34);

            sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOT COMMITION(SEP)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE AMOUNT(SEP)</td>", (Char)34);

            sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOT COMMITION(OCT)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE AMOUNT(OCT)</td>", (Char)34);

            sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOT COMMITION(NOV)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE AMOUNT(NOV)</td>", (Char)34);

            sb.AppendFormat("<td style={0}font-weight:bold;{0}>TOT COMMITION(DEC)</td>", (Char)34);
            sb.AppendFormat("<td style={0}font-weight:bold;{0}>INCENTIVE AMOUNT(DEC)</td>", (Char)34);


            sb.Append("</tr>");

            foreach (var item in model)
            {
                sb.Append("<tr>");
                //sb.AppendFormat("<td></td>", (Char)34);



                sb.AppendFormat("<td>{1}</td>", (Char)34, item.SalesMgCode);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.SalesMgName);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.Benchmark);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.IncentivePerc);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.JanComm);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.JanInctve);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.FebComm);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.FebInctve);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.MarComm);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.MarInctve);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.AplComm);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.AplInctve);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.MayComm);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.MayInctve);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.JunComm);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.JunInctve);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.JulyComm);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.JulyInctve);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.AugComm);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.AugInctve);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.SepComm);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.SepInctve);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.OctComm);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.OctInctve);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.NovComm);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.NovInctve);

                sb.AppendFormat("<td>{1}</td>", (Char)34, item.DecComm);
                sb.AppendFormat("<td>{1}</td>", (Char)34, item.DecInctve);

                sb.Append("</tr>");




            }
            sb.Append("</Table>");
            string ExcelFileName = "SalesIncentive.xls";
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