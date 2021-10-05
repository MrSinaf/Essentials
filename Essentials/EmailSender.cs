using System.Net.Mail;
using System.Net;
using System;

namespace SinafProduction.Essentials
{
	public class EmailSender
	{
		private readonly string stmpHost;
		private readonly int tlsPort;
		private readonly string userMail;
		private readonly string userPass;
		
		public EmailSender(string stmpHost, int tlsPort, string userMail, string userPass)
		{
			this.stmpHost = stmpHost;
			this.tlsPort = tlsPort;
			this.userMail = userMail;
			this.userPass = userPass;
		}

		public void Send(string to, string subject, string body, Attachment attachment)
		{
			using var server = new SmtpClient(stmpHost, tlsPort) { Credentials = new NetworkCredential(userMail, userPass) };
			using var msg = new MailMessage(userMail, to) { Subject = $"[{Environment.UserName}] - {subject}", Body = body };
			if (attachment != null)
				msg.Attachments.Add(attachment);
			server.Send(msg);
		}
	}
}