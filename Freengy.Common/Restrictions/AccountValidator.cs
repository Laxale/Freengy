// Created by Laxale 18.04.2018
//
//

using System;
using System.Text;
using System.Threading.Tasks;

using Freengy.Common.Enums;
using Freengy.Common.Models;


namespace Freengy.Common.Restrictions 
{
    /// <summary>
    /// Contains validation logic for <see cref="UserAccount"/> model.
    /// </summary>
    public class AccountValidator 
    {
        private readonly UserAccount account;

        /// <summary>
        /// Maximum account level.
        /// </summary>
        public const int MaxLevel = 80;

        /// <summary>
        /// Minimum account level.
        /// </summary>
        public const int MinLevel = 1;


        /// <summary>
        /// Creates instance of <see cref="AccountValidator"/> for a given <see cref="UserAccount"/>.
        /// </summary>
        /// <param name="account">Account to validate.</param>
        public AccountValidator(UserAccount account) 
        {
            this.account = account ?? throw new ArgumentNullException(nameof(account));
        }


        /// <summary>
        /// Trim account properties that are out of limited bounds.
        /// </summary>
        /// <returns>New <see cref="UserAccount"/> clone of incame account nstance with a trimmed properties.</returns>
        public UserAccount Trim() 
        {
            var trimmedAccount = new UserAccount
            {
                Id = account.Id,
                Name = account.Name,
                Level = account.Level,
                UniqueId = account.UniqueId,
                Privilege = account.Privilege,
                RegistrationTime = account.RegistrationTime,
            };

            if (trimmedAccount.Level < MinLevel)
            {
                trimmedAccount.Level = MinLevel;
            }

            if (trimmedAccount.Level > MaxLevel)
            {
                trimmedAccount.Level = MaxLevel;
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
            return !string.IsNullOrWhiteSpace(account.Id) && account.UniqueId != Guid.Empty;
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
            return account.Level > 0 && account.Level <= MaxLevel;
        }

        private bool IsRegistrationDateValid() 
        {
            return account.RegistrationTime > DateTime.MinValue && account.RegistrationTime < DateTime.Now;
        }
    }
}