// Created by Laxale 01.11.2016
//
//


namespace Freengy.GamePlugin.Helpers 
{
    using System;
    using System.Configuration;

    using Freengy.GamePlugin.Constants;


    /// <summary>
    /// Contains logic to validate plugin settings (config)
    /// </summary>
    public sealed class PluginSettingsValidator 
    {
        private readonly SettingsBase settings;


        public PluginSettingsValidator(SettingsBase settings) 
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            this.settings = settings;
        }


        /// <summary>
        /// Checks if main plugin view name exists in provided settings. If not - adds it
        /// </summary>
        /// <param name="mainViewType">Plugin's main view type</param>
        public void EnsureMainViewIsRegistered(Type mainViewType) 
        {
            if (mainViewType == null) throw new ArgumentNullException(nameof(mainViewType));

            string mainViewTypeName = mainViewType.FullName;

            this.EnsureMainViewImpl(mainViewTypeName);
        }

        /// <summary>
        /// Checks if main plugin view name exists in provided settings. If not - adds it
        /// </summary>
        /// <param name="mainViewTypeName">Plugin's main view full type name</param>
        public void EnsureMainViewIsRegistered(string mainViewTypeName) 
        {
            if (string.IsNullOrWhiteSpace(mainViewTypeName)) throw new ArgumentNullException(nameof(mainViewTypeName));
            
            this.EnsureMainViewImpl(mainViewTypeName);
        }


        private void EnsureMainViewImpl(string mainViewTypeName) 
        {
            if (this.TryAddMainViewSetting(mainViewTypeName)) return;
 
            if (this.settings.Properties[StringConstants.MainGameViewSettingsKey]?.DefaultValue?.ToString() == mainViewTypeName) return;

            this.settings.Properties.Remove(StringConstants.MainGameViewSettingsKey);

            if (!this.TryAddMainViewSetting(mainViewTypeName))
            {
                throw new InvalidOperationException("Smth is wroooong. Failed to add main view setting");
            }
        }

        private bool TryAddMainViewSetting(string mainViewTypeName) 
        {
            try
            {
                var setting = new SettingsProperty(StringConstants.MainGameViewSettingsKey)
                {
                    DefaultValue = mainViewTypeName
                };

                this.settings.Properties.Add(setting);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}