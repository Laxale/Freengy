// Created by Laxale 26.04.2018
//
//

using System;

using Freengy.Common.Constants;
using Freengy.Common.Interfaces;


namespace Freengy.Networking.Helpers 
{
    /// <summary>
    /// Contains <see cref="IHttpActor"/> extensions.
    /// </summary>
    public static class HttpActorExtensions 
    {
        /// <summary>
        /// Add client session token to request headers.
        /// </summary>
        /// <param name="actor"><see cref="IHttpActor"/> request sender.</param>
        /// <param name="sessionToken">Client session token.</param>
        /// <returns>this.</returns>
        public static IHttpActor SetClientSessionToken(this IHttpActor actor, string sessionToken) 
        {
            if (string.IsNullOrWhiteSpace(sessionToken)) throw new ArgumentNullException(nameof(sessionToken));

            actor.AddHeader(FreengyHeaders.Client.ClientSessionTokenHeaderName, sessionToken);

            return actor;
        }

        /// <summary>
        /// Set the HTTP client address for server to send messages to.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="clientAddress">HTTP address of a client.</param>
        /// <returns>this.</returns>
        public static IHttpActor SetClientAddress(this IHttpActor actor, string clientAddress) 
        {
            actor.AddHeader(FreengyHeaders.Client.ClientAddressHeaderName, clientAddress);

            return actor;
        }
    }
}