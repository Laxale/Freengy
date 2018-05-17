// Created by Laxale 19.10.2016
//
//

using System;
using System.IO;
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
using Freengy.Common.Helpers.Result;
using Freengy.Common.Interfaces;
using Freengy.Networking.Helpers;
using Freengy.Networking.Messages;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;
using Freengy.Base.DefaultImpl;
using Freengy.Base.Extensions;
using Freengy.Base.Messages.Base;
using Freengy.Base.Models;
using Freengy.Base.Models.Readonly;
using Freengy.Common.Constants;
using Freengy.Common.ErrorReason;

using NLog;

using LocalizedRes = Freengy.Localization.StringResources;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// <see cref="ILoginController"/> implementer.
    /// </summary>
    internal class LoginController : ILoginController 
    {
        private readonly Func<string> clientAddressGetter;

        private readonly IImageCacher imageCacher;
        private readonly ITaskWrapper taskWrapper;
        private readonly IAccountManager accountManager;

        private readonly MessageBase messageLoggedIn;
        private readonly MessageBase messageLogInAttempt;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMyServiceLocator serviceLocator = MyServiceLocator.Instance;

        
        #region Singleton

        private static LoginController instance;

        private LoginController() 
        {
            messageLoggedIn = new MessageLogInSuccess();
            messageLogInAttempt = new MessageLogInAttempt();

            imageCacher = serviceLocator.Resolve<IImageCacher>();
            taskWrapper = serviceLocator.Resolve<ITaskWrapper>();
            accountManager = serviceLocator.Resolve<IAccountManager>();
            clientAddressGetter = () => serviceLocator.Resolve<IHttpClientParametersProvider>().GetClientAddressAsync().Result;

            this.Subscribe<MessageAvatarChanged>(OnAvatarChanged);
            this.Subscribe<MessageExpirienceGained>(OnExpirienceGained);
        }


        /// <summary>
        /// Единственный инстанс <see cref="LoginController"/>.
        /// </summary>
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
        /// Синхронизировать данные моего аккаунта с сервера.
        /// </summary>
        /// <returns>Результат синхронизации.</returns>
        public Result<AccountState> SyncMyAccount() 
        {
            try
            {
                using (var actor = serviceLocator.Resolve<IHttpActor>())
                {
                    actor
                        .AddHeader(FreengyHeaders.Client.ClientIdHeaderName, MyAccountState.Account.Id.ToString())
                        .SetRequestAddress(Url.Http.Sync.SyncAccount)
                        .SetClientSessionToken(MySessionToken);

                    Result<AccountStateModel> result = actor.GetAsync<AccountStateModel>().Result;

                    if (result.Failure)
                    {
                        return Result<AccountState>.Fail(result.Error);
                    }

                    MyAccountState.UpdateFromModel(result.Value);
                    PrivateAccountModel updatedAccount = MyAccountState.Account.ToModel().ToPrivate();
                    accountManager.UpdateMyAccount(updatedAccount);
                    this.Publish(new MessageMyAccountUpdated(MyAccountState));

                    return Result<AccountState>.Ok(MyAccountState);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

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

                using (var httpActor = serviceLocator.Resolve<IHttpActor>())
                {
                    httpActor.SetRequestAddress(Url.Http.RegisterUrl);

                    Result<RegistrationRequest> result = httpActor.PostAsync<RegistrationRequest, RegistrationRequest>(request).Result;
                    if (result.Failure)
                    {
                        return Result<UserAccount>.Fail(new UnexpectedErrorReason(result.Error.Message));
                    }
                    if (result.Value.CreatedAccount == null)
                    {
                        return Result<UserAccount>.Fail(new UnexpectedErrorReason($"Failed to register '{ userName }': { result.Value.Status.ToString() }"));
                    }

                    var createdAccount = new UserAccount(result.Value.CreatedAccount);
                    var privateAccountModel = result.Value.CreatedAccount.ToPrivate();
                    var saltHeaderValue = httpActor.ResponceMessage.Headers.GetValues(FreengyHeaders.Server.NextPasswordSaltHeaderName);
                    privateAccountModel.NextLoginSalt = saltHeaderValue.First();

                    accountManager.SaveLoginTime(privateAccountModel);

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

            Result<AccountStateModel> loginResult = InvokeLogInOrOut(loginModel);

            return loginResult;
        }

        /// <summary>
        /// Attempts to log the user out.
        /// </summary>
        public Result<AccountStateModel> LogOut() 
        {
            var loginModel = new LoginModel
            {
                IsLoggingIn = false,
                Account = MyAccountState.Account.ToModel()
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
                this.Publish(messageLogInAttempt);

                AccountStateModel stateModel = LogInImpl(loginModel);

                if (stateModel.OnlineStatus != targetStatus)
                {
                    return Result<AccountStateModel>.Fail(new UnexpectedErrorReason(stateModel.OnlineStatus.ToString()));
                }

                MyAccountState = new AccountState(stateModel);

                if (stateModel.OnlineStatus == AccountOnlineStatus.Online)
                {
                    Task.Run(() => CacheMyAvatar());
                }

                return Result<AccountStateModel>.Ok(stateModel);
            }
            catch (Exception ex)
            {
                //string direction = loginModel.IsLoggingIn ? "in" : "out";
                string message = $"{ LocalizedRes.Error }. { ex.Message }";
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

            using (var actor = serviceLocator.Resolve<IHttpActor>())
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
                    var privateAccountModel = stateModel.AccountModel.ToPrivate();
                    //пока что салт не меняется после каждого логина. сервер не проставляет заголовок
                    //privateAccountModel.NextLoginSalt = actor.ResponceMessage.Headers.GetSaltHeaderValue();
                    accountManager.SaveLoginTime(privateAccountModel);
                    SaveOnlineState(loginModel, auth);
                    
                    this.Publish(messageLoggedIn);
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
            Result<PrivateAccountModel> result = accountManager.GetStoredAccount(loginModel.Account.Name);
            if (result.Failure)
            {
                throw new InvalidOperationException(result.Error.Message);
            }

            var loginSalt = result.Value.NextLoginSalt;

            loginModel.PasswordHash = new Hasher().GetHash(loginSalt + loginModel.Password);
        }

        private void OnExpirienceGained(MessageExpirienceGained message) 
        {
            if (MyAccountState == null)
            {
                // так может быть, если сервер начислил опыт за онлайн до того, как закончилась клиентская логика логина
                return;
            }

            MyAccountState.Account.AddExp(message.Amount);

            this.Publish(new MessageMyAccountUpdated(MyAccountState));
        }

        private void OnAvatarChanged(MessageAvatarChanged message) 
        {
            Result<UserAvatarModel> existingAvatar = MyAccountState.Account.GetExtensionPayload<AvatarExtension, UserAvatarModel>();

            if (existingAvatar.Value == null)
            {
                var newModel = new UserAvatarModel
                {
                    AvatarPath = message.NewAvatarPath,
                    LastModified = DateTime.Now,
                    ParentId = MyAccountState.Account.Id
                };
                var extension = new AvatarExtension(newModel);
                MyAccountState.Account.AddExtension<AvatarExtension, UserAvatarModel>(extension);
            }
            else
            {
                existingAvatar.Value.AvatarBlob = null;
                existingAvatar.Value.AvatarPath = message.NewAvatarPath;
            }

            this.Publish(new MessageMyAccountUpdated(MyAccountState));
        }

        private void CacheMyAvatar() 
        {
            var myAvatarResult = accountManager.GetUserAvatars(new[] { MyAccountState.Account.Id });
            if (myAvatarResult.Failure)
            {
                throw new InvalidOperationException(myAvatarResult.Error.Message);
            }

            //аватара у меня может и не быть
            UserAvatarModel myAvatar = myAvatarResult.Value.FirstOrDefault();
            if (myAvatar == null)
            {
                return;
            }

            if (!File.Exists(myAvatar.AvatarPath))
            {
                myAvatar.AvatarPath = imageCacher.CacheAvatar(myAvatar).Value;
            }

            var avatarExtension = new AvatarExtension(myAvatar);
            MyAccountState.Account.AddExtension<AvatarExtension, UserAvatarModel>(avatarExtension);

            this.Publish(new MessageMyAccountUpdated(MyAccountState));
        }
    }
}