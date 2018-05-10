// Created by Laxale 24.04.2018
//
//

namespace Freengy.Common.Extensions 
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;

    using Freengy.Common.Constants;
    using Freengy.Common.Models;


    /// <summary>
    /// Contains extension methods for HTTP messages.
    /// </summary>
    public static class HttpMessageExtensions 
    {
        /// <summary>
        /// Get the <see cref="SessionAuth"/> model from responce headers.
        /// </summary>
        /// <param name="headers">HTTP message headers.</param>
        /// <returns><see cref="SessionAuth"/> instance from headers.</returns>
        public static SessionAuth GetSessionAuth(this HttpHeaders headers) 
        {
            var auth = new SessionAuth();
            bool hasClientToken = headers.TryGetValues(FreengyHeaders.Client.ClientAddressHeaderName, out IEnumerable<string> clientValues);
            bool hasServerToken = headers.TryGetValues(FreengyHeaders.Server.ServerSessionTokenHeaderName, out IEnumerable<string> serverValues);

            if (hasClientToken)
            {
                auth.ClientToken = clientValues.FirstOrDefault();
            }
            if (hasServerToken)
            {
                auth.ServerToken = serverValues.FirstOrDefault();
            }

            return auth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string GetSaltHeaderValue(this HttpHeaders headers) 
        {
            bool hasSaltHeader = headers.TryGetValues(FreengyHeaders.Client.ClientAddressHeaderName, out IEnumerable<string> headerValues);

            if (hasSaltHeader)
            {
                return headerValues.First();
            }

            return null;
        }
    }
}