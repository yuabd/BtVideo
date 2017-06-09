using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Web;
using System.Linq;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using BtVideo.Helpers;

namespace BtVideo.Models.Others
{
	public class SiteSettings : ConfigurationSection
	{
		[StringLength(50),Required]
		public string CompanyName  { get; set; }
		[StringLength(50),Required]
		public string CompanyWebsite  { get; set; }
		[MaxLength(20)]
		public string ICP { get; set; }
		[MaxLength(80)]
		public string Title { get; set; }
		[MaxLength(100)]
		public string Keywords { get; set; }
		[MaxLength(200)]
		public string Description { get; set; }
		[MaxLength(100)]
		public string Address { get; set; }
		[RegularExpression("^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "邮箱格式错误")]
		[StringLength(60),Required]
		public string CompanyEmail  { get; set; }
		[RegularExpression("^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "邮箱格式错误")]
		[StringLength(60),Required]
		public string CompanyEmailAuto  { get; set; }
		[StringLength(30),Required]
		public string CompanyPhoneNo  { get; set; }
		[MaxLength(10)]
		public string QQ { get; set; }
		

		public SiteSettings()
		{
			var xml = XDocument.Load(HttpContext.Current.Server.MapPath("~/SiteSettings.xml"));
			XAttribute field;
			
			field = (from m in xml.Descendants("companyName") select m.Attribute("value")).SingleOrDefault();
			CompanyName = field.Value;
			field = (from m in xml.Descendants("companyWebsite") select m.Attribute("value")).SingleOrDefault();
			CompanyWebsite = field.Value;
			field = (from m in xml.Descendants("icp") select m.Attribute("value")).SingleOrDefault();
			ICP = field.Value;
			field = (from m in xml.Descendants("title") select m.Attribute("value")).SingleOrDefault();
			Title = field.Value;
			field = (from m in xml.Descendants("keywords") select m.Attribute("value")).SingleOrDefault();
			Keywords = field.Value;
			field = (from m in xml.Descendants("description") select m.Attribute("value")).SingleOrDefault();
			Description = field.Value;
			field = (from m in xml.Descendants("address") select m.Attribute("value")).SingleOrDefault();
			Address = field.Value;
			field = (from m in xml.Descendants("companyEmail") select m.Attribute("value")).SingleOrDefault();
			CompanyEmail = field.Value;
			field = (from m in xml.Descendants("companyEmailAuto") select m.Attribute("value")).SingleOrDefault();
			CompanyEmailAuto = field.Value;
			field = (from m in xml.Descendants("companyPhoneNo") select m.Attribute("value")).SingleOrDefault();
			CompanyPhoneNo = field.Value;
			field = (from m in xml.Descendants("qq") select m.Attribute("value")).SingleOrDefault();
			QQ = field.Value;
		}

	}
}