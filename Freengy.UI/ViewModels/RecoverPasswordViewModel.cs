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
    using Freengy.Base.Helpers;
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
            this.CommandFinish = new Command<Window>(this.CommandFinishImpl, this.CanFinish);
            this.CommandChangePassword = new Command(this.ChangePasswordImpl, this.CanChangePassword);
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults) 
        {
            base.ValidateFields(validationResults);

            this.CheckPasswordEmptiness(validationResults);
            this.CheckPasswordsEquality(validationResults);
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
            get { return (string)this.GetValue(RecoverPasswordViewModel.ValidationCodeProperty); }

            set
            {
                this.SetValue(RecoverPasswordViewModel.ValidationCodeProperty, value);

                this.IsCodeValid = this.secureDecorator.GetSecureString().Equals(this.ValidationCode);
            }
        }
        public string PasswordConfirmation 
        {
            get { return (string)GetValue(PasswordConfirmationProperty); }
            private set { SetValue(PasswordConfirmationProperty, value); }
        }


        public static readonly PropertyData IsCodeSentProperty =
            ModelBase.RegisterProperty<RecoverPasswordViewModel, bool>(recViewModel => recViewModel.IsCodeSent, () => false);

        public static readonly PropertyData IsCodeValidProperty =
            ModelBase.RegisterProperty<RecoverPasswordViewModel, bool>(recViewModel => recViewModel.IsCodeValid, () => false);

        public static readonly PropertyData IsPasswordChangedProperty =
            ModelBase.RegisterProperty<RecoverPasswordViewModel, bool>(recViewModel => recViewModel.IsPasswordChanged, () => false);

        public static readonly PropertyData ValidationCodeProperty =
            ModelBase.RegisterProperty<RecoverPasswordViewModel, string>(recViewModel => recViewModel.ValidationCode, () => string.Empty);

        public static readonly PropertyData PasswordConfirmationProperty =
            ModelBase.RegisterProperty<RecoverPasswordViewModel, string>(viewModel => viewModel.PasswordConfirmation, () => string.Empty);

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
                        MessageBox.Show(parentTask.Exception.GetReallyRootException().Message, "Failed to send message");
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
            return 
                this.IsCodeValid && 
                !this.IsPasswordChanged &&
                !string.IsNullOrWhiteSpace(this.Password) &&
                this.Password == this.PasswordConfirmation &&
                !string.IsNullOrWhiteSpace(this.PasswordConfirmation) &&
                Account.IsGoodPassword(this.Password);
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

        private void CheckPasswordEmptiness(List<IFieldValidationResult> validationResults) 
        {
            if (string.IsNullOrWhiteSpace(this.Password))
            {
                validationResults.Add
                (
                    FieldValidationResult.CreateError(PasswordProperty, CommonRes.ValueCannotBeEmptyFormat, CommonRes.PasswordText)
                );
            }

            if (string.IsNullOrWhiteSpace(this.PasswordConfirmation))
            {
                validationResults.Add
                (
                    FieldValidationResult.CreateError(PasswordConfirmationProperty, CommonRes.ValueCannotBeEmptyFormat, CommonRes.PasswordText)
                );
            }
        }

        private void CheckPasswordsEquality(List<IFieldValidationResult> validationResults) 
        {
            if (this.Password == this.PasswordConfirmation) return;

            validationResults.Add(FieldValidationResult.CreateError(CredentialViewModel.PasswordProperty, LocalRes.PasswordsMustBeEqualText));
            validationResults.Add(FieldValidationResult.CreateError(RecoverPasswordViewModel.PasswordConfirmationProperty, LocalRes.PasswordsMustBeEqualText));
        }
    }
}