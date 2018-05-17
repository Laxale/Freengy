// Created by Laxale 17.05.2018
//
//

using System;
using Freengy.Base.Models.Readonly;


namespace Freengy.Base.Models.Extension 
{
    /// <summary>
    /// Представляет иконку как расширение свойств аккаунта.
    /// </summary>
    public class IconExtension : GenericAccountExtension<UserIconModel> 
    {
        /// <summary>
        /// Конструирует <see cref="IconExtension"/> с заданной моделью пользовательской иконки.
        /// </summary>
        /// <param name="iconModel">Модель пользовательской иконки.</param>
        public IconExtension(UserIconModel iconModel) 
        {
            ExtensionPayload = iconModel ?? throw new ArgumentNullException(nameof(iconModel));
        }


        /// <summary>
        /// Полезная нагрузка расширения.
        /// </summary>
        public override UserIconModel ExtensionPayload { get; }
    }
}