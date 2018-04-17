// Created by Laxale 12.11.2016
//
//

using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;

using Freengy.Base.Helpers;
using Freengy.Base.ViewModels;
using Freengy.Networking.Interfaces;

using Catel.IoC;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using Freengy.Networking.Models;


namespace Freengy.UI.ViewModels 
{   
    internal class RegistrationViewModel : CredentialViewModel 
    {
        private readonly IPleaseWaitService waiter;
        private readonly ILoginController loginController;
        

        public RegistrationViewModel() 
        {
            waiter = serviceLocator.ResolveType<IPleaseWaitService>();
            loginController = serviceLocator.ResolveType<ILoginController>();
        }


        #region override

        protected override void SetupCommands() 
        {
            CommandFinish   = new Command<Window>(CommandFinishImpl, CanFinish);
            CommandRegister = new Command(CommandRegisterImpl, CanCallRegistration);
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
            RegisterProperty<RegistrationViewModel, bool>(regViewModel => regViewModel.Registered, () => false);
        public static readonly PropertyData IsCodeSentProperty =
            RegisterProperty<RegistrationViewModel, bool>(regViewModel => regViewModel.IsCodeSent, () => false);

        #endregion properties


        #region privates

        private void CommandRegisterImpl() 
        {
            waiter.Show("Checking data");

            var loginModel = new LoginModel
            {
                UserName = UserName
            };

            loginController.Register(loginModel);

            waiter.Hide();

            Registered = true;
        }
        private bool CanCallRegistration() 
        {
            if (Registered) return false;

            List<IFieldValidationResult> validationResults = new List<IFieldValidationResult>();
            ValidateFields(validationResults);

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
            return Registered;
        }

        #endregion privates
    }
}