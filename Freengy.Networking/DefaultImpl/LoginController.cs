﻿// Created by Laxale 19.10.2016
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

using Newtonsoft.Json;


namespace Freengy.Networking.DefaultImpl 
{
    internal class LoginController : ILoginController
    {
        #region vars

        private readonly ITaskWrapper taskWrapper;
        private readonly MessageBase messageLoggedIn;
        private readonly MessageBase messageLoggInAttempt;
        private readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        private readonly IMessageMediator messageMediator = MessageMediator.Default;

        private readonly MediaTypes mediaTypes = new MediaTypes();

        private readonly Configuration networkingConfig;
        private readonly string serverAddress;
        private readonly string registrationActionSubPath;

    #endregion vars

        
        #region Singleton

        private static LoginController instance;

        private LoginController() 
        {
            this.messageLoggedIn = new MessageLogInSuccess();
            this.messageLoggInAttempt = new MessageLogInAttempt();

            this.taskWrapper = this.serviceLocator.ResolveType<ITaskWrapper>();

            this.networkingConfig = ConfigurationManager.OpenExeConfiguration(typeof(LoginController).Assembly.Location);
            this.serverAddress = this.networkingConfig.AppSettings.Settings["FreengyServerAddress"].Value;
            this.registrationActionSubPath =
                $"{ this.networkingConfig.AppSettings.Settings["RegistrationControllerName"].Value }/" +
                $"{ this.networkingConfig.AppSettings.Settings["RegistrationActionName"].Value }";
        }

        public static LoginController Instance => LoginController.instance ?? (LoginController.instance = new LoginController());

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
            var registratioRequest = new RegistrationRequestModel(loginParameters.UserName);

            var handler = new HttpClientHandler
            {
                UseCookies = true
            };

            HttpClient client = new HttpClient(handler);
            var request = new RegistrationRequestModel(loginParameters.UserName);
            string jsonRequest = JsonConvert.SerializeObject(request);
            string jsonMediaType = this.mediaTypes.GetStringValue(MediaTypesEnum.Json);
            var httpRequest = new StringContent(jsonRequest, Encoding.UTF8, jsonMediaType);
            string registrationActionFullPath = $"{ this.serverAddress }/{ this.registrationActionSubPath }";

            HttpResponseMessage response = client.PostAsync(registrationActionFullPath, httpRequest).Result;
            
            return true;
        }

        public void LogIn(LoginModel loginParameters) 
        {
            this.LogInTask(loginParameters);
            this.LogInTaskContinuator();
        }

        public Task LogInAsync(LoginModel loginParameters) 
        {
            return this.taskWrapper.Wrap(() => this.LogInTask(loginParameters), this.LogInTaskContinuator);
        }


        private async void LogInTask(LoginModel loginParameters) 
        {
            this.messageMediator.SendMessage(this.messageLoggInAttempt);

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
            this.messageMediator.SendMessage(this.messageLoggedIn);
        }
        private void LogInTaskContinuator(Task parentTask) 
        {
            // TODO implement

            if (parentTask.Exception != null)
            {
                var message = parentTask.Exception.GetReallyRootException().Message;
            }

            this.messageMediator.SendMessage(this.messageLoggedIn);
        }
    }
}