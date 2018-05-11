// Created by Laxale 11.05.2018
//
//

using Freengy.Common.Enums;
using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Models.Update 
{
    public class PrivilegeUpdate : FriendUpdate 
    {
        public PrivilegeUpdate(UserAccount friendAccount, AccountPrivilege newPrivilege) : base(friendAccount) 
        {
            UpdateDescription = $"is now {newPrivilege}";
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