using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Thinktecture.Relay.Server.Communication;
using Thinktecture.Relay.Server.Diagnostics;
using Thinktecture.Relay.Server.Dto;
using Thinktecture.Relay.Server.Http.Filters;
using Thinktecture.Relay.Server.OnPremise;
using Thinktecture.Relay.Server.Repository;

namespace Thinktecture.Relay.Server.Controller.ManagementWeb
{
	[Authorize(Roles = "Admin")]
	[ManagementWebModuleBindingFilter]
	[NoCache]
	public class LinkController : ApiController
	{
		private readonly ILinkRepository _linkRepository;
		private readonly IBackendCommunication _backendCommunication;
		private readonly IRequestLogger _requestLogger;

		public LinkController(ILinkRepository linkRepository, IBackendCommunication backendCommunication, IRequestLogger requestLogger)
		{
			_linkRepository = linkRepository;
			_backendCommunication = backendCommunication;
			_requestLogger = requestLogger;
		}

		[HttpGet]
		[ActionName("links")]
		public IHttpActionResult GetLinks([FromUri] PageRequest paging)
		{
			var result = _linkRepository.GetLinks(paging);

			return Ok(result);
		}

		[HttpGet]
		[ActionName("link")]
		public IHttpActionResult GetLink(Guid id)
		{
			var link = _linkRepository.GetLinkDetails(id);

			if (link == null)
				return BadRequest();

			return Ok(link);
		}

		[HttpPost]
		[ActionName("link")]
		public IHttpActionResult CreateLink(CreateLink link)
		{
			var result = _linkRepository.CreateLink(link.SymbolicName, link.UserName);

			// TODO: Fill route
			return Created("", result);
		}

		[HttpPut]
		[ActionName("link")]
		public IHttpActionResult UpdateLink(LinkDetails link)
		{
			if (link == null)
			{
				return BadRequest();
			}

			var result = _linkRepository.UpdateLink(link);

			return result ? (IHttpActionResult)Ok() : BadRequest();
		}

		[HttpGet]
		[ActionName("userNameAvailability")]
		public IHttpActionResult GetUserNameAvailability(string userName)
		{
			if (_linkRepository.IsUserNameAvailable(userName))
			{
				return Ok();
			}

			return Conflict();
		}

		[HttpDelete]
		[ActionName("link")]
		public IHttpActionResult DeleteLink(Guid id)
		{
			_linkRepository.DeleteLink(id);

			return Ok();
		}

		[HttpPut]
		[ActionName("state")]
		public IHttpActionResult SetDisabledState(LinkState state)
		{
			if (state == null)
				return BadRequest();

			var link = _linkRepository.GetLinkDetails(state.Id);

			if (link == null)
				return NotFound();

			link.IsDisabled = state.IsDisabled;

			var result = _linkRepository.UpdateLink(link);

			return result ? (IHttpActionResult)Ok() : BadRequest();
		}

		[HttpGet]
		[ActionName("ping")]
		public async Task<IHttpActionResult> SendPing(Guid id)
		{
			var requestId = Guid.NewGuid().ToString();
			var request = new OnPremiseConnectorRequest()
			{
				HttpMethod = "PING",
				Url = String.Empty,
				RequestStarted = DateTime.UtcNow,
				OriginId = _backendCommunication.OriginId,
				RequestId = requestId
			};

			var task = _backendCommunication.GetResponseAsync(requestId);
			_backendCommunication.SendOnPremiseConnectorRequest(id, request);

			var response = await task.ConfigureAwait(false);
			request.RequestFinished = DateTime.UtcNow;
			_requestLogger.LogRequest(request, response, id, _backendCommunication.OriginId, "DEBUG/PING/", response?.StatusCode);

			return (response != null) ? (IHttpActionResult)Ok() : new StatusCodeResult(HttpStatusCode.GatewayTimeout, Request);
		}
	}
}
