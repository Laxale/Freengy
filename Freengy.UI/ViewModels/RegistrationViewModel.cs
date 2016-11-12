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

            if (string.IsNullOrWhiteSpace(this.UserName))
            {
                validationResults.Add
                (
                    FieldValidationResult.CreateError(UserNameProperty, CommonRes.ValueCannotBeEmptyFormat, CommonRes.HostNameText)
                );
            }
            else if (Common.HasInvalidSymbols(this.UserName))
            {
                FieldValidationResult.CreateError(UserNameProperty, CommonRes.ValuesContainsInvalidSymbols, CommonRes.HostNameText);
            }
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
            ModelBase.RegisterProperty<RegistrationViewModel, string>(regViewModel => regViewModel.UserName, () => string.Empty);
        
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
    }
}