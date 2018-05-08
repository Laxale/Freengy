// Created by Laxale 17.11.2016
//
//

using System;
using System.Linq;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Freengy.Base.ViewModels;
using Freengy.Diagnostics.Interfaces;
using Freengy.Diagnostics.ViewModels;


namespace Freengy.Diagnostics.Helpers 
{
    internal class DiagnosticsCategoryDecorator : WaitableViewModel 
    {
        private readonly IDiagnosticsCategory category;


        public DiagnosticsCategoryDecorator(IDiagnosticsCategory category) 
        {
            this.category = category ?? throw new ArgumentNullException(nameof(category));

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
            
            OnPropertyChanged(nameof(IsRunningUnits));
        }
    }
}