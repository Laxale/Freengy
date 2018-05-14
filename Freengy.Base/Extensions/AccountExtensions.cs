// Created by Laxale 06.05.2018
//
//

using Freengy.Base.Models;
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
    }
}
