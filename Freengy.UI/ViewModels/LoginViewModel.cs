// Created by Laxale 19.10.2016
//
//

using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;

using Freengy.Common.Enums;
using Freengy.Common.Models;
using Freengy.Base.Settings;
using Freengy.Base.Extensions;
using Freengy.Base.ViewModels;
using Freengy.Base.Interfaces;
using Freengy.UI.Views;
using Freengy.Networking.Interfaces;
using Freengy.Networking.Constants;
using Freengy.Common.Helpers.Result;

using Catel.IoC;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using Freengy.Common.Extensions;
using NLog;

using Prism.Regions;

using CommonRes = Freengy.CommonResources.StringResources;


namespace Freengy.UI.ViewModels 
{   
    /// <summary>
    /// ViewModel for <see cref="LoginView"/>.
    /// </summary>
    internal class LoginViewModel : CredentialViewModel, INavigationAware 
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IPleaseWaitService waiter;
        private readonly IAccountManager accountManager;
        private readonly ILoginController loginController;

        private UserAccount CurrentAccount => loginController.CurrentAccount;


        public LoginViewModel() 
        {
            // need to resolve it by interface to avoid knowledge about concrete implementer
            waiter = serviceLocator.ResolveType<IPleaseWaitService>();
            accountManager = serviceLocator.ResolveType<IAccountManager>();
            loginController = serviceLocator.ResolveType<ILoginController>();

            CheckServerAsync();
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

        public bool SavePassword 
        {
            get => (bool)GetValue(SavePasswordProperty);

            set => SetValue(SavePasswordProperty, value);
        }
        
        public bool IsServerAvailable 
        {
            get => (bool)GetValue(IsServerAvailableProperty);

            private set => SetValue(IsServerAvailableProperty, value);
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

        #endregion Overrides


        #region Privates

        private async void CheckServerAsync() 
        {
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(Url.Http.HelloUrl);

                    IsServerAvailable = response.StatusCode == HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "Failed to check server");
                    IsServerAvailable = false;
                }
            }
        }

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
            var lastLoggedResult = accountManager.GetLastLoggedIn();
            var settings = AppSettings.Instance;

            if (lastLoggedResult.Success && lastLoggedResult.Value != null)
            {
                UserName = CurrentAccount?.Name ?? lastLoggedResult.Value.Name;
            }

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

                    ReportMessage("Failed to decrypt password");
                }
            }
        }

        private async void CommandLoginImpl() 
        {
            await taskWrapper.Wrap(LoginAction, LoginContinuator);
        }

        private void LoginAction() 
        {
            SetBusySilent();

            LoginModel loginModel = GetCurrentLoginParameters();
            Result<AccountState> result = loginController.LogIn(loginModel);

            if (result.Failure)
            {
                ReportMessage(result.Error.Message);
            }
            else if(result.Value.OnlineStatus != AccountOnlineStatus.Online)
            {
                ReportMessage($"Failed to log in: { result.Value }");
            }
            else
            {
                accountManager.SaveLastLoggedIn(loginController.CurrentAccount);
                ReportMessage(null);
            }
        }

        private void LoginContinuator(Task parentTask) 
        {
            ClearBusyState();

            if (parentTask.Exception != null)
            {
                ReportMessage(parentTask.Exception.GetReallyRootException().Message);
            }
        }

        private LoginModel GetCurrentLoginParameters() 
        {
            var parameters = new LoginModel();
            
            if (CurrentAccount != null)
            {
                parameters.Account = 
                    UserName == CurrentAccount?.Name ? 
                        CurrentAccount.ToModel() : 
                        new UserAccountModel { Name = UserName };
            }
            else
            {
                parameters.Account = new UserAccountModel { Name = UserName };
            }

            parameters.PasswordHash = Password; // TODO: waaaat?

            return parameters;
        }

        #endregion Privates


        #region properties data

        public static readonly PropertyData SavePasswordProperty =
            RegisterProperty<LoginViewModel, bool>(loginViewModel => loginViewModel.SavePassword, () => false);

        public static readonly PropertyData IsServerAvailableProperty =
            RegisterProperty<LoginViewModel, bool>(loginViewModel => loginViewModel.IsServerAvailable, () => false);

        #endregion properties data
    }
}