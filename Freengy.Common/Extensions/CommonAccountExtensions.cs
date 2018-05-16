// Created by Laxale 20.04.2018
//
//

using System;
using Freengy.Common.Database;
using Freengy.Common.Models;


namespace Freengy.Common.Extensions 
{
    /// <summary>
    /// Contains extension methos for user account models.
    /// </summary>
    public static class CommonAccountExtensions 
    {
        /// <summary>
        /// Воспринять обновление свойств аккаунта.
        /// </summary>
        /// <param name="accountModel"></param>
        /// <param name="updatedModel"></param>
        public static void AcceptProperties(this UserAccountModel accountModel, UserAccountModel updatedModel) 
        {
            if (accountModel.Id != updatedModel.Id)
            {
                throw new InvalidOperationException("Account Id mismatch");
            }

            accountModel.Name = updatedModel.Name;
            accountModel.Expirience = updatedModel.Expirience;
            accountModel.Privilege = updatedModel.Privilege;
            accountModel.LastLogInTime = updatedModel.LastLogInTime;
            accountModel.RegistrationTime = updatedModel.RegistrationTime;
        }
    }
}