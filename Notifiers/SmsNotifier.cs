using System;
using System.Threading.Tasks;
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;

namespace Nml.Refactor.Me.Notifiers
{
	public class SmsNotifier : Notifier, INotifier
	{
		private readonly IStringMessageBuilder _messageBuilder;

		public SmsNotifier(IStringMessageBuilder messageBuilder, IOptions options, ILogger<TeamsNotifier> logger) : base(options, logger)
		{
			_messageBuilder = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
		}
		
		public async Task Notify(NotificationMessage message)
		{
			var smsApiClient = new SmsApiClient(Options.Sms.ApiUri, Options.Sms.ApiKey);
			var smsMessage = _messageBuilder.CreateMessage(message);
			try
			{
				await smsApiClient.SendAsync(message.To, smsMessage);
				Logger.LogTrace("Sms sent.");
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, $"Failed to send sms. Message : {ex.Message}");
				throw;
			}
		}
	}
}
