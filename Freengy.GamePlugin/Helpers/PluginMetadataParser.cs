// Created by Laxale 28.10.2016
//
//


namespace Freengy.GamePlugin.Helpers 
{
    using System;
    using System.IO;
    using System.Text;
    using System.Linq;

    using Freengy.GamePlugin.Attributes;


    /// <summary>
    /// Metadata parser. Allows to find metainformation without loading assembly
    /// <para>Loading assembly is not welcome, as it cannot be unloaded from application - only whole assemblly's AppDomain</para>
    /// </summary>
    internal class PluginMetadataParser 
    {
        #region vars

        private bool isParsed;

        private string solidTypesLine;
        private string[] typeLinesArray;
        private readonly string assemblyPath;

        private const string DllExtension = ".dll";
        private static readonly string MainViewAttributeName = typeof(MainGameViewAttribute).Name;

        /// <summary>
        /// Plugin metadata contains meaningful information before this substring
        /// </summary>
        private const string EndOfTypesLine = ".??5";
        /// <summary>
        /// Plugin metadata contains meaningful information after this substring
        /// </summary>
        private const string StartOfTypesLine = "<Module>";

        private const string StartOfViewTypeFullNameLine = "%";

        #endregion vars

        
        public PluginMetadataParser(string assemblyPath) 
        {
            if (string.IsNullOrWhiteSpace(assemblyPath)) throw new ArgumentNullException(nameof(assemblyPath));

            if (!assemblyPath.EndsWith(DllExtension))
            {
                throw new ArgumentException($"File '{ assemblyPath }' is not a valid assembly");
            }

            this.assemblyPath = assemblyPath;
        }


        public string GetMainPluginViewName() 
        {
            if (!this.isParsed) this.ParseFile();

            string mainPluginViewName = this.typeLinesArray.FirstOrDefault(line => line.StartsWith(StartOfViewTypeFullNameLine));
            
            if (mainPluginViewName == null)
            {
                throw new ArgumentException("Main view full name was not found in metadata. Assembly is invalid?");
            }

            return mainPluginViewName.Replace(StartOfViewTypeFullNameLine, string.Empty);
        }


        private void ParseFile() 
        {
            this.solidTypesLine = this.GetTypesLine();

            this.typeLinesArray = this.SplitAndFilterTypeLine(this.solidTypesLine);

            this.isParsed = true;
        }
        
        /// <summary>
        /// Get string that contains important assembly's types information
        /// </summary>
        /// <returns>Types information solid string</returns>
        private string GetTypesLine() 
        {
            string assemblyText = File.ReadAllText(this.assemblyPath, Encoding.ASCII);
            string typeLine = assemblyText.Split(new string[] { StartOfTypesLine, EndOfTypesLine }, StringSplitOptions.RemoveEmptyEntries)[1];

            return typeLine;
        }

        private string[] SplitAndFilterTypeLine(string typeLine) 
        {
            var builder = new StringBuilder();

            char[] array = typeLine.ToCharArray();

            foreach (char symbol in array)
            {
                if (char.IsLetter(symbol) || char.IsDigit(symbol) || char.IsPunctuation(symbol) || symbol == '\0')
                {
                    builder.Append(symbol);
                }
            }

            var typeStrings = builder.ToString().Split('\0');

            return typeStrings;
        }
    }
}