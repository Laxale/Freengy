// Created by Laxale 11.05.2018
//
//


namespace Freengy.Base.Models.Update 
{
    /// <summary>
    /// Содержит возможные типы изменений в аккаунтах друзей.
    /// </summary>
    public enum FriendUpdateType 
    {
        /// <summary>
        /// Дефолт. Не определено.
        /// </summary>
        None,

        /// <summary>
        /// Друг изменил название аккаунта.
        /// </summary>
        NameChange,

        /// <summary>
        /// Аккаунт поднял свой уровень. Какой молодец!
        /// </summary>
        LevelUp,

        /// <summary>
        /// Изменён эмо-статус аккаунта.
        /// </summary>
        StatusChange,

        /// <summary>
        /// Изменено описание аккаунта.
        /// </summary>
        DescriptionChange,

        /// <summary>
        /// Изменился онлайн-статус аккаунта.
        /// </summary>
        OnlineState,
    }
}