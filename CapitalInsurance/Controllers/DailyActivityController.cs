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
            //Type();
            DailyActivity Model = new DailyActivityRepository().DAEmployeeDetails(UserID);
            Model.DailyActivityItems = new List<DailyActivityItem>();
            Model.DailyActivityItems.Add(new DailyActivityItem());
            Model.DailyActivityDate = DateTime.Now;
            return View(Model);
        }
        //void Type()
        //{
        //    ViewBag.Type = new SelectList((new DropdownRepository()).FillDAType(), "Id", "Name");
        //}
    }
}