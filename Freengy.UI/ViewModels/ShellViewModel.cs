// Created by Laxale 19.10.2016
//
//

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

using Freengy.Base.ErrorReasons;
using Freengy.Base.Messages;
using Freengy.Base.ViewModels;
using Freengy.Base.Helpers.Commands;
using Freengy.Base.Windows;
using Freengy.Common.Helpers.Result;
using Freengy.UI.Helpers;
using Freengy.UI.Messages;
using Freengy.Base.Helpers;
using Freengy.Base.Interfaces;
using Freengy.Settings.Views;
using Freengy.UI.Views;

using Catel.IoC;

using KeyEventArgs = System.Windows.Input.KeyEventArgs;


namespace Freengy.UI.ViewModels 
{
    /// <summary>
    /// Вьюмодель для <see cref="ShellView"/>.
    /// </summary>
    public class ShellViewModel : WaitableViewModel 
    {
        private readonly ICurtainedExecutor curtainedExecutor;


        public ShellViewModel() 
        {
            curtainedExecutor = ServiceLocatorProperty.ResolveType<ICurtainedExecutor>();
            Mediator.SendMessage(new MessageInitializeModelRequest(this, "Loading shell"));
        }


        protected override void SetupCommands() 
        {
            CommandLogOut = new MyCommand(LogOutImpl);
            CommandShowSettings = new MyCommand(CommandShowSettingsImpl);
            CommandShowAlbums = new MyCommand(ShowAlbumsImpl);
        }

        
        /// <summary>
        /// Command to log the user out.
        /// </summary>
        public MyCommand CommandLogOut { get; private set; }

        /// <summary>
        /// Command to show app settings.
        /// </summary>
        public MyCommand CommandShowSettings { get; private set; }

        /// <summary>
        /// Command to show my albums.
        /// </summary>
        public MyCommand CommandShowAlbums { get; private set; }


        private void LogOutImpl() 
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

        private void CommandShowSettingsImpl() 
        {
            curtainedExecutor.ExecuteWithCurtain
            (
                KnownCurtainedIds.MainWindowId,
                () => new SettingsWindow().ShowDialog()
            );
        }

        private void ShowAlbumsImpl() 
        {
            var content = new AlbumsView();
            var hostWindow = new EmptyCustomToolWindow
            {
                Title = "My albums",
                MainContent = content,
                Height = 400,
                Width = 600,
                MaxHeight = 800,
                MaxWidth = 1000
            };

            void OnClosed(object sender, EventArgs args)
            {
                hostWindow.Closed -= OnClosed;
                hostWindow.KeyDown -= OnWindowKeyPressed;
                (content.DataContext as IDisposable)?.Dispose();
            }

            void OnWindowKeyPressed(object sender, KeyEventArgs args)
            {
                Mediator.SendMessage(new MessageParentWindowKeyDown((Window) sender, args));
            }

            void TryShowWindow()
            {
                try
                {
                    curtainedExecutor.ExecuteWithCurtain
                    (
                        KnownCurtainedIds.MainWindowId,
                        () => hostWindow.ShowDialog()
                    );
                }
                catch (Exception ex)
                {
                    ReportMessage(ex.Message);
                    //throw;
                }
            }

            hostWindow.Closed += OnClosed;
            hostWindow.KeyDown += OnWindowKeyPressed;

            TryShowWindow();
        }
    }
}