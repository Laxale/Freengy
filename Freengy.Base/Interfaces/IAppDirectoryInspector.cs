// Created by Laxale 25.10.2016
//
//


namespace Freengy.Base.Interfaces 
{
    using System.Collections.Generic;

    using Freengy.Base.Helpers;


    /// <summary>
    /// Implementer is capable of providing application working directory's information
    /// </summary>
    public interface IAppDirectoryInspector 
    {
        /// <summary>
        /// Application working directory name
        /// </summary>
        string WorkingDirectoryName { get; }

        /// <summary>
        /// Application working directory full path
        /// </summary>
        string WorkingDirectoryPath { get; }

        /// <summary>
        /// Get paths to all .dll assemblies in a given .\WorkingDirectory\subfolderName
        /// </summary>
        /// <param name="subFolderName">Inner directory to inspect</param>
        /// <returns>Paths to all inner .dll assemblies</returns>
        IEnumerable<string> GetDllsInSubFolder(string subFolderName);

        /// <summary>
        /// Get paths to all .exe assemblies in a given .\WorkingDirectory\subfolderName
        /// </summary>
        /// <param name="subFolderName">Inner directory to inspect</param>
        /// <returns>Paths to all inner .exe assemblies</returns>
        IEnumerable<string> GetExecutablesInSubFolder(string subFolderName);

        /// <summary>
        /// Get paths to all files in a given .\WorkingDirectory\subfolderName by filter (file extension or whatever)
        /// </summary>
        /// <param name="subFolderName">Inner directory to inspect</param>
        /// <param name="filter"><see cref="FileSearchFilterBase"/> containing string pattern to use during search</param>
        /// <returns>Paths to all inner files defined by filter</returns>
        IEnumerable<string> GetAnyFilesInSubFolder(string subFolderName, FileSearchFilterBase filter);
    }
}