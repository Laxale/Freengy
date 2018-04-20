// Created by Laxale 19.10.2016
//
//

using Freengy.Base.ErrorReasons;
using Freengy.Base.Messages;
using Freengy.Base.ViewModels;
using Freengy.Common.Helpers.Result;
using Freengy.UI.Helpers;
using Freengy.UI.Messages;
using Freengy.Base.Helpers;
using Freengy.Settings.Views;


namespace Freengy.UI.ViewModels 
{
    public class ShellViewModel : WaitableViewModel 
    {
        public ShellViewModel() 
        {
            Mediator.SendMessage(new MessageInitializeModelRequest(this, "Loading shell"));
        }


        protected override void SetupCommands() 
        {
            CommandLogOut = new MyCommand(LogOutImpl);
            CommandShowSettings = new MyCommand(CommandShowSettingsImpl);
        }

        
        /// <summary>
        /// Command to log the user out.
        /// </summary>
        public MyCommand CommandLogOut { get; private set; }

        /// <summary>
        /// Command to show app settings.
        /// </summary>
        public MyCommand CommandShowSettings { get; private set; }


        private void LogOutImpl(object notUsed) 
        {
            void LogoutInvoker()
            {
                SetBusySilent();

                Result logoutResult = new LogOutController(ServiceLocatorProperty).LogOut();

                if (logoutResult.Failure)
                {
                    if (logoutResult.Error is UserCancelledReason)
                    {
                        return;
                    }
                }

                Mediator.SendMessage<MessageBase>(new MessageLogoutRequest());
            }

            TaskerWrapper.Wrap(LogoutInvoker, task => ClearBusyState());
        }

        private void CommandShowSettingsImpl(object notUsed) 
        {
            new SettingsWindow().ShowDialog();
        }
    }
}