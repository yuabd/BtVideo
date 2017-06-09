using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace BtVideo.Models.Others
{
	public class MailBag
	{
		public string ToMailAddress { get; set; }
		public string CcMailAddress { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
		public string CompanyName { get; protected set; }
		public string CompanyMail { get; protected set; }
		public string CompanyWebsite { get; protected set; }
		public string CompanyMailAuto { get; protected set; }
		public bool IsBodyText { get; set; }

		public MailBag()
		{
			SiteSettings site = new SiteSettings();
			CompanyName = site.CompanyName;
			CompanyMail = site.CompanyEmail;
			CompanyWebsite = site.CompanyWebsite;
			CompanyMailAuto = site.CompanyEmailAuto;
		}

		public void Send(bool notifySite)
		{
			if (!string.IsNullOrEmpty(ToMailAddress))
			{
				try
				{
					MailAddress fromAddress;
					fromAddress = new MailAddress(CompanyMailAuto, CompanyName);

					//handle multiple to: addresses
					MailAddress toAddress;
					string[] toAddresses = ToMailAddress.Split(',', ';');
					toAddress = new MailAddress(toAddresses[0]);	//take the first one as the main address

					MailMessage message = new MailMessage(fromAddress, toAddress);

					//check if multiple TO addresses, then add after 2nd one
					for (int i = 1; i < toAddresses.Count<string>(); i++)
					{
						message.To.Add(toAddresses[i]);
					}

					//check if CC is available. Accepts multiple separated with a (",")
					if (!string.IsNullOrEmpty(CcMailAddress))
						message.CC.Add(CcMailAddress);

					// if notify site, add company email in Bcc
					if (notifySite)
						message.Bcc.Add(CompanyMail);

					//prepare the message
					Message += string.Format("<p>Copyright &copy; {0} <br /> <a href='http://{1}'>{1}</a></p>",
						CompanyName,
						CompanyWebsite
						);

					message.Subject = Subject;
					message.Body = Message;
					message.IsBodyHtml = !IsBodyText;

					SmtpClient client = new SmtpClient();
					client.Send(message);
				}
				catch (Exception e)
				{
					BtVideo.Helpers.GlobalHelper.WriteInLog(e.Message);
				}
			}
		}
	}
}
