using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;

namespace Nml.Refactor.Me.Notifiers
{
	public class TeamsNotifier : Notifier, INotifier
	{
		private readonly IWebhookMessageBuilder _messageBuilder;

		public TeamsNotifier(IWebhookMessageBuilder messageBuilder, IOptions options, ILogger<TeamsNotifier> logger) : base(options, logger)
		{
			_messageBuilder = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
		}

		public async Task Notify(NotificationMessage message)
		{
			var serviceEndPoint = new Uri(Options.Teams.WebhookUri);
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Post, serviceEndPoint);
			request.Content = new StringContent(
				_messageBuilder.CreateMessage(message).ToString(),
				Encoding.UTF8,
				"application/json");
			try
			{
				var response = await client.SendAsync(request);
				Logger.LogTrace($"Message sent. {response.StatusCode} -> {response.Content}");
			}
			catch (AggregateException e)
			{
				foreach (var exception in e.Flatten().InnerExceptions)
					Logger.LogError(exception, $"Failed to send message. {exception.Message}");

				throw;
			}
		}
	}
}
