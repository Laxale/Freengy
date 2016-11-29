// Created by Laxale 27.11.2016
//
//


namespace Freengy.Settings.Messages 
{
    using System;

    using Freengy.Base.Messages;


    /// <summary>
    /// Represents that some of settings unit viewmodels has changed - new can save settings changes
    /// </summary>
    internal class MessageSettingChanged : MessageBase 
    {
        public MessageSettingChanged(string settingsUnitName, bool isChanged) 
        {
            if (string.IsNullOrWhiteSpace(settingsUnitName)) throw new ArgumentNullException(nameof(settingsUnitName));

            this.SettingsUnitName = settingsUnitName;
            this.IsChanged = isChanged;
        }


        public bool IsChanged { get; }

        public string SettingsUnitName { get; }
    }
}