using System;
using System.Reflection;
using Serilog;
using Thinktecture.Relay.OnPremiseConnector.OnPremiseTarget;

namespace Thinktecture.Relay.OnPremiseConnector.SignalR
{
	internal class RelayServerConnectionFactory : IRelayServerConnectionFactory
	{
		private readonly IOnPremiseTargetConnectorFactory _onPremiseTargetConnectorFactory;
		private readonly ILogger _logger;

		public RelayServerConnectionFactory(IOnPremiseTargetConnectorFactory onPremiseTargetConnectorFactory, ILogger logger)
		{
			_onPremiseTargetConnectorFactory = onPremiseTargetConnectorFactory;
			_logger = logger;
		}

		public IRelayServerConnection Create(Assembly versionAssembly, string userName, string password, Uri relayServer, int requestTimeout, int tokenRefreshWindow)
		{
			return new RelayServerConnection(versionAssembly, userName, password, relayServer, requestTimeout, tokenRefreshWindow, _onPremiseTargetConnectorFactory, _logger);
		}
	}
}
