// Created by Laxale 14.05.2018
//
//

using Freengy.Base.Models;
using Freengy.Common.Constants;
using Freengy.Database;
using Freengy.Database.Context;


namespace Freengy.Base.DbContext 
{
    /// <summary>
    /// Не используется, нужен для выполнения миграций.
    /// </summary>
    public class AccountContext : SimpleDbContext<PrivateAccountModel> 
    {
        static AccountContext() 
        {
            string appDataFolderPath = Initializer.GetFolderPathInAppData(FreengyPaths.AppDataRootFolderName);
            Initializer.SetStorageDirectoryPath(appDataFolderPath);
            Initializer.SetDbFileName("freengy-client.db");
        }
    }
}