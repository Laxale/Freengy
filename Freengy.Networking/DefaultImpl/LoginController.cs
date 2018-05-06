// Created by Laxale 19.10.2016
//
//

using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Freengy.Base.Messages;
using Freengy.Base.Interfaces;
using Freengy.Common.Enums;
using Freengy.Common.Models;
using Freengy.Common.Helpers;
using Freengy.Common.Extensions;
using Freengy.Common.Helpers.ErrorReason;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models.Readonly;
using Freengy.Common.Interfaces;
using Freengy.Networking.Helpers;
using Freengy.Networking.Messages;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;

using NLog;

using Catel.IoC;
using Catel.Messaging;

using Freengy.Base.Extensions;
using Freengy.Base.Models;
using Freengy.Common.Constants;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// <see cref="ILoginController"/> implementer.
    /// </summary>
    internal class LoginController : ILoginController 
    {
        private readonly Func<string> clientAddressGetter;
        private readonly IAccountManager accountManager;
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
            accountManager = serviceLocator.ResolveType<IAccountManager>();
            clientAddressGetter = () => serviceLocator.ResolveType<IHttpClientParametersProvider>().GetClientAddressAsync().Result;
        }

        public static ILoginController Instance => instance ?? (instance = new LoginController());

        #endregion Singleton


        /// <summary>
        /// The unique token obtained from server in login result.
        /// </summary>
        public string MySessionToken { get; private set; }

        /// <summary>
        /// The unique token obtained from server. Used to authorize server messages on client.
        /// </summary>
        public string ServerSessionToken { get; private set; }

        /// <summary>
        /// Password that user was logged in with.
        /// </summary>
        public string LoggedInPassword { get; private set; }

        /// <summary>
        /// Returns current user account in usage.
        /// </summary>
        public AccountState MyAccountState { get; private set; }


        /// <summary>
        /// Attempts to register new user.
        /// </summary>
        /// <param name="userName">Desired new user name.</param>
        /// <param name="password">User password.</param>
        /// <returns>Registration result - new account or error details.</returns>
        public Result<UserAccount> Register(string userName, string password) 
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return Result<UserAccount>.Fail(new UnexpectedErrorReason("User name should not be empty"));
                }

                var request = new RegistrationRequest(userName)
                {
                    Password = password
                };

                using (var httpActor = serviceLocator.ResolveType<IHttpActor>())
                {
                    httpActor.SetRequestAddress(Url.Http.RegisterUrl);

                    Result<RegistrationRequest> result = httpActor.PostAsync<RegistrationRequest, RegistrationRequest>(request).Result;
                    if (result.Failure)
                    {
                        return Result<UserAccount>.Fail(new UnexpectedErrorReason(result.Error.Message));
                    }
                    if (result.Value.CreatedAccount == null)
                    {
                        return Result<UserAccount>.Fail(new UnexpectedErrorReason(request.Status.ToString()));
                    }

                    var createdAccount = new UserAccount(result.Value.CreatedAccount);
                    var privateAccountModel = result.Value.CreatedAccount.ToPrivate();
                    var saltHeaderValue = httpActor.ResponceMessage.Headers.GetValues(FreengyHeaders.NextPasswordSaltHeaderName);
                    privateAccountModel.NextLoginSalt = saltHeaderValue.First();

                    accountManager.SaveLastLoggedIn(privateAccountModel);

                    return Result<UserAccount>.Ok(createdAccount);
                }
            }
            catch (Exception ex)
            {
                string message = $"Failed to register account '{ userName }'";
                logger.Error(ex, message);

                return Result<UserAccount>.Fail(new UnexpectedErrorReason(message));
            }
        }

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
                Account = MyAccountState.Account.ToModel(),
                PasswordHash = "fffuuuu"
            };
            
            return InvokeLogInOrOut(loginModel);
        }

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

                AccountStateModel stateModel = LogInImpl(loginModel);

                if (stateModel.OnlineStatus != targetStatus)
                {
                    return Result<AccountStateModel>.Fail(new UnexpectedErrorReason(stateModel.OnlineStatus.ToString()));
                }

                MyAccountState = new AccountState(stateModel);
                return Result<AccountStateModel>.Ok(stateModel);
            }
            catch (Exception ex)
            {
                string direction = loginModel.IsLoggingIn ? "in" : "out";
                string message = $"Failed to log '{loginModel?.Account?.Name}' {direction}. { ex.Message }";
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
                string myAddress = clientAddressGetter();
                if (string.IsNullOrWhiteSpace(myAddress))
                {
                    throw new InvalidOperationException("My address is empty!");
                }

                actor.SetClientAddress(myAddress)
                     .SetRequestAddress(Url.Http.LogInUrl);

                Result<AccountStateModel> result = actor.PostAsync<LoginModel, AccountStateModel>(loginModel).Result;
                if (result.Failure)
                {
                    throw new InvalidOperationException(result.Error.Message);
                }

                AccountStateModel stateModel = result.Value;
                HttpResponseMessage responceMessage = actor.ResponceMessage;
                SessionAuth auth = responceMessage.Headers.GetSessionAuth();

                if (stateModel.OnlineStatus == AccountOnlineStatus.Online)
                {
                    var privateAccountModel = stateModel.Account.ToPrivate();
                    //пока что салт не меняется после каждого логина. сервер не проставляет заголовок
                    //privateAccountModel.NextLoginSalt = actor.ResponceMessage.Headers.GetSaltHeaderValue();
                    accountManager.SaveLastLoggedIn(privateAccountModel);
                    SaveOnlineState(loginModel, auth);
                    messageMediator.SendMessage(messageLoggedIn);
                }
                else if(stateModel.OnlineStatus == AccountOnlineStatus.Offline)
                {
                    SaveOfflineState();
                }

                return stateModel;
            }
        }

        private void SaveOnlineState(LoginModel loginModel, SessionAuth auth) 
        {
            MySessionToken = auth.ClientToken;
            ServerSessionToken = auth.ServerToken;
            LoggedInPassword = loginModel.Password;
        }

        private void SaveOfflineState() 
        {
            MySessionToken = string.Empty;
            ServerSessionToken = string.Empty;
            LoggedInPassword = string.Empty;
        }

        private void HashThePassword(LoginModel loginModel) 
        {
            var loginSalt = accountManager.GetStoredAccount(loginModel.Account.Name).Value.NextLoginSalt;

            loginModel.PasswordHash = new Hasher().GetHash(loginSalt + loginModel.Password);
        }
    }
}