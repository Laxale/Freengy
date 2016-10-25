// Created by Laxale 25.10.2016
//
//


namespace Freengy.Base.Interfaces 
{
    using System.Collections.Generic;


    /// <summary>
    /// Implementer is capable of providing application working directory's information
    /// </summary>
    public interface IAppDirectoryInspector 
    {
        string WorkingDirectoryName { get; }

        string WorkingDirectoryPath { get; }

        /// <summary>
        /// Get paths to all .dll assemblies in a given .\WorkingDirectory\subfolderName
        /// </summary>
        /// <param name="subFolderName">Inner directory to inspect</param>
        /// <returns>Paths to all inner .dll assemblies</returns>
        IEnumerable<string> GetDllsInSubFolder(string subFolderName);

        IEnumerable<string> GetExecutablesInSubFolder(string subFolderName);
    }
}