// Created by Laxale 11.05.2018
//
//

using Freengy.Base.Models.Readonly;

namespace Freengy.Base.Models.Update 
{
    /// <summary>
    /// Запись о том, что изменилось название аккаунта.
    /// </summary>
    public class NameUpdate : FriendUpdate 
    {
        /// <summary>
        /// Конструирует <see cref="NameUpdate"/> для заданного аккаунта.
        /// </summary>
        /// <param name="friendAccount">Аккаунт, название которого изменилось.</param>
        /// <param name="previousName">Предыдущее название аккаунта.</param>
        public NameUpdate(UserAccount friendAccount, string previousName) : base(friendAccount)
        {
            UpdateDescription = $"changed name from { previousName }";
        }


        /// <summary>
        /// Возвращает текстовое описание изменения.
        /// </summary>
        public override string UpdateDescription { get; }

        /// <summary>
        /// Тип изменения в аккаунте.
        /// </summary>
        public override FriendUpdateType ChangeType { get; } = FriendUpdateType.NameChange;
    }
}