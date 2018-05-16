// Created by Laxale 11.05.2018
//
//

using Freengy.Base.Models.Readonly;

namespace Freengy.Base.Models.Update 
{
    /// <summary>
    /// Представляет собой запись о любом изменении аккаунта друга.
    /// </summary>
    public abstract class FriendUpdate 
    {
        protected FriendUpdate(UserAccount friendAccount) 
        {
            FriendAccount = friendAccount;
        }


        /// <summary>
        /// Тип изменения в аккаунте.
        /// </summary>
        public abstract FriendUpdateType ChangeType { get; }

        /// <summary>
        /// Аккаунт, состояние которого изменилось.
        /// </summary>
        public UserAccount FriendAccount { get; }

        /// <summary>
        /// Возвращает текстовое описание изменения.
        /// </summary>
        public abstract string UpdateDescription { get; }
    }
}