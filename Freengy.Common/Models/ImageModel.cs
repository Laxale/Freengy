// Created by Laxale 04.05.2018
//
//

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Freengy.Common.Database;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Модель хранимого изображения.
    /// </summary>
    public class ImageModel : ChildComplexDbObject<AlbumModel> 
    {
        /// <summary>
        /// Локальный путь к изображению (если оно сохранено локально).
        /// </summary>
        public string LocalUrl { get; set; }

        /// <summary>
        /// Путь к данному изображению на неком сервере.
        /// </summary>
        public string RemoteUrl { get; set; }


        /// <summary>
        /// Создать реальный объект из объекта-прокси EF.
        /// </summary>
        /// <param name="dbProxy">Прокси-объект, полученный из базы, который нужно превратить в реальный объект.</param>
        /// <returns>Реальный объект <see cref="DbObject"/>.</returns>
        public override DbObject CreateFromProxy(DbObject dbProxy) 
        {
            return dbProxy as ImageModel;
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
            throw new NotImplementedException();
        }
    }
}