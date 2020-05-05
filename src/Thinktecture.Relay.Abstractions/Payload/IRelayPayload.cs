using System;
using System.Collections.Generic;
using System.IO;

namespace Thinktecture.Relay.Payload
{
	/// <summary>
	/// Common metadata of the request/response payload.
	/// </summary>
	public interface IRelayPayload
	{
		/// <summary>
		/// The unique id of the request.
		/// </summary>
		/// <remarks>This should not be changed.</remarks>
		Guid RequestId { get; set; }

		/// <summary>
		/// The unique id of the server which created the request.
		/// </summary>
		/// <remarks>This should not be changed.</remarks>
		Guid RequestOriginId { get; set; }

		/// <summary>
		/// The HTTP headers provided.
		/// </summary>
		IDictionary<string, string[]> HttpHeaders { get; set; }
	}
}
