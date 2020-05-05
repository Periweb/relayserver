using Microsoft.Extensions.DependencyInjection.Extensions;
using Thinktecture.Relay.Payload;
using Thinktecture.Relay.Server;
using Thinktecture.Relay.Server.Factories;
using Thinktecture.Relay.Server.HealthChecks;
using Thinktecture.Relay.Server.Middleware;

// ReSharper disable once CheckNamespace; (extension methods on IServiceCollection namespace)
namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Extension methods for the <see cref="IServiceCollection"/>.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds the server to the <see cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/>.</param>
		/// <returns>The <see cref="IServiceCollection"/>.</returns>
		public static IServiceCollection AddRelayServer(this IServiceCollection services)
			=> services.AddRelayServer<ClientRequest>();

		/// <summary>
		/// Adds the server to the <see cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/>.</param>
		/// <typeparam name="TRequest">The type of request.</typeparam>
		/// <returns>The <see cref="IServiceCollection"/>.</returns>
		public static IServiceCollection AddRelayServer<TRequest>(this IServiceCollection services)
			where TRequest : IRelayClientRequest, new()
		{
			services.TryAddScoped<IRelayClientRequestFactory<TRequest>, RelayClientRequestFactory<TRequest>>();
			services.TryAddScoped<RelayMiddleware<TRequest>>();

			services.AddHealthChecks()
				.AddCheck<TransportHealthCheck>("Transport", tags: new[] { "ready" });

			return services;
		}
	}
}
