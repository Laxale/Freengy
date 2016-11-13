// Created by Laxale 12.11.2016
//
//


namespace Freengy.UI.ViewModels 
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    using Freengy.Base.Helpers;
    using Freengy.Base.ViewModels;
    using Freengy.Networking.Interfaces;
    using Freengy.Networking.DefaultImpl;

    using Catel.IoC;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;

    using Prism.Regions;

    using LocalRes = Freengy.UI.Properties.Resources;
    using CommonRes = Freengy.CommonResources.StringResources;


    internal class RegistrationViewModel : WaitableViewModel 
    {
        private readonly IPleaseWaitService waiter;


        public RegistrationViewModel()
        {
            this.waiter = base.serviceLocator.ResolveType<IPleaseWaitService>();
        }


        protected override void SetupCommands() 
        {
            this.CommandFinish   = new Command<Window>(this.CommandFinishImpl, this.CanFinish);
            this.CommandRegister = new Command(this.CommandRegisterImpl, this.CanCallRegistration);
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults) 
        {
            base.ValidateFields(validationResults);

            this.ValidateEmail(validationResults);
            this.ValidateUserName(validationResults);
            this.ValidatePassword(validationResults);
        }


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

        public string UserName 
        {
            get { return (string)GetValue(UserNameProperty); }

            set { SetValue(UserNameProperty, value); }
        }

        public string Password 
        {
            get { return (string)GetValue(PasswordProperty); }

            set { SetValue(PasswordProperty, value); }
        }

        public string Email 
        {
            get { return (string)GetValue(EmailProperty); }

            set { SetValue(EmailProperty, value); }
        }

        public static readonly PropertyData RegisteredProperty =
            ModelBase.RegisterProperty<RegistrationViewModel, bool>(regViewModel => regViewModel.Registered, () => false);
        public static readonly PropertyData UserNameProperty =
            ModelBase.RegisterProperty<RegistrationViewModel, string>(regViewModel => regViewModel.UserName, () => string.Empty);
        public static readonly PropertyData EmailProperty =
            ModelBase.RegisterProperty<RegistrationViewModel, string>(regViewModel => regViewModel.Email, () => string.Empty);
        public static readonly PropertyData PasswordProperty =
            ModelBase.RegisterProperty<RegistrationViewModel, string>(regViewModel => regViewModel.Password, () => string.Empty);

        #endregion properties


        private void CommandRegisterImpl() 
        {
            this.waiter.Show("Checking data");

            System.Threading.Thread.Sleep(500);

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
            if (this.Registered) return true;

            return true;
        }

        private void ValidateUserName(List<IFieldValidationResult> validationResults) 
        {
            if (string.IsNullOrWhiteSpace(this.UserName))
            {
                string error = string.Format(CommonRes.ValueCannotBeEmptyFormat, CommonRes.UserNameText);
                validationResults.Add(FieldValidationResult.CreateError(UserNameProperty, error));
            }
            else if (Common.HasInvalidSymbols(this.UserName))
            {
                string error = string.Format(CommonRes.ValuesContainsInvalidSymbols, CommonRes.UserNameText);
                validationResults.Add(FieldValidationResult.CreateError(UserNameProperty, error));
            }
        }
        private void ValidatePassword(List<IFieldValidationResult> validationResults) 
        {
            if (Account.IsGoodPassword(this.Password)) return;

            string error = 
                string.Format
                (
                    CommonRes.EntityDoesntMatchRequirementsFormat, 
                    CommonRes.PasswordText,
                    LocalRes.PasswordRequirementsText
                );

            validationResults.Add(FieldValidationResult.CreateError(PasswordProperty, error));
        }
        private void ValidateEmail(List<IFieldValidationResult> validationResults) 
        {
            // empty email is okay
            if (string.IsNullOrWhiteSpace(this.Email)) return;

            if (Account.IsValidEmail(this.Email)) return;

            string error = 
                string.Format
                (
                    CommonRes.EntityDoesntMatchRequirementsFormat, 
                    CommonRes.EmailText,
                    LocalRes.EmailRequirementsText
                );

            validationResults.Add(FieldValidationResult.CreateError(EmailProperty, error));
        }
    }
}