// Created by Laxale 15.11.2016
//
//


namespace Freengy.GameList.Diagnostics 
{
    using System;
    using System.Collections.Generic;

    using Freengy.Diagnostics.Interfaces;

    using Catel.IoC;


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


        public IEnumerable<IDiagnosticsUnit> TestUnits => this.diagnosticUnits;

        public string Name { get; } = typeof(GameListDiagnosticsCategory).FullName;


        private void FillUnits() 
        {
            var testGameDllsUnit = this.unitFactory.CreateInstance("Are there game dlls in game folder?", this.GameDllsAtomicTest);

            this.diagnosticUnits.Add(testGameDllsUnit);
        }

        private bool GameDllsAtomicTest() 
        {
            return true;
        }
    }
}