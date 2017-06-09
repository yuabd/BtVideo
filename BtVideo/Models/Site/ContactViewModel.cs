using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BtVideo.Helpers;

namespace BtVideo.Models.Site
{
	public class ContactViewModel
	{
		[Required(ErrorMessage = "必填项")]
		public string ContactName { get; set; }

		[Required(ErrorMessage = "必填项")]
		[RegularExpression("^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "邮箱格式错误")]
		public string Email { get; set; }

		public string PhoneNo { get; set; }

		[Required(ErrorMessage = "必填项")]
		public string Message { get; set; }

		public string Country { get; set; }

		[Required(ErrorMessage = "必填项")]
		[Compare("ValidationCodeSource")]
		public string ValidationCodeMatch { get; set; }

		public string ValidationCodeSource { get; set; }


	}
}