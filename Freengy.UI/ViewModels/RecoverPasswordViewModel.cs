// Created by Laxale 13.11.2016
//
//


namespace Freengy.UI.ViewModels 
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Freengy.Unsafe;
    using Freengy.Base.ViewModels;
    using Freengy.Base.Extensions;
    
    using Catel.Data;
    using Catel.MVVM;
    
    using ActiveUp.Net.Mail;
    
    using LocalRes = Freengy.UI.Properties.Resources;
    using CommonRes = Freengy.CommonResources.StringResources;


    public class RecoverPasswordViewModel : CredentialViewModel 
    {
        #region vars

        private const int ValidationCodeMinimum = 10000;
        private const int ValidationCodeMaximum = 90000;
        private readonly SecureStringDecorator secureDecorator = new SecureStringDecorator();

        #endregion vars


        #region override

        protected override bool IsEmailMandatory => true;

        protected override void SetupCommands() 
        {
            this.CommandSendCode = new Command(this.SendCodeImpl, this.CanSendCode);
            this.CommandChangePassword = new Command(this.ChangePasswordImpl, this.CanChangePassword);
            this.CommandFinish = new Command<Window>(this.CommandFinishImpl, this.CanFinish);
        }

        protected override void ValidatePassword(List<IFieldValidationResult> validationResults) { }

        #endregion override


        #region commands

        public Command CommandSendCode { get; private set; }

        public Command CommandChangePassword{ get; private set; }

        public Command<Window> CommandFinish { get; private set; }

        #endregion commands


        #region properties

        /// <summary>
        /// Is being compared with one sent to provided email
        /// </summary>
        public string ValidationCode
        {
            get { return (string)GetValue(ValidationCodeProperty); }

            set
            {
                SetValue(ValidationCodeProperty, value);

                this.IsCodeValid = this.secureDecorator.GetSecureString().Equals(this.ValidationCode);
            }
        }

        public bool IsCodeSent 
        {
            get { return (bool)GetValue(IsCodeSentProperty); }

            private set { SetValue(IsCodeSentProperty, value); }
        }

        public bool IsCodeValid 
        {
            get { return (bool)GetValue(IsCodeValidProperty); }

            private set { SetValue(IsCodeValidProperty, value); }
        }

        public bool IsPasswordChanged 
        {
            get { return (bool)GetValue(IsPasswordChangedProperty); }

            private set { SetValue(IsPasswordChangedProperty, value); }
        }

        public static readonly PropertyData IsCodeSentProperty =
            ModelBase.RegisterProperty<RecoverPasswordViewModel, bool>(recViewModel => recViewModel.IsCodeSent, () => false);
        public static readonly PropertyData IsPasswordChangedProperty =
            ModelBase.RegisterProperty<RecoverPasswordViewModel, bool>(recViewModel => recViewModel.IsPasswordChanged, () => false);
        public static readonly PropertyData IsCodeValidProperty =
            ModelBase.RegisterProperty<RecoverPasswordViewModel, bool>(recViewModel => recViewModel.IsCodeValid, () => false);
        public static readonly PropertyData ValidationCodeProperty =
            RegisterProperty<RecoverPasswordViewModel, string>(recViewModel => recViewModel.ValidationCode, () => string.Empty);

        #endregion properties


        private async void SendCodeImpl() 
        {
            Action sendAction =
                () =>
                {
                    base.IsWaiting = true;
                    this.IsCodeSent = false;

                    this.GenerateNewValidationCode();

                    this.SendCode();
                };

            Action<Task> sendContinuator =
                parentTask =>
                {
                    base.IsWaiting = false;

                    if (parentTask.Exception != null)
                    {
                        System.Windows.MessageBox.Show(parentTask.Exception.GetReallyRootException().Message, "Failed to send message");
                        return;
                    }

                    this.IsCodeSent = true;
                };

            await base.taskWrapper.Wrap(sendAction, sendContinuator);
        }
        private void SendCode() 
        {
            string body = string.Format(LocalRes.PasswordRecoveryEmailBodyFormat, this.secureDecorator.GetSecureString());

            var message = new SmtpMessage
            {
                BodyText = { Text = body },
                From = { Email = CommonRes.ProjectFakeEmail },
                Subject = LocalRes.PasswordRecoveryEmailSubjectText,
            };

            message.To.Add(this.Email);

            message.DirectSend();
        }
        private bool CanSendCode() 
        {
            List<IFieldValidationResult> validationResults = new List<IFieldValidationResult>();
            base.ValidateFields(validationResults);

            bool canTryRegister = !validationResults.Any();

            return canTryRegister;
        }

        private void ChangePasswordImpl() 
        {
            this.IsPasswordChanged = true;
        }
        private bool CanChangePassword() 
        {
            return this.IsCodeValid && !this.IsPasswordChanged;
        }

        private void CommandFinishImpl(Window registrationWindow)
        {
            // send message
            registrationWindow.Close();
        }
        private bool CanFinish(Window registrationWindow) 
        {
            return this.IsPasswordChanged;
        }

        private void GenerateNewValidationCode() 
        {
            Helpers.UiDispatcher.Invoke
            (
               () =>
               {
                   int randomCode = new Random(new Random().Next()).Next(ValidationCodeMinimum, ValidationCodeMaximum);

                   this.secureDecorator.SetSecureString(randomCode.ToString());
               }
            );
        }
    }
}