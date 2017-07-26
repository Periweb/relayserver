﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Thinktecture.Relay.Server.Dto;

namespace Thinktecture.Relay.Server.Repository
{
    public interface ILinkRepository
    {
        IEnumerable<Link> GetLinks();
        PageResult<Link> GetLinks(PageRequest paging);
        Link GetLink(Guid linkId);
        Link GetLink(string userName);
        CreateLinkResult CreateLink(string symbolicName, string userName);
        bool UpdateLink(Link linkId);
        void DeleteLink(Guid linkId);

        bool Authenticate(string userName, [Optional] string password, out Guid linkId);
        bool IsUserNameAvailable(string userName);
        Task AddOrRenewActiveConnection(string registrationInformationLinkId, string originId, string registrationInformationConnectionId, int connectorVersion);
        Task RenewActiveConnection(string connectionId);
        Task RemoveActiveConnection(string connectionId);
        void DeleteAllActiveConnectionsForOrigin(string originId);
    }
}
