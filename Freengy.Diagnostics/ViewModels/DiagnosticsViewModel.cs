// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.ViewModels 
{
    using System.Windows.Data;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;

    using Freengy.Base.ViewModels;
    using Freengy.Diagnostics.Helpers;
    using Freengy.Diagnostics.Interfaces;

    using Catel.IoC;
    using Catel.Data;
    using Catel.MVVM;
    

    internal class DiagnosticsViewModel : WaitableViewModel
    {
        private readonly IDiagnosticsController diagnosticsController;
        private readonly ObservableCollection<DiagnosticsCategoryDecorator> diagnosticsCategories = new ObservableCollection<DiagnosticsCategoryDecorator>();


        public DiagnosticsViewModel() 
        {
            this.diagnosticsController = base.serviceLocator.ResolveType<IDiagnosticsController>();
            this.DiagnosticsCategories = CollectionViewSource.GetDefaultView(this.diagnosticsCategories);
        }

        protected override void SetupCommands() 
        {
            this.CommandRunUnits = new Command<DiagnosticsCategoryDecorator>(this.RunUnitsImpl);
        }

        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            this.FillCategories();
        }


        public Command<DiagnosticsCategoryDecorator> CommandRunUnits { get; private set; }


        public ICollectionView DiagnosticsCategories { get; }

        public bool IsCategoryFilterEmpty => string.IsNullOrWhiteSpace(this.CategoryFilter);

        public string CategoryFilter 
        {
            get { return (string)GetValue(CategoryFilterProperty); }

            set
            {
                SetValue(CategoryFilterProperty, value);

                this.DiagnosticsCategories.Filter =
                    category =>
                    {
                        var decorator = category as DiagnosticsCategoryDecorator;

                        if (decorator == null) return false;

                        if (string.IsNullOrWhiteSpace(value)) return true;

                        return decorator.DisplayedName.Contains(value);
                    };

                RaisePropertyChanged(nameof(this.IsCategoryFilterEmpty));
            }
        }

        public static readonly PropertyData CategoryFilterProperty =
            ModelBase.RegisterProperty<DiagnosticsViewModel, string>(diagViewModel => diagViewModel.CategoryFilter, () => string.Empty);


        private void FillCategories() 
        {
            var registeredCategories = this.diagnosticsController.GetAllCategories();

            base
                .guiDispatcher
                .InvokeOnGuiThread
                (
                    () =>
                    {
                        foreach (var category in registeredCategories)
                        {
                            this.diagnosticsCategories.Add(new DiagnosticsCategoryDecorator(category));
                        }
                    }
                );
        }

        private void RunUnitsImpl(DiagnosticsCategoryDecorator categoryDecorator) 
        {
            foreach (DiagnosticsUnitViewModel testUnitViewModel in categoryDecorator.UnitViewModels)
            {
                testUnitViewModel.Run();
            }
        }
        private bool CanRunUnits(IDiagnosticsCategory category) 
        {
            return category != null;
        }
    }
}