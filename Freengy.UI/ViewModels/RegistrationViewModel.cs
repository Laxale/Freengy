﻿// Created by Laxale 12.11.2016
//
//


namespace Freengy.UI.ViewModels 
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Freengy.Base.Helpers;
    using Freengy.Base.ViewModels;
    using Freengy.Networking.Interfaces;
    using Freengy.SharedWebTypes.Objects;

    using Catel.IoC;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;
    

    internal class RegistrationViewModel : CredentialViewModel 
    {
        private readonly IPleaseWaitService waiter;
        private readonly ILoginController loginController;



        public RegistrationViewModel() 
        {
            this.waiter = base.serviceLocator.ResolveType<IPleaseWaitService>();
            this.loginController = base.serviceLocator.ResolveType<ILoginController>();
        }


        #region override

        protected override void SetupCommands() 
        {
            this.CommandFinish   = new Command<Window>(this.CommandFinishImpl, this.CanFinish);
            this.CommandRegister = new Command(this.CommandRegisterImpl, this.CanCallRegistration);
        }
        
        #endregion override


        #region commands

        public Command CommandRegister { get; private set; }

        public Command<Window> CommandFinish { get; private set; }

        #endregion commands


        #region properties
        
        public bool Registered 
        {
            get { return (bool)GetValue(RegisteredProperty); }

            private set { SetValue(RegisteredProperty, value); }
        }
        public bool IsCodeSent 
        {
            get { return (bool)GetValue(IsCodeSentProperty); }

            private set { SetValue(IsCodeSentProperty, value); }
        }

        public static readonly PropertyData RegisteredProperty =
            ModelBase.RegisterProperty<RegistrationViewModel, bool>(regViewModel => regViewModel.Registered, () => false);
        public static readonly PropertyData IsCodeSentProperty =
            ModelBase.RegisterProperty<RegistrationViewModel, bool>(regViewModel => regViewModel.IsCodeSent, () => false);

        #endregion properties


        #region privates

        private void CommandRegisterImpl() 
        {
            this.waiter.Show("Checking data");

            var loginModel = new LoginModel
            {
                UserName = this.UserName
            };

            this.loginController.Register(loginModel);

            this.waiter.Hide();

            this.Registered = true;
        }
        private bool CanCallRegistration() 
        {
            if (this.Registered) return false;

            List<IFieldValidationResult> validationResults = new List<IFieldValidationResult>();
            this.ValidateFields(validationResults);

            bool canTryRegister = !validationResults.Any();

            return canTryRegister;
        }

        private void CommandFinishImpl(Window registrationWindow)
        {
            // send message
            registrationWindow.Close();
        }
        private bool CanFinish(Window registrationWindow) 
        {
            return this.Registered;
        }

        #endregion privates
    }
}