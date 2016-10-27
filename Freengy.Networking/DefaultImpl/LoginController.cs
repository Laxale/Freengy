// Created by Laxale 19.10.2016
//
//


namespace Freengy.Networking.DefaultImpl 
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Freengy.Base.Messages;
    using Freengy.Base.Interfaces;
    using Freengy.Networking.Messages;
    using Freengy.Networking.Interfaces;

    using Catel.IoC;
    using Catel.Messaging;


    public class LoginController : ILoginController 
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

        public void LogIn(ILoginParameters loginParameters) 
        {
            this.taskWrapper.Wrap(this.LogInTask, this.LogInTaskContinuator);
        }


        private void LogInTask() 
        {
            this.messageMediator.SendMessage(this.messageLoggInAttempt);
            // TODO implement
            // log in
            // web api or whatever else - details must be hidden by abstract strategy class

            Thread.Sleep(500);
        }

        private void LogInTaskContinuator(Task parentTask) 
        {
            // TODO implement
            this.messageMediator.SendMessage(this.messageLoggedIn);
        }
    }
}