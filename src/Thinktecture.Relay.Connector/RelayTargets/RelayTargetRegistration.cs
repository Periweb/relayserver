using System;
using Thinktecture.Relay.Connector.Abstractions;
using Thinktecture.Relay.Transport;

namespace Thinktecture.Relay.Connector.RelayTargets
{
	internal class RelayTargetRegistration<TRequest, TResponse>
		where TRequest : IRelayClientRequest
		where TResponse : IRelayTargetResponse
	{
		public IRelayTargetOptions Options { get; }
		public Func<IServiceProvider, IRelayTarget<TRequest, TResponse>> Factory { get; }

		public RelayTargetRegistration(IRelayTargetOptions options, Func<IServiceProvider, IRelayTarget<TRequest, TResponse>> factory)
		{
			Options = options;
			Factory = factory;
		}
	}
}
