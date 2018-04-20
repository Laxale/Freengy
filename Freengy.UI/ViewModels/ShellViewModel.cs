// Created by Laxale 19.10.2016
//
//

using System;
using System.Threading.Tasks;
using System.Windows;

using Freengy.Base.ErrorReasons;
using Freengy.Base.Messages;
using Freengy.Base.ViewModels;
using Freengy.Common.Helpers.Result;
using Freengy.Networking.Interfaces;
using Freengy.Settings.ViewModels;
using Freengy.UI.Helpers;
using Freengy.UI.Messages;

using Catel.IoC;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;


namespace Freengy.UI.ViewModels 
{
    public class ShellViewModel : WaitableViewModel 
    {
        protected override void SetupCommands() 
        {
            CommandLogOut = new Command(LogOutImpl);
            CommandShowSettings = new Command(CommandShowSettingsImpl);
        }

        
        /// <summary>
        /// Command to log the user out.
        /// </summary>
        public Command CommandLogOut { get; private set; }

        /// <summary>
        /// Command to show app settings.
        /// </summary>
        public Command CommandShowSettings { get; private set; }


        public bool ShowVisualHints 
        {
            get => (bool)GetValue(ShowVisualHintsProperty);
            private set => SetValue(ShowVisualHintsProperty, value);
        }

        public static readonly PropertyData ShowVisualHintsProperty =
            RegisterProperty<ShellViewModel, bool>(viewModel => viewModel.ShowVisualHints, () => true);


        private void LogOutImpl() 
        {
            void LogoutInvoker()
            {
                SetBusySilent();

                Result logoutResult = new LogOutController(serviceLocator).LogOut();

                if (logoutResult.Failure)
                {
                    if (logoutResult.Error is UserCancelledReason)
                    {
                        return;
                    }
                }

                messageMediator.SendMessage<MessageBase>(new MessageLogoutRequest());
            }

            taskWrapper.Wrap(LogoutInvoker, task => ClearBusyState());
        }

        private async void CommandShowSettingsImpl() 
        {
            var settingsViewModel = serviceLocator.ResolveType<SettingsViewModel>();

            bool? result = await uiVisualizer.ShowDialogAsync(settingsViewModel);

            ShowVisualHints = false;
        }
    }
}