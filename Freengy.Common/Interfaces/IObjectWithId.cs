// Created by Laxale 18.10.2018
//
//

using System;


namespace Freengy.Common.Interfaces 
{
    /// <summary>
    /// Interface of an object with unique identifier.
    /// </summary>
    public interface IObjectWithId 
    {
        /// <summary>
        /// Returns unique identifier of an implementer object.
        /// </summary>
        Guid UniqueId { get; }
    }
}