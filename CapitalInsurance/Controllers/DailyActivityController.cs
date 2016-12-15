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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DailyActivity()
        {

            DailyActivity Model = new DailyActivityRepository().DAEmployeeDetails(UserID);
            Model.DailyActivityItems = new List<DailyActivityItem>();
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityDate = DateTime.Now;
            return View(Model);
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
            Model.DailyActivityDate = DateTime.Now;
            return View(Model);
        }
      [HttpPost]
        public ActionResult Create(DailyActivity Model)
        {
            //Model.TranDate = System.DateTime.Now;
            //Model.CreatedDate = System.DateTime.Now;
            //Model.CreatedBy = UserID;
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
            return RedirectToAction("Index", new { type = 1 });
        }
    }
}