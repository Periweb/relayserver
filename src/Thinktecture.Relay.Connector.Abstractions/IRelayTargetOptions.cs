using System;

namespace Thinktecture.Relay.Connector.Abstractions
{
	/// <summary>
	/// An implementation of options for an <see cref="IRelayTarget{TRequest,TResponse}"/>.
	/// </summary>
	public interface IRelayTargetOptions
	{
		/// <summary>
		/// Gets the maximum amount of time waiting for the target to return a result.
		/// </summary>
		/// <remarks>The default value is 100 seconds.</remarks>
		TimeSpan Timeout { get; }
	}
}
