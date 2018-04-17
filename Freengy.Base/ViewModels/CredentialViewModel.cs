// Created by Laxale 13.11.2016
//
//

using System.Collections.Generic;

using Catel.Data;

using Freengy.Base.Helpers;

using CommonRes = Freengy.CommonResources.StringResources;


namespace Freengy.Base.ViewModels 
{
    public class CredentialViewModel : WaitableViewModel 
    {
        protected override void SetupCommands() { }


        protected override void ValidateFields(List<IFieldValidationResult> validationResults) 
        {
            base.ValidateFields(validationResults);

            ValidateEmail(validationResults);
            ValidateUserName(validationResults);
            ValidatePassword(validationResults);
        }

        /// <summary>
        /// Not all viewmodels are goind to require email. They can override this switch
        /// </summary>
        protected virtual bool IsEmailMandatory => false;

        protected virtual void ValidatePassword(List<IFieldValidationResult> validationResults) 
        {
            if (Account.IsGoodPassword(Password)) return;

            string error =
                string.Format
                (
                    CommonRes.EntityDoesntMatchRequirementsFormat,
                    CommonRes.PasswordText,
                    CommonRes.PasswordRequirementsText
                );

            validationResults.Add(FieldValidationResult.CreateError(PasswordProperty, error));
        }


        #region Properties

        public string Email 
        {
            get { return (string)GetValue(EmailProperty); }

            set { SetValue(EmailProperty, value); }
        }

        public string UserName 
        {
            get { return (string)GetValue(UserNameProperty); }

            set { SetValue(UserNameProperty, value); }
        }

        public string Password 
        {
            get { return (string)GetValue(PasswordProperty); }

            set { SetValue(PasswordProperty, value); }
        }

        public static readonly PropertyData EmailProperty =
            RegisterProperty<CredentialViewModel, string>(credViewModel => credViewModel.Email, () => "axel.1777@mail.ru");
        public static readonly PropertyData UserNameProperty =
            RegisterProperty<CredentialViewModel, string>(credViewModel => credViewModel.UserName, () => "tost");
        public static readonly PropertyData PasswordProperty =
            RegisterProperty<CredentialViewModel, string>(credViewModel => credViewModel.Password, () => string.Empty);

        #endregion Properties


        private void ValidateEmail(List<IFieldValidationResult> validationResults) 
        {
            // empty email is okay
            if (string.IsNullOrWhiteSpace(Email) && IsEmailMandatory)
            {
                string emptyError = string.Format(CommonRes.ValueCannotBeEmptyFormat, CommonRes.EmailText);
                validationResults.Add(FieldValidationResult.CreateError(EmailProperty, emptyError));
            }

            if (Account.IsValidEmail(Email)) return;

            string error =
                string.Format
                (
                    CommonRes.EntityDoesntMatchRequirementsFormat,
                    CommonRes.EmailText,
                    CommonRes.EmailRequirementsText
                );

            validationResults.Add(FieldValidationResult.CreateError(EmailProperty, error));
        }

        private void ValidateUserName(List<IFieldValidationResult> validationResults) 
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                string error = string.Format(CommonRes.ValueCannotBeEmptyFormat, CommonRes.UserNameText);
                validationResults.Add(FieldValidationResult.CreateError(UserNameProperty, error));
            }
            else if (Common.HasInvalidSymbols(UserName))
            {
                string error = string.Format(CommonRes.ValuesContainsInvalidSymbols, CommonRes.UserNameText);
                validationResults.Add(FieldValidationResult.CreateError(UserNameProperty, error));
            }
        }
    }
}