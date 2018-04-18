// Created by Laxale 19.10.2016
//
//

using System;
using System.IO;
using System.Text;
using System.Runtime;
using System.Net.Http;
using System.Threading;
using System.Configuration;
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
        private readonly MessageBase messageLoggInAttempt;
        private readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        private readonly IMessageMediator messageMediator = MessageMediator.Default;

        private readonly MediaTypes mediaTypes = new MediaTypes();


        #region Singleton

        private static LoginController instance;

        private LoginController() 
        {
            messageLoggedIn = new MessageLogInSuccess();
            messageLoggInAttempt = new MessageLogInAttempt();

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

        /// <summary>
        /// Returns current user account in usage.
        /// </summary>
        public UserAccount CurrentAccount { get; private set; }


        public Result<UserAccount> Register(string userName) 
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return Result<UserAccount>.Fail(new UnexpectedErrorReason("User name should not be empty"));
                }

                var handler = new HttpClientHandler
                {
                    UseCookies = true
                };

                using (HttpClient client = new HttpClient(handler))
                {
                    var request = new RegistrationRequest(userName);
                    string jsonRequest = JsonConvert.SerializeObject(request, Formatting.Indented);
                    string jsonMediaType = mediaTypes.GetStringValue(MediaTypesEnum.Json);
                    var httpRequest = new StringContent(jsonRequest, Encoding.UTF8, jsonMediaType);
                    
                    HttpResponseMessage response = client.PostAsync(Url.Http.ServerHttpRegisterUrl, httpRequest).Result;

                    Stream responceStream = response.Content.ReadAsStreamAsync().Result;
                    request = new SerializeHelper().DeserializeObject<RegistrationRequest>(responceStream);

                    if (request.CreatedAccount == null)
                    {
                        return Result<UserAccount>.Fail(new UnexpectedErrorReason(request.Status.ToString()));
                    }

                    CurrentAccount = request.CreatedAccount;

                    return Result<UserAccount>.Ok(request.CreatedAccount);
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
        public Result<AccountState> LogIn(LoginModel loginModel) 
        {
            try
            {
                if (loginModel?.Account == null)
                {
                    return Result<AccountState>.Fail(new UnexpectedErrorReason("Logging empty account is denied"));
                }

                messageMediator.SendMessage(messageLoggInAttempt);

                Thread.Sleep(500);

                AccountState logInStatus = LogInImpl(loginModel);

                if (logInStatus.OnlineStatus == AccountOnlineStatus.Online)
                {
                    CurrentAccount = loginModel.Account;
                    return Result<AccountState>.Ok(logInStatus);
                }

                return Result<AccountState>.Fail(new UnexpectedErrorReason(logInStatus.ToString()));
            }
            catch (Exception ex)
            {
                string message = $"Failed to log '{ loginModel?.Account?.Name }' in";
                logger.Error(ex, message);

                return Result<AccountState>.Fail(new UnexpectedErrorReason(message));
            }
        }

        /// <summary>
        /// Attempts to log the user out.
        /// </summary>
        public AccountOnlineStatus LogOut() 
        {
            throw new NotImplementedException();
        }

        public async Task<Result<AccountState>> LogInAsync(LoginModel loginParameters) 
        {
            return await Task.Run(() => LogIn(loginParameters));
        }


        private AccountState LogInImpl(LoginModel loginModel) 
        {
            using (SHA512 keccak = SHA512.Create())
            {
                byte[] passwordBytes = Encoding.ASCII.GetBytes(loginModel.PasswordHash);

                byte[] hash = keccak.ComputeHash(passwordBytes);
            }

            var handler = new HttpClientHandler
            {
                UseCookies = true
            };

            //X509Certificate2 certificate = GetMyX509Certificate();

            using (var client = new HttpClient(handler))
            {
                var serializedRequest = JsonConvert.SerializeObject(loginModel);
                var content = new StringContent(serializedRequest);
                HttpResponseMessage result = client.PostAsync(Url.Http.ServerHttpLogInUrl, content).Result;

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new InvalidOperationException("Login attempt failed");
                }

                var loginState  = new SerializeHelper().DeserializeObject<AccountState>(result.Content.ReadAsStreamAsync().Result);

                if (loginState.OnlineStatus == AccountOnlineStatus.Online)
                {
                    messageMediator.SendMessage(messageLoggedIn);
                }

                return loginState;
            }
        }
    }
}