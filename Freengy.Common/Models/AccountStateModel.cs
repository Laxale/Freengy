// Created by Laxale 18.04.2018
//
//

using Freengy.Common.Enums;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// User account state model.
    /// </summary>
    public class AccountStateModel 
    {
        /// <summary>
        /// User's current address is set by server.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Filled by server in responce to login request.
        /// </summary>
        public string SessionToken { get; set; }

        /// <summary>
        /// User account model.
        /// </summary>
        public UserAccountModel Account { get; set; }

        /// <summary>
        /// User account online status.
        /// </summary>
        public AccountOnlineStatus OnlineStatus { get; set; }
    }
}