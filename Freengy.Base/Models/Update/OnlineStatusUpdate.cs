// Created by Laxale 11.05.2018
//
//

using Freengy.Common.Enums;
using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Models.Update 
{
    public class OnlineStatusUpdate : FriendUpdate 
    {
        public OnlineStatusUpdate(UserAccount friendAccount, AccountOnlineStatus newStatus) : base(friendAccount) 
        {
            UpdateDescription = $"is {newStatus}";
        }


        /// <summary>
        /// Тип изменения в аккаунте.
        /// </summary>
        public override FriendUpdateType ChangeType { get; } = FriendUpdateType.OnlineState;

        /// <summary>
        /// Возвращает текстовое описание изменения.
        /// </summary>
        public override string UpdateDescription { get; }
    }
}