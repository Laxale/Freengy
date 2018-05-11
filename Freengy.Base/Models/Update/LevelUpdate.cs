// Created by Laxale 11.05.2018
//
//

using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Models.Update 
{
    public class LevelUpdate : FriendUpdate 
    {
        public LevelUpdate(UserAccount friendAccount) : base(friendAccount) 
        {
            UpdateDescription = $"achieved level {FriendAccount.Level}";
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