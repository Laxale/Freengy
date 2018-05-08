// Created by Laxale 15.11.2016
//
//

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Freengy.Base.DefaultImpl;
using Freengy.Base.Interfaces;
using Freengy.Diagnostics.Interfaces;
using Freengy.Diagnostics.ViewModels;
using Freengy.Diagnostics.Views;


namespace Freengy.Diagnostics.DefaultImpl 
{
    internal class DiagnosticsController : IDiagnosticsController
    {
        private readonly IGuiDispatcher guiDispatcher;

        private static readonly object Locker = new object();

        private readonly IDictionary<string, IDiagnosticsCategory> diagnosticsCategories = new Dictionary<string, IDiagnosticsCategory>();


        #region Singleton

        private static IDiagnosticsController instance;

        private DiagnosticsController() 
        {
            guiDispatcher = MyServiceLocator.Instance.Resolve<IGuiDispatcher>();
        }

        public static IDiagnosticsController Instance => instance ?? (instance = new DiagnosticsController());

        #endregion Singleton


        public async Task ShowDialogAsync() 
        {
            await 
                Task.Factory.StartNew(() =>
                {
                    guiDispatcher.InvokeOnGuiThread
                    (
                        () =>
                        {
                            new DiagnosticsWindow().ShowDialog();
                        });
                    }
                );
        }

        public Task ShowDialogAsync(IDiagnosticsCategory diagnosticsCategory) 
        {
            throw new NotImplementedException();
        }

        public void RegisterCategory(IDiagnosticsCategory diagnosticsCategory) 
        {
            ThrowIfInvalidCategory(diagnosticsCategory);

            lock (Locker)
            {
                if (diagnosticsCategories.Keys.Contains(diagnosticsCategory.Name)) return;

                diagnosticsCategories.Add(diagnosticsCategory.Name, diagnosticsCategory);
            }
        }

        public bool IsCategoryRegistered(string diagnosticsCategoryName) 
        {
            if (string.IsNullOrWhiteSpace(diagnosticsCategoryName)) throw new ArgumentNullException(nameof(diagnosticsCategoryName));

            lock (Locker)
            {
                bool isRegistered = diagnosticsCategories.ContainsKey(diagnosticsCategoryName);

                return isRegistered;
            }
        }

        public bool IsCategoryRegistered(IDiagnosticsCategory diagnosticsCategory) 
        {
            if (diagnosticsCategory == null) throw new ArgumentNullException(nameof(diagnosticsCategory));
            if (string.IsNullOrWhiteSpace(diagnosticsCategory.Name)) throw new ArgumentNullException(nameof(diagnosticsCategory.Name));

            lock (Locker)
            {
                bool isRegistered = diagnosticsCategories.ContainsKey(diagnosticsCategory.Name);

                return isRegistered;
            }
        }

        public void UnregisterCategory(string diagnosticsCategoryName) 
        {
            if (string.IsNullOrWhiteSpace(diagnosticsCategoryName)) throw new ArgumentNullException(nameof(diagnosticsCategoryName));

            TryRemoveUnitByName(diagnosticsCategoryName);
        }

        public void UnregisterCategory(IDiagnosticsCategory diagnosticsCategory) 
        {
            if (diagnosticsCategory == null) throw new ArgumentNullException(nameof(diagnosticsCategory));
            if (string.IsNullOrWhiteSpace(diagnosticsCategory.Name)) throw new ArgumentNullException(nameof(diagnosticsCategory.Name));

            TryRemoveUnitByName(diagnosticsCategory.Name);
        }

        public IEnumerable<IDiagnosticsCategory> GetAllCategories() 
        {
            lock (Locker)
            {
                return diagnosticsCategories.Values.ToArray();
            }
        }


        private void TryRemoveUnitByName(string diagnosticsUnitName) 
        {
            lock (Locker)
            {
                try
                {
                    diagnosticsCategories.Remove(diagnosticsUnitName);
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