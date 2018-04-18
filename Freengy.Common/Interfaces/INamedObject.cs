// Created by Laxale 18.10.2018
//
//


namespace Freengy.Common.Interfaces 
{
    /// <summary>
    /// Interface of an object with Name.
    /// </summary>
    public interface INamedObject 
    {
        /// <summary>
        /// Returns the name of an implementer object.
        /// </summary>
        string Name { get; }
    }
}