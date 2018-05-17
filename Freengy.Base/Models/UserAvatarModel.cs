// Created by Laxale 16.05.2018
//
//

using System;
using System.Collections.Generic;

using Freengy.Common.Database;
using Freengy.Common.Models;


namespace Freengy.Base.Models 
{
    public class UserAvatarModel : ChildComplexDbObject<PrivateAccountModel> 
    {
        /// <summary>
        /// Возвращает или задаёт путь к локальному изображению аватара.
        /// </summary>
        public string AvatarPath { get; set; }

        /// <summary>
        /// Возвращает или задаёт двоичный блоб изображения аватара.
        /// </summary>
        public byte[] AvatarBlob { get; set; }

        /// <summary>
        /// Возвращает или задаёт дату последней модификации аватара.
        /// </summary>
        public DateTime LastModified { get; set; }


        /// <summary>
        /// Создать реальный объект из объекта-прокси EF.
        /// </summary>
        /// <param name="dbProxy">Прокси-объект, полученный из базы, который нужно превратить в реальный объект.</param>
        /// <returns>Реальный объект <see cref="DbObject"/>.</returns>
        public override DbObject CreateFromProxy(DbObject dbProxy) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Заполнить актуальными данными зависимые свойства типа public <see cref="List{T}"/> MyList { get; set; }.
        /// Обнулить навигационные свойства.
        /// </summary>
        /// <returns>Ссылка на сам <see cref="ComplexDbObject"/> с заполненными мап-пропертями и обнулёнными навигационными.</returns>
        public override ComplexDbObject PrepareMappedProps() 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить список названий вложенных пропертей класса (которые не простых типов данных).
        /// </summary>
        /// <returns>Список названий вложенных пропертей класса.</returns>
        protected override IEnumerable<string> GetIncludedPropNames() 
        {
            throw new NotImplementedException();
        }
    }
}