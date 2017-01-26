﻿// Created by Laxale 15.11.2016
//
//


namespace Freengy.GameList.Diagnostics 
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Freengy.Base.Interfaces;
    using Freengy.Settings.Interfaces;
    using Freengy.Settings.ModuleSettings;

    using Freengy.Diagnostics.Interfaces;

    using Catel.IoC;

    using LocalRes = Freengy.GameList.Resources;


    internal class GameListDiagnosticsCategory : IDiagnosticsCategory 
    {
        #region vars

        private static readonly string GameFolderUnitName = LocalRes.WhatIsYourGameFolderText;
        private static readonly string TestAssembliesUnitName = "Are there assemblies in game folder?";
        private static readonly string TestGameConfigPairsUnitName = "Are there game assemblies with configs in game folder?";

        private readonly ISettingsFacade settingsFacade;
        private readonly IDiagnosticsUnitFactory unitFactory;
        private readonly IAppDirectoryInspector appDirectoryInspector;
        private readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        private readonly List<IDiagnosticsUnit> diagnosticUnits = new List<IDiagnosticsUnit>();

        #endregion vars


        internal GameListDiagnosticsCategory()
        {
            this.settingsFacade = this.serviceLocator.ResolveType<ISettingsFacade>();
            this.unitFactory = this.serviceLocator.ResolveType<IDiagnosticsUnitFactory>();
            this.appDirectoryInspector = this.serviceLocator.ResolveType<IAppDirectoryInspector>();

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
            var gamelistUnit = this.settingsFacade.GetUnit<GameListSettingsUnit>();

            this.diagnosticUnits.First(unit => unit.Name == GameFolderUnitName).ResultInfo = gamelistUnit.GamesFolderPath;

            return true;
        }
        private bool AnyAssembliesAtomicTest() 
        {
            System.Threading.Thread.Sleep(1000);

            string gamesPath = this.settingsFacade.GetUnit<GameListSettingsUnit>().GamesFolderPath;
            IEnumerable<string> dllsInGamesPath = this.appDirectoryInspector.GetDllsInFolder(gamesPath).ToArray();

            bool foundDlls = dllsInGamesPath.Any();

            string message =
                foundDlls ? 
                    $"Found assemblies in game folder:{ Environment.NewLine } { string.Join(Environment.NewLine, dllsInGamesPath) }" : 
                    "Nothing";

            this
                .diagnosticUnits
                .First(unit => unit.Name == TestAssembliesUnitName)
                .ResultInfo = message;

            return foundDlls;
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