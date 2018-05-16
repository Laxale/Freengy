// Created by Laxale 06.05.2018
//
//

using System;
using Freengy.Base.Models;
using Freengy.Base.Models.Readonly;
using Freengy.Common.Models;


namespace Freengy.Base.Extensions 
{
    /// <summary>
    /// Содержит расширения для моделей аккаунтов.
    /// </summary>
    public static class AccountExtensions 
    {
        /// <summary>
        /// Преобразовать <see cref="UserAccountModel"/> в <see cref="PrivateAccountModel"/>.
        /// </summary>
        /// <param name="commonModel">Общая модель аккаунта.</param>
        /// <returns>Инстанс <see cref="PrivateAccountModel"/>.</returns>
        public static PrivateAccountModel ToPrivate(this UserAccountModel commonModel) 
        {
            var privateModel = new PrivateAccountModel
            {
                Id = commonModel.Id,
                Name = commonModel.Name,
                Expirience = commonModel.Expirience,
                Privilege = commonModel.Privilege,
                LastLogInTime = commonModel.LastLogInTime,
                RegistrationTime = commonModel.RegistrationTime
            };

            privateModel.Albums.AddRange(commonModel.Albums);

            return privateModel;
        }

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
                Expirience = (int)account.GetCurrentExp(),
                Privilege = account.Privilege,
                LastLogInTime = account.LastLogInTime,
                RegistrationTime = account.RegistrationTime
            };

            return model;
        }

        public static void AcceptProperties(this UserAvatarModel targetModel, UserAvatarModel foreignModel) 
        {
            if(targetModel.Id != foreignModel.Id) throw new InvalidOperationException($"Avatar id mismatch");
            if(targetModel.ParentId != foreignModel.ParentId) throw new InvalidOperationException($"Avatar parent id mismatch");

            targetModel.AvatarPath = foreignModel.AvatarPath;
            targetModel.AvatarBlob = foreignModel.AvatarBlob;
            targetModel.LastModified = foreignModel.LastModified;
        }
    }
}