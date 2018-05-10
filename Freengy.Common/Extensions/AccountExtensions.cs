// Created by Laxale 20.04.2018
//
//

using System;
using Freengy.Common.Database;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;


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
                Name = account.Name,
                Level = account.Level,
                Privilege = account.Privilege,
                LastLogInTime = account.LastLogInTime,
                RegistrationTime = account.RegistrationTime
            };

            return model;
        }

        /// <summary>
        /// Воспринять обновление свойств аккаунта.
        /// </summary>
        /// <param name="accountModel"></param>
        /// <param name="updatedModel"></param>
        public static void AcceptProperties(this UserAccountModel accountModel, UserAccountModel updatedModel) 
        {
            if (accountModel.Id != updatedModel.Id)
            {
                throw new InvalidOperationException($"Id mismatch");
            }

            accountModel.Name = accountModel.Name;
            accountModel.Level = accountModel.Level;
            accountModel.Privilege = accountModel.Privilege;
            accountModel.LastLogInTime = accountModel.LastLogInTime;
            accountModel.RegistrationTime = accountModel.RegistrationTime;
        }
    }
}