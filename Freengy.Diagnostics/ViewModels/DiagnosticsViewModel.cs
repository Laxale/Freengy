// Created by Laxale 15.11.2016
//
//

using System.Windows.Data;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Freengy.Base.ViewModels;
using Freengy.Diagnostics.Helpers;
using Freengy.Diagnostics.Interfaces;
using Freengy.Base.Helpers.Commands;


namespace Freengy.Diagnostics.ViewModels 
{
    internal class DiagnosticsViewModel : WaitableViewModel 
    {
        private readonly IDiagnosticsController diagnosticsController;

        private readonly ObservableCollection<DiagnosticsCategoryDecorator> diagnosticsCategories =
            new ObservableCollection<DiagnosticsCategoryDecorator>();

        private string categoryFilter;


        public DiagnosticsViewModel() 
        {
            diagnosticsController = ServiceLocator.Resolve<IDiagnosticsController>();
            DiagnosticsCategories = CollectionViewSource.GetDefaultView(diagnosticsCategories);
        }


        #region override

        protected override void SetupCommands() 
        {
            CommandRaiseCanRunUnits = new MyCommand(() => CommandRunUnits.RaiseCanExecuteChanged());
            CommandRunUnits = new MyCommand<DiagnosticsCategoryDecorator>(RunUnitsImpl, CanRunUnits);
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

        public MyCommand<DiagnosticsCategoryDecorator> CommandRunUnits { get; private set; }

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
                .InvokeOnGuiThread(() =>
                {
                    foreach (var category in registeredCategories)
                    {
                        var categoryDecorator = new DiagnosticsCategoryDecorator(category);
                        categoryDecorator.PropertyChanged += CategoryPropertyListener;
                        diagnosticsCategories.Add(categoryDecorator);
                    }
                });
        }
        private void CategoryPropertyListener(object sender, PropertyChangedEventArgs args) 
        {
            var senderDecorator = (DiagnosticsCategoryDecorator)sender;

            if (args.PropertyName != nameof(senderDecorator.IsRunningUnits)) return;

            CommandRunUnits.RaiseCanExecuteChanged();
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