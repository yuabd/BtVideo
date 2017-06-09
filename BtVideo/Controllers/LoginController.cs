using BtVideo.Models.Site;
using BtVideo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BtVideo.Controllers
{
    public class LoginController : Controller
    {
        private MembershipService membershipService = new MembershipService();

        // GET: Login
        public ActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginMessage = membershipService.Login(model.UserID, model.Password, model.RememberMe);

                if (loginMessage == "OK")
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl.ToString());
                    else
                        return RedirectToAction("Index", "Blog", new { area = "Admin" });
                }
                else
                {
                    ViewBag.LoginError = loginMessage;

                    return View("Index", model);
                }
            }
            else
            {
                return View("Index", model);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return Redirect("/");
        }
    }
}