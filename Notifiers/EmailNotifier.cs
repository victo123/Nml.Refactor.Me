using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;

namespace Nml.Refactor.Me.Notifiers
{
	public class EmailNotifier : Notifier, INotifier
	{
		private readonly IMailMessageBuilder _messageBuilder;

		public EmailNotifier(IMailMessageBuilder messageBuilder, IOptions options, ILogger<TeamsNotifier> logger) : base(options, logger)
		{
			_messageBuilder = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
		}
		
		public async Task Notify(NotificationMessage message)
		{
			var smtp = new SmtpClient(Options.Email.SmtpServer);
			smtp.Credentials = new NetworkCredential(Options.Email.UserName, Options.Email.Password);
			var mailMessage = _messageBuilder.CreateMessage(message);
			
			try
			{
				await smtp.SendMailAsync(mailMessage);
				Logger.LogTrace($"Message sent.");
			}
			catch (Exception e)
			{
				Logger.LogError(e, $"Failed to send message. {e.Message}");
				throw;
			}
		}
	}
}
