// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.DefaultImpl
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Freengy.Diagnostics.Interfaces;


    internal class DiagnosticsController : IDiagnosticsController
    {
        private static readonly object Locker = new object();

        private readonly IDictionary<string, IDiagnosticsUnit> diagnosticUnits =
            new Dictionary<string, IDiagnosticsUnit>();


        #region Singleton

        private static IDiagnosticsController instance;

        private DiagnosticsController()
        {

        }

        public static IDiagnosticsController Instance =>
            DiagnosticsController.instance ?? (DiagnosticsController.instance = new DiagnosticsController());

        #endregion Singleton


        public void ShowDialogAsync(IDiagnosticsUnit diagnosticsUnit)
        {
            throw new NotImplementedException();
        }

        public void RegisterUnit(IDiagnosticsUnit diagnosticsUnit) 
        {
            this.ThrowIfInvalidUnit(diagnosticsUnit);

            lock (Locker)
            {
                if (this.diagnosticUnits.Keys.Contains(diagnosticsUnit.Name)) return;

                this.diagnosticUnits.Add(diagnosticsUnit.Name, diagnosticsUnit);
            }
        }

        public bool IsUnitRegistered(string diagnosticsUnitName) 
        {
            if (string.IsNullOrWhiteSpace(diagnosticsUnitName)) throw new ArgumentNullException(nameof(diagnosticsUnitName));

            lock (Locker)
            {
                bool isRegistered = this.diagnosticUnits.ContainsKey(diagnosticsUnitName);

                return isRegistered;
            }
        }

        public bool IsUnitRegistered(IDiagnosticsUnit diagnosticsUnit) 
        {
            if (diagnosticsUnit == null) throw new ArgumentNullException(nameof(diagnosticsUnit));
            if (string.IsNullOrWhiteSpace(diagnosticsUnit.Name)) throw new ArgumentNullException(nameof(diagnosticsUnit.Name));

            lock (Locker)
            {
                bool isRegistered = this.diagnosticUnits.ContainsKey(diagnosticsUnit.Name);

                return isRegistered;
            }
        }

        public void UnregisterUnit(string diagnosticsUnitName) 
        {
            if (string.IsNullOrWhiteSpace(diagnosticsUnitName)) throw new ArgumentNullException(nameof(diagnosticsUnitName));

            this.TryRemoveUnitByName(diagnosticsUnitName);
        }

        public void UnregisterUnit(IDiagnosticsUnit diagnosticsUnit) 
        {
            if (diagnosticsUnit == null) throw new ArgumentNullException(nameof(diagnosticsUnit));
            if (string.IsNullOrWhiteSpace(diagnosticsUnit.Name)) throw new ArgumentNullException(nameof(diagnosticsUnit.Name));

            this.TryRemoveUnitByName(diagnosticsUnit.Name);
        }


        private void TryRemoveUnitByName(string diagnosticsUnitName) 
        {
            lock (Locker)
            {
                try
                {
                    this.diagnosticUnits.Remove(diagnosticsUnitName);
                }
                catch (Exception)
                {
                    // not throwing if key not found
                    throw;
                }
            }
        }

        private void ThrowIfInvalidUnit(IDiagnosticsUnit diagnosticsUnit) 
        {
            if (diagnosticsUnit == null) throw new ArgumentNullException(nameof(diagnosticsUnit));
            if (string.IsNullOrWhiteSpace(diagnosticsUnit.Name)) throw new ArgumentNullException(nameof(diagnosticsUnit.Name));
            if (diagnosticsUnit.TestCases == null) throw new ArgumentNullException(nameof(diagnosticsUnit.TestCases));
            if (!diagnosticsUnit.TestCases.Any()) throw new InvalidOperationException("TestCases are empty!");
        }
    }
}