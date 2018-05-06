// Created by Laxale 17.04.2018
//
//

using System.Security;

using Freengy.Common.Enums;

using Newtonsoft.Json;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Log-in process model.
    /// </summary>
    public class LoginModel 
    {
        /// <summary>
        /// Is use logging in or out.
        /// </summary>
        public bool IsLoggingIn { get; set; }

        /// <summary>
        /// Name of user trying to log in.
        /// </summary>
        public UserAccountModel Account { get; set; }

        /// <summary>
        /// Hash of user password.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [JsonIgnore]
        public string Password { get; set; }

        /// <summary>
        /// Status is set by server when processed request.
        /// </summary>
        public AccountOnlineStatus LogInStatus { get; set; }
    }
}