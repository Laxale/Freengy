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
using Freengy.Common.Extensions;

using Catel.Data;
using Catel.MVVM;

using ActiveUp.Net.Mail;
using LocalRes = Freengy.UI.Properties.Resources;
using CommonRes = Freengy.CommonResources.StringResources;


namespace Freengy.UI.ViewModels 
{
    using Freengy.Base.Helpers.Commands;


    public class RecoverPasswordViewModel : CredentialViewModel 
    {
        private const int ValidationCodeMinimum = 10000;
        private const int ValidationCodeMaximum = 90000;
        private readonly SecureStringDecorator secureDecorator = new SecureStringDecorator();


        public MyCommand CommandSendCode { get; private set; }

        public MyCommand CommandChangePassword { get; private set; }

        public MyCommand<Window> CommandFinish { get; private set; }


        protected override bool IsEmailMandatory => true;

        protected override void SetupCommands() 
        {
            CommandSendCode = new MyCommand(SendCodeImpl, CanSendCode);
            CommandFinish = new MyCommand<Window>(window => window.Close(), CanFinish);
            CommandChangePassword = new MyCommand(ChangePasswordImpl, CanChangePassword);
        }


        #region properties


        private bool isCodeSent;

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


        private bool isCodeValid;

        public bool IsCodeValid 
        {
            get => isCodeValid;

            set
            {
                if (isCodeValid == value) return;

                isCodeValid = value;

                OnPropertyChanged();
            }
        }


        private bool isPasswordChanged;

        public bool IsPasswordChanged 
        {
            get => isPasswordChanged;

            set
            {
                if (isPasswordChanged == value) return;

                isPasswordChanged = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Is being compared with one sent to provided email
        /// </summary>

        private string validationCode;

        public string ValidationCode
        {
            get => validationCode;

            set
            {
                if (validationCode == value) return;

                validationCode = value;

                OnPropertyChanged();
            }
        }


        private string passwordConfirmation;

        public string PasswordConfirmation 
        {
            get => passwordConfirmation;

            set
            {
                if (passwordConfirmation == value) return;

                passwordConfirmation = value;

                OnPropertyChanged();
            }
        }

        #endregion properties


        private async void SendCodeImpl() 
        {
            void SendAction()
            {
                SetBusyState("Sending code");
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

            await TaskerWrapper.Wrap(SendAction, SendContinuator);
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
            //base.ValidateFields(validationResults);

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
                Password.Length > 0 &&
                //Password == PasswordConfirmation &&
                Password == null && // TODO: return confirmation check!
                !string.IsNullOrWhiteSpace(PasswordConfirmation) &&
                Account.IsGoodPassword(Password);
        }

        private bool CanFinish(Window registrationWindow) 
        {
            return IsPasswordChanged;
        }

        private void GenerateNewValidationCode() 
        {
            Helpers.UiDispatcher.Invoke(() =>
            {
                int randomCode = new Random(new Random().Next()).Next(ValidationCodeMinimum, ValidationCodeMaximum);

                secureDecorator.SetSecureString(randomCode.ToString());
            });
        }

        private void CheckPasswordEmptiness(List<IFieldValidationResult> validationResults) 
        {
            if (Password.Length < 1)
            {
                validationResults.Add
                (
                    null
                    //FieldValidationResult.CreateError(PasswordProperty, CommonRes.ValueCannotBeEmptyFormat, CommonRes.PasswordText)
                );
            }

            if (string.IsNullOrWhiteSpace(PasswordConfirmation))
            {
                validationResults.Add
                (
                    null
                    //FieldValidationResult.CreateError(PasswordConfirmationProperty, CommonRes.ValueCannotBeEmptyFormat, CommonRes.PasswordText)
                );
            }
        }

        private void CheckPasswordsEquality(List<IFieldValidationResult> validationResults) 
        {
            //if (Password == PasswordConfirmation) return;
            if (Password == null) return;

            //validationResults.Add(FieldValidationResult.CreateError(PasswordProperty, LocalRes.PasswordsMustBeEqualText));
            //validationResults.Add(FieldValidationResult.CreateError(PasswordConfirmationProperty, LocalRes.PasswordsMustBeEqualText));
        }
    }
}