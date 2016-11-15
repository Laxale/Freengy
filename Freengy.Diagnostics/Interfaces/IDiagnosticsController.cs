// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.Interfaces 
{
    public interface IDiagnosticsController 
    {
        /// <summary>
        /// Show controller dialog with selected unit in it
        /// </summary>
        /// <param name="diagnosticsUnit"><see cref="IDiagnosticsUnit"/> object to be selected on dialog start</param>
        void ShowDialogAsync(IDiagnosticsUnit diagnosticsUnit);

        void RegisterUnit(IDiagnosticsUnit diagnosticsUnit);

        bool IsUnitRegistered(string diagnosticsUnitName);
        bool IsUnitRegistered(IDiagnosticsUnit diagnosticsUnit);

        void UnregisterUnit(string diagnosticsUnitName);
        void UnregisterUnit(IDiagnosticsUnit diagnosticsUnit);
    }
}