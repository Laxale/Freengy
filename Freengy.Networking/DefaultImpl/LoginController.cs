// Created by Laxale 19.10.2016
//
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime;
using System.Net.Http;
using System.Threading;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using Freengy.Common.Enums;
using Freengy.Common.Models;
using Freengy.Common.Helpers;
using Freengy.Common.Helpers.ErrorReason;
using Freengy.Common.Helpers.Result;
using Freengy.Base.Messages;
using Freengy.Base.Interfaces;
using Freengy.Networking.Messages;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;

using NLog;

using Catel.IoC;
using Catel.Messaging;
using Freengy.Common.Extensions;
using Newtonsoft.Json;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// <see cref="ILoginController"/> implementer.
    /// </summary>
    internal class LoginController : ILoginController 
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ITaskWrapper taskWrapper;
        private readonly MessageBase messageLoggedIn;
        private readonly MessageBase messageLogInAttempt;
        private readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        private readonly IMessageMediator messageMediator = MessageMediator.Default;

        private readonly MediaTypes mediaTypes = new MediaTypes();


        #region Singleton

        private static LoginController instance;

        private LoginController() 
        {
            messageLoggedIn = new MessageLogInSuccess();
            messageLogInAttempt = new MessageLogInAttempt();

            taskWrapper = serviceLocator.ResolveType<ITaskWrapper>();
        }

        public static LoginController Instance => instance ?? (instance = new LoginController());

        #endregion Singleton

        
        public bool IsLoggedIn 
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns current user account in usage.
        /// </summary>
        public UserAccount CurrentAccount { get; private set; }


        /// <inheritdoc />
        /// <summary>
        /// Attempts to register new user.
        /// </summary>
        /// <param name="userName">Desired new user name.</param>
        /// <returns>Registration result - new account or error details.</returns>
        public Result<UserAccount> Register(string userName) 
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return Result<UserAccount>.Fail(new UnexpectedErrorReason("User name should not be empty"));
                }

                var request = new RegistrationRequest(userName);

                using (var httpActor = serviceLocator.ResolveType<IHttpActor>())
                {
                    httpActor.SetAddress(Url.Http.RegisterUrl);

                    request = httpActor.PostAsync<RegistrationRequest, RegistrationRequest>(request).Result;

                    if (request.CreatedAccount == null)
                    {
                        return Result<UserAccount>.Fail(new UnexpectedErrorReason(request.Status.ToString()));
                    }

                    CurrentAccount = new UserAccount(request.CreatedAccount);

                    return Result<UserAccount>.Ok(CurrentAccount);
                }

                
            }
            catch (Exception ex)
            {
                string message = $"Failed to register account '{ userName }'";
                logger.Error(ex, message);

                return Result<UserAccount>.Fail(new UnexpectedErrorReason(message));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Attempts to log the user in.
        /// </summary>
        /// <param name="loginModel">User account data to log in.</param>
        /// <returns>Login result.</returns>
        public Result<AccountState> LogIn(LoginModel loginModel) 
        {
            if (loginModel?.Account == null)
            {
                return Result<AccountState>.Fail(new UnexpectedErrorReason("Logging empty account is denied"));
            }

            loginModel.IsLoggingIn = true;

            return InvokeLogInOrOut(loginModel);
        }

        /// <summary>
        /// Attempts to log the user out.
        /// </summary>
        public Result<AccountState> LogOut() 
        {
            var loginModel = new LoginModel
            {
                IsLoggingIn = false,
                Account = CurrentAccount.ToModel(),
                PasswordHash = "fffuuuu"
            };
            
            return InvokeLogInOrOut(loginModel);
        }

        /// <inheritdoc />
        /// <summary>
        /// Attempts to log in the user asynchronously.
        /// </summary>
        /// <param name="loginModel">User account data to log in.</param>
        /// <returns>Logging user in <see cref="T:System.Threading.Tasks.Task" />.</returns>
        public async Task<Result<AccountState>> LogInAsync(LoginModel loginModel) 
        {
            return await Task.Run(() => LogIn(loginModel));
        }


        private Result<AccountState> InvokeLogInOrOut(LoginModel loginModel)
        {
            AccountOnlineStatus targetStatus = loginModel.IsLoggingIn ? AccountOnlineStatus.Online : AccountOnlineStatus.Offline;

            try
            {
                messageMediator.SendMessage(messageLogInAttempt);

                Thread.Sleep(500);

                AccountState state = LogInImpl(loginModel);

                if (state.OnlineStatus != targetStatus)
                {
                    return Result<AccountState>.Fail(new UnexpectedErrorReason(state.OnlineStatus.ToString()));
                }

                CurrentAccount = new UserAccount(state.Account);
                return Result<AccountState>.Ok(state);
            }
            catch (Exception ex)
            {
                string direction = loginModel.IsLoggingIn ? "in" : "out";
                string message = $"Failed to log '{loginModel?.Account?.Name}' {direction}";
                logger.Error(ex, message);

                return Result<AccountState>.Fail(new UnexpectedErrorReason(message));
            }
        }


        private AccountState LogInImpl(LoginModel loginModel) 
        {
            //X509Certificate2 certificate = GetMyX509Certificate();
            HashThePassword(loginModel);

            using (var actor = serviceLocator.ResolveType<IHttpActor>())
            {
                actor.SetAddress(Url.Http.LogInUrl);

                AccountState result = actor.PostAsync<LoginModel, AccountState>(loginModel).Result;

                if (result.OnlineStatus == AccountOnlineStatus.Online)
                {
                    messageMediator.SendMessage(messageLoggedIn);
                }

                return result;
            }
        }

        private void HashThePassword(LoginModel loginModel) 
        {
            using (SHA512 keccak = SHA512.Create())
            {
                byte[] passwordBytes = Encoding.ASCII.GetBytes(loginModel.Password);

                byte[] hash = keccak.ComputeHash(passwordBytes);

                loginModel.PasswordHash = Convert.ToBase64String(hash);
            }
        }
    }
}