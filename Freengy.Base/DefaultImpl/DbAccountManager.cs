// Created by Laxale 18.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using Freengy.Base.DbContext;
using Freengy.Base.Extensions;
using Freengy.Base.Interfaces;
using Freengy.Base.Models;
using Freengy.Common.ErrorReason;
using Freengy.Common.Extensions;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models;
using Freengy.Common.Models.Avatar;
using Freengy.Database.Context;

using NLog;


namespace Freengy.Base.DefaultImpl 
{
    /// <summary>
    /// <see cref="IAccountManager"/> implementer.
    /// </summary>
    public class DbAccountManager : IAccountManager 
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Get account that was last logged in.
        /// </summary>
        /// <param name="userName">Name of stored account.</param>
        /// <returns>Result of searching last user account.</returns>
        public Result<PrivateAccountModel> GetStoredAccount(string userName) 
        {
            try
            {
                using (var context = new SimpleDbContext<PrivateAccountModel>())
                {
                    var targetModel = context.Objects.FirstOrDefault(acc => acc.Name == userName);

                    return
                        targetModel == null ? 
                            Result<PrivateAccountModel>.Fail(new UnexpectedErrorReason($"Account '{userName}' not found")) : 
                            Result<PrivateAccountModel>.Ok(targetModel);

                }
            }
            catch (Exception ex)
            {
                string message = "Failed to get last logged in account";
                logger.Error(ex, message);

                return Result<PrivateAccountModel>.Fail(new UnexpectedErrorReason(message));
            }
        }

        /// <summary>
        /// Save given account as last logged in.
        /// </summary>
        /// <param name="accountModel">Account to save as last logged in.</param>
        /// <returns>Save result.</returns>
        public Result SaveLoginTime(PrivateAccountModel accountModel) 
        {
            try
            {
                using (var context = new SimpleDbContext<PrivateAccountModel>())
                {
                    var savedAcc = context.Objects.FirstOrDefault(savedModel => savedModel.Id == accountModel.Id);

                    if (savedAcc != null)
                    {
                        savedAcc.LastLogInTime = accountModel.LastLogInTime;
                    }
                    else
                    {
                        context.Objects.Add(accountModel);
                    }

                    context.SaveChanges();

                    return Result.Ok();
                }
            }
            catch (Exception ex)
            {
                string message = "Failed to save last logged in account";
                logger.Error(ex, message);

                return Result.Fail(new UnexpectedErrorReason(message));
            }
        }

        /// <summary>
        /// Сохранить изменения аккаунта в базу клиента.
        /// </summary>
        /// <param name="myAccountModel">Модель аккаунта для сохранения в базе.</param>
        /// <returns>Результат сохранения.</returns>
        public Result UpdateMyAccount(PrivateAccountModel myAccountModel) 
        {
            try
            {
                using (var context = new SimpleDbContext<PrivateAccountModel>())
                {
                    var savedAcc = context.Objects.FirstOrDefault(savedModel => savedModel.Id == myAccountModel.Id);

                    if (savedAcc != null)
                    {
                        savedAcc.AcceptProperties(myAccountModel);
                    }
                    else
                    {
                        context.Objects.Add(myAccountModel);
                    }

                    context.SaveChanges();

                    return Result.Ok();
                }
            }
            catch (Exception ex)
            {
                string message = "Failed to save account changes";
                logger.Error(ex, message);

                return Result.Fail(new UnexpectedErrorReason(message));
            }
        }

        /// <summary>
        /// Сохранить аватар пользователя.
        /// </summary>
        /// <param name="avatarModel">Модель аватара пользователя.</param>
        /// <returns>Результат сохранения аватара.</returns>
        public Result SaveUserAvatar(UserAvatarModel avatarModel) 
        {
            try
            {
                using (var context = new UserAvatarContext())
                {
                    var savedAvatar = context.Objects.FirstOrDefault(avatar => avatar.Id == avatarModel.Id);

                    if (savedAvatar != null)
                    {
                        savedAvatar.AcceptProperties(avatarModel);
                    }
                    else
                    {
                        context.Objects.Add(avatarModel);
                    }

                    context.SaveChanges();

                    return Result.Ok();
                }
            }
            catch (Exception ex)
            {
                string message = "Failed to save user avatar";
                logger.Error(ex, message);

                return Result<IEnumerable<UserAvatarModel>>.Fail(new UnexpectedErrorReason(message));
            }
        }

        /// <summary>
        /// Сохранить аватар пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="avatarBlob">Блоб аватара.</param>
        /// <returns>Результат сохранения.</returns>
        public Result SaveUserAvatar(Guid userId, byte[] avatarBlob) 
        {
            try
            {
                using (var context = new UserAvatarContext())
                {
                    var savedAvatar = context.Objects.FirstOrDefault(avatar => avatar.ParentId == userId);

                    if (savedAvatar != null)
                    {
                        savedAvatar.AvatarPath = null;
                        savedAvatar.AvatarBlob = avatarBlob;
                        savedAvatar.LastModified = DateTime.Now;
                    }
                    else
                    {
                        var newAvatar = new UserAvatarModel
                        {
                            AvatarBlob = avatarBlob,
                            AvatarPath = null,
                            LastModified = DateTime.Now,
                            ParentId = userId
                        };

                        context.Objects.Add(newAvatar);
                    }

                    context.SaveChanges();

                    return Result.Ok();
                }
            }
            catch (Exception ex)
            {
                string message = "Failed to save user avatar";
                logger.Error(ex, message);

                return Result<IEnumerable<UserAvatarModel>>.Fail(new UnexpectedErrorReason(message));
            }
        }

        /// <summary>
        /// Получить коллекцию аватаров пользователей.
        /// </summary>
        /// <param name="userIds">Коллекция идентификаторов пользователей.</param>
        /// <returns>Коллекцию сохранённых аватаров пользователей.</returns>
        public Result<IEnumerable<UserAvatarModel>> GetUserAvatars(IEnumerable<Guid> userIds) 
        {
            try
            {
                using (var context = new UserAvatarContext())
                {
                    var savedAvatars = context.Objects.Where(avatar => userIds.Contains(avatar.ParentId)).ToList();

                    return Result<IEnumerable<UserAvatarModel>>.Ok(savedAvatars);
                }
            }
            catch (Exception ex)
            {
                string message = "Failed to get user avatars";
                logger.Error(ex, message);

                return Result<IEnumerable<UserAvatarModel>>.Fail(new UnexpectedErrorReason(message));
            }
        }

        /// <summary>
        /// Получить информацию о датах модификации аватаров пользователей.
        /// </summary>
        /// <param name="userIds">Коллекция идентификаторов пользователей.</param>
        /// <returns>Коллекцию данных о временах модификации аватаров пользователей.</returns>
        public Result<IEnumerable<ObjectModificationTime>> GetUserAvatarsCache(IEnumerable<Guid> userIds) 
        {
            try
            {
                using (var context = new UserAvatarContext())
                {
                    var savedAvatars = context.Objects.Where(avatar => userIds.Contains(avatar.ParentId));
                    var cacheInformations = savedAvatars
                        .Select(avatar =>
                            new ObjectModificationTime
                            {
                                ObjectId = avatar.ParentId,
                                ModificationTime = avatar.LastModified
                            })
                        .ToList();

                    return Result<IEnumerable<ObjectModificationTime>>.Ok(cacheInformations);
                }
            }
            catch (Exception ex)
            {
                string message = "Failed to get user avatars";
                logger.Error(ex, message);

                return Result<IEnumerable<ObjectModificationTime>>.Fail(new UnexpectedErrorReason(message));
            }
        }
    }
}