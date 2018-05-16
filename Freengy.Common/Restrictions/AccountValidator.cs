// Created by Laxale 18.04.2018
//
//

using System;
using System.Text;
using System.Threading.Tasks;

using Freengy.Common.Enums;
using Freengy.Common.Helpers;
using Freengy.Common.Models;


namespace Freengy.Common.Restrictions 
{
    /// <summary>
    /// Contains validation logic for <see cref="UserAccountModel"/> model.
    /// </summary>
    public class AccountValidator 
    {
        private readonly UserAccountModel account;

        
        /// <summary>
        /// Creates instance of <see cref="AccountValidator"/> for a given <see cref="UserAccountModel"/>.
        /// </summary>
        /// <param name="account">Account to validate.</param>
        public AccountValidator(UserAccountModel account) 
        {
            this.account = account ?? throw new ArgumentNullException(nameof(account));
        }


        /// <summary>
        /// Trim account properties that are out of limited bounds.
        /// </summary>
        /// <returns>New <see cref="UserAccount"/> clone of incame account nstance with a trimmed properties.</returns>
        public UserAccountModel Trim() 
        {
            var trimmedAccount = new UserAccountModel
            {
                Id = account.Id,
                Name = account.Name,
                Expirience = account.Expirience,
                //UniqueId = account.UniqueId,
                Privilege = account.Privilege,
                RegistrationTime = account.RegistrationTime,
            };

            if (trimmedAccount.Level < ExpirienceCalculator.MinLevel)
            {
                trimmedAccount.Expirience = 1;
            }

            if (trimmedAccount.Level > ExpirienceCalculator.MaxLevel)
            {
                trimmedAccount.Expirience = (int)ExpirienceCalculator.GetExpOfLevel(ExpirienceCalculator.MaxLevel);
            }

            if (trimmedAccount.Privilege < 0)
            {
                trimmedAccount.Privilege = AccountPrivilege.Common;
            }

            if (trimmedAccount.Privilege > AccountPrivilege.UltramarineImperator)
            {
                trimmedAccount.Privilege = AccountPrivilege.UltramarineImperator;
            }


            return trimmedAccount;
        }

        /// <summary>
        /// Check if account properties are in limited bounds.
        /// </summary>
        /// <returns>True if account data is valid.</returns>
        public bool IsValid() 
        {
            bool isValid = IsLevelValid() && IsNameValid() && IsRegistrationDateValid() && AreIdentifiersValid() && IsPrivilegeValid();

            return isValid;
        }


        private bool AreIdentifiersValid() 
        {
            //return !string.IsNullOrWhiteSpace(account.Id) && account.UniqueId != Guid.Empty;
            return account.Id != Guid.Empty;
        }

        private bool IsNameValid() 
        {
            return !string.IsNullOrWhiteSpace(account.Name);
        }

        private bool IsPrivilegeValid() 
        {
            return account.Privilege > 0 && account.Privilege <= AccountPrivilege.UltramarineImperator;
        }

        private bool IsLevelValid() 
        {
            return account.Level >= ExpirienceCalculator.MinLevel && account.Level <= ExpirienceCalculator.MaxLevel;
        }

        private bool IsRegistrationDateValid() 
        {
            return account.RegistrationTime > DateTime.MinValue && account.RegistrationTime < DateTime.Now;
        }
    }
}