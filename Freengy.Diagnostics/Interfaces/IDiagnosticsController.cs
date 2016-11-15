// Created by Laxale 15.11.2016
//
//


namespace Freengy.Diagnostics.Interfaces 
{
    using System.Threading.Tasks;


    public interface IDiagnosticsController 
    {
        Task ShowDialogAsync();

        /// <summary>
        /// Show controller dialog with selected unit in it
        /// </summary>
        /// <param name="diagnosticsCategory"><see cref="IDiagnosticsCategory"/> object to be selected on dialog start</param>
        Task ShowDialogAsync(IDiagnosticsCategory diagnosticsCategory);

        void RegisterCategory(IDiagnosticsCategory diagnosticsCategory);

        bool IsCategoryRegistered(string diagnosticsCategoryName);
        bool IsCategoryRegistered(IDiagnosticsCategory diagnosticsCategory);

        void UnregisterCategory(string diagnosticsCategoryName);
        void UnregisterCategory(IDiagnosticsCategory diagnosticsCategory);
    }
}