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
        public string UserAddress { get; set; }

        /// <summary>
        /// User account.
        /// </summary>
        public UserAccount Account { get; }

        /// <summary>
        /// User online status.
        /// </summary>
        public AccountOnlineStatus AccountStatus { get; set; }


        /// <summary>
        /// Update properties from state model.
        /// </summary>
        /// <param name="stateModel">State model to update properties from.</param>
        public void UpdateFromModel(AccountStateModel stateModel) 
        {
            if(stateModel == null) throw new ArgumentNullException(nameof(stateModel));
            if(stateModel.Account.Id != Account.Id) throw new InvalidOperationException("Account id mismatch");

            UserAddress = stateModel.Address;
            AccountStatus = stateModel.OnlineStatus;

            Account.UpdateFromModel(stateModel.Account);
        }
    }
}