﻿// Created by Laxale 20.04.2018
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
        public string Name { get; private set; }

        /// <summary>
        /// Account level.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Account privilege.
        /// </summary>
        public AccountPrivilege Privilege { get; private set; }
        
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

        /// <summary>
        /// Update properties from account model.
        /// </summary>
        /// <param name="model">Account model to update properties from.</param>
        public void UpdateFromModel(UserAccountModel model) 
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (Id != model.Id) throw new InvalidOperationException("Account id mismatch");

            Name = model.Name;
            Level = model.Level;
            Privilege = model.Privilege;
        }
    }
}