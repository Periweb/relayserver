namespace Microsoft.Extensions.DependencyInjection
{
	internal class RelayConnectorBuilder : IRelayConnectorBuilder
	{
		public IServiceCollection Services { get; }

		public RelayConnectorBuilder()
		{
			Services = new ServiceCollection();
		}

		// TODO logger factory with extension method
	}
}
