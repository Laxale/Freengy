// Created by Laxale 19.10.2016
//
//

using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;

using Freengy.Base.Settings;
using Freengy.Base.Extensions;
using Freengy.Base.ViewModels;
using Freengy.UI.Views;
using Freengy.Networking.Models;
using Freengy.Networking.Interfaces;
using Freengy.Networking.DefaultImpl;

using Catel.IoC;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using Prism.Regions;

using CommonRes = Freengy.CommonResources.StringResources;


namespace Freengy.UI.ViewModels 
{   
    /// <summary>
    /// ViewModel for <see cref="LoginView"/>.
    /// </summary>
    internal class LoginViewModel : CredentialViewModel, INavigationAware 
    {
        #region Variables

        private readonly IPleaseWaitService waiter;
        private readonly ILoginController loginController;
        
        #endregion Variables


        public LoginViewModel() 
        {
            // need to resolve it by interface to avoid knowledge about concrete implementer
            waiter = serviceLocator.ResolveType<IPleaseWaitService>();
            loginController = serviceLocator.ResolveType<ILoginController>();
        }


        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext) 
        {
            // need to log?
            var t = 0;
            if (t > 0) { }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) 
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) 
        {
            var t = 0;
            if (t > 0) { }
        }

        #endregion INavigationAware


        #region Public properties

        public string Port 
        {
            get { return (string)GetValue(PortProperty); }

            set { SetValue(PortProperty, value); }
        }

        public string HostName 
        {
            get { return (string)GetValue(HostNameProperty); }

            set { SetValue(HostNameProperty, value); }
        }

        public bool SavePassword 
        {
            get { return (bool)GetValue(SavePasswordProperty); }

            set { SetValue(SavePasswordProperty, value); }
        }
        
        public string Information 
        {
            get { return (string)GetValue(InformationProperty); }

            private set { SetValue(InformationProperty, value); }
        }

        public bool HasInformation 
        {
            get { return (bool)GetValue(HasInformationProperty); }

            private set { SetValue(HasInformationProperty, value); }
        }

        #endregion Public properties


        #region Commands

        public Command CommandLogin { get; private set; }

        public Command CommandCreateAccount { get; private set; }

        public Command CommandRecoverPassword { get; private set; }
        
        #endregion Commands


        #region Overrides

        protected override void SetupCommands() 
        {
            CommandCreateAccount   = new Command(CreateAccountImpl);
            CommandRecoverPassword = new Command(RecoverPasswordImpl);
            CommandLogin = new Command(CommandLoginImpl, CanLogIn);
        }

        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            LoadCredsFromSettings();
        }

        public override void ReportMessage(string information) 
        {
            HasInformation = !string.IsNullOrWhiteSpace(information);

            Information = information;
        }

        protected override void InitializationContinuator(Task parentTask) 
        {
            base.InitializationContinuator(parentTask);

            if (parentTask.Exception != null)
            {
                ReportMessage(parentTask.Exception.GetReallyRootException().Message);
            }
            else
            {
//                ((Action)this.LoadCredsFromSettings)
//                    .ExecuteWithExceptionWrap(exception => this.ReportMessage(exception.GetReallyRootException().Message));
            }
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults) 
        {
            base.ValidateFields(validationResults);

            if (string.IsNullOrWhiteSpace(HostName))
            {
                validationResults.Add
                (
                    //new FieldValidationResult(HostNameProperty, ValidationResultType.Error, CommonRes.ValueCannotBeEmptyFormat, CommonRes.HostNameText)
                    FieldValidationResult.CreateError(HostNameProperty, CommonRes.ValueCannotBeEmptyFormat, CommonRes.HostNameText)
                );
            }

            if (string.IsNullOrWhiteSpace(UserName))
            {
                validationResults.Add
                (
                    new FieldValidationResult(UserNameProperty, ValidationResultType.Error, CommonRes.ValueCannotBeEmptyFormat, CommonRes.UserNameText)
                );
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                validationResults.Add
                (
                    new FieldValidationResult(PasswordProperty, ValidationResultType.Error, CommonRes.ValueCannotBeEmptyFormat, CommonRes.PasswordText)
                );
            }

            if (string.IsNullOrWhiteSpace(Port))
            {
                validationResults.Add
                (
                    new FieldValidationResult(PortProperty, ValidationResultType.Error, CommonRes.ValueCannotBeEmptyFormat, CommonRes.PasswordText)
                );
            }
        }

        #endregion Overrides


        #region Privates

        private async void CreateAccountImpl() 
        {
            var registrationViewModel = typeFactory.CreateInstance<RegistrationViewModel>();

            await uiVisualizer.ShowDialogAsync(registrationViewModel);
        }

        private async void RecoverPasswordImpl() 
        {
            var recoverViewModel = typeFactory.CreateInstance<RecoverPasswordViewModel>();

            await uiVisualizer.ShowDialogAsync(recoverViewModel);
        }

        private bool CanLogIn() 
        {
            bool canLogin =
                !string.IsNullOrWhiteSpace(UserName) &&
                !string.IsNullOrWhiteSpace(Password);

            return canLogin;
        }

        private void LoadCredsFromSettings() 
        {
            var settings = AppSettings.Instance;

            //this.Port     = settings.Port;
            //this.HostName = settings.HostName;
            //this.UserName = settings.UserName;
            Port = settings.Port;
            HostName = "localhost";
            UserName = @"Администратор";
            //            this.HostName = @"w610sstd64en-55";
            //            this.UserName = @"w610sstd64en-55\Debugger";
            Password = "Qwerty1234";
//            return;

            SavePassword = settings.SavePassword;

            if (!string.IsNullOrWhiteSpace(settings.LastUserSession) && SavePassword)
            {
                try
                {
                    var userSession = Convert.FromBase64String(settings.LastUserSession);
                    var entropy = new byte[] { 0x50, 0x50, 0x52, 0x2d, 0x54, 0x68, 0x65, 0x42, 0x65, 0x73, 0x74 };
                    Password = Encoding.Unicode.GetString(ProtectedData.Unprotect(userSession, entropy, DataProtectionScope.CurrentUser));
                }
                catch
                {
                    Password = string.Empty;

//                    this.ReportMessage(CommonRes.FailedToDecryptPasswordError);
                }
            }
        }

        private async void CommandLoginImpl() 
        {
            await taskWrapper.Wrap(LoginAction, LoginContinuator);
        }

        private void LoginAction() 
        {
            IsWaiting = true;

            loginController.LogIn(GetCurrentLoginParameters());
        }
        private void LoginContinuator(Task parentTask) 
        {
            IsWaiting = false;

            if (parentTask.Exception != null)
            {
                ReportMessage(parentTask.Exception.GetReallyRootException().Message);
            }
        }

        private LoginModel GetCurrentLoginParameters() 
        {
            var parameters = serviceLocator.ResolveType<LoginModel>();

            parameters.UserName = UserName;
            parameters.PasswordHash = Password; // TODO: waaaat?
            parameters.UserName = UserName;

            return parameters;
        }

        #endregion Privates


        #region properties data

        public static readonly PropertyData PortProperty =
            RegisterProperty<LoginViewModel, string>(loginViewModel => loginViewModel.Port, () => string.Empty);

        public static readonly PropertyData HostNameProperty =
            RegisterProperty<LoginViewModel, string>(loginViewModel => loginViewModel.HostName, () => string.Empty);

        public static readonly PropertyData InformationProperty =
            RegisterProperty<LoginViewModel, string>(loginViewModel => loginViewModel.Information, () => string.Empty);

        public static readonly PropertyData HasInformationProperty =
            RegisterProperty<LoginViewModel, bool>(loginViewModel => loginViewModel.HasInformation, () => false);

        public static readonly PropertyData SavePasswordProperty =
            RegisterProperty<LoginViewModel, bool>(loginViewModel => loginViewModel.SavePassword, () => false);

        #endregion properties data
    }
}