// Created by Laxale 15.05.2018
//
//

using System;
using System.Collections.Generic;

using Freengy.Common.Database;


namespace Freengy.Common.Models.Avatar 
{
    /// <summary>
    /// Модель аватара пользователя или альбома или группы или чего угодно.
    /// </summary>
    public abstract class AvatarModel<TParent> : ChildComplexDbObject<TParent> where TParent : ComplexDbObject, new () 
    {
        protected AvatarModel() 
        {
            
        }

        /// <summary>
        /// Возвращает или задаёт бинарный блоб изображения.
        /// </summary>
        public byte[] ImageBlob { get; set; }

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
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Заполнить актуальными данными зависимые свойства типа public <see cref="List{T}"/> MyList { get; set; }.
        /// Обнулить навигационные свойства.
        /// </summary>
        /// <returns>Ссылка на сам <see cref="ComplexDbObject"/> с заполненными мап-пропертями и обнулёнными навигационными.</returns>
        public override ComplexDbObject PrepareMappedProps() 
        {
            NavigationParent = null;

            return this;
        }

        /// <summary>
        /// Получить список названий вложенных пропертей класса (которые не простых типов данных).
        /// </summary>
        /// <returns>Список названий вложенных пропертей класса.</returns>
        protected override IEnumerable<string> GetIncludedPropNames() 
        {
            throw new System.NotImplementedException();
        }
    }
}