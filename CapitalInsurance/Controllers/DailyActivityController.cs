using Capital.DAL;
using Capital.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CapitalInsurance.Controllers
{
    public class DailyActivityController : BaseController
    {
        // GET: DailyActivity
        public ActionResult Index(string type = "all")
        {
            FillSalesManager();
            return View();
        }
        public ActionResult Create()
        {
          
            DailyActivity Model = new DailyActivityRepository().DAEmployeeDetails(UserID);
            Model.DailyActivityItems = new List<DailyActivityItem>();
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.TranDate = DateTime.Now;
        
            return View(Model);
        }
        [HttpPost]
        public ActionResult Create(DailyActivity Model)
        {
            Model.CreatedBy = UserID;
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View(Model);
            }
            Result res = new DailyActivityRepository().Insert(Model);
            if (res.Value)
            {
                TempData["Success"] = "Saved Successfully!";
            }
            else
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult DailyActivityReport(DateTime? From, DateTime? To, int EmpId = 0,string type="all")
        {
            var list = new DailyActivityRepository().GetDailyActivityDetails(From, To, EmpId, type);
            return PartialView("_DailyActivityReport", list);
           
        }
        void FillSalesManager()
        {
            ViewBag.SalesManager = new SelectList((new DropdownRepository()).GetDailyActivityComboforEmployee(UserID), "Id", "Name");
            //ViewBag.SalesManager = new SelectList((new SalesManagerRepository()).GetSalesManagers(), "SalesMgId", "SalesMgName");
        }
        public ActionResult PreviousDailyActivity(int id)
        {
            return View(new DailyActivityRepository().GetPreviousDailyActivityList(id));
        }
       public ActionResult Edit(int id)
        {
            DailyActivity model = new DailyActivityRepository().GetDailyActivityHdDetails(id);
            model.DailyActivityItems = new DailyActivityRepository().GetDailyActivity(id);
            return View("Create", model);
        }
       [HttpPost]
       public ActionResult Edit(DailyActivity model)
       {
           if (!ModelState.IsValid)
           {
               var allErrors = ModelState.Values.SelectMany(v => v.Errors);
               return View(model);
           }
           Result res = new DailyActivityRepository().Update(model);


           if (res.Value)
           {
               TempData["Success"] = "Updated Successfully!";
           }
           else
           {

           }
           return RedirectToAction("Create");
       }
    }
}