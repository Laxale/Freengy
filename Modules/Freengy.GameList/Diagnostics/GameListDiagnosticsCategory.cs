// Created by Laxale 15.11.2016
//
//


namespace Freengy.GameList.Diagnostics 
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Freengy.Diagnostics.Interfaces;

    using Catel.IoC;

    using LocalRes = Freengy.GameList.Resources;


    internal class GameListDiagnosticsCategory : IDiagnosticsCategory
    {
        private const string GameFolderUnitName = "What is your game folder?";
        private const string TestAssembliesUnitName = "Are there assemblies in game folder?";
        private const string TestGameConfigPairsUnitName = "Are there game assemblies with configs in game folder?";

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
            var gameFolderUnit = this.unitFactory.CreateInstance(GameFolderUnitName, this.GameFolderAtomicUnit);
            var testAssembliesUnit = this.unitFactory.CreateInstance(TestAssembliesUnitName, this.AnyAssembliesAtomicTest);
            var testGameConfigPairsUnit = this.unitFactory.CreateInstance(TestGameConfigPairsUnitName, this.AnyGameDllsAtomicTest);
            
            this.diagnosticUnits.Add(gameFolderUnit);
            this.diagnosticUnits.Add(testAssembliesUnit);
            this.diagnosticUnits.Add(testGameConfigPairsUnit);
        }


        private bool GameFolderAtomicUnit()
        {
            this.diagnosticUnits.First(unit => unit.Name == GameFolderUnitName).ResultInfo =
                "Your game folder is: blabla";

            return true;
        }
        private bool AnyAssembliesAtomicTest() 
        {
            System.Threading.Thread.Sleep(1000);

            this.diagnosticUnits.First(unit => unit.Name == TestAssembliesUnitName).ResultInfo =
                "Found assemblies ig game folder : blabla";

            return false;
        }

        private bool AnyGameDllsAtomicTest() 
        {
            System.Threading.Thread.Sleep(2000);

            this.diagnosticUnits.First(unit => unit.Name == GameFolderUnitName).ResultInfo =
                "Found '{0}' assemblies with game configs";

            return true;
        }
    }
}