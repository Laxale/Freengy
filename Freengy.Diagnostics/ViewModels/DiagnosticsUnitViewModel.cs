// Created by Laxale 16.11.2016
//
//


namespace Freengy.Diagnostics.ViewModels 
{
    using System;

    using Freengy.Base.ViewModels;
    using Freengy.Diagnostics.Interfaces;
    
    using Catel.Data;


    public class DiagnosticsUnitViewModel : WaitableViewModel 
    {
        private readonly IDiagnosticsUnit diagnosticsUnit;


        public DiagnosticsUnitViewModel(IDiagnosticsUnit diagnosticsUnit) 
        {
            this.diagnosticsUnit = diagnosticsUnit;
        }


        public void Run()
        {
            Action runAction =
                () =>
                {
                    this.IsRunning = true;

                    this.diagnosticsUnit.UnitTest()
                };
        }

        protected override void SetupCommands() 
        {
            
        }


        export snippet
        public type PropName 
        {
            get { return (type)GetValue(Property); }
            private set { SetValue(Property, value); }
        }

        public static readonly PropertyData Property =
            ModelBase.RegisterProperty<viewmodelType, bool>(viewModel => viewModel.PropName, () => type);

        


        public bool IsFinished { get; private set; }

        public bool Succeeded { get; private set; }

        public string UnitName => this.diagnosticsUnit.Name;


        public static readonly PropertyData IsRunningProperty =
            ModelBase.RegisterProperty<DiagnosticsUnitViewModel, bool>(diagViewModel => diagViewModel.IsRunning, () => false);
    }
}