// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.Module
{
    using System;

    using Catel.IoC;

    using Prism.Modularity;

    using Freengy.Diagnostics.Interfaces;
    using Freengy.Diagnostics.DefaultImpl;


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
            ServiceLocator.Default.RegisterInstance(DiagnosticsController.Instance);

            ServiceLocator.Default.RegisterType<IDiagnosticsUnitFactory, DiagnosticsUnitFactory>(RegistrationType.Transient);
        }
    }
}