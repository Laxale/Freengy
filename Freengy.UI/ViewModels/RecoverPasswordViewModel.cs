// Created by Laxale 13.11.2016
//
//

using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;

using Freengy.Unsafe;
using Freengy.Base.Helpers;
using Freengy.Base.ViewModels;
using Freengy.Base.Extensions;

using Catel.Data;
using Catel.MVVM;

using ActiveUp.Net.Mail;

using LocalRes = Freengy.UI.Properties.Resources;
using CommonRes = Freengy.CommonResources.StringResources;


namespace Freengy.UI.ViewModels 
{
    public class RecoverPasswordViewModel : CredentialViewModel 
    {
        private const int ValidationCodeMinimum = 10000;
        private const int ValidationCodeMaximum = 90000;
        private readonly SecureStringDecorator secureDecorator = new SecureStringDecorator();


        #region override

        protected override bool IsEmailMandatory => true;

        protected override void SetupCommands() 
        {
            CommandSendCode = new Command(SendCodeImpl, CanSendCode);
            CommandFinish = new Command<Window>(CommandFinishImpl, CanFinish);
            CommandChangePassword = new Command(ChangePasswordImpl, CanChangePassword);
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults) 
        {
            base.ValidateFields(validationResults);

            CheckPasswordEmptiness(validationResults);
            CheckPasswordsEquality(validationResults);
            base.ValidatePassword(validationResults);
        }

        protected override void ValidatePassword(List<IFieldValidationResult> validationResults) { }

        #endregion override


        #region commands

        public Command CommandSendCode { get; private set; }

        public Command CommandChangePassword{ get; private set; }

        public Command<Window> CommandFinish { get; private set; }

        #endregion commands


        #region properties

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

        /// <summary>
        /// Is being compared with one sent to provided email
        /// </summary>
        public string ValidationCode 
        {
            get { return (string)GetValue(ValidationCodeProperty); }

            set
            {
                SetValue(ValidationCodeProperty, value);

                IsCodeValid = secureDecorator.GetSecureString().Equals(ValidationCode);
            }
        }
        public string PasswordConfirmation 
        {
            get { return (string)GetValue(PasswordConfirmationProperty); }
            set { SetValue(PasswordConfirmationProperty, value); }
        }


        public static readonly PropertyData IsCodeSentProperty =
            RegisterProperty<RecoverPasswordViewModel, bool>(recViewModel => recViewModel.IsCodeSent, () => false);

        public static readonly PropertyData IsCodeValidProperty =
            RegisterProperty<RecoverPasswordViewModel, bool>(recViewModel => recViewModel.IsCodeValid, () => false);

        public static readonly PropertyData IsPasswordChangedProperty =
            RegisterProperty<RecoverPasswordViewModel, bool>(recViewModel => recViewModel.IsPasswordChanged, () => false);

        public static readonly PropertyData ValidationCodeProperty =
            RegisterProperty<RecoverPasswordViewModel, string>(recViewModel => recViewModel.ValidationCode, () => string.Empty);

        public static readonly PropertyData PasswordConfirmationProperty =
            RegisterProperty<RecoverPasswordViewModel, string>(viewModel => viewModel.PasswordConfirmation, () => string.Empty);

        #endregion properties


        private async void SendCodeImpl() 
        {
            void SendAction()
            {
                SetBusy("Sending code");
                IsCodeSent = false;

                GenerateNewValidationCode();

                SendCode();
            }

            void SendContinuator(Task parentTask)
            {
                ClearBusyState();

                if (parentTask.Exception != null)
                {
                    MessageBox.Show(parentTask.Exception.GetReallyRootException().Message, "Failed to send message");
                    return;
                }

                IsCodeSent = true;
            }

            await taskWrapper.Wrap(SendAction, SendContinuator);
        }
        private void SendCode() 
        {
            string body = string.Format(LocalRes.PasswordRecoveryEmailBodyFormat, secureDecorator.GetSecureString());

            var message = new SmtpMessage
            {
                BodyText = { Text = body },
                From = { Email = CommonRes.ProjectFakeEmail },
                Subject = LocalRes.PasswordRecoveryEmailSubjectText,
            };

            message.To.Add(Email);

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
            IsPasswordChanged = true;
        }
        private bool CanChangePassword() 
        {
            return 
                IsCodeValid && 
                !IsPasswordChanged &&
                !string.IsNullOrWhiteSpace(Password) &&
                Password == PasswordConfirmation &&
                !string.IsNullOrWhiteSpace(PasswordConfirmation) &&
                Account.IsGoodPassword(Password);
        }

        private void CommandFinishImpl(Window registrationWindow)
        {
            // send message
            registrationWindow.Close();
        }
        private bool CanFinish(Window registrationWindow) 
        {
            return IsPasswordChanged;
        }

        private void GenerateNewValidationCode() 
        {
            Helpers.UiDispatcher.Invoke
            (
               () =>
               {
                   int randomCode = new Random(new Random().Next()).Next(ValidationCodeMinimum, ValidationCodeMaximum);

                   secureDecorator.SetSecureString(randomCode.ToString());
               }
            );
        }

        private void CheckPasswordEmptiness(List<IFieldValidationResult> validationResults) 
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                validationResults.Add
                (
                    FieldValidationResult.CreateError(PasswordProperty, CommonRes.ValueCannotBeEmptyFormat, CommonRes.PasswordText)
                );
            }

            if (string.IsNullOrWhiteSpace(PasswordConfirmation))
            {
                validationResults.Add
                (
                    FieldValidationResult.CreateError(PasswordConfirmationProperty, CommonRes.ValueCannotBeEmptyFormat, CommonRes.PasswordText)
                );
            }
        }

        private void CheckPasswordsEquality(List<IFieldValidationResult> validationResults) 
        {
            if (Password == PasswordConfirmation) return;

            validationResults.Add(FieldValidationResult.CreateError(PasswordProperty, LocalRes.PasswordsMustBeEqualText));
            validationResults.Add(FieldValidationResult.CreateError(PasswordConfirmationProperty, LocalRes.PasswordsMustBeEqualText));
        }
    }
}