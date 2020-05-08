// ReSharper disable once CheckNamespace; (same IServiceCollection namespace)
namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Connector builder extensions.
	/// </summary>
	public interface IRelayConnectorBuilder
	{
		/// <summary>
		/// Gets the application service collection.
		/// </summary>
		IServiceCollection Services { get; }
	}
}
