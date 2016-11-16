// Created by Laxale 16.11.2016
//
//


namespace Freengy.Diagnostics.ViewModels 
{
    using Freengy.Diagnostics.Interfaces;

    using Catel.MVVM;


    public class DiagnosticsUnitViewModel : ViewModelBase 
    {
        private readonly IDiagnosticsUnit diagnosticsUnit;


        public DiagnosticsUnitViewModel(IDiagnosticsUnit diagnosticsUnit) 
        {
            this.diagnosticsUnit = diagnosticsUnit;
        }


        public bool IsRunning { get; private set; }

        public bool IsFinished { get; private set; }

        public bool Succeeded { get; private set; }

        public string Name => this.diagnosticsUnit.Name;
    }
}