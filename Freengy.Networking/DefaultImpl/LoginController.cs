// Created by Laxale 19.10.2016
//
//


namespace Freengy.Networking.DefaultImpl 
{
    using System;
    using System.Text;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Security.Cryptography.X509Certificates;

    using Freengy.Base.Messages;
    using Freengy.Base.Interfaces;
    using Freengy.Base.Extensions;
    using Freengy.Networking.Messages;
    using Freengy.Networking.Interfaces;

    using Catel.IoC;
    using Catel.Messaging;


    internal class LoginController : ILoginController 
    {
        #region vars

        private readonly ITaskWrapper taskWrapper;
        private readonly MessageBase messageLoggedIn;
        private readonly MessageBase messageLoggInAttempt;
        private readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        private readonly IMessageMediator messageMediator = MessageMediator.Default;

        #endregion vars

        
        #region Singleton

        private static LoginController instance;

        private LoginController() 
        {
            this.messageLoggedIn = new MessageLogInSuccess();
            this.messageLoggInAttempt = new MessageLogInAttempt();

            this.taskWrapper = this.serviceLocator.ResolveType<ITaskWrapper>();
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

        public async Task LogIn(ILoginParameters loginParameters) 
        {
            await this.taskWrapper.Wrap(() => this.LogInTask(loginParameters), this.LogInTaskContinuator);
        }


        private async void LogInTask(ILoginParameters loginParameters) 
        {
            this.messageMediator.SendMessage(this.messageLoggInAttempt);
            // TODO implement
            using (var keccak = System.Security.Cryptography.SHA512.Create())
            {
                byte[] passwordBytes = Encoding.ASCII.GetBytes(loginParameters.Password);

                byte[] hash = keccak.ComputeHash(passwordBytes);
            }

            var handler = new HttpClientHandler
            {
                UseCookies = true
            };

            //X509Certificate2 certificate = GetMyX509Certificate();

            HttpClient client = new HttpClient(handler);

            var result = await client.GetAsync("http://localhost:44000");
            
            Thread.Sleep(500);
        }

        private void LogInTaskContinuator(Task parentTask) 
        {
            // TODO implement

            if (parentTask.Exception != null)
            {
                var message = parentTask.Exception.GetReallyRootException().Message;
            }

            //this.messageMediator.SendMessage(this.messageLoggedIn);
        }
    }
}