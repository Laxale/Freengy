// Created by Laxale 17.04.2018
//
//

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Freengy.Common.Database 
{
    /// <summary>
    /// Базовый класс для наследования сложных объектов. 
    /// Сложные объекты содержат кастомные типы данных, коллекции.
    /// Сложные объекты нуждаются в маппинге, то есть в отдельном контексте.
    /// </summary>
    public abstract class ComplexDbObject : DbObject 
    {
        /// <summary>
        /// Набор вложенных свойств сложного объекта, разделённых точкой. Нужен для правильного чтения вложенных свойств из базы.
        /// </summary>
        [NotMapped]
        public string IncludedPropertyNames => string.Join(".", GetIncludedPropNames());


        /// <summary>
        /// Создать реальный объект из объекта-прокси EF.
        /// </summary>
        /// <param name="dbProxy">Прокси-объект, полученный из базы, который нужно превратить в реальный объект.</param>
        /// <returns>Реальный объект <see cref="DbObject"/>.</returns>
        public abstract DbObject CreateFromProxy(DbObject dbProxy);

        /// <summary>
        /// Заполнить актуальными данными зависимые свойства типа public <see cref="List{T}"/> MyList { get; set; }.
        /// </summary>
        /// <returns>Ссылка на сам <see cref="ComplexDbObject"/> с заполненными мап-пропертями.</returns>
        public abstract ComplexDbObject PrepareMappedProps();


        /// <summary>
        /// Получить список названий вложенных пропертей класса (которые не простых типов данных).
        /// </summary>
        /// <returns>Список названий вложенных пропертей класса.</returns>
        protected abstract List<string> GetIncludedPropNames();
    }
}
