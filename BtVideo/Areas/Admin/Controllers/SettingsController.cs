using BtVideo.Models;
using BtVideo.Models.Others;
using BtVideo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace BtVideo.Areas.Admin.Controllers
{
	[Authorize(Roles="Administrator")]
    public class SettingsController : Controller
    {
		private MembershipService membershipService = new MembershipService();
        //
        // GET: /Admin/Settings/

        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Company()
		{
			SiteSettings siteSettings = new SiteSettings();
			return View(siteSettings);
		}

		[HttpPost]
		public ActionResult Company(SiteSettings siteSettings)
		{
			var xml = XDocument.Load(Server.MapPath("~/SiteSettings.xml"));
			XAttribute field;

			field = (from m in xml.Descendants("companyName") select m.Attribute("value")).SingleOrDefault();
			field.SetValue(siteSettings.CompanyName);
			field = (from m in xml.Descendants("companyWebsite") select m.Attribute("value")).SingleOrDefault();
			field.SetValue(siteSettings.CompanyWebsite);
			field = (from m in xml.Descendants("icp") select m.Attribute("value")).SingleOrDefault();
			field.SetValue(siteSettings.ICP != null ? siteSettings.ICP : "");
			field = (from m in xml.Descendants("title") select m.Attribute("value")).SingleOrDefault();
			field.SetValue(siteSettings.Title != null ? siteSettings.Title : "");
			field = (from m in xml.Descendants("keywords") select m.Attribute("value")).SingleOrDefault();
			field.SetValue(siteSettings.Keywords != null ? siteSettings.Keywords : "");
			field = (from m in xml.Descendants("description") select m.Attribute("value")).SingleOrDefault();
			field.SetValue(siteSettings.Description != null ? siteSettings.Description : "");

			xml.Save(Server.MapPath("~/SiteSettings.xml"));
			return View(siteSettings);
		}

		[HttpPost]
		public ActionResult SiteSettings(SiteSettings siteSettings)
		{
			var xml = XDocument.Load(Server.MapPath("~/SiteSettings.xml"));
			XAttribute field;
			field = (from m in xml.Descendants("address") select m.Attribute("value")).SingleOrDefault();
			field.SetValue(siteSettings.Address != null ? siteSettings.Address : "");
			field = (from m in xml.Descendants("qq") select m.Attribute("value")).SingleOrDefault();
			field.SetValue(siteSettings.QQ != null ? siteSettings.QQ : "");
			field = (from m in xml.Descendants("companyEmail") select m.Attribute("value")).SingleOrDefault();
			field.SetValue(siteSettings.CompanyEmail);
			field = (from m in xml.Descendants("companyEmailAuto") select m.Attribute("value")).SingleOrDefault();
			field.SetValue(siteSettings.CompanyEmailAuto);
			field = (from m in xml.Descendants("companyPhoneNo") select m.Attribute("value")).SingleOrDefault();
			field.SetValue(siteSettings.CompanyPhoneNo);
			
			xml.Save(Server.MapPath("~/SiteSettings.xml"));
			return View("Company", siteSettings);
		}

		public ActionResult Users()
		{
			var users = membershipService.GetUsers();
			return View(users);
		}

		[HttpPost]
		public ActionResult Users(User user, int _userID, string[] userRole)
		{
			if (_userID == 0)
				membershipService.InsertUser(user,userRole);
			else
			{
				user.UserID = _userID;
				membershipService.UpdateUser(user,userRole);
			}

			membershipService.Save();
			return RedirectToAction("Users");
		}

		public JsonResult GetUserRoles(int id)
		{
			string[] rolelist = membershipService.GetUserRoles(id).Split(',');

			return Json(rolelist, JsonRequestBehavior.AllowGet);
		}

		public JsonResult CheckUser(int userID)
		{
			var role = membershipService.GetUser(userID);
			return Json(role == null, JsonRequestBehavior.AllowGet);
		}

		public ActionResult DeleteUser(int id)
		{
			membershipService.DeleteUser(id);
			membershipService.Save();
			return RedirectToAction("Users");
		}

		public ActionResult Roles()
		{
			var roles = membershipService.GetUserRoles();
			return View(roles);
		}

		[HttpPost]
		public ActionResult Roles(UserRole userRole, string oldRole)
		{
			if (!string.IsNullOrEmpty(userRole.RoleID))
			{
				if (string.IsNullOrEmpty(oldRole))
					membershipService.InsertUserRole(userRole);
				else
					membershipService.UpdateUserRole(userRole, oldRole);

				membershipService.Save();
			}

			return RedirectToAction("Roles");
		}

		public ActionResult DeleteRole(string id)
		{
			membershipService.DeleteUserRole(id);
			membershipService.Save();
			return RedirectToAction("Roles");
		}

		public JsonResult CheckRole(string roleID)
		{
			var role = membershipService.GetUserRole(roleID);
			return Json(role == null, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Password()
		{
			var userID = Convert.ToInt32(User.Identity.Name);
			var user = membershipService.GetUser(userID);
			return View(user);
		}

		[HttpPost]
		public ActionResult Password(User user)
		{
			if (ModelState.IsValid)
			{
				membershipService.ChangePassword(user.UserID, user.Password);
				membershipService.Save();

				return View(user);
			}
			else
			{
				return View(user);
			}
			
		}
    }
}
