// Created by Laxale 10.05.2018
//
//


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Модель запроса на редактирование данных аккаунта.
    /// </summary>
    public class EditAccountModel 
    {
        /// <summary>
        /// Возвращает или задаёт название аккаунта.
        /// </summary>
        public string NewName { get; set; }

        /// <summary>
        /// Возвращает или задаёт описание аккаунта.
        /// </summary>
        public string NewDescription { get; set; }

        /// <summary>
        /// Возвращает или задаёт текстовый статус аккаунта.
        /// </summary>
        public string NewEmoteStatus { get; set; }

        /// <summary>
        /// Возвращает или задаёт Base64 блоб аватара аккаунта.
        /// </summary>
        public string NewImageBlob { get; set; }
    }
}