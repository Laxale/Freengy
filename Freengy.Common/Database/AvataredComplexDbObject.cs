// Created by Laxale 15.05.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Freengy.Common.Models.Avatar;


namespace Freengy.Common.Database 
{
    /// <summary>
    /// TODO тестовый класс для пробы двойного аватарного generic
    /// </summary>
    /// <typeparam name="TAvatarModel"></typeparam>
    public class AvataredComplexDbObject<TAvatarModel> : ComplexDbObject where TAvatarModel : AvatarModel<AvataredComplexDbObject<TAvatarModel>>, new () 
    {
        public AvatarModel<AvataredComplexDbObject<TAvatarModel>> Avatar { get; set; }


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