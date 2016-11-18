// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.Interfaces 
{
    using System;
    
    using Freengy.Base.Interfaces;


    public interface IDiagnosticsUnit : INamedObject 
    {
        Func<bool> UnitTest { get; }

        string ResultInfo { get; set; }
    }
}