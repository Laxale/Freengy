// Created by Laxale 13.11.2016
//
//


namespace Freengy.UI.ViewModels 
{
    using System;
    using System.Linq;
    using System.Net;
    //using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Freengy.Unsafe;
    using Freengy.Base.ViewModels;
    using Freengy.Base.Extensions;
    
    using Catel.IoC;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;

    using ActiveUp.Net.Mail;
    
    using LocalRes = Freengy.UI.Properties.Resources;
    using CommonRes = Freengy.CommonResources.StringResources;


    public class RecoverPasswordViewModel : CredentialViewModel 
    {
        private const int ValidationCodeMinimum = 10000;
        private const int ValidationCodeMaximum = 90000;
        private readonly SecureStringDecorator secureDecorator = new SecureStringDecorator();


        #region override

        protected override bool IsEmailMandatory => true;

        protected override void SetupCommands() 
        {
            this.CommandSendCode = new Command(this.SendCodeImpl, this.CanSendCode);
        }

        protected override void ValidatePassword(List<IFieldValidationResult> validationResults) { }

        #endregion override


        public Command CommandSendCode { get; private set; }


        #region properties

        /// <summary>
        /// Is being compared with one sent to provided email
        /// </summary>
        public string ValidationCode
        {
            get { return (string)GetValue(ValidationCodeProperty); }

            set { SetValue(ValidationCodeProperty, value); }
        }

        public bool IsCodeSent 
        {
            get { return (bool)GetValue(RecoverPasswordViewModel.IsCodeSentProperty); }

            private set { SetValue(RecoverPasswordViewModel.IsCodeSentProperty, value); }
        }

        public static readonly PropertyData IsCodeSentProperty =
            ModelBase.RegisterProperty<RecoverPasswordViewModel, bool>(recViewModel => recViewModel.IsCodeSent, () => false);
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

        private void GenerateNewValidationCode() 
        {
            int randomCode = new Random(new Random().Next()).Next(ValidationCodeMinimum, ValidationCodeMaximum);

            this.secureDecorator.SetSecureString(randomCode.ToString());
        }
    }
}