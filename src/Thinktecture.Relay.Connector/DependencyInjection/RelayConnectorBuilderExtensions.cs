using Thinktecture.Relay.Connector;
using Thinktecture.Relay.Connector.Abstractions;
using Thinktecture.Relay.Connector.RelayTargets;
using Thinktecture.Relay.Transport;

// ReSharper disable once CheckNamespace; (same IServiceCollection namespace)
namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Extension methods for the <see cref="IRelayConnectorBuilder"/>.
	/// </summary>
	public static class RelayConnectorBuilderExtensions
	{
		/// <summary>
		/// Adds the <see cref="IRelayTarget{TRequest,TResponse}"/> to the <see cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="builder">The <see cref="IRelayConnectorBuilder"/>.</param>
		/// <param name="id">The unique id of the target.</param>
		/// <param name="options">The <see cref="IRelayTargetOptions"/>.</param>
		/// <typeparam name="TTarget">The type of target.</typeparam>
		/// <typeparam name="TRequest">The type of request.</typeparam>
		/// <typeparam name="TResponse">The type of response.</typeparam>
		/// <returns>The <see cref="IRelayConnectorBuilder"/>.</returns>
		public static IRelayConnectorBuilder AddTarget<TTarget, TRequest, TResponse>(
			this IRelayConnectorBuilder builder,
			string id,
			IRelayTargetOptions options = null)
			where TTarget : class, IRelayTarget<TRequest, TResponse>
			where TResponse : IRelayTargetResponse
			where TRequest : IRelayClientRequest
		{
			builder.Services.Configure<RelayConnectorOptions<TRequest, TResponse>>(connector =>
			{
				var registration =
					new RelayTargetRegistration<TRequest, TResponse>(options, services => ActivatorUtilities.CreateInstance<TTarget>(services));
				connector.Targets.Add(id, registration);
			});

			return builder;
		}
	}
}
