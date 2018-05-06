// Created by Laxale 20.10.2016
//
//

using Freengy.Common.Helpers.Result;


namespace Freengy.Base.Interfaces 
{
    /// <summary>
    /// Intercase of an activity that can be cancelled.
    /// </summary>
    public interface IUserActivity 
    {
        /// <summary>
        /// Cancel activity.
        /// </summary>
        /// <returns>Result of a cancel attempt.</returns>
        Result Cancel();

        /// <summary>
        /// Возвращает значение - можно ли остановить данную активити без ведома юзера.
        /// </summary>
        bool CanCancelInSilent { get; }
    }
}