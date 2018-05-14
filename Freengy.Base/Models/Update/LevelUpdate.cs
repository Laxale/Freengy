// Created by Laxale 11.05.2018
//
//

using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Models.Update 
{
    public class LevelUpdate : FriendUpdate 
    {
        public LevelUpdate(UserAccount friendAccount, bool isStateUpdated) : base(friendAccount) 
        {
            var actualLevel = isStateUpdated ? FriendAccount.Level : FriendAccount.Level + 1;
            UpdateDescription = $"achieved level { actualLevel }";
        }

        /// <summary>
        /// Тип изменения в аккаунте.
        /// </summary>
        public override FriendUpdateType ChangeType { get; } = FriendUpdateType.LevelUp;

        /// <summary>
        /// Возвращает текстовое описание изменения.
        /// </summary>
        public override string UpdateDescription { get; }
    }
}