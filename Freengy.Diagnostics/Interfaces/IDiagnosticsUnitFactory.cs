// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.Interfaces 
{
    using System;


    /// <summary>
    /// Factory interface for creating <see cref="IDiagnosticsUnit"/> objects.
    /// </summary>
    public interface IDiagnosticsUnitFactory 
    {
        /// <summary>
        /// Create a <see cref="IDiagnosticsUnit"/> objects with given parameters.
        /// </summary>
        /// <param name="name">Whatever name of a unit.</param>
        /// <param name="atomicTest">A <see cref="Func{Boolean}"/> delegate containing unit's logic.</param>
        /// <returns></returns>
        IDiagnosticsUnit CreateInstance(string name, Func<bool> atomicTest);
    }
}