// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.Interfaces 
{
    using System;
    using System.Collections.Generic;

    using Freengy.Base.Interfaces;


    /// <summary>
    /// Represents single diagnosic unit, which must be registered in <see cref="IDiagnosticsController"/>
    /// by unique name (can be diagnostics owner type full name). Thus single type can register single unit
    /// </summary>
    public interface IDiagnosticsUnit : INamedObject 
    {
        /// <summary>
        /// Atomic tests to be processed by <see cref="IDiagnosticsController"/>
        /// </summary>
        IEnumerable<Func<bool>> TestCases { get; }
    }
}