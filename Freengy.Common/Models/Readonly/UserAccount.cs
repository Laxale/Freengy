// Created by Laxale 20.04.2018
//
//

using System;

using Freengy.Common.Database;
using Freengy.Common.Enums;
using Freengy.Common.Interfaces;


namespace Freengy.Common.Models.Readonly 
{
    /// <summary>
    /// Read-only wrapper other fragile <see cref="UserAccountModel" /> that must not be used in client code.
    /// </summary>
    public class UserAccount : DbObject, INamedObject 
    {
        public UserAccount(UserAccountModel accountModel)
        {
            Id = accountModel.Id;
            Name = accountModel.Name;
            //Id = accountModel.UniqueId;
            LastLogInTime = accountModel.LastLogInTime;
            RegistrationTime = accountModel.RegistrationTime;
            Level = accountModel.Level;
            Privilege = accountModel.Privilege;
        }


        /// <summary>
        /// Account name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Account level.
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// Account privilege.
        /// </summary>
        public AccountPrivilege Privilege { get; }
        
        /// <summary>
        /// Account last login time.
        /// </summary>
        public DateTime LastLogInTime { get; }

        /// <summary>
        /// Account registration time.
        /// </summary>
        public DateTime RegistrationTime { get; }


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{Name} [Level {Level} | {Privilege}]";
        }
    }
}