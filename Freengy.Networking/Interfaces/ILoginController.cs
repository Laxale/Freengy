// Created by Laxale 19.10.2016
//
//

using System.Threading.Tasks;

using Freengy.Common.Enums;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;


namespace Freengy.Networking.Interfaces 
{
    /// <summary>
    /// Log-in process controlles interface.
    /// </summary>
    public interface ILoginController 
    {
        /// <summary>
        /// The unique token obtained from server in login result. Used to authorize client requests on server.
        /// </summary>
        string MySessionToken { get; }

        /// <summary>
        /// The unique token obtained from server. Used to authorize server messages on client.
        /// </summary>
        string ServerSessionToken { get; }

        /// <summary>
        /// Password that user was logged in with.
        /// </summary>
        string LoggedInPassword { get; }

        /// <summary>
        /// Returns current user account state in usage.
        /// </summary>
        AccountState MyAccountState { get; }

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
        Result<AccountStateModel> LogIn(LoginModel loginModel);

        /// <summary>
        /// Attempts to log the user out.
        /// </summary>
        /// <returns>Logout result.</returns>
        Result<AccountStateModel> LogOut();

        /// <summary>
        /// Attempts to log in the user asynchronously.
        /// </summary>
        /// <param name="loginModel">User account data to log in.</param>
        /// <returns>Logging user in <see cref="Task"/>.</returns>
        Task<Result<AccountStateModel>> LogInAsync(LoginModel loginModel);
    }
}