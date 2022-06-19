using Nml.Refactor.Me.Dependencies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nml.Refactor.Me.Notifiers
{
    public class Notifier
    {
        private readonly IOptions _options;
        private readonly ILogger<TeamsNotifier> _logger;

        public Notifier(IOptions options, ILogger<TeamsNotifier> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IOptions Options => _options;

        public ILogger<TeamsNotifier> Logger => _logger;
    }
}
