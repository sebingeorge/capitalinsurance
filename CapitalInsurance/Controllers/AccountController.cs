﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CapitalInsurance.Models;
using System.Web.Security;
using System.Configuration;
using Capital.Domain;
using Capital.DAL;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace CapitalInsurance.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                string[] url = returnUrl.Split('/');
                if (url[url.Length - 1] == "LogOff")
                    returnUrl = "/";
            }
            catch (NullReferenceException) { returnUrl = "/"; }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string saltpassword = String.Concat(salt, model.Password);
            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(saltpassword, "sha1");

            var user = (new UserRepository()).GetUserByUserNameAndPassword(model.Username, hashedPassword);

            if (user == null || user.UserName == null)
            {
                ModelState.AddModelError("", "Invalid User name or Password. Please try again.");
                return View(model);
            }
            else
            {
                SignIn(user, model.OrganizationId, model.RememberMe, HttpContext.Response.Cookies);
                return RedirectToLocal(returnUrl);
            }
        }

        private void SignIn(User user, int OrganizationId, bool isPersistent, HttpCookieCollection cookiecollection)
        {
            var userData = String.Format("{0}|{1}|{2}|{3}|{4}",
                user.UserId, user.UserName, user.UserPassword, user.UserEmail, user.UserRole);
            var ticket = new FormsAuthenticationTicket(1, userData, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(360), isPersistent, userData, FormsAuthentication.FormsCookiePath);
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { HttpOnly = true };
            cookiecollection.Add(authCookie);

            HttpCookie userCookie = new HttpCookie("userCookie") { HttpOnly = true };
            userCookie.Values.Add("UserId", user.UserId.ToString());
            userCookie.Values.Add("UserName", user.UserName.ToString());
            userCookie.Values.Add("publicKey", ConvertPasswordToPublicKey(user.UserPassword));
            userCookie.Values.Add("UserEmail", user.UserEmail.ToString());
            userCookie.Values.Add("UserRole", user.UserRole.ToString());
            cookiecollection.Add(userCookie);
            Session.Add("user", userCookie);
            Session.Timeout = 360;
            UserRepository repo = new UserRepository();
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            repo.InsertLoginHistory(user, Session.SessionID.ToString(), ip, OrganizationId.ToString());

            List<Modules> modules = repo.GetModules(user.UserId);
            ModuleWisePermission modulepermission = new ModuleWisePermission();
            foreach (var item in modules)
            {
                if(item.isPermission == 1)
                {
                    switch (item.ModuleName)
                    {
                        case "Admin":
                            modulepermission.Admin = true;
                            break;
                        case "Documentation":
                            modulepermission.Documentation = true;
                            break;
                        case "Sales":
                            modulepermission.Sales = true;
                            break;
                        case "Finance":
                            modulepermission.Finance = true;
                            break;
                        case "Reports":
                            modulepermission.MISReports = true;
                            break;
                        default:
                            break;
                    }
                }                
            }
            Session.Add("ModulePermission", modulepermission);
            //return userCookie;
        }
        public string ConvertPasswordToPublicKey(string encrytedpwd)
        {
            return GetMD5CryptoString(encrytedpwd);
        }
        protected string GetMD5CryptoString(string original)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(original);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes);
        }
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(int? UserId)
        {
            if ((UserId ?? 0) == 0)
            {
                Capital.Domain.RegisterViewModel model = new Capital.Domain.RegisterViewModel();
                ViewBag.UserRole = new SelectList((new UserRepository()).GetUserRole(), "RoleId", "RoleName");
                ViewBag.Employee = new SelectList((new DropdownRepository()).GetSalesManagers(), "Id", "Name");
                model.Module = new UserRepository().GetModules(0);
                model.Forms = new UserRepository().GetFormsVsUser(UserId ?? 0).ToList();
                FillModulesDropdown(model.Module);
                return View(model);
            }
            else
            {
                ViewBag.UserRole = new SelectList((new UserRepository()).GetUserRole(), "RoleId", "RoleName");
                //Capital.Domain.RegisterViewModel model = new UserRepository().GetUserInfo(Id);
                Capital.Domain.User user = new UserRepository().GetUserById(UserId ?? 0);
                Capital.Domain.RegisterViewModel model = new Capital.Domain.RegisterViewModel()
                {
                    ConfirmPassword = "",
                    Email = user.UserEmail,
                    Password = "",
                    SalesMgId = user.SalesMgId,
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserRole = user.UserRole ?? 0,
                    Module = new UserRepository().GetModules(UserId),
                    Reporting = user.Reporting
                };


                ViewBag.Employee = new SelectList((new SalesManagerRepository()).GetSalesManagers(), "SalesMgId", "SalesMgName", user.SalesMgId);
                model.Module = new System.Collections.Generic.List<Modules>();
                var modules = (new UserRepository()).GetModules(UserId);

                model.Forms = new UserRepository().GetFormsVsUser(UserId ?? 0).ToList();

                foreach (var item in modules)
                {
                    model.Module.Add(item);
                }
              
                FillModulesDropdown(model.Module);
                return View(model);
            }

        }
        private void FillModulesDropdown(List<Modules> list)
        {
            List<Dropdown> moduleNames = new List<Dropdown>();
            foreach (var item in list)
            {
                moduleNames.Add(new Dropdown { Id = item.ModuleId, Name = item.ModuleName });
            }
            ViewBag.moduleList = new SelectList(moduleNames, "Id", "Name");
        }
        [AllowAnonymous]
        public ActionResult Edit(int Id)
        {            
            User user = new UserRepository().GetUserById(Id);
            Capital.Domain.RegisterViewModel model = new Capital.Domain.RegisterViewModel()
            {
                ConfirmPassword = "",
                Email = user.UserEmail,
                Password = "",
                SalesMgId = user.SalesMgId,
                UserId = user.UserId,
                UserName = user.UserName,
                UserRole = user.UserRole ?? 0,
                Module = new UserRepository().GetModules(Id),
                Reporting=user.Reporting
            };
            ViewBag.UserRole = new SelectList((new UserRepository()).GetUserRole(), "RoleId", "RoleName", user.UserRole);
            ViewBag.Employee = new SelectList((new SalesManagerRepository()).GetSalesManagers(), "SalesMgId", "SalesMgName", user.SalesMgId);
            FillModulesDropdown(model.Module);
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Capital.Domain.RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Capital.Domain.User user = new Capital.Domain.User()
                {
                    ConfirmPassword = model.Password,
                    UserEmail = model.Email,
                    UserId = model.UserId,
                    UserName = model.UserName,
                    UserPassword = model.Password,
                    UserRole = model.UserRole,
                    UserSalt = ConfigurationManager.AppSettings["salt"].ToString(),
                    SalesMgId = model.SalesMgId,
                    Module = model.Module,
                    Reporting=model.Reporting
                };
                int res = 0;
                if ((user.UserId ?? 0) == 0)
                {
                    string salt = ConfigurationManager.AppSettings["salt"].ToString();
                    string saltpassword = String.Concat(salt, user.UserPassword);
                    string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(saltpassword, "sha1");

                    user.UserPassword = hashedPassword;
                    user.UserSalt = salt;
                    user.Forms = model.Forms.Where(x => x.hasPermission).ToList();
                    res = (new UserRepository()).InsertUser(user);
                    TempData["Success"] = "Saved Successfully!";
                }
                else
                {
                    if (user.UserPassword != null && user.UserPassword != "")
                    {
                        string salt = ConfigurationManager.AppSettings["salt"].ToString();
                        string saltpassword = String.Concat(salt, user.UserPassword);
                        string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(saltpassword, "sha1");

                        user.UserPassword = hashedPassword;
                        user.UserSalt = salt;
                    }
                    user.Forms = model.Forms.Where(x => x.hasPermission).ToList();
                    res = (new UserRepository()).UpdateUser(user);
                    TempData["Success"] = "Updated Successfully!";
                }
                if (res > 0)
                {
                    return RedirectToAction("UserList");
                }
            }
            var allErrors = ModelState.Values.SelectMany(v => v.Errors);
            ViewBag.UserRole = new SelectList((new UserRepository()).GetUserRole(), "RoleId", "RoleName");
            ViewBag.Employee = new SelectList((new SalesManagerRepository()).GetSalesManagers(), "SalesMgId", "SalesMgName");
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult UserList()
        {
            return View(new UserRepository().GetUserAndModuleInfoList());
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }



        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
      
        //
        // POST: /Account/LogOff

        public ActionResult LogOff()
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            SignOff();
            return RedirectToAction("Index", "Login");
        }
        private void SignOff()
        {
            UserRepository repo = new UserRepository();
            string sessionid = Session.SessionID;
            repo.UpdateLoginHistory(sessionid);

            Session.Abandon();
            UnsetAuthorizationCookie(HttpContext.Response, HttpContext.Request.Cookies);
            ////UserRepository repo = new UserRepository();
            //string sessionid = Session.SessionID;
            ////repo.UpdateLoginHistory(sessionid);

            //Session.Abandon();
            //UnsetAuthorizationCookie(HttpContext.Response, HttpContext.Request.Cookies);
        }
        private void UnsetAuthorizationCookie(HttpResponseBase httpresponsebase, HttpCookieCollection cookiecollection)
        {
            HttpCookie authCookie = cookiecollection[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                cookiecollection.Remove(FormsAuthentication.FormsCookieName);
                authCookie.Expires = DateTime.Now.AddDays(-10);
                authCookie.Value = null;
                httpresponsebase.SetCookie(authCookie);
            }
            HttpCookie userCookie = cookiecollection["userCookie"];
            if (userCookie != null)
            {
                cookiecollection.Remove("userCookie");
                userCookie.Expires = DateTime.Now.AddDays(-10);
                userCookie.Value = null;
                httpresponsebase.SetCookie(userCookie);
            }
        }
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            Session.Remove("formPermission");
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
      
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(Capital.Domain.ResetPassword model)
        {

            if (ModelState.IsValid)
            {
                Capital.Domain.User user = new Capital.Domain.User()
                {
                    ConfirmPassword = model.Password,
                    UserId = UserID,
                    UserPassword = model.Password,
                    UserSalt = ConfigurationManager.AppSettings["salt"].ToString(),

                };
                int res = 0;
                if (user.UserPassword != null && user.UserPassword != "")
                {
                    string salt = ConfigurationManager.AppSettings["salt"].ToString();
                    string saltpassword = String.Concat(salt, user.UserPassword);
                    string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(saltpassword, "sha1");

                    user.UserPassword = hashedPassword;
                    user.UserSalt = salt;
                }

                res = (new UserRepository()).UpdateUserPassword(user);
            }
            return RedirectToAction("LogOff");

        }

    }
}