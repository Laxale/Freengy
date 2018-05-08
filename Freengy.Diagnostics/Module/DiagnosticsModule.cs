// Created by Laxale 15.11.2016
//
//

using System;

using Freengy.Base.DefaultImpl;
using Freengy.Diagnostics.Interfaces;
using Freengy.Diagnostics.DefaultImpl;
using Freengy.Common.Helpers;

using Prism.Modularity;


namespace Freengy.Diagnostics.Module
{   

    public sealed class DiagnosticsModule : IModule 
    {        
        public void Initialize() 
        {
            using (new StatisticsDeployer(nameof(DiagnosticsModule)))
            {
                MyServiceLocator.Instance.RegisterInstance(DiagnosticsController.Instance);
                MyServiceLocator.Instance.RegisterIfNotRegistered<IDiagnosticsUnitFactory, DiagnosticsUnitFactory>();
            }
        }
    }
}