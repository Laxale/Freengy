﻿// Created by Laxale 17.11.2016
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
        private readonly IDiagnosticsCategory category;


        public DiagnosticsCategoryDecorator(IDiagnosticsCategory category) 
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            this.category = category;

            this.UnitViewModels = category.TestUnits.Select(unit => new DiagnosticsUnitViewModel(unit)).ToArray();
        }


        public string Description => this.category.Description;

        public string DisplayedName => this.category.DisplayedName;

        public IEnumerable<DiagnosticsUnitViewModel> UnitViewModels { get; }
    }
}