// Created by Laxale 16.05.2018
//
//

using System;


namespace Freengy.Base.Models.Extension 
{
    /// <summary>
    /// Представляет аватар как расширение свойств аккаунта.
    /// </summary>
    public class AvatarExtension : GenericAccountExtension<UserAvatarModel> 
    {
        /// <summary>
        /// Конструирует <see cref="AvatarExtension"/> с заданной моделью пользовательского аватара.
        /// </summary>
        /// <param name="avatarModel">Модель пользовательского аватара.</param>
        public AvatarExtension(UserAvatarModel avatarModel) 
        {
            ExtensionPayload = avatarModel ?? throw new ArgumentNullException(nameof(avatarModel));
        }


        /// <summary>
        /// Полезная нагрузка расширения.
        /// </summary>
        public override UserAvatarModel ExtensionPayload { get; }
    }
}