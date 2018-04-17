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
using System.Security.Cryptography.X509Certificates;

using Freengy.Networking.Constants;
using Freengy.Networking.Enum;
using Freengy.Networking.Helpers;
using Freengy.Base.Messages;
using Freengy.Base.Interfaces;
using Freengy.Base.Extensions;
using Freengy.Networking.Models;
using Freengy.Networking.Messages;
using Freengy.Networking.Interfaces;

using NLog;

using Catel.IoC;
using Catel.Messaging;

using Newtonsoft.Json;


namespace Freengy.Networking.DefaultImpl 
{
    using System.Security.Cryptography;


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

        public RegistrationStatus Register(string userName) 
        {
            var handler = new HttpClientHandler
            {
                UseCookies = true
            };

            using (HttpClient client = new HttpClient(handler))
            {
                var request = new RegistrationRequestModel(userName);
                string jsonRequest = JsonConvert.SerializeObject(request);
                string jsonMediaType = mediaTypes.GetStringValue(MediaTypesEnum.Json);
                var httpRequest = new StringContent(jsonRequest, Encoding.UTF8, jsonMediaType);
                
                HttpResponseMessage response = client.PostAsync(Url.Http.ServerHttpRegisterUrl, httpRequest).Result;

                Stream responceStream = response.Content.ReadAsStreamAsync().Result;
                request = new SerializeHelper().DeserializeObject<RegistrationRequestModel>(responceStream);

                return request.Status;
            }
        }

        public AccountOnlineStatus LogIn(LoginModel loginParameters) 
        {
            messageMediator.SendMessage(messageLoggInAttempt);

            Thread.Sleep(500);

            using (SHA512 keccak = SHA512.Create())
            {
                byte[] passwordBytes = Encoding.ASCII.GetBytes(loginParameters.PasswordHash);

                byte[] hash = keccak.ComputeHash(passwordBytes);
            }

            var handler = new HttpClientHandler
            {
                UseCookies = true
            };

            //X509Certificate2 certificate = GetMyX509Certificate();

            using (var client = new HttpClient(handler))
            {
                var serializedRequest = JsonConvert.SerializeObject(loginParameters);
                var content = new StringContent(serializedRequest);
                HttpResponseMessage result = client.PostAsync(Url.Http.ServerHttpLogInUrl, content).Result;

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new InvalidOperationException("Login attempt failed");
                }

                loginParameters = new SerializeHelper().DeserializeObject<LoginModel>(result.Content.ReadAsStreamAsync().Result);

                if (loginParameters.LogInStatus == AccountOnlineStatus.Online)
                {
                    messageMediator.SendMessage(messageLoggedIn);
                }

                return loginParameters.LogInStatus;
            }
        }

        public async Task<AccountOnlineStatus> LogInAsync(LoginModel loginParameters) 
        {
            return await Task.Run(() => LogIn(loginParameters));
        }
    }
}