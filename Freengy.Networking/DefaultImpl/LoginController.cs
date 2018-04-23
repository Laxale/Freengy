// Created by Laxale 19.10.2016
//
//

using System;
using System.Threading;
using System.Threading.Tasks;

using Freengy.Common.Enums;
using Freengy.Common.Models;
using Freengy.Common.Helpers;
using Freengy.Common.Extensions;
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
using Freengy.Common.Models.Readonly;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// <see cref="ILoginController"/> implementer.
    /// </summary>
    internal class LoginController : ILoginController
    {
        private readonly string clientAddress;
        private readonly ITaskWrapper taskWrapper;
        private readonly MessageBase messageLoggedIn;
        private readonly MessageBase messageLogInAttempt;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        private readonly IMessageMediator messageMediator = MessageMediator.Default;

        
        #region Singleton

        private static LoginController instance;

        private LoginController() 
        {
            messageLoggedIn = new MessageLogInSuccess();
            messageLogInAttempt = new MessageLogInAttempt();

            taskWrapper = serviceLocator.ResolveType<ITaskWrapper>();
            clientAddress = serviceLocator.ResolveType<IHttpClientParametersProvider>().ClientAddress;
        }

        public static LoginController Instance => instance ?? (instance = new LoginController());

        #endregion Singleton


        /// <summary>
        /// The unique token obtained from server in login result.
        /// </summary>
        public string SessionToken { get; private set; }

        /// <summary>
        /// Password that user was logged in with.
        /// </summary>
        public string LoggedInPassword { get; private set; }

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
                    httpActor.SetRequestAddress(Url.Http.RegisterUrl);

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
        public Result<AccountStateModel> LogIn(LoginModel loginModel) 
        {
            if (loginModel?.Account == null)
            {
                return Result<AccountStateModel>.Fail(new UnexpectedErrorReason("Logging empty account is denied"));
            }

            loginModel.IsLoggingIn = true;

            return InvokeLogInOrOut(loginModel);
        }

        /// <summary>
        /// Attempts to log the user out.
        /// </summary>
        public Result<AccountStateModel> LogOut() 
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
        public async Task<Result<AccountStateModel>> LogInAsync(LoginModel loginModel) 
        {
            return await Task.Run(() => LogIn(loginModel));
        }


        private Result<AccountStateModel> InvokeLogInOrOut(LoginModel loginModel) 
        {
            AccountOnlineStatus targetStatus = loginModel.IsLoggingIn ? AccountOnlineStatus.Online : AccountOnlineStatus.Offline;

            try
            {
                messageMediator.SendMessage(messageLogInAttempt);

                Thread.Sleep(300);

                AccountStateModel stateModel = LogInImpl(loginModel);

                if (stateModel.OnlineStatus != targetStatus)
                {
                    return Result<AccountStateModel>.Fail(new UnexpectedErrorReason(stateModel.OnlineStatus.ToString()));
                }

                CurrentAccount = new UserAccount(stateModel.Account);
                return Result<AccountStateModel>.Ok(stateModel);
            }
            catch (Exception ex)
            {
                string direction = loginModel.IsLoggingIn ? "in" : "out";
                string message = $"Failed to log '{loginModel?.Account?.Name}' {direction}";
                logger.Error(ex, message);

                return Result<AccountStateModel>.Fail(new UnexpectedErrorReason(message));
            }
        }


        private AccountStateModel LogInImpl(LoginModel loginModel) 
        {
            //X509Certificate2 certificate = GetMyX509Certificate();
            if (string.IsNullOrWhiteSpace(loginModel.Password))
            {
                loginModel.Password = LoggedInPassword;
            }

            HashThePassword(loginModel);

            using (var actor = serviceLocator.ResolveType<IHttpActor>())
            {
                actor.SetClientAddress(clientAddress).SetRequestAddress(Url.Http.LogInUrl);

                AccountStateModel result = actor.PostAsync<LoginModel, AccountStateModel>(loginModel).Result;

                if (result.OnlineStatus == AccountOnlineStatus.Online)
                {
                    SessionToken = result.SessionToken;
                    LoggedInPassword = loginModel.Password;
                    messageMediator.SendMessage(messageLoggedIn);
                }

                return result;
            }
        }

        private void HashThePassword(LoginModel loginModel) 
        {
            loginModel.PasswordHash = new Hasher().GetHash(loginModel.Password);
        }
    }
}