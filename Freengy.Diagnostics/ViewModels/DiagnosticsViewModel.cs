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
        #region vars

        private readonly IDiagnosticsController diagnosticsController;

        private readonly ObservableCollection<DiagnosticsCategoryDecorator> diagnosticsCategories = 
            new ObservableCollection<DiagnosticsCategoryDecorator>();

        #endregion vars


        public DiagnosticsViewModel() 
        {
            this.diagnosticsController = base.serviceLocator.ResolveType<IDiagnosticsController>();
            this.DiagnosticsCategories = CollectionViewSource.GetDefaultView(this.diagnosticsCategories);
        }


        #region override

        protected override void SetupCommands() 
        {
            this.CommandRaiseCanRunUnits = new Command(() => this.CommandRunUnits.RaiseCanExecuteChanged());
            this.CommandRunUnits = new Command<DiagnosticsCategoryDecorator>(this.RunUnitsImpl, this.CanRunUnits);
        }

        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            this.FillCategories();
        }

        #endregion override


        #region commands

        public Command CommandRaiseCanRunUnits { get; private set; }

        public Command<DiagnosticsCategoryDecorator> CommandRunUnits { get; private set; }

        #endregion commands


        public string CategoryFilter 
        {
            get { return (string)this.GetValue(DiagnosticsViewModel.CategoryFilterProperty); }

            set
            {
                this.SetValue(DiagnosticsViewModel.CategoryFilterProperty, value);

                this.SetCategoryFilter(value);

                this.RaisePropertyChanged(nameof(this.IsCategoryFilterEmpty));
            }
        }

        public bool ShowWindowTitle => false;

        public ICollectionView DiagnosticsCategories { get; }

        public bool IsCategoryFilterEmpty => string.IsNullOrWhiteSpace(this.CategoryFilter);


        public static readonly PropertyData CategoryFilterProperty =
            ModelBase.RegisterProperty<DiagnosticsViewModel, string>(diagViewModel => diagViewModel.CategoryFilter, () => string.Empty);


        private void SetCategoryFilter(string value) 
        {
            this.DiagnosticsCategories.Filter =
                category =>
                {
                    var decorator = category as DiagnosticsCategoryDecorator;

                    if (decorator == null) return false;

                    return 
                        string.IsNullOrWhiteSpace(value) ||
                        decorator.DisplayedName.ToLower().Contains(value.ToLower());
                };
        }

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
                            var categoryDecorator = new DiagnosticsCategoryDecorator(category);
                            categoryDecorator.PropertyChanged += this.CategoryPropertyListener;
                            this.diagnosticsCategories.Add(categoryDecorator);
                        }
                    }
                );
        }
        private void CategoryPropertyListener(object sender, PropertyChangedEventArgs args) 
        {
            var senderDecorator = (DiagnosticsCategoryDecorator)sender;

            if (args.PropertyName != nameof(senderDecorator.IsRunningUnits)) return;

            this.CommandRunUnits.RaiseCanExecuteChanged();
        }

        private void RunUnitsImpl(DiagnosticsCategoryDecorator categoryDecorator) 
        {
            foreach (DiagnosticsUnitViewModel testUnitViewModel in categoryDecorator.UnitViewModels)
            {
                testUnitViewModel.Run();
            }
        }
        private bool CanRunUnits(DiagnosticsCategoryDecorator category) 
        {
            return category != null && !category.IsRunningUnits;
        }
    }
}