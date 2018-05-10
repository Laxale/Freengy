// Created by Laxale 23.04.2018
//
//


namespace Freengy.Common.Constants 
{
    /// <summary>
    /// Contains names of a custon HTTP headers.
    /// </summary>
    public static class FreengyHeaders 
    {
        /// <summary>
        /// Содержит клиентские заголовки.
        /// </summary>
        public static class Client 
        {
            /// <summary>
            /// The name of a custom ClientAddress header.
            /// </summary>
            public const string ClientAddressHeaderName = "ClientAddress";

            /// <summary>
            /// The name of custom ClientSessionToken header.
            /// </summary>
            public const string ClientSessionTokenHeaderName = "ClientSessionToken";

            /// <summary>
            /// The name of custom ClientPassword header.
            /// </summary>
            public const string ClientPasswordHeaderName = "ClientPassword";

            /// <summary>
            /// The name of custom ClientId header.
            /// </summary>
            public const string ClientIdHeaderName = "ClientIdHeader";
        }

        /// <summary>
        /// Содержит серверные заголовки.
        /// </summary>
        public static class Server
        {
            /// <summary>
            /// The name of custom NextPasswordSalt header.
            /// </summary>
            public const string NextPasswordSaltHeaderName = "NextPasswordSalt";

            /// <summary>
            /// The name of custom ServerSessionToken header.
            /// </summary>
            public const string ServerSessionTokenHeaderName = "ServerSessionToken";
        }
    }
}