// Created by Laxale 19.10.2016
//
//

using System.Threading.Tasks;

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
        /// <param name="loginParameters">Log-in attempt data model.</param>
        /// <returns>True if new user is registered.</returns>
        bool Register(LoginModel loginParameters);

        /// <summary>
        /// Attempts to log in the user.
        /// </summary>
        /// <param name="loginParameters">User data model.</param>
        void LogIn(LoginModel loginParameters);

        /// <summary>
        /// Attempts to log in the user asynchronously.
        /// </summary>
        /// <param name="loginParameters">User data model.</param>
        /// <returns></returns>
        Task LogInAsync(LoginModel loginParameters);
    }
}