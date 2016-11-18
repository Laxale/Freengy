// Created by Laxale 18.11.2016
//
//


namespace Freengy.FriendList.Diagnostics 
{
    using System.Collections.Generic;

    using Freengy.Diagnostics.Interfaces;

    using Catel.IoC;

    using LocalRes = FriendList.Resources;


    internal class FriendListDiagnosticsCategory : IDiagnosticsCategory 
    {
        private readonly IDiagnosticsUnitFactory unitFactory;
        private readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        private readonly List<IDiagnosticsUnit> diagnosticUnits = new List<IDiagnosticsUnit>();


        internal FriendListDiagnosticsCategory() 
        {
            this.unitFactory = this.serviceLocator.ResolveType<IDiagnosticsUnitFactory>();

            this.FillUnits();
        }


        #region properties

        public string DisplayedName { get; } = LocalRes.DiagnosticsCategoryName;

        public string Name { get; } = typeof(FriendListDiagnosticsCategory).FullName;

        public string Description { get; } = LocalRes.DiagnosticsCategoryDescription;

        public IEnumerable<IDiagnosticsUnit> TestUnits => this.diagnosticUnits;

        #endregion properties


        private void FillUnits() 
        {
            var testAssembliesUnit = this.unitFactory.CreateInstance("Wut?", this.AnyAssembliesAtomicTest);
            var testGameDllsUnit = this.unitFactory.CreateInstance("Are there any wut?", this.AnyGameDllsAtomicTest);
            var testGameConfigsUnit = this.unitFactory.CreateInstance("Are there wut wut?", this.ANyGameConfigsAtomicTest);
            
            this.diagnosticUnits.Add(testAssembliesUnit);
            this.diagnosticUnits.Add(testGameDllsUnit);
            this.diagnosticUnits.Add(testGameConfigsUnit);
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