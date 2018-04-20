// Created by Laxale 20.04.2018
//
//

using Freengy.Common.Models;


namespace Freengy.Common.Extensions 
{
    /// <summary>
    /// Contains extension methos for <see cref="UserAccount"/>.
    /// </summary>
    public static class AccountExtensions 
    {
        /// <summary>
        /// Convert read-only <see cref="UserAccount"/> to <see cref="UserAccountModel"/>.
        /// </summary>
        /// <param name="account">Read-only user account.</param>
        /// <returns><see cref="UserAccountModel"/> instance.</returns>
        public static UserAccountModel ToModel(this UserAccount account) 
        {
            var model = new UserAccountModel
            {
                Id = account.Id,
                UniqueId = account.UniqueId,
                Name = account.Name,
                Level = account.Level,
                Privilege = account.Privilege,
                LastLogInTime = account.LastLogInTime,
                RegistrationTime = account.RegistrationTime
            };

            return model;
        }
    }
}