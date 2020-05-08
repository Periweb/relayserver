using System;
using Thinktecture.Relay.Connector.Abstractions;

namespace Thinktecture.Relay.Connector.RelayTargets
{
	/// <inheritdoc />
	public class WebTargetOptions : IRelayTargetOptions
	{
		/// <summary>
		/// The base <see cref="Uri"/> used in a HTTP request.
		/// </summary>
		public Uri BaseAddress { get; }

		/// <inheritdoc />
		public TimeSpan Timeout { get; }

		public WebTargetOptions(Uri baseAddress, TimeSpan? timeout = null)
		{
			BaseAddress = baseAddress;
			Timeout = timeout ?? TimeSpan.FromSeconds(100);
		}
	}
}
