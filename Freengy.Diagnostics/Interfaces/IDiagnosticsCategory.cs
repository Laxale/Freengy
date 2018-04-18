// Created by Laxale 15.11.2016
//
//

using System;
using System.Collections.Generic;

using Freengy.Common.Interfaces;


namespace Freengy.Diagnostics.Interfaces 
{    
    /// <summary>
    /// Represents single diagnosic unit, which must be registered in <see cref="IDiagnosticsController"/>
    /// by unique name (can be diagnostics owner type full name). Thus single type can register single unit
    /// </summary>
    public interface IDiagnosticsCategory : INamedObject 
    {
        string Description { get; }

        string DisplayedName { get; }
        
        /// <summary>
        /// Atomic 'unit' tests to be processed by <see cref="IDiagnosticsController"/>
        /// </summary>
        IEnumerable<IDiagnosticsUnit> TestUnits { get; }
    }
}