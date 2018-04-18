// Created by Laxale 15.11.2016
//
//

using System;

using Freengy.Common.Interfaces;


namespace Freengy.Diagnostics.Interfaces 
{    
    public interface IDiagnosticsUnit : INamedObject 
    {
        Func<bool> UnitTest { get; }

        string ResultInfo { get; set; }
    }
}