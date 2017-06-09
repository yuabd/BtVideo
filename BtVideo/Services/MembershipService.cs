using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using BtVideo.Models;

namespace BtVideo.Services
{
	public class MembershipService : DbAccess
	{
		//private SiteDataContext db = new SiteDataContext();

		// Login operations

		public string Login(int userID, string clearPassword, bool rememberMe)
		{
			string loginMessage = "OK";

			if (UserAuthenticated(userID, clearPassword))
			{
				string roles = GetUserRoles(userID);

				GenerateAuthenticationTicket(userID, rememberMe, roles);
				SaveLoginDate(userID);

				Save();	//commit all changes
			}
			else
			{
				loginMessage = "用户名或者密码错误！";
			}

			return loginMessage;
		}

		private bool UserAuthenticated(int userID, string clearPassword)
		{
			string encryptedPassword = EncryptPassword(clearPassword);

			var r = GetUser(userID, encryptedPassword);

			return r != null;
		}

		public void ChangePassword(int userID, string clearNewPassword)
		{
			var user = GetUser(userID);
			user.Password = EncryptPassword(clearNewPassword);
		}

		public string EncryptPassword(string clearPassword)
		{
			return FormsAuthentication.HashPasswordForStoringInConfigFile(clearPassword, "MD5");
		}

		private static void GenerateAuthenticationTicket(int userID, bool rememberMe, string roles)
		{
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
			   1, // Ticket version
			   userID.ToString(), // Username associated with ticket
			   DateTime.Now, // Date/time issued
			   DateTime.Now.AddDays(30), // Date/time to expire
			   rememberMe, // persistent user cookie
			   roles, // User-data, in this case the roles
			   FormsAuthentication.FormsCookiePath);// Path cookie valid for Encrypt the cookie using the machine key for secure transport

			string hash = FormsAuthentication.Encrypt(ticket);
			HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

			cookie.Expires = DateTime.Now.AddHours(12);

			if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

			// Add the cookie to the list for outgoing response
			HttpContext.Current.Response.Cookies.Add(cookie);
		}

		// Repository functions

		public User GetUser(int userID)
		{
			return db.Users.Find(userID);
		}

		public string GetUserRoles(int userID)
		{
			var roleList = "";
			var roles = from u in db.UserRoleJoins
                        .Include("UserRole")
                        where u.UserID == userID select u;

			foreach (var item in roles)
			{
				roleList += item.UserRole.RoleID + ",";
			}

			if (!string.IsNullOrEmpty(roleList))
				roleList = roleList.Substring(0, roleList.Length - 1);	// remove last comma (,) symbol

			return roleList;
		}

		public void SaveLoginDate(int userID)
		{
			var user = db.Users.SingleOrDefault(u => u.UserID == userID);

			user.DateLastLogin = DateTime.Now;
		}

		public User GetUser(int userID, string encryptedPassword)
		{
			return db.Users.SingleOrDefault(u => u.UserID == userID && u.Password == encryptedPassword);
		}

		// Others

		public void Save()
		{
			db.SaveChanges();
		}

		//users

		public IEnumerable<User> GetUsers()
		{
			return db.Users.OrderByDescending(m => m.DateCreated).ToList();
		}

		public void InsertUser(User user, string[] roles)
		{
			if (!string.IsNullOrEmpty(user.Password))
			{
				user.DateCreated = DateTime.Now;
				user.DateLastLogin = DateTime.Now;
				user.Password = EncryptPassword(user.Password);
				db.Users.Add(user);

				Save();

				foreach (var item in roles)
				{
					UserRoleJoin userRoleJoin = new UserRoleJoin();
					userRoleJoin.RoleID = item.ToString();
					userRoleJoin.UserID = user.UserID;
					db.UserRoleJoins.Add(userRoleJoin);
				}
			}
		}

		public void UpdateUser(User user, string[] roles)
		{
			if (!string.IsNullOrEmpty(user.Password))
			{
				var u = GetUser(user.UserID);
				u.IsActive = user.IsActive;
				if (u.Password.Substring(0, 10) != user.Password)
					u.Password = EncryptPassword(user.Password);

				foreach (var item in db.UserRoleJoins.Where(m => m.UserID == user.UserID))
				{
					db.UserRoleJoins.Remove(item);
				}
				Save();

				foreach (var item in roles)
				{
					UserRoleJoin userRoleJoin = new UserRoleJoin();
					userRoleJoin.RoleID = item.ToString();
					userRoleJoin.UserID = u.UserID;
					db.UserRoleJoins.Add(userRoleJoin);
				}
			}
		}

		public void DeleteUser(int userID)
		{
			var user = GetUser(userID);
			db.Users.Remove(user);
		}

		//user Role

		public UserRole GetUserRole(string roleID)
		{
			return db.UserRoles.Find(roleID);
		}

		public IEnumerable<UserRole> GetUserRoles()
		{
			return db.UserRoles.Where(m => m.RoleID != "Administrator");
		}

		public void DeleteUserRole(string roleID)
		{
			var u = db.UserRoles.Find(roleID);
			db.UserRoles.Remove(u);
		}

		public void InsertUserRole(UserRole userRole)
		{
			db.UserRoles.Add(userRole);
		}

		public void UpdateUserRole(UserRole userRole, string oldRole)
		{
			var u = db.UserRoles.Find(oldRole);
			db.UserRoles.Remove(u);
			db.UserRoles.Add(userRole);
		}
	}
}
