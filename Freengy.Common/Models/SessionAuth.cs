// Created by Laxale 24.04.2018
//
//


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Holds token information for authorizing client-server pair communication.
    /// </summary>
    public class SessionAuth 
    {
        /// <summary>
        /// Unique client token is generated on login.
        /// </summary>
        public string ClientToken { get; set; }

        /// <summary>
        /// Unique server token is generated on login.
        /// </summary>
        public string ServerToken { get; set; }
    }
}