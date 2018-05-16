// Created by Laxale 15.05.2018
//
//

using System;
using System.Collections.Generic;


namespace Freengy.Common.Models.Avatar 
{
    /// <summary>
    /// Модель ответа сервера на запрос аватаров (или кэш-информации о них) пользователей.
    /// </summary>
    public class UserAvatarsReply 
    {
        /// <summary>
        /// Возвращает или задаёт коллекцию аватаров пользователей.
        /// </summary>
        public List<AvatarModel> UserAvatars { set; get; }

        /// <summary>
        /// Возвращает или задаёт коллекцию меток времени последней модификации аватаров пользователей.
        /// </summary>
        public List<ObjectModificationTime> AvatarsModifications { get; set; }
    }
}