﻿// Created by Laxale 15.11.2016
//
//


namespace Freengy.GameList.Diagnostics 
{
    using System;
    using System.Collections.Generic;

    using Freengy.Diagnostics.Interfaces;

    using Catel.IoC;

    using LocalRes = Freengy.GameList.Resources;


    internal class GameListDiagnosticsCategory : IDiagnosticsCategory 
    {
        private readonly IDiagnosticsUnitFactory unitFactory;
        private readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        private readonly List<IDiagnosticsUnit> diagnosticUnits = new List<IDiagnosticsUnit>();


        internal GameListDiagnosticsCategory() 
        {
            this.unitFactory = this.serviceLocator.ResolveType<IDiagnosticsUnitFactory>();

            this.FillUnits();
        }


        #region properties

        public string DisplayedName { get; } = LocalRes.DiagnosticsCategoryName;

        public string Name { get; } = typeof(GameListDiagnosticsCategory).FullName;

        public string Description { get; } = LocalRes.DiagnosticsCategoryDescription;

        public IEnumerable<IDiagnosticsUnit> TestUnits => this.diagnosticUnits;

        #endregion properties


        private void FillUnits() 
        {
            var gameFolderUnit = this.unitFactory.CreateInstance("What is your game folder?", this.AnyAssembliesAtomicTest);
            var testAssembliesUnit = this.unitFactory.CreateInstance("Are there assemblies in game folder?", this.AnyAssembliesAtomicTest);
            var testGameDllsUnit = this.unitFactory.CreateInstance("Are there game dlls in game folder?", this.AnyGameDllsAtomicTest);
            var testGameConfigsUnit = this.unitFactory.CreateInstance("Are there game configs in game folder?", this.ANyGameConfigsAtomicTest);
            
            this.diagnosticUnits.Add(gameFolderUnit);
            this.diagnosticUnits.Add(testAssembliesUnit);
            this.diagnosticUnits.Add(testGameDllsUnit);
            this.diagnosticUnits.Add(testGameConfigsUnit);
        }


        private bool GameFolderAtomicUnit() 
        {
            return true;
        }
        private bool AnyAssembliesAtomicTest() 
        {
            System.Threading.Thread.Sleep(1000);

            return false;
        }

        private bool AnyGameDllsAtomicTest() 
        {
            System.Threading.Thread.Sleep(2000);

            return true;
        }

        private bool ANyGameConfigsAtomicTest() 
        {
            System.Threading.Thread.Sleep(3000);

            return false;
        }
    }
}