// Created by Laxale 17.05.2018
//
//

using Freengy.Base.Models.Readonly;
using Freengy.Base.ViewModels;


namespace Freengy.UI.ViewModels 
{
    /// <summary>
    /// Вьюмодель выбираемой иконки.
    /// </summary>
    internal class SelectableIconViewModel : BasicViewModel 
    {
        /// <summary>
        /// Конструирует <see cref="SelectableIconViewModel"/> для данной иконки и флага возможности использовать её.
        /// </summary>
        /// <param name="iconModel">Модель иконки.</param>
        /// <param name="myLevel">Уровень моего аккаунта.</param>
        /// <param name="requiredLevel">Требуемый для иконки уровень аккаунта.</param>
        public SelectableIconViewModel(UserIconModel iconModel, uint myLevel, uint requiredLevel) 
        {
            IconModel = iconModel;
            MyLevel = myLevel;
            RequiredLevel = requiredLevel;

            IsSelectable = myLevel >= requiredLevel;
        }


        /// <summary>
        /// Возвращает значение - могу ли я использовать эту иконку.
        /// </summary>
        public bool IsSelectable { get; }

        /// <summary>
        /// Возвращает ссылку на модель иконки.
        /// </summary>
        public UserIconModel IconModel { get; }

        /// <summary>
        /// Возвращает уровень моего аккаунта.
        /// </summary>
        public uint MyLevel { get; }

        /// <summary>
        /// Возвращает требуемый для использования иконки уровень.
        /// </summary>
        public uint RequiredLevel { get; }
    }
}