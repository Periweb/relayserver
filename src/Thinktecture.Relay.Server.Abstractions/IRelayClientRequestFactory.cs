using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Thinktecture.Relay.Payload;

namespace Thinktecture.Relay.Server
{
	/// <summary>
	/// An implementation of a factory to create an instance of a class implementing <see cref="IRelayClientRequest"/>.
	/// </summary>
	public interface IRelayClientRequestFactory<TRequest>
		where TRequest : IRelayClientRequest
	{
		/// <summary>
		/// Create an instance of a class implementing <see cref="IRelayClientRequest"/> from <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="HttpContext"/> of the current request.</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation, which wraps the creation of an instance
		/// implementing <see cref="IRelayClientRequest"/>.</returns>
		/// <remarks>Some properties will always be set on the request after calling this method (e.g. Target).</remarks>
		Task<TRequest> CreateAsync(HttpContext context);
	}
}
