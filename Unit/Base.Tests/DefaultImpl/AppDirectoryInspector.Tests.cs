// Created by Laxale 25.10.2016
//
//


namespace Base.Tests.DefaultImpl 
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;

    using Freengy.Base.Interfaces;
    using Freengy.Base.DefaultImpl;

    using NUnit.Framework;
    

    [TestFixture]
    public class AppDirectoryInspectorTests 
    {
        #region vars

        private readonly IAppDirectoryInspector inspector = new AppDirectoryInspector();

        private string emptyDirectoryName;
        private string emptyDirectoryPath;
        private string goodDirectoryName;
        private string goodDirectoryPath;
        private const string GoodDirectoryCommand = "good";
        private const string EmptyDirectoryCommand = "empty";
        private const string FirstDllName = "firstDll.dll";
        private const string SecondDllName = "secondDll.dll";
        private const string FirstExeName = "firstExe.exe";
        private const string SecondExeName = "secondExe.exe";
        private readonly string workingDirectoryPath = Environment.CurrentDirectory;
        private readonly string workingDirectoryName = Environment.CurrentDirectory.Split('\\', '/').Last();

        private readonly IDictionary<string, string> folderShortcuts = new Dictionary<string, string>();
        
        #endregion vars
        

        [OneTimeSetUp]
        public void SetUp() 
        {
            var testId = Guid.NewGuid();
            this.goodDirectoryName  = $"InspectorTest_Good.{ testId }";
            this.emptyDirectoryName = $"InspectorTest_Emty.{ testId }";

            this.goodDirectoryPath  = Path.Combine(Environment.CurrentDirectory, this.goodDirectoryName);
            this.emptyDirectoryPath = Path.Combine(Environment.CurrentDirectory, this.emptyDirectoryName);
            
            Directory.CreateDirectory(this.goodDirectoryPath);
            Directory.CreateDirectory(this.emptyDirectoryPath);

            this.folderShortcuts.Add(AppDirectoryInspectorTests.GoodDirectoryCommand, this.goodDirectoryPath);
            this.folderShortcuts.Add(AppDirectoryInspectorTests.EmptyDirectoryCommand, this.emptyDirectoryPath);

            this.CreateTestDlls();
            this.CreateTestExecutables();
        }

        [OneTimeTearDown]
        public void TearDown() 
        {
            Directory.Delete(this.emptyDirectoryPath);
            Directory.Delete(this.goodDirectoryPath, true);
        }


        [Test(Description = "Inspector must know a current working directory name")]
        public void WorkingDirectoryName_Test() 
        {
            Assert.AreEqual(this.workingDirectoryName, this.inspector.WorkingDirectoryName);
        }

        [Test(Description = "Inspector must know a current working directory path")]
        public void WorkingDirectoryPath_Test() 
        {
            Assert.AreEqual(this.workingDirectoryPath, this.inspector.WorkingDirectoryPath);
        }

        [Test(Description = "Try to search dll in null subfolder")]
        public void GetDllsInSubFolder_NullCase() 
        {
            Assert.Throws<ArgumentNullException>(() => this.inspector.GetDllsInSubFolder(null));
        }

        [Test(Description = "Try to search dll in not existing subfolder")]
        public void GetDllsInSubFolder_DoesntExistCase() 
        {
            Assert.Throws<DirectoryNotFoundException>(() => this.inspector.GetDllsInSubFolder("i dont exist!"));
        }


        [TestCase("",      -1,    Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryBase,
                                  Description = "Try to search dll in root subfolder", 
                                  Ignore = "Root directory of R# environment is not equal to test assembly's one")]
        [TestCase(EmptyDirectoryCommand,  0,    Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryBase,
                                                Description = "Try to search dll in empty subfolder")]
        [TestCase(GoodDirectoryCommand,   2,    Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryBase,
                                                Description = "Try to search dll in good subfolder - it really has 2 dlls")]
        public void GetDllsInSubFolder_Cases(string subFolderCommand, int dllsCount)
        {
            // resharper working directory was
            // .\username\AppData\Local\JetBrains\Installations\ReSharperPlatformVs14
            string folderToSearch = this.folderShortcuts[subFolderCommand];
            
            IEnumerable<string> foundDlls = this.inspector.GetDllsInSubFolder(folderToSearch);

            Assert.AreEqual(dllsCount, foundDlls.Count());
        }


        [TestCase("", -1, Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryBase,
                          Description = "Try to search executables in root subfolder",
                          Ignore = "Root directory of R# environment is not equal to test assembly's one")]
        [TestCase(EmptyDirectoryCommand, 0, Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryBase,
                                        Description = "Try to search executables in empty subfolder")]
        [TestCase(GoodDirectoryCommand, 2, Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryBase,
                                        Description = "Try to search executables in good subfolder - it really has 2 dlls")]
        public void GetExecutablesInSubFolder_Cases(string subFolderCommand, int dllsCount)
        {
            string folderToSearch = this.folderShortcuts[subFolderCommand];

            IEnumerable<string> foundExes = this.inspector.GetExecutablesInSubFolder(folderToSearch);

            Assert.AreEqual(dllsCount, foundExes.Count());
        }


        private void CreateTestDlls() 
        {
            File.WriteAllText(Path.Combine(this.goodDirectoryPath, AppDirectoryInspectorTests.FirstDllName), "not much");
            File.WriteAllText(Path.Combine(this.goodDirectoryPath, AppDirectoryInspectorTests.SecondDllName), "not much either");
        }

        private void CreateTestExecutables() 
        {
            File.WriteAllText(Path.Combine(this.goodDirectoryPath, FirstExeName), "not much");
            File.WriteAllText(Path.Combine(this.goodDirectoryPath, SecondExeName), "not much either");
        }
    }
}