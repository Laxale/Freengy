// Created by Laxale 19.10.2016
//
//


namespace Freengy.Networking.DefaultImpl 
{
    using System;

    using Freengy.Base.Messages;
    using Freengy.Networking.Messages;
    using Freengy.Networking.Interfaces;

    using Catel.Messaging;


    public class LoginController : ILoginController 
    {
        private readonly MessageBase messageLoggedIn;
        private readonly IMessageMediator messageMediator = MessageMediator.Default;


        #region Singleton

        private static LoginController instance;

        private LoginController()
        {
            this.messageLoggedIn = new MessageLoggedIn();
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
            // log in
            // web api or whatever else - details must be hidden by abstract strategy class

            this.messageMediator.SendMessage(this.messageLoggedIn);
        }
    }
}