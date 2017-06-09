using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace BtVideo.Models.Others
{
	public class MessageTemplate
	{
		public string TemplatePath { get; protected set; }
		public string Content { get; private set; }
		public string Subject { get { return ReadSubject(); } }

		/// <summary>
		/// Initialize the template class
		/// </summary>
		/// <param name="templatePath">The virtual path to the .html template file. Example:" /Content/Templates/template.htm"</param>
		public MessageTemplate(string templatePath)
		{
			TemplatePath = HttpContext.Current.Server.MapPath(templatePath);
			Load();
		}

		/// <summary>
		/// Reads the HTML template indicated in the TemplatePath
		/// </summary>
		private void Load()
		{
			Content = File.ReadAllText(TemplatePath);
		}

		/// <summary>
		/// Replace values inside a HTML template file. Key inside template must be inside [] symbols, example: [CompanyName]
		/// </summary>
		/// <param name="Key">The name of the value, example: "CompanyName"</param>
		/// <param name="Value">The value for the Key, example: "Cloud One Design"</param>
		public void Set(string Key, string Value)
		{
			string key = string.Format("[{0}]", Key);
			Content = Content.Replace(key, Value);
		}

		private string ReadSubject()
		{
			int titleBegin = Content.IndexOf("<title>");
			int titleEnd = Content.IndexOf("</title>");
			string subject = "";

			if (titleBegin > 0 && titleEnd > 0)
				subject = Content.Substring(titleBegin + 7, titleEnd - titleBegin - 7);

			return subject;
		}
	}
}
