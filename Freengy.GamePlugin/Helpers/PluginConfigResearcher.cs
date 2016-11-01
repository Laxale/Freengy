// Created by Laxale 28.10.2016
//
//


namespace Freengy.GamePlugin.Helpers 
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using Freengy.GamePlugin.Constants;


    /// <summary>
    /// Allows to read provided plugin's config
    /// </summary>
    internal class PluginConfigResearcher 
    {
        #region vars

        private readonly string pluginConfigPath;

        private const string DllExtension = ".dll";
        private const string ConfigExtension = ".config";

        #endregion vars

        
        public PluginConfigResearcher(string pluginAssemblyPath) 
        {
            if (string.IsNullOrWhiteSpace(pluginAssemblyPath)) throw new ArgumentNullException(nameof(pluginAssemblyPath));

            if (!pluginAssemblyPath.EndsWith(DllExtension))
            {
                throw new ArgumentException($"File '{ pluginAssemblyPath }' is not a valid assembly");
            }

            this.pluginConfigPath = $"{ pluginAssemblyPath }{ ConfigExtension }";
        }


        public bool GetMainPluginViewName(out string mainViewTypeName) 
        {
            try
            {
                bool gotViewName = this.GetMainPluginViewNameImpl(out mainViewTypeName);

                return gotViewName;
            }
            catch (Exception)
            {
                // log?
                mainViewTypeName = null;
                return false;
                //throw;
            }
        }

        private bool GetMainPluginViewNameImpl(out string mainViewTypeName) 
        {
            mainViewTypeName = null;

            XDocument config = XDocument.Load(this.pluginConfigPath);

            if (config.Root == null) throw new InvalidOperationException("Config has no root!");
            
            var rootElements = config.Root.Elements();
            var appSettingsSection = rootElements.FirstOrDefault(element => element.Name == "applicationSettings");

            if (appSettingsSection == null) throw new InvalidOperationException("appSettingsSection was not found!");

            string pluginSettingsSectionName = this.GetPluginSettingsSectionName();

            var pluginSettingsSection = appSettingsSection.Elements().FirstOrDefault(element => element.Name == pluginSettingsSectionName);

            if (pluginSettingsSection == null) throw new InvalidOperationException("pluginSettingsSection was not found!");
            
            var mainViewTypeEntry = pluginSettingsSection.Elements().FirstOrDefault(this.IsMainViewTypeEntry);

            if (mainViewTypeEntry == null) throw new InvalidOperationException("mainViewTypeEntry was not found!");

            mainViewTypeName = mainViewTypeEntry.Value;

            return true;
        }

        private string GetPluginSettingsSectionName() 
        {
            string pluginSettingsSectionName =
                new FileInfo(this.pluginConfigPath)
                .Name
                .Replace(DllExtension, ".Settings")
                .Replace(ConfigExtension, string.Empty);

            return pluginSettingsSectionName;
        }

        private bool IsMainViewTypeEntry(XElement element) 
        {
            var attributes = element.Attributes();

            return attributes.Any(attribute => attribute.Value == StringConstants.MainGameViewSettingsKey);
        }
    }
}