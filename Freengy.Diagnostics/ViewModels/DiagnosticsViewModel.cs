// Created by Laxale 15.11.2016
//
//


using Freengy.Base.Helpers;

namespace Freengy.Diagnostics.ViewModels 
{
    using System.Windows.Data;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;

    using Base.ViewModels;
    using Helpers;
    using Interfaces;

    using Catel.IoC;
    using Catel.Data;
    using Catel.MVVM;
    

    internal class DiagnosticsViewModel : WaitableViewModel 
    {
        private readonly IDiagnosticsController diagnosticsController;

        private readonly ObservableCollection<DiagnosticsCategoryDecorator> diagnosticsCategories =
            new ObservableCollection<DiagnosticsCategoryDecorator>();

        private string categoryFilter;


        public DiagnosticsViewModel() 
        {
            diagnosticsController = ServiceLocatorProperty.ResolveType<IDiagnosticsController>();
            DiagnosticsCategories = CollectionViewSource.GetDefaultView(diagnosticsCategories);
        }


        #region override

        protected override void SetupCommands() 
        {
            CommandRaiseCanRunUnits = new MyCommand(arg => CommandRunUnits.RaiseCanExecuteChanged());
            CommandRunUnits = new MyCommand(RunUnitsImpl, CanRunUnits);
        }

        /// <inheritdoc />
        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            FillCategories();
        }

        #endregion override


        #region commands

        public MyCommand CommandRaiseCanRunUnits { get; private set; }

        public MyCommand CommandRunUnits { get; private set; }

        #endregion commands

        
        public string CategoryFilter 
        {
            get => categoryFilter;

            set
            {
                if (categoryFilter == value) return;

                categoryFilter = value;

                SetCategoryFilter(value);

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCategoryFilterEmpty));
            }
        }

        public bool ShowWindowTitle => false;

        public ICollectionView DiagnosticsCategories { get; }

        public bool IsCategoryFilterEmpty => string.IsNullOrWhiteSpace(CategoryFilter);


        public static readonly PropertyData CategoryFilterProperty =
            ModelBase.RegisterProperty<DiagnosticsViewModel, string>(diagViewModel => diagViewModel.CategoryFilter, () => string.Empty);


        private void SetCategoryFilter(string value) 
        {
            DiagnosticsCategories.Filter =
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
            var registeredCategories = diagnosticsController.GetAllCategories();

            GUIDispatcher
                .InvokeOnGuiThread
                (
                    () =>
                    {
                        foreach (var category in registeredCategories)
                        {
                            var categoryDecorator = new DiagnosticsCategoryDecorator(category);
                            categoryDecorator.PropertyChanged += CategoryPropertyListener;
                            diagnosticsCategories.Add(categoryDecorator);
                        }
                    }
                );
        }
        private void CategoryPropertyListener(object sender, PropertyChangedEventArgs args) 
        {
            var senderDecorator = (DiagnosticsCategoryDecorator)sender;

            if (args.PropertyName != nameof(senderDecorator.IsRunningUnits)) return;

            CommandRunUnits.RaiseCanExecuteChanged();
        }

        private void RunUnitsImpl(object categoryDecorator) 
        {
            foreach (DiagnosticsUnitViewModel testUnitViewModel in ((DiagnosticsCategoryDecorator)categoryDecorator).UnitViewModels)
            {
                testUnitViewModel.Run();
            }
        }
        private bool CanRunUnits(object category) 
        {
            return category != null && !((DiagnosticsCategoryDecorator)category).IsRunningUnits;
        }
    }
}