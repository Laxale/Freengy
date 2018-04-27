// Created by Laxale 15.11.2016
//
//

using System;

using Freengy.Diagnostics.Views;
using Freengy.Diagnostics.Interfaces;
using Freengy.Diagnostics.ViewModels;
using Freengy.Diagnostics.DefaultImpl;

using Catel.IoC;
using Catel.Services;
using Freengy.Base.Helpers;
using Freengy.Common.Helpers;
using Prism.Modularity;


namespace Freengy.Diagnostics.Module
{   

    public sealed class DiagnosticsModule : IModule 
    {
//        #region Singleton
//
//        private static DiagnosticsModule instance;
//
//        private DiagnosticsModule() 
//        {
//
//        }
//
//        public static DiagnosticsModule Instance => 
//            DiagnosticsModule.instance ?? (DiagnosticsModule.instance = new DiagnosticsModule());
//
//        #endregion Singleton

        
        public void Initialize() 
        {
            using (new StatisticsDeployer(nameof(DiagnosticsModule)))
            {
                ServiceLocator.Default.RegisterInstance(DiagnosticsController.Instance);
                ServiceLocator.Default.RegisterType<IDiagnosticsUnitFactory, DiagnosticsUnitFactory>(RegistrationType.Transient);
            }
        }
    }
}