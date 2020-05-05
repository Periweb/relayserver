using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Thinktecture.Relay.Connector;
using Thinktecture.Relay.Connector.Abstractions;
using Thinktecture.Relay.Payload;

// ReSharper disable once CheckNamespace; (extension methods on IServiceCollection namespace)
namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Extension methods for the <see cref="IServiceCollection"/>.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds the connector to the <see cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/>.</param>
		/// <param name="serverBaseAddress">The base <see cref="Uri"/> of the server.</param>
		/// <returns>The <see cref="IServiceCollection"/>.</returns>
		public static IServiceCollection AddRelayConnector(this IServiceCollection services, Uri serverBaseAddress)
			=> services.AddRelayConnector<TargetResponse>(serverBaseAddress);

		/// <summary>
		/// Adds the connector to the <see cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/>.</param>
		/// <param name="serverBaseAddress">The base <see cref="Uri"/> of the server.</param>
		/// <typeparam name="TResponse">The type of response.</typeparam>
		/// <returns>The <see cref="IServiceCollection"/>.</returns>
		public static IServiceCollection AddRelayConnector<TResponse>(this IServiceCollection services, Uri serverBaseAddress)
			where TResponse : IRelayTargetResponse, new()
		{
			services.TryAddScoped<IRelayTargetResponseFactory<TResponse>, RelayTargetResponseFactory<TResponse>>();

			services.AddHttpClient("relayserver", client => client.BaseAddress = serverBaseAddress);

			return services;
		}
	}
}
