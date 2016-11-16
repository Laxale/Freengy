// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.ViewModels 
{
    using System.Windows.Data;
    using System.ComponentModel;
    using System.Collections.ObjectModel;

    using Freengy.Base.ViewModels;
    using Freengy.Diagnostics.Interfaces;

    using Catel.IoC;
    using System.Threading.Tasks;

    internal class DiagnosticsViewModel : WaitableViewModel
    {
        private readonly IDiagnosticsController diagnosticsController;
        private readonly ObservableCollection<IDiagnosticsCategory> diagnosticsCategories = new ObservableCollection<IDiagnosticsCategory>();


        public DiagnosticsViewModel() 
        {
            this.diagnosticsController = base.serviceLocator.ResolveType<IDiagnosticsController>();
            this.DiagnosticsCategories = CollectionViewSource.GetDefaultView(this.diagnosticsCategories);
        }

        protected override void SetupCommands() 
        {
            
        }

        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            this.FillCategories();
        }


        public ICollectionView DiagnosticsCategories { get; }


        private void FillCategories()
        {
            var registeredCategories = this.diagnosticsController.GetCategories();

            base
                .guiDispatcher
                .InvokeOnGuiThread
                (
                    () =>
                    {
                        foreach (var vategory in registeredCategories)
                        {
                            this.diagnosticsCategories.Add(category);
                        }
                    }
                );
        }
    }
}