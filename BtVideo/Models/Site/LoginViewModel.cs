using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BtVideo.Models.Site
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "必填项")]
		public int UserID { get; set; }

		[Required(ErrorMessage = "必填项")]
		public string Password { get; set; }

		public bool RememberMe { get; set; }

		public LoginViewModel()
		{
		}
	}
}