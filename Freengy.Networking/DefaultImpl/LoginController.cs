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
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using Freengy.Base.Messages;
using Freengy.Base.Interfaces;
using Freengy.Base.Extensions;
using Freengy.Networking.Models;
using Freengy.Networking.Messages;
using Freengy.Networking.Interfaces;

using Catel.IoC;
using Catel.Messaging;
using Freengy.Networking.Constants;
using Newtonsoft.Json;


namespace Freengy.Networking.DefaultImpl 
{
    internal class LoginController : ILoginController 
    {
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

        public bool Register(LoginModel loginParameters) 
        {
            var handler = new HttpClientHandler
            {
                UseCookies = true
            };

            using (HttpClient client = new HttpClient(handler))
            {
                var request = new RegistrationRequestModel(loginParameters.UserName);
                string jsonRequest = JsonConvert.SerializeObject(request);
                string jsonMediaType = mediaTypes.GetStringValue(MediaTypesEnum.Json);
                var httpRequest = new StringContent(jsonRequest, Encoding.UTF8, jsonMediaType);
                
                HttpResponseMessage response = client.PostAsync(Url.Http.ServerHttpRegisterUrl, httpRequest).Result;
            
                return true;
            }
        }

        public void LogIn(LoginModel loginParameters) 
        {
            LogInTask(loginParameters);
            LogInTaskContinuator();
        }

        public Task LogInAsync(LoginModel loginParameters) 
        {
            return taskWrapper.Wrap(() => LogInTask(loginParameters), LogInTaskContinuator);
        }


        private async void LogInTask(LoginModel loginParameters) 
        {
            messageMediator.SendMessage(messageLoggInAttempt);

            Thread.Sleep(1000);
            
            return; // skip for now

            // TODO implement
            using (var keccak = System.Security.Cryptography.SHA512.Create())
            {
                byte[] passwordBytes = Encoding.ASCII.GetBytes(loginParameters.PasswordHash);

                byte[] hash = keccak.ComputeHash(passwordBytes);
            }

            var handler = new HttpClientHandler
            {
                UseCookies = true
            };

            //X509Certificate2 certificate = GetMyX509Certificate();

            HttpClient client = new HttpClient(handler);

            var result = await client.GetAsync("http://localhost:44000");
            
            
        }

        private void LogInTaskContinuator() 
        {
            messageMediator.SendMessage(messageLoggedIn);
        }
        private void LogInTaskContinuator(Task parentTask) 
        {
            // TODO implement

            if (parentTask.Exception != null)
            {
                var message = parentTask.Exception.GetReallyRootException().Message;
            }

            messageMediator.SendMessage(messageLoggedIn);
        }
    }
}