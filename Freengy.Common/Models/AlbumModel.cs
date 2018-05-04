// Created by Laxale 04.05.2018
//
//

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Freengy.Common.Database;
using Freengy.Common.Interfaces;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Модель хранимого альбома изображений.
    /// </summary>
    public class AlbumModel : ComplexDbObject, INamedObject 
    {
        /// <summary>
        /// Gets or sets the name of album.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp of album.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Foreign key for setting album's owner.
        /// </summary>
        [Required]
        public Guid OwnerAccountId { get; set; }

        /// <summary>
        /// Gets or sets album's owner account (navigation property).
        /// </summary>
        [ForeignKey(nameof(OwnerAccountId))]
        public UserAccountModel OwnerAccountModel { get; set; }

        /// <summary>
        /// Изображения, принадлежащие данному альбому.
        /// </summary>
        public virtual List<ImageModel> Images { get; set; } = new List<ImageModel>();


        /// <summary>
        /// Создать реальный объект из объекта-прокси EF.
        /// </summary>
        /// <param name="dbProxy">Прокси-объект, полученный из базы, который нужно превратить в реальный объект.</param>
        /// <returns>Реальный объект <see cref="DbObject"/>.</returns>
        public override DbObject CreateFromProxy(DbObject dbProxy) 
        {
            return dbProxy as AlbumModel;
        }

        /// <summary>
        /// Заполнить актуальными данными зависимые свойства типа public <see cref="List{T}"/> MyList { get; set; }.
        /// </summary>
        /// <returns>Ссылка на сам <see cref="ComplexDbObject"/> с заполненными мап-пропертями.</returns>
        public override ComplexDbObject PrepareMappedProps() 
        {
            OwnerAccountModel = null;

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