// Created by Laxale 18.04.2018
//
//

using System;
using System.Collections.Generic;
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
    public class UserAccount : ComplexDbObject, INamedObject, IObjectWithId 
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

        /// <summary>
        /// Создать реальный объект из объекта-прокси EF.
        /// </summary>
        /// <param name="dbProxy">Прокси-объект, полученный из базы, который нужно превратить в реальный объект.</param>
        /// <returns>Реальный объект <see cref="DbObject"/>.</returns>
        public override DbObject CreateFromProxy(DbObject dbProxy) 
        {
            return (UserAccount) dbProxy;
        }

        /// <summary>
        /// Заполнить актуальными данными зависимые свойства типа public <see cref="List{T}"/> MyList { get; set; }.
        /// </summary>
        /// <returns>Ссылка на сам <see cref="ComplexDbObject"/> с заполненными мап-пропертями.</returns>
        public override ComplexDbObject PrepareMappedProps()
        {
            return this;
        }

        /// <summary>
        /// Получить список названий вложенных пропертей класса (которые не простых типов данных).
        /// </summary>
        /// <returns>Список названий вложенных пропертей класса.</returns>
        protected override List<string> GetIncludedPropNames() 
        {
            return new List<string>();
        }
    }
}