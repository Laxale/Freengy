﻿// Created by Laxale 12.11.2016
//
//

using System.Windows;
using System.Threading;
using System.Threading.Tasks;

using Freengy.Common.Helpers.Result;
using Freengy.Base.Helpers;
using Freengy.Common.Models;
using Freengy.Base.ViewModels;
using Freengy.Networking.Interfaces;
using Freengy.Base.Messages;

using Catel.IoC;
using Catel.Services;


namespace Freengy.UI.ViewModels 
{
    using Freengy.Base.Helpers.Commands;


    internal class RegistrationViewModel : CredentialViewModel 
    {
        private readonly ILoginController loginController;

        private bool registered;
        private bool isCodeSent;


        public RegistrationViewModel() 
        {
            loginController = ServiceLocatorProperty.ResolveType<ILoginController>();

            Mediator.SendMessage(new MessageInitializeModelRequest(this, "Loading registration"));
        }


        public MyCommand CommandRegister { get; private set; }

        public MyCommand<Window> CommandFinish { get; private set; }


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

        /// <inheritdoc />
        protected override bool IsEmailMandatory { get; } = true;


        protected override void SetupCommands() 
        {
            CommandFinish = new MyCommand<Window>(win => win.Close(), CanFinish);
            CommandRegister = new MyCommand(CommandRegisterImpl, CanCallRegistration);
        }


        private void CommandRegisterImpl() 
        {
            Result<UserAccount> result = null;

            void Method()
            {
                SetBusyState("Registering");
                Thread.Sleep(1000);
                result = loginController.Register(UserName);
            }

            void Continuator()
            {
                ClearBusyState();

                if (result.Failure)
                {
                    ReportMessage(result.Error.Message);
                }
                else
                {
                    Registered = true;
                }
            }

            Task.Run(() => Method()).ContinueWith(task => Continuator());
        }

        private bool CanCallRegistration() 
        {
            return !Registered && !HasValidationErrors;
        }

        private bool CanFinish(Window registrationWindow) 
        {
            return Registered;
        }
    }
}