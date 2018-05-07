// Created by Laxale 19.10.2016
//
//

using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows;
using Freengy.Common.Enums;
using Freengy.Common.Models;
using Freengy.Common.Helpers;
using Freengy.Common.Extensions;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models.Readonly;
using Freengy.Base.Helpers;
using Freengy.Base.Settings;
using Freengy.Base.Messages;
using Freengy.Base.Extensions;
using Freengy.Base.ViewModels;
using Freengy.Base.Interfaces;
using Freengy.Base.Helpers.Commands;
using Freengy.Networking.Interfaces;
using Freengy.Networking.Constants;
using Freengy.UI.Views;
using Freengy.UI.Windows;
using Freengy.Base.Windows;

using NLog;

using Catel.IoC;

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
        private static readonly Configuration uiConfiguration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

        private readonly string lastLoggedName;
        private readonly ILoginController loginController;
        private readonly string lastLoggedParameterName = "LastLoggedUserName";

        private bool mustSavePassword;
        
        private UserAccount CurrentAccount => loginController.MyAccountState?.Account;


        public LoginViewModel() 
        {
            lastLoggedName = uiConfiguration.AppSettings.Settings[lastLoggedParameterName].Value;
            loginController = ServiceLocatorProperty.ResolveType<ILoginController>();

            Mediator.SendMessage(new MessageInitializeModelRequest(this, "Loading"));

            CheckServerAsync();
        }


        /// <summary>
        /// Command to invoke login process.
        /// </summary>
        public MyCommand CommandLogin { get; private set; }

        /// <summary>
        /// Create new user account command.
        /// </summary>
        public MyCommand CommandCreateAccount { get; private set; }

        /// <summary>
        /// Recover user password command.
        /// </summary>
        public MyCommand CommandRecoverPassword { get; private set; }


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


        public bool MustSavePassword 
        {
            get => mustSavePassword;

            set
            {
                if (mustSavePassword == value) return;

                mustSavePassword = value;

                OnPropertyChanged();
            }
        }
        
        
        protected override void SetupCommands() 
        {
            CommandCreateAccount = new MyCommand(CreateAccountImpl);
            CommandRecoverPassword = new MyCommand(RecoverPasswordImpl);
            CommandLogin = new MyCommand(CommandLoginImpl, CanLogIn);
        }

        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            LoadCredsFromSettings();
        }


        #region Privates

        private async void CheckServerAsync() 
        {
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(Url.Http.HelloUrl);

                    bool isOnline = response.StatusCode == HttpStatusCode.OK;

                    Mediator.SendMessage(new MessageServerOnlineStatus(isOnline));
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "Failed to check server");
                    Mediator.SendMessage(new MessageServerOnlineStatus(false));
                }
            }
        }

        private void CreateAccountImpl() 
        {
            var viewModel = new RegistrationViewModel();
            var view = new RegistrationView
            {
                Margin = new Thickness(8)
            };
            
            var win = new EmptyCustomToolWindow
            {
                Title = "Register new account",
                MainContent = view,
                DataContext = viewModel,
                Height = 300,
                MaxHeight = 400,
                Width = 400,
                MaxWidth = 500
            };

            win.ShowDialog();
        }

        private void RecoverPasswordImpl() 
        {
            var model = new RecoverPasswordViewModel();
            var win = new RecoverPasswordWindow { DataContext = model };

            win.ShowDialog();
        }

        private bool CanLogIn() 
        {
            bool canLogin =
                !string.IsNullOrWhiteSpace(UserName) &&
                Password?.Length >= Account.MinimumPasswordLength;

            return canLogin;
        }

        private void LoadCredsFromSettings() 
        {
            var settings = AppSettings.Instance;

            UserName = CurrentAccount?.Name ?? lastLoggedName;

            MustSavePassword = settings.SavePassword;

            if (!string.IsNullOrWhiteSpace(settings.LastUserSession) && MustSavePassword)
            {
                try
                {
                    var userSession = Convert.FromBase64String(settings.LastUserSession);
                    var entropy = new byte[] { 0x50, 0x50, 0x52, 0x2d, 0x54, 0x68, 0x65, 0x42, 0x65, 0x73, 0x74 };
                    //Password = Encoding.Unicode.GetString(ProtectedData.Unprotect(userSession, entropy, DataProtectionScope.CurrentUser));
                }
                catch
                {
                    ReportMessage("Failed to decrypt password");
                }
            }
        }

        private async void CommandLoginImpl() 
        {
            await TaskerWrapper.Wrap(LoginAction, LoginContinuator);
        }

        private void LoginAction() 
        {
            SetBusySilent();

            LoginModel loginModel = GetCurrentLoginParameters();
            Result<AccountStateModel> result = loginController.LogIn(loginModel);

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
                uiConfiguration.AppSettings.Settings.Remove(lastLoggedParameterName);
                uiConfiguration.AppSettings.Settings.Add(lastLoggedParameterName, UserName);
                uiConfiguration.Save();

                ClearInformation();
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

            UserAccountModel CreateEmptyNew()
            {
                return new UserAccountModel
                {
                    Id = Guid.Empty,
                    Name = UserName
                };
            }

            if (CurrentAccount != null)
            {
                parameters.Account = UserName == CurrentAccount?.Name ? CurrentAccount.ToModel() : CreateEmptyNew();
            }
            else
            {
                parameters.Account = CreateEmptyNew();
            }

            parameters.Password = this.Password;

            return parameters;
        }

        #endregion Privates
    }
}