// Created by Laxale 03.05.2018
//
//

using System;


namespace Freengy.Base.Interfaces 
{
    /// <summary>
    /// Интерфейс исполнителя некоего действия при занавешенном окне-получателе (или любом вью, не только окне).
    /// </summary>
    public interface ICurtainedExecutor 
    {
        /// <summary>
        /// Выполнить действие при занавешенном вью-получателе запроса.
        /// </summary>
        /// <param name="acceptorId">Уникальный идентификатор получателя запроса.</param>
        /// <param name="method">Действие для выполнения.</param>
        void ExecuteWithCurtain(Guid acceptorId, Action method);
    }
}