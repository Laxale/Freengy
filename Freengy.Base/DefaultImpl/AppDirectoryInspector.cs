// Created by Laxale 25.10.2016
//
//

using System;
using System.IO;
using System.Collections.Generic;

using Freengy.Base.Helpers;
using Freengy.Base.Messages;
using Freengy.Base.Interfaces;


namespace Freengy.Base.DefaultImpl 
{    
    /// <summary>
    /// Default <see cref="IAppDirectoryInspector"/> implementer
    /// </summary>
    public class AppDirectoryInspector : IAppDirectoryInspector
    {
        private readonly FileSystemWatcher watcher;


        public AppDirectoryInspector() 
        {
            this.watcher = new FileSystemWatcher(this.WorkingDirectoryPath)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true
            };
            this.watcher.Created += this.FileSystemListener;

            this.WorkingDirectoryName = new DirectoryInfo(this.WorkingDirectoryPath).Name;
        }


        public string WorkingDirectoryName { get; }

        public string WorkingDirectoryPath => Environment.CurrentDirectory;

        public IEnumerable<string> GetDllsInFolder(string folderPath) 
        {
            if (string.IsNullOrWhiteSpace(folderPath)) throw new ArgumentNullException(nameof(folderPath));

            if (!Directory.Exists(folderPath)) throw new DirectoryNotFoundException(nameof(folderPath));

            IEnumerable<string> foundDlls = Directory.EnumerateFiles(folderPath, "*.dll");

            return foundDlls;
        }

        public IEnumerable<string> GetDllsInSubFolder(string subFolderName) 
        {
            return this.GetFilesInSubfolderByFilter(subFolderName, "*.dll");
        }

        public IEnumerable<string> GetExecutablesInSubFolder(string subFolderName) 
        {
            return this.GetFilesInSubfolderByFilter(subFolderName, "*.exe");
        }

        public IEnumerable<string> GetAnyFilesInSubFolder(string subFolderName, FileSearchFilterBase filter) 
        {
            return this.GetFilesInSubfolderByFilter(subFolderName, filter?.SearchFilter ?? "*");
        }


        private void FileSystemListener(object sender, FileSystemEventArgs args) 
        {
            var changedMessage = new MessageWorkingDirectoryChanged(args);
            this.Publish(changedMessage);
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