// Created by Laxale 25.10.2016
//
//


namespace Freengy.Base.DefaultImpl 
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;

    using Freengy.Base.Interfaces;
    

    /// <summary>
    /// Default <see cref="IAppDirectoryInspector"/> implementer
    /// </summary>
    public class AppDirectoryInspector : IAppDirectoryInspector
    {
        public AppDirectoryInspector() 
        {
            this.WorkingDirectoryName = new DirectoryInfo(Environment.CurrentDirectory).Name;
        }


        public string WorkingDirectoryName { get; }

        public string WorkingDirectoryPath => Environment.CurrentDirectory;

        public IEnumerable<string> GetDllsInSubFolder(string subFolderName)
        {
            return this.GetFilesInSubfolderByFilter(subFolderName, "*.dll");
        }

        public IEnumerable<string> GetExecutablesInSubFolder(string subFolderName) 
        {
            return this.GetFilesInSubfolderByFilter(subFolderName, "*.exe");
        }


        private IEnumerable<string> GetFilesInSubfolderByFilter(string subFolderName, string filter) 
        {
            if (subFolderName == null) throw new ArgumentNullException(nameof(subFolderName));

            string pathToInspect = Path.Combine(this.WorkingDirectoryPath, subFolderName);

            if (!Directory.Exists(pathToInspect)) throw new DirectoryNotFoundException(nameof(subFolderName));

            IEnumerable<string> foundFiles = Directory.EnumerateFiles(pathToInspect, filter);

            return foundFiles;
        }
    }
}