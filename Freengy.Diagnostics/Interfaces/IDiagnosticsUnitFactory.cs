// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.Interfaces 
{
    using System;


    public interface IDiagnosticsUnitFactory 
    {
        IDiagnosticsUnit CreateInstance(string name, Func<bool> atomicTest);
    }
}