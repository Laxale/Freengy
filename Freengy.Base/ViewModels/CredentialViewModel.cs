﻿// Created by Laxale 13.11.2016
//
//

using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.ServiceModel.Security;


using Freengy.Base.Helpers;

using CommonRes = Freengy.CommonResources.StringResources;


namespace Freengy.Base.ViewModels 
{
    using System.Linq;


    public abstract class CredentialViewModel : WaitableViewModel, IDataErrorInfo  
    {
        private readonly Dictionary<string, string> validationErrors = new Dictionary<string, string>();

        private string email;
        private string userName;
        private string password = string.Empty;
        //private SecureString password;


        /// <summary>
        /// Not all viewmodels are going to require email. They can override this switch
        /// </summary>
        protected virtual bool IsEmailMandatory => false;

        protected virtual string ValidatePassword() 
        {
            if (Account.IsGoodPassword(Password)) return null;

            string error =
                string.Format
                (
                    CommonRes.EntityDoesntMatchRequirementsFormat,
                    CommonRes.PasswordText,
                    CommonRes.PasswordRequirementsText
                );

            return error;
        }


        /// <summary>
        /// Возвращает флаг наличия ошибок валидации вьюмодели.
        /// </summary>
        public bool HasValidationErrors => validationErrors.Any(pair => !string.IsNullOrWhiteSpace(pair.Value));

        public string Email 
        {
            get => email;

            set
            {
                if (email == value) return;

                email = value;

                OnPropertyChanged();
            }
        }

        public string UserName 
        {
            get => userName;

            set
            {
                if (userName == value) return;

                userName = value;

                OnPropertyChanged();
            }
        }

        public string Password 
        {
            get => password;

            set
            {
                if (password == value) return;

                password = value;

                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>Gets the error message for the property with the given name.</summary>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        /// <param name="columnName">The name of the property whose error message to get. </param>
        public string this[string columnName] 
        {
            get
            {
                string error = null;

                switch (columnName)
                {
                    case nameof(UserName):
                        error = ValidateUserName();
                        validationErrors[nameof(UserName)] = error;
                        break;

                    case nameof(Password):
                        error = ValidatePassword();
                        validationErrors[nameof(Password)] = error;
                        break;

                    case nameof(Email):
                        error = ValidateEmail();
                        validationErrors[nameof(Email)] = error;
                        break;
                }

                Error = error;

                return Error;
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        public string Error { get; private set; }


        private string ValidateEmail() 
        {
            if (string.IsNullOrWhiteSpace(Email) && IsEmailMandatory)
            {
                return string.Format(CommonRes.ValueCannotBeEmptyFormat, CommonRes.EmailText);
            }

            if (Account.IsValidEmail(Email)) return null;

            string error =
                string.Format
                (
                    CommonRes.EntityDoesntMatchRequirementsFormat,
                    CommonRes.EmailText,
                    CommonRes.EmailRequirementsText
                );

            return error;
        }

        private string ValidateUserName() 
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                return string.Format(CommonRes.ValueCannotBeEmptyFormat, CommonRes.UserNameText);
            }

            if (Helpers.Common.HasInvalidSymbols(UserName))
            {
                return string.Format(CommonRes.ValuesContainsInvalidSymbols, CommonRes.UserNameText);
            }

            return null;
        }
    }
}