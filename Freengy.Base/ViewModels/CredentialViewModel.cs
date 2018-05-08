// Created by Laxale 13.11.2016
//
//

using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.ServiceModel.Security;


using Freengy.Base.Helpers;
using Freengy.Base.Interfaces;

using LocalizedRes = Freengy.Localization.StringResources;


namespace Freengy.Base.ViewModels 
{
    public abstract class CredentialViewModel : WaitableViewModel, IDataErrorInfo  
    {
        private readonly Dictionary<string, string> validationErrors = new Dictionary<string, string>();

        private string email;
        private string userName;
        private string password = string.Empty;
        //private SecureString password;


        protected CredentialViewModel(ITaskWrapper taskWrapper, IGuiDispatcher guiDispatcher, IMyServiceLocator serviceLocator) : 
            base(taskWrapper, guiDispatcher, serviceLocator)
        {

        }


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
                    LocalizedRes.EntityDoesntMatchRequirementsFormat,
                    LocalizedRes.PasswordText,
                    LocalizedRes.PasswordRequirementsText
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
                return string.Format(LocalizedRes.ValueCannotBeEmptyFormat, LocalizedRes.EmailText);
            }

            if (Account.IsValidEmail(Email)) return null;

            string error =
                string.Format
                (
                    LocalizedRes.EntityDoesntMatchRequirementsFormat,
                    LocalizedRes.EmailText,
                    LocalizedRes.EmailRequirementsText
                );

            return error;
        }

        private string ValidateUserName() 
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                return string.Format(LocalizedRes.ValueCannotBeEmptyFormat, LocalizedRes.UserNameText);
            }

            if (Helpers.Common.HasInvalidSymbols(UserName))
            {
                return string.Format(LocalizedRes.ValuesContainsInvalidSymbols, LocalizedRes.UserNameText);
            }

            return null;
        }
    }
}