// Created by Laxale 20.04.2018
//
//

using System;


namespace Freengy.Base.Attributes 
{
    /// <summary>
    /// Для целей связывания вью и вьюмоделей объявляет тип вьюмодели, соответствующий типу вью, который помечен данным атрибутом.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class HasViewModelAttribute : Attribute 
    {
        /// <inheritdoc />
        /// <summary>
        /// Конструирует <see cref="T:Freengy.Base.Attributes.HasViewModelAttribute" /> с заданным типом вьюмодели.
        /// </summary>
        /// <param name="viewModelType">Тип вьюмодели, соответствующий типу вью, который помечен данным атрибутом.</param>
        public HasViewModelAttribute(Type viewModelType) 
        {
            ViewModelType = viewModelType;
        }


        /// <summary>
        /// Тип вьюмодели, соответствующий типу вью, который помечен данным атрибутом.
        /// </summary>
        public Type ViewModelType { get; }
    }
}
