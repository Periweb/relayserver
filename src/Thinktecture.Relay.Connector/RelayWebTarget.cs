using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Thinktecture.Relay.Connector.Abstractions;
using Thinktecture.Relay.Payload;

namespace Thinktecture.Relay.Connector
{
	/// <inheritdoc cref="IRelayTarget{TRequest,TResponse}"/> />
	public class RelayWebTarget<TRequest, TResponse> : IRelayTarget<TRequest, TResponse>, IDisposable
		where TRequest : IRelayClientRequest
		where TResponse : IRelayTargetResponse
	{
		private readonly HttpClient _client;
		private readonly IRelayTargetResponseFactory<TResponse> _responseFactory;

		/// <summary>
		/// Creates an instance of <see cref="RelayWebTarget{TRequest,TResponse}"/>.
		/// </summary>
		/// <param name="responseFactory">The <see cref="IRelayTargetResponseFactory{TResponse}"/> for creating the <typeparamref name="TResponse"/></param>
		/// <param name="clientFactory">The <see cref="IHttpClientFactory"/> for creating the <see cref="HttpClient"/>.</param>
		public RelayWebTarget(IRelayTargetResponseFactory<TResponse> responseFactory, IHttpClientFactory clientFactory)
		{
			_responseFactory = responseFactory;
			_client = clientFactory.CreateClient("relayserver");
			// TODO timeout
		}

		/// <inheritdoc />
		public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
		{
			var requestMessage = new HttpRequestMessage(new HttpMethod(request.HttpMethod), request.Url);

			foreach (var header in request.HttpHeaders)
			{
				requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
			}

			if (request.BodyContent != null)
			{
				requestMessage.Content = new StreamContent(request.BodyContent);

				foreach (var header in request.HttpHeaders)
				{
					requestMessage.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
				}

				requestMessage.Content.Headers.ContentLength = request.BodySize;
			}

			var responseMessage = await _client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
			var response = await _responseFactory.CreateAsync(responseMessage);

			response.RequestId = request.RequestId;
			response.RequestOriginId = request.RequestOriginId;

			// TODO tracing into RequestStart, RequestDuration

			return response;
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="RelayWebTarget{TRequest,TResponse}"/> and optionally releases the
		/// managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to releases only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_client.Dispose();
			}
		}

		/// <inheritdoc />
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
