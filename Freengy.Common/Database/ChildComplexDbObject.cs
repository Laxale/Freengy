// Created by Laxale 19.04.2018
//
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Freengy.Common.Database 
{
    /// <summary>
    /// Базовый класс для наследования сложных объектов, привязанных к родительскому объекту.
    /// </summary>
    /// <typeparam name="TComplexParent">Родительский сложный объект.</typeparam>
    public abstract class ChildComplexDbObject<TComplexParent> : ComplexDbObject where TComplexParent : ComplexDbObject, new() 
    {
        /// <summary>
        /// Внешний ключ для связи с родительским объектом <see cref="TComplexParent"/>.
        /// </summary>
        [Required]
        public Guid ParentId { get; set; }

        /// <summary>
        /// Навигационное свойство - родительский объект <see cref="TComplexParent"/>.
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public TComplexParent NavigationParent { get; set; }
    }
}