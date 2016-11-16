﻿// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.DefaultImpl 
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Freengy.Base.Interfaces;
    using Freengy.Diagnostics.Interfaces;
    using Freengy.Diagnostics.ViewModels;

    using Catel.IoC;
    using Catel.Services;


    internal class DiagnosticsController : IDiagnosticsController
    {
        private readonly IGuiDispatcher guiDispatcher;
        private readonly IUIVisualizerService uiVisualizer;

        private static readonly object Locker = new object();

        private readonly IDictionary<string, IDiagnosticsCategory> diagnosticUnits = new Dictionary<string, IDiagnosticsCategory>();


        #region Singleton

        private static IDiagnosticsController instance;

        private DiagnosticsController() 
        {
            this.guiDispatcher = ServiceLocator.Default.ResolveType<IGuiDispatcher>();
            this.uiVisualizer = ServiceLocator.Default.ResolveType<IUIVisualizerService>();
        }

        public static IDiagnosticsController Instance =>
            DiagnosticsController.instance ?? (DiagnosticsController.instance = new DiagnosticsController());

        #endregion Singleton


        public async Task ShowDialogAsync()
        {
            await 
                Task
                .Factory
                .StartNew
                (
                    () =>
                    {
                        this.guiDispatcher.InvokeOnGuiThread
                        (
                            () => this.uiVisualizer.ShowAsync<DiagnosticsViewModel>()
                        );
                    }
                );
        }

        public Task ShowDialogAsync(IDiagnosticsCategory diagnosticsCategory) 
        {
            throw new NotImplementedException();
        }

        public void RegisterCategory(IDiagnosticsCategory diagnosticsCategory) 
        {
            this.ThrowIfInvalidCategory(diagnosticsCategory);

            lock (Locker)
            {
                if (this.diagnosticUnits.Keys.Contains(diagnosticsCategory.Name)) return;

                this.diagnosticUnits.Add(diagnosticsCategory.Name, diagnosticsCategory);
            }
        }

        public bool IsCategoryRegistered(string diagnosticsCategoryName) 
        {
            if (string.IsNullOrWhiteSpace(diagnosticsCategoryName)) throw new ArgumentNullException(nameof(diagnosticsCategoryName));

            lock (Locker)
            {
                bool isRegistered = this.diagnosticUnits.ContainsKey(diagnosticsCategoryName);

                return isRegistered;
            }
        }

        public bool IsCategoryRegistered(IDiagnosticsCategory diagnosticsCategory) 
        {
            if (diagnosticsCategory == null) throw new ArgumentNullException(nameof(diagnosticsCategory));
            if (string.IsNullOrWhiteSpace(diagnosticsCategory.Name)) throw new ArgumentNullException(nameof(diagnosticsCategory.Name));

            lock (Locker)
            {
                bool isRegistered = this.diagnosticUnits.ContainsKey(diagnosticsCategory.Name);

                return isRegistered;
            }
        }

        public void UnregisterCategory(string diagnosticsCategoryName) 
        {
            if (string.IsNullOrWhiteSpace(diagnosticsCategoryName)) throw new ArgumentNullException(nameof(diagnosticsCategoryName));

            this.TryRemoveUnitByName(diagnosticsCategoryName);
        }

        public void UnregisterCategory(IDiagnosticsCategory diagnosticsCategory) 
        {
            if (diagnosticsCategory == null) throw new ArgumentNullException(nameof(diagnosticsCategory));
            if (string.IsNullOrWhiteSpace(diagnosticsCategory.Name)) throw new ArgumentNullException(nameof(diagnosticsCategory.Name));

            this.TryRemoveUnitByName(diagnosticsCategory.Name);
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

        private void ThrowIfInvalidCategory(IDiagnosticsCategory diagnosticsCategory) 
        {
            if (diagnosticsCategory == null) throw new ArgumentNullException(nameof(diagnosticsCategory));
            if (string.IsNullOrWhiteSpace(diagnosticsCategory.Name)) throw new ArgumentNullException(nameof(diagnosticsCategory.Name));
            if (diagnosticsCategory.TestUnits == null) throw new ArgumentNullException(nameof(diagnosticsCategory.TestUnits));
            if (!diagnosticsCategory.TestUnits.Any()) throw new InvalidOperationException("TestUnits are empty!");
        }
    }
}