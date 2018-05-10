// Created by Laxale 18.04.2018
//
//

using Freengy.Base.Models;
using Freengy.Common.Models;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models.Readonly;


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
        /// <returns></returns>
        Result UpdateMyAccount(PrivateAccountModel myAccountModel);
    }
}