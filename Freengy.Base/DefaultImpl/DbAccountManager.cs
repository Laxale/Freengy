// Created by Laxale 18.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;

using Freengy.Base.Interfaces;
using Freengy.Base.Models;
using Freengy.Common.ErrorReason;
using Freengy.Common.Extensions;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
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
        /// <param name="myAccountModel"></param>
        /// <returns></returns>
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
    }
}