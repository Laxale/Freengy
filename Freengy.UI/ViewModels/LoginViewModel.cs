// Created 19.10.2016
//
//


namespace Freengy.UI.ViewModels 
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    using Freengy.Base.Settings;
    using Freengy.Base.Extensions;
    using Freengy.Base.ViewModels;
    using Freengy.Networking.Interfaces;
    using Freengy.Networking.DefaultImpl;

    using Catel.IoC;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;

    using Prism.Regions;
    
    using CommonRes = Freengy.CommonResources.StringResources;

    
    public class LoginViewModel : WaitableViewModel, INavigationAware 
    {
        #region Variables

        public readonly IPleaseWaitService waiter;
        private readonly ILoginController loginController;
        
        #endregion Variables


        public LoginViewModel() : base(true) 
        {
            this.loginController = new LoginController();
            this.waiter = this.serviceLocator.ResolveType<IPleaseWaitService>();
        }


        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext) 
        {
            var t = 0;
            if (t > 0) { }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) 
        {
            var t = 0;
            if (t > 0) { }

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

            set
            {
                value.FilterNewLineSymbols();
                SetValue(HostNameProperty, value);
            }
        }

        public string UserName 
        {
            get { return (string)GetValue(UserNameProperty); }

            set
            {
                value.FilterNewLineSymbols();
                SetValue(UserNameProperty, value);
            }
        }

        public string Password 
        {
            get { return (string)GetValue(PasswordProperty); }

            set { SetValue(PasswordProperty, value); }
        }

        public bool SavePassword 
        {
            get { return (bool)GetValue(SavePasswordProperty); }

            private set { SetValue(SavePasswordProperty, value); }
        }


        public string Information 
        {
            get { return (string)this.GetValue(InformationProperty); }

            private set { this.SetValue(InformationProperty, value); }
        }

        public bool HasInformation 
        {
            get { return (bool)GetValue(HasInformationProperty); }

            private set { SetValue(HasInformationProperty, value); }
        }

        #endregion Public properties


        #region commands

        public Command LoginCommand { get; private set; }

        #endregion commands


        #region Overrides

        protected override void SetupCommands() 
        {
            this.LoginCommand = new Command(() => this.loginController.LogIn(), LoginViewModel.CanLogIn);
        }

        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();
        }

        public override void ReportMessage(string information) 
        {
            this.HasInformation = !string.IsNullOrWhiteSpace(information);

            this.Information = information;
        }

        protected override void InitializationContinuator(Task parentTask) 
        {
            base.InitializationContinuator(parentTask);

            if (parentTask.Exception != null)
            {
                this.ReportMessage(parentTask.Exception.GetReallyRootException().Message);
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

            if (string.IsNullOrWhiteSpace(this.HostName))
            {
                validationResults.Add
                (
                    //new FieldValidationResult(HostNameProperty, ValidationResultType.Error, CommonRes.ValueCannotBeEmptyFormat, CommonRes.HostNameText)
                    FieldValidationResult.CreateError(HostNameProperty, CommonRes.ValueCannotBeEmptyFormat, CommonRes.HostNameText)
                );
            }

            if (string.IsNullOrWhiteSpace(this.UserName))
            {
                validationResults.Add
                (
                    new FieldValidationResult(UserNameProperty, ValidationResultType.Error, CommonRes.ValueCannotBeEmptyFormat, CommonRes.UserNameText)
                );
            }

            if (string.IsNullOrWhiteSpace(this.Password))
            {
                validationResults.Add
                (
                    new FieldValidationResult(PasswordProperty, ValidationResultType.Error, CommonRes.ValueCannotBeEmptyFormat, CommonRes.PasswordText)
                );
            }

            if (string.IsNullOrWhiteSpace(this.Port))
            {
                validationResults.Add
                (
                    new FieldValidationResult(PortProperty, ValidationResultType.Error, CommonRes.ValueCannotBeEmptyFormat, CommonRes.PasswordText)
                );
            }
        }

        #endregion Overrides


        #region Privates

        private void LoadCredsFromSettings() 
        {
            var settings = AppSettings.Instance;

            //this.Port     = settings.Port;
            //this.HostName = settings.HostName;
            //this.UserName = settings.UserName;
            this.Port = settings.Port;
            this.HostName = "localhost";
            this.UserName = @"Администратор";
            //            this.HostName = @"w610sstd64en-55";
            //            this.UserName = @"w610sstd64en-55\Debugger";
            this.Password = "Qwerty123";
//            return;

            this.SavePassword = settings.SavePassword;

            if (!string.IsNullOrWhiteSpace(settings.LastUserSession) && this.SavePassword)
            {
                try
                {
                    var userSession = Convert.FromBase64String(settings.LastUserSession);
                    var entropy = new byte[] { 0x50, 0x50, 0x52, 0x2d, 0x54, 0x68, 0x65, 0x42, 0x65, 0x73, 0x74 };
                    this.Password = Encoding.Unicode.GetString(ProtectedData.Unprotect(userSession, entropy, DataProtectionScope.CurrentUser));
                }
                catch
                {
                    this.Password = string.Empty;

//                    this.ReportMessage(CommonRes.FailedToDecryptPasswordError);
                }
            }
        }

        private static bool CanLogIn() 
        {
            return true;
        }

        #endregion Privates


        #region properties data

        public static readonly PropertyData PortProperty =
            RegisterProperty<LoginViewModel, string>(loginViewModel => loginViewModel.Port, () => string.Empty);

        public static readonly PropertyData HostNameProperty =
            RegisterProperty<LoginViewModel, string>(loginViewModel => loginViewModel.HostName, () => string.Empty);

        public static readonly PropertyData UserNameProperty =
            RegisterProperty<LoginViewModel, string>(loginViewModel => loginViewModel.UserName, () => string.Empty);

        public static readonly PropertyData PasswordProperty =
            RegisterProperty<LoginViewModel, string>(loginViewModel => loginViewModel.Password, () => string.Empty);

        public static readonly PropertyData InformationProperty =
            RegisterProperty<LoginViewModel, string>(loginViewModel => loginViewModel.Information, () => string.Empty);

        public static readonly PropertyData HasInformationProperty =
            RegisterProperty<LoginViewModel, bool>(loginViewModel => loginViewModel.HasInformation, () => false);

        public static readonly PropertyData SavePasswordProperty =
            RegisterProperty<LoginViewModel, bool>(loginViewModel => loginViewModel.SavePassword, () => false);

        #endregion properties data
    }
}
