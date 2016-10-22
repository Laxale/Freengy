// Created by Laxale 20.10.2016
//
//


namespace Freengy.Base.Settings 
{
    using System.Configuration;


    /// <summary>
    /// Holds user-specific application settings
    /// </summary>
    [SettingsGroupName("Application")]
    public class AppSettings : ApplicationSettingsBase 
    {
        #region Singleton

        private static AppSettings instance;

        private AppSettings() 
        {

        }

        public static AppSettings Instance => AppSettings.instance ?? (AppSettings.instance = new AppSettings());

        #endregion Singleton
        

        public override void Save() 
        {
            lock (this)
            {
                base.Save();
            }
        }


        [UserScopedSetting]
        [DefaultSettingValue("0")]
        public string Port 
        {
            get { return (string)this["Port"]; }
            set { this["Port"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string UserName 
        {
            get { return (string)this["UserName"]; }

            set { this["UserName"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string HostName 
        {
            get { return (string)this["HostName"]; }
            set { this["HostName"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string Password 
        {
            get { return (string)this["Password"]; }
            set { this["Password"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("false")]
        public bool SavePassword 
        {
            get { return (bool)this["SavePassword"]; }
            set { this["SavePassword"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string LastUserSession 
        {
            get { return (string)this["LastUserSession"]; }
            set { this["LastUserSession"] = value; }
        }
    }
}