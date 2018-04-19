// Created by Laxale 18.04.2018
//
//

using System;
using System.ComponentModel.DataAnnotations.Schema;

using Freengy.Common.Constants;
using Freengy.Common.Database;
using Freengy.Common.Enums;
using Freengy.Common.Interfaces;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Data model of a user account.
    /// </summary>
    public class UserAccount : DbObject, INamedObject, IObjectWithId 
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets or sets unique user identifier.
        /// </summary>
        [NotMapped]
        public Guid UniqueId { set; get; }

        /// <summary>
        /// Gets or sets level of a user account.
        /// </summary>
        public int Level { get; set; } = 1;

        /// <summary>
        /// Gets or sets privileges of account.
        /// </summary>
        public AccountPrivilege Privilege { get; set; } = AccountPrivilege.Common;

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets user account registration date.
        /// </summary>
        public DateTime RegistrationTime { get; set; }

        /// <summary>
        /// Gets or sets last user log-in time.
        /// </summary>
        public DateTime LastLogInTime { get; set; }


        /// <summary>
        /// Set <see cref="UserAccount.UniqueId"/> equal to main <see cref="UserAccount.Id"/> property.
        /// </summary>
        public void SyncUniqueIdToId() 
        {
            UniqueId = Guid.Parse(Id);
        }
    }
}