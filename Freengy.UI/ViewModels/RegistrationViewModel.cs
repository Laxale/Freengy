// Created by Laxale 12.11.2016
//
//

using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using Freengy.Common.Enums;
using Freengy.Common.Helpers.Result;
using Freengy.Base.Helpers;
using Freengy.Common.Models;
using Freengy.Base.ViewModels;
using Freengy.Networking.Interfaces;

using Catel.IoC;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;


namespace Freengy.UI.ViewModels 
{   
    internal class RegistrationViewModel : CredentialViewModel 
    {
        private readonly IPleaseWaitService waiter;
        private readonly ILoginController loginController;
        

        public RegistrationViewModel() 
        {
            waiter = ServiceLocatorProperty.ResolveType<IPleaseWaitService>();
            loginController = ServiceLocatorProperty.ResolveType<ILoginController>();
        }


        #region override

        protected override void SetupCommands() 
        {
            CommandFinish   = new MyCommand(CommandFinishImpl, CanFinish);
            CommandRegister = new MyCommand(CommandRegisterImpl, CanCallRegistration);
        }
        
        #endregion override


        #region commands

        public MyCommand CommandRegister { get; private set; }

        public MyCommand CommandFinish { get; private set; }

        #endregion commands


        #region properties


        private bool registered;

        public bool Registered 
        {
            get => registered;

            set
            {
                if (registered == value) return;

                registered = value;

                OnPropertyChanged();
            }
        }


        private bool isCodeSent;

        public bool IsCodeSent 
        {
            get => isCodeSent;

            set
            {
                if (isCodeSent == value) return;

                isCodeSent = value;

                OnPropertyChanged();
            }
        }

        #endregion properties


        #region privates

        private void CommandRegisterImpl(object notUsed) 
        {
            waiter.Show("Checking data");

            Result<UserAccount> result = loginController.Register(UserName);

            waiter.Hide();

            if (result.Failure)
            {
                ReportMessage(result.Error.Message);
            }
            else
            {
                Registered = true;
            }
        }
        private bool CanCallRegistration(object notUsed) 
        {
            if (Registered) return false;

            List<IFieldValidationResult> validationResults = new List<IFieldValidationResult>();
            //ValidateFields(validationResults);

            bool canTryRegister = !validationResults.Any();

            return canTryRegister;
        }

        private void CommandFinishImpl(object registrationWindow)
        {
            // send message
            ((Window)registrationWindow).Close();
        }
        private bool CanFinish(object registrationWindow) 
        {
            return Registered;
        }

        #endregion privates
    }
}