// Created by Laxale 23.04.2018
//
//


using System;

using Freengy.Common.Enums;


namespace Freengy.Common.Models.Readonly 
{
    /// <summary>
    /// Readonly user account state wrapper other volatile <see cref="AccountStateModel"/>.
    /// </summary>
    public class AccountState 
    {
        /// <summary>
        /// Creates a new <see cref="AccountState"/> other the given model.
        /// </summary>
        /// <param name="model">Account state model to hide in this wrapper.</param>
        public AccountState(AccountStateModel model) 
        {
            if(model == null) throw new ArgumentNullException(nameof(model));
            if(model.Account == null) throw new ArgumentNullException(nameof(model.Account));

            UserAddress = model.Address;
            AccountStatus = model.OnlineStatus;
            Account = new UserAccount(model.Account);
        }


        /// <summary>
        /// User's current IP address.
        /// </summary>
        public string UserAddress { get; }

        /// <summary>
        /// User account.
        /// </summary>
        public UserAccount Account { get; }

        /// <summary>
        /// User online status.
        /// </summary>
        public AccountOnlineStatus AccountStatus { get; }
    }
}