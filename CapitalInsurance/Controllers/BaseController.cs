﻿using CapitalInsurance.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capital.DAL;
using Capital.Domain;
namespace CapitalInsurance.Controllers
{
    [AuthorizeUser]

    public class BaseController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {            
            return base.BeginExecuteCore(callback, state);

        }

        public int UserID
        {
            get
            {
                HttpCookie usr = (HttpCookie)Session["user"];
                int Id = usr == null ? 0 : Convert.ToInt32(usr["UserId"]);
                return Id;
            }
            set
            {
            }
        }
        public string UserName
        {
            get
            {
                HttpCookie usr = (HttpCookie)Session["user"];
                return usr["UserName"].ToString();
            }
            set
            {

            }
        }
        public DateTime FYStartdate
        {
            get
            {
                FinancialYearRepository repo = new FinancialYearRepository();
                return repo.GetFinStartDate();

            }
            set
            {

            }
        }
    }
}