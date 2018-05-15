// Created by Laxale 18.04.2018
//
//

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Freengy.Common.Constants;
using Freengy.Common.Database;
using Freengy.Common.Enums;
using Freengy.Common.Helpers;
using Freengy.Common.Interfaces;
using Freengy.Common.Models.Avatar;
using Freengy.Common.Models.Readonly;

using Newtonsoft.Json;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Data model of a user account.
    /// </summary>
    [Table(nameof(UserAccount) + "s")]
    public class UserAccountModel : AvataredComplexDbObject<UserAvatarModel>, INamedObject 
    {
        /// <summary>
        /// Gets or sets level of a user account.
        /// </summary>
        public int Expirience { get; set; }

        /// <summary>
        /// Возвращает рассчитанное значение уровня для данного аккаунта.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public uint Level => ExpirienceCalculator.GetLevelForExp((uint)Expirience);

        /// <summary>
        /// Gets or sets privileges of account.
        /// </summary>
        public AccountPrivilege Privilege { get; set; } = AccountPrivilege.Common;

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
        /// Возвращает или задаёт модель аватара пользователя.
        /// </summary>
        //public UserAvatarModel Avatar { get; set; }

        /// <summary>
        /// Gets or sets user albums list.
        /// </summary>
        public virtual List<AlbumModel> Albums { get; set; } = new List<AlbumModel>();


        public override string ToString() 
        {
            return $"{Name} [Level {Expirience} {Privilege}]";
        }

        /// <summary>
        /// Создать реальный объект из объекта-прокси EF.
        /// </summary>
        /// <param name="dbProxy">Прокси-объект, полученный из базы, который нужно превратить в реальный объект.</param>
        /// <returns>Реальный объект <see cref="DbObject"/>.</returns>
        public override DbObject CreateFromProxy(DbObject dbProxy) 
        {
            return (UserAccountModel) dbProxy;
        }

        /// <summary>
        /// Заполнить актуальными данными зависимые свойства типа public <see cref="T:System.Collections.Generic.List`1" /> MyList { get; set; }.
        /// </summary>
        /// <returns>Ссылка на сам <see cref="T:Freengy.Common.Database.ComplexDbObject" /> с заполненными мап-пропертями.</returns>
        public override ComplexDbObject PrepareMappedProps() 
        {
            return this;
        }

        /// <summary>
        /// Получить список названий вложенных пропертей класса (которые не простых типов данных).
        /// </summary>
        /// <returns>Список названий вложенных пропертей класса.</returns>
        protected override IEnumerable<string> GetIncludedPropNames() 
        {
            return new List<string>();
        }
    }
}