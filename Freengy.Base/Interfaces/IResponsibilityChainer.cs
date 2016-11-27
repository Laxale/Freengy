// Created by Laxale 23.10.2016
//
//


namespace Freengy.Base.Interfaces 
{
    using System;
    using System.Threading.Tasks;


    /// <summary>
    /// Represents "responsibility chain" pattern implementer
    /// </summary>
    public interface IResponsibilityChainer<TObjectType> 
    {
        /// <summary>
        /// Push object through processing chain
        /// </summary>
        ///<param name="targetToProcess">Object that must be processed</param>
        /// <returns>True if object was successfully processed</returns>
        bool Handle(TObjectType targetToProcess);

        /// <summary>
        /// Push object through processing chain
        /// </summary>
        ///<param name="targetToProcess">Object that must be processed</param>
        /// <returns>True if object was successfully processed</returns>
        Task<bool> HandleAsync(TObjectType targetToProcess);

        /// <summary>
        /// Add processing chain element
        /// </summary>
        /// <param name="handler">Chain element</param>
        IResponsibilityChainer<TObjectType> AddHandler(Func<TObjectType, bool> handler);

        /// <summary>
        /// Remove handler from a chain
        /// </summary>
        /// <param name="handler">Target handler</param>
        IResponsibilityChainer<TObjectType> RemoveHandler(Func<TObjectType, bool> handler);
    }
}