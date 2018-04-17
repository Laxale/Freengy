// Created by Laxale 19.10.2016
//
//

using System.Threading.Tasks;
using Freengy.Networking.Enum;
using Freengy.Networking.Models;


namespace Freengy.Networking.Interfaces 
{
    /// <summary>
    /// Log-in process controlles interface.
    /// </summary>
    public interface ILoginController 
    {
        /// <summary>
        /// Returns true if user is logged in.
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// Attempts to register new user.
        /// </summary>
        /// <param name="userName">Desired new user name.</param>
        /// <returns>True if new user is registered.</returns>
        RegistrationStatus Register(string userName);

        /// <summary>
        /// Attempts to log in the user.
        /// </summary>
        /// <param name="loginParameters">User data model.</param>
        AccountOnlineStatus LogIn(LoginModel loginParameters);

        /// <summary>
        /// Attempts to log in the user asynchronously.
        /// </summary>
        /// <param name="loginParameters">User data model.</param>
        /// <returns></returns>
        Task<AccountOnlineStatus> LogInAsync(LoginModel loginParameters);
    }
}