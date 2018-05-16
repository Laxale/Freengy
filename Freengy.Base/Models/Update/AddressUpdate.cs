// Created by Laxale 11.05.2018
//
//

using Freengy.Base.Models.Readonly;

namespace Freengy.Base.Models.Update 
{
    public class AddressUpdate : FriendUpdate 
    {
        public AddressUpdate(UserAccount friendAccount, string newAddress) : base(friendAccount) 
        {
            UpdateDescription = $"changed address to { newAddress }";
        }

        /// <summary>
        /// Тип изменения в аккаунте.
        /// </summary>
        public override FriendUpdateType ChangeType { get; } = FriendUpdateType.None;

        /// <summary>
        /// Возвращает текстовое описание изменения.
        /// </summary>
        public override string UpdateDescription { get; }
    }
}