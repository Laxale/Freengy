// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.DefaultImpl 
{
    using System;

    using Freengy.Diagnostics.Interfaces;


    /// <summary>
    /// Must be created only by <see cref="IDiagnosticsUnitFactory"/>.
    /// Class actually could be placed inside the factory for more incapsulation
    /// </summary>
    internal class DiagnosticsUnit : IDiagnosticsUnit 
    {
        public DiagnosticsUnit(string name, Func<bool> unitTest) 
        {
            this.Name = name;
            this.UnitTest = unitTest;
        }


        public string Name { get; }

        public Func<bool> UnitTest { get; }
    }
}