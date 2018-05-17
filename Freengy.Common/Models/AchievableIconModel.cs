// Created by Laxale 17.05.2018
//
//


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Модель иконки, требующей определённого уровня для использования.
    /// </summary>
    public class AchievableIconModel : BinaryDataModel 
    {
        /// <summary>
        /// Возвращает или задаёт необходимый для использования иконки уровень.
        /// </summary>
        public uint RequiredLevel { get; set; }
    }
}