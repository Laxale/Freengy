// Created by Laxale 17.11.2016
//
//


namespace Freengy.Diagnostics.Helpers 
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Freengy.Diagnostics.Interfaces;
    using Freengy.Diagnostics.ViewModels;


    internal class DiagnosticsCategoryDecorator 
    {
        private IDiagnosticsCategory category;


        public DiagnosticsCategoryDecorator(IDiagnosticsCategory category) 
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            this.category = category;

            this.UnitViewModels = category.TestUnits.Select(unit => new DiagnosticsUnitViewModel(unit));
        }


        public string DisplayedName => this.category.DisplayedName;

        public IEnumerable<DiagnosticsUnitViewModel> UnitViewModels { get; }
    }
}