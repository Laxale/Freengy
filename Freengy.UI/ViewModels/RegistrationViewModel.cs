// Created by Laxale 12.11.2016
//
//


namespace Freengy.UI.ViewModels 
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    using Freengy.Base.Helpers;
    using Freengy.Base.ViewModels;
    using Freengy.Networking.Interfaces;
    using Freengy.Networking.DefaultImpl;

    using Catel.IoC;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;

    using Prism.Regions;

    using CommonRes = Freengy.CommonResources.StringResources;


    internal class RegistrationViewModel : WaitableViewModel 
    {
        protected override void SetupCommands() 
        {
            
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults) 
        {
            base.ValidateFields(validationResults);

            if (string.IsNullOrWhiteSpace(this.UserName))
            {
                validationResults.Add
                (
                    FieldValidationResult.CreateError(UserNameProperty, CommonRes.ValueCannotBeEmptyFormat, CommonRes.HostNameText)
                );
            }
            else if (Common.HasInvalidSymbols(this.UserName))
            {
                FieldValidationResult.CreateError(UserNameProperty, CommonRes.ValuesContainsInvalidSymbols, CommonRes.HostNameText);
            }
        }


        public string UserName 
        {
            get { return (string)GetValue(UserNameProperty); }

            set { SetValue(UserNameProperty, value); }
        }

        public string Email 
        {
            get { return (string)GetValue(EmailProperty); }

            set { SetValue(EmailProperty, value); }
        }

        public static readonly PropertyData UserNameProperty =
            RegisterProperty<RegistrationViewModel, string>(regViewModel => regViewModel.UserName, () => string.Empty);
        public static readonly PropertyData EmailProperty =
            RegisterProperty<RegistrationViewModel, string>(regViewModel => regViewModel.UserName, () => string.Empty);
    }
}