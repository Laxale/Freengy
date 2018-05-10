// Created by Laxale 24.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

using Freengy.Common.Constants;
using Freengy.Common.Models;

using Nancy;


namespace Freengy.Common.Extensions
{
    /// <summary>
    /// Contains extension methods for HTTP messages.
    /// </summary>
    public static class NancyHttpMessageExtensions 
    {
        /// <summary>
        /// Get the <see cref="SessionAuth"/> model from headers.
        /// </summary>
        /// <param name="nancyHeaders">HTTP message headers.</param>
        /// <returns><see cref="SessionAuth"/> instance from headers.</returns>
        public static SessionAuth GetSessionAuth(this RequestHeaders nancyHeaders) 
        {
            var auth = new SessionAuth();
            var clientTokens = nancyHeaders[FreengyHeaders.Client.ClientSessionTokenHeaderName].ToList();
            var serverTokens = nancyHeaders[FreengyHeaders.Server.ServerSessionTokenHeaderName].ToList();

            auth.ClientToken = clientTokens.FirstOrDefault();
            auth.ServerToken = serverTokens.FirstOrDefault();

            return auth;
        }

        /// <summary>
        /// Получить из заголовков идентификатор аккаунта клиента.
        /// </summary>
        /// <param name="nancyHeaders">Заголовки сообщения.</param>
        /// <returns>Идентификатор аккаунта клиента или <see cref="Guid.Empty"/>.</returns>
        public static Guid GetClientId(this RequestHeaders nancyHeaders) 
        {
            var clientId = nancyHeaders[FreengyHeaders.Client.ClientIdHeaderName].ToList().FirstOrDefault();

            return clientId == null ? Guid.Empty : Guid.Parse(clientId);
        }
    }
}