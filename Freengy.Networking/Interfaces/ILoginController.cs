﻿// Created by Laxale 19.10.2016
//
//

using System.Threading.Tasks;

using Freengy.Common.Enums;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models;


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
        /// Returns current user account in usage.
        /// </summary>
        UserAccount CurrentAccount { get; }

        /// <summary>
        /// Attempts to register new user.
        /// </summary>
        /// <param name="userName">Desired new user name.</param>
        /// <returns>Registration result - new account or error details.</returns>
        Result<UserAccount> Register(string userName);

        /// <summary>
        /// Attempts to log the user in.
        /// </summary>
        /// <param name="loginModel">User account data to log in.</param>
        /// <returns>Login result.</returns>
        Result<AccountState> LogIn(LoginModel loginModel);

        /// <summary>
        /// Attempts to log the user out.
        /// </summary>
        AccountOnlineStatus LogOut();

        /// <summary>
        /// Attempts to log in the user asynchronously.
        /// </summary>
        /// <param name="loginModel">User account data to log in.</param>
        /// <returns>Logging user in <see cref="Task"/>.</returns>
        Task<Result<AccountState>> LogInAsync(LoginModel loginModel);
    }
}