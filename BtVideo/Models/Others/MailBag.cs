using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Configuration;

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


        private string m_SmtpUserName, m_SmtpUserPassword;

        public bool IsBodyHtml { set; get; }

        public int BodyEncoding { set; get; }

        public bool? DefaultCredentials { set; get; }

        public bool? EnableSsl { set; get; }

        public int? Port { set; get; }

        public string Host { get; set; }

        public string From { get; set; }

        public string Err { get; set; }

        public MailBag(bool? isWeb = true)
		{
			SiteSettings site = new SiteSettings();
			CompanyName = site.CompanyName;
			CompanyMail = site.CompanyEmail;
			CompanyWebsite = site.CompanyWebsite;
			CompanyMailAuto = site.CompanyEmailAuto;

            //从config读取邮件配置
            MailSettingsSectionGroup mailSettings;
            if (isWeb == true)
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~/");
                mailSettings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            }
            else
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);//OpenExeConfiguration2个方法的参数我也没搞清楚到底该怎么用。
                mailSettings = NetSectionGroup.GetSectionGroup(config).MailSettings;
            }

            this.m_SmtpUserName = mailSettings.Smtp.Network.UserName;
            this.m_SmtpUserPassword = mailSettings.Smtp.Network.Password;
            this.From = mailSettings.Smtp.From;
            this.DefaultCredentials = mailSettings.Smtp.Network.DefaultCredentials;
            this.EnableSsl = mailSettings.Smtp.Network.EnableSsl;
            this.Port = mailSettings.Smtp.Network.Port;
            this.Host = mailSettings.Smtp.Network.Host;

            this.IsBodyHtml = true;
            this.BodyEncoding = 936;
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

					SmtpClient client = new SmtpClient(this.Host);
                    if (EnableSsl.HasValue) client.EnableSsl = EnableSsl.Value;
                    client.Credentials = new NetworkCredential(this.m_SmtpUserName, this.m_SmtpUserPassword);//要身份验证

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
