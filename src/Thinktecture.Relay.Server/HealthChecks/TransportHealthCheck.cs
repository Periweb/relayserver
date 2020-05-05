using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Thinktecture.Relay.Server.HealthChecks
{
	/// <inheritdoc />
	public class TransportHealthCheck : IHealthCheck
	{
		/// <inheritdoc />
		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
		{
			return Task.FromResult(HealthCheckResult.Healthy());
		}
	}
}
