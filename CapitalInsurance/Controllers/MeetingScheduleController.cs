using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalInsurance.Controllers
{
    public class MeetingScheduleController : Controller
    {
        // GET: MeetingSchedule
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MeetingSchedule()
        {
            return View();
        }
        public ActionResult PreviousMeetingSchedule()
        {
            return View();
        }
    }
}