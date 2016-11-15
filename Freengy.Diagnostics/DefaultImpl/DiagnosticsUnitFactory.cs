// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.DefaultImpl 
{
    using System;

    using Freengy.Diagnostics.Interfaces;
    

    internal class DiagnosticsUnitFactory : IDiagnosticsUnitFactory 
    {
        public IDiagnosticsUnit CreateInstance(string name, Func<bool> atomicTest) 
        {
            if (atomicTest == null) throw new ArgumentNullException(nameof(atomicTest));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            return new DiagnosticsUnit(name, atomicTest);
        }
    }
}