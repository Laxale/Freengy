// Created by Laxale 17.04.2018
//
//

using Freengy.Common.Enums;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Log-in process model.
    /// </summary>
    public class LoginModel 
    {
        /// <summary>
        /// Name of user trying to log in.
        /// </summary>
        public UserAccount Account { get; set; }

        /// <summary>
        /// Hash of user password.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Status is set by server when processed request.
        /// </summary>
        public AccountOnlineStatus LogInStatus { get; set; }
    }
}