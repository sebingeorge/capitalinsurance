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

namespace CapitalInsurance.Controllers
{
    public class Customer_InvoiceController : BaseController
    {
        // GET: Customer_Invoice
        public ActionResult Index()
        {
            FillCustomer();
            return View();
        }
        public ActionResult PendingPolicyForCustomerInvoice(int ClientId = 0)
        {

            return PartialView("_PendingPolicyForCustomerInvoice", new CustomerInvoiceRepository().GetPendingPoilcyforInvoice(ClientId));
        }
        public ActionResult Create(FormCollection collection)
        {
            var SelectedIds = collection.GetValues("selectedAddressesIds");
            List<int> PolicyIds = new List<int>();
            if(SelectedIds != null)
            {
                foreach (var item in SelectedIds)
                {
                    if(item.ToString() != "false")
                    {
                        PolicyIds.Add(Convert.ToInt32(item.ToString()));
                    }
                }
            }
            CustomerInvoice Model = new CustomerInvoiceRepository().GetPolicyDetailsForInvoice(PolicyIds);
            Model.Items = new CustomerInvoiceRepository().GetPolicyDetailsForInvoiceDetails(PolicyIds);
            var internalid = CustomerInvoiceRepository.GetNextDocNo();
            Model.CusInvoiceRefNo = "CIB/INV/" + internalid;
            Model.CusInvoiceDate = DateTime.Now;

            return View(Model);
        }
        public ActionResult Save(CustomerInvoice model)
        {
            
            model.CreatedDate = System.DateTime.Now;
            model.CreatedBy = UserID;
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            Result res = new CustomerInvoiceRepository().Insert(model);
            if (res.Value)
            {
                TempData["Success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult PreviousCustomerInvoice()
        {
            return View(new CustomerInvoiceRepository().GetCustomerInvoiceList());
        }
        public ActionResult Print(int Id)
        {
            
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CustomerInvoice.rpt"));

            DataSet ds = new DataSet();
            ds.Tables.Add("Head");
            ds.Tables.Add("Items");

            //    //-------HEAD

        
            ds.Tables["Head"].Columns.Add("DocumentNo");
            ds.Tables["Head"].Columns.Add("DocumentDate");
            ds.Tables["Head"].Columns.Add("Location");
            ds.Tables["Head"].Columns.Add("InsuredName");
            //-------DT
            ds.Tables["Items"].Columns.Add("EffectiveDate");
            ds.Tables["Items"].Columns.Add("InsuranceType");
            ds.Tables["Items"].Columns.Add("PolicyOrEndorseNo");
            ds.Tables["Items"].Columns.Add("Company");
            ds.Tables["Items"].Columns.Add("Premium");
            CustomerInvoiceRepository repo = new CustomerInvoiceRepository();
            var Head = repo.GetCustomerInvoiceHdDetails();
            DataRow dr = ds.Tables["Head"].NewRow();
            dr["DocumentNo"] = Head.CusInvoiceRefNo;
            dr["DocumentDate"] = Head.CusInvoiceDate.Value.ToString("dd-MMM-yyyy");
            dr["InsuredName"] = Head.CusName;
           
            ds.Tables["Head"].Rows.Add(dr);

            var Items = repo.GetCustomerInvoicePrint(Id);

            foreach (var item in Items)
            {
                var CustomerInvoiceItem = new CustomerInvoiceItem
                {
                    EffectiveDate = item.EffectiveDate,
                    TranType = item.TranType,
                    PolicyNo=item.PolicyNo,
                    InsCmpName = item.InsCmpName,
                    TotalPremium = item.TotalPremium
                };

                DataRow dri = ds.Tables["Items"].NewRow();
                dri["EffectiveDate"] = CustomerInvoiceItem.EffectiveDate.Value.ToString("dd-MMM-yyyy");
                dri["InsuranceType"] = CustomerInvoiceItem.TranType;
                dri["PolicyOrEndorseNo"] = CustomerInvoiceItem.PolicyNo;
                dri["Company"] = CustomerInvoiceItem.InsCmpName;
                dri["Premium"] = CustomerInvoiceItem.TotalPremium;
                ds.Tables["Items"].Rows.Add(dri);
            }

            ds.WriteXml(Path.Combine(Server.MapPath("~/XML"), "CustomerInvoice.xml"), XmlWriteMode.WriteSchema);

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
        void FillCustomer()
        {
            ViewBag.Customer = new SelectList((new DropdownRepository()).GetCustomerFrmPolicy(), "Id", "Name");
        }
    }
}