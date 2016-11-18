// Created by Laxale 17.11.2016
//
//


namespace Freengy.Diagnostics.Helpers 
{
    using System;
    using System.Linq;
    using System.Windows.Data;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Freengy.Diagnostics.Interfaces;
    using Freengy.Diagnostics.ViewModels;

    using Catel.MVVM;


    internal class DiagnosticsCategoryDecorator : ViewModelBase 
    {
        private readonly IDiagnosticsCategory category;


        public DiagnosticsCategoryDecorator(IDiagnosticsCategory category) 
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            this.category = category;

            this.AttachUnits();
        }


        public string Description => this.category.Description;

        public string DisplayedName => this.category.DisplayedName;

        public IEnumerable<DiagnosticsUnitViewModel> UnitViewModels { get; private set; }

        /// <summary>
        /// Are there any tests running?
        /// </summary>
        public bool IsRunningUnits => this.UnitViewModels.Any(unit => unit.IsRunning);


        private void AttachUnits() 
        {
            this.UnitViewModels = 
                category
                .TestUnits
                .Select
                (
                    unit =>
                    {
                        var unitViewModel = new DiagnosticsUnitViewModel(unit);

                        unitViewModel.PropertyChanged += this.UnitFinishedListener;

                        return unitViewModel;
                    }
                )
                .ToArray();
        }

        private void UnitFinishedListener(object sender, PropertyChangedEventArgs args) 
        {
            var senderUnit = (DiagnosticsUnitViewModel)sender;

            if(args.PropertyName != nameof(senderUnit.IsFinished)) return;
            
            base.RaisePropertyChanged(() => this.IsRunningUnits);
        }
    }
}