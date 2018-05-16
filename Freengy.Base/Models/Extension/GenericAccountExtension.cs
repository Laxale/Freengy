// Created by Laxale 16.05.2018
//
//


namespace Freengy.Base.Models.Extension 
{
    /// <summary>
    /// Базовый класс для расширений аккаунтов.
    /// </summary>
    public abstract class GenericAccountExtension<TPayload> : AccountExtension 
    {
        /// <summary>
        /// Полезная нагрузка расширения.
        /// </summary>
        public abstract TPayload ExtensionPayload { get; }
    }
}