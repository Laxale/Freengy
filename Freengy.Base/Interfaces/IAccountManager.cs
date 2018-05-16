// Created by Laxale 18.04.2018
//
//

using System;
using System.Collections.Generic;
using Freengy.Base.Models;
using Freengy.Common.Models;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models.Avatar;


namespace Freengy.Base.Interfaces 
{
    /// <summary>
    /// Interface for managing client accounts.
    /// </summary>
    public interface IAccountManager 
    {
        /// <summary>
        /// Get account that was last logged in.
        /// </summary>
        /// <param name="userName">Name of stored account.</param>
        /// <returns>Result of searching last user account.</returns>
        Result<PrivateAccountModel> GetStoredAccount(string userName);

        /// <summary>
        /// Save given account as last logged in.
        /// </summary>
        /// <param name="accountModel">Account to save as last logged in.</param>
        /// <returns>Save result.</returns>
        Result SaveLoginTime(PrivateAccountModel accountModel);

        /// <summary>
        /// Сохранить изменения аккаунта в базу клиента.
        /// </summary>
        /// <param name="myAccountModel"></param>
        /// <returns>Результат сохранения.</returns>
        Result UpdateMyAccount(PrivateAccountModel myAccountModel);

        /// <summary>
        /// Сохранить аватар пользователя.
        /// </summary>
        /// <param name="avatarModel">Модель аватара пользователя.</param>
        /// <returns>Результат сохранения аватара.</returns>
        Result SaveUserAvatar(UserAvatarModel avatarModel);

        /// <summary>
        /// Сохранить аватар пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="avatarBlob">Блоб аватара.</param>
        /// <returns>Результат сохранения.</returns>
        Result SaveUserAvatar(Guid userId, byte[] avatarBlob);

        /// <summary>
        /// Получить коллекцию аватаров пользователей.
        /// </summary>
        /// <param name="userIds">Коллекция идентификаторов пользователей.</param>
        /// <returns>Коллекцию сохранённых аватаров пользователей.</returns>
        Result<IEnumerable<UserAvatarModel>> GetUserAvatars(IEnumerable<Guid> userIds);

        /// <summary>
        /// Получить информацию о датах модификации аватаров пользователей.
        /// </summary>
        /// <param name="userIds">Коллекция идентификаторов пользователей.</param>
        /// <returns>Коллекцию данных о временах модификации аватаров пользователей.</returns>
        Result<IEnumerable<ObjectModificationTime>> GetUserAvatarsCache(IEnumerable<Guid> userIds);
    }
}