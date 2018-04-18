// Created by Laxale 24.10.2016
//
//

using System;
using System.IO;


namespace Freengy.Settings.Constants 
{
    public static class SettingsConstants 
    {
        public const string DatabaseFolderName = "db";
        public const string SettingsDbFileName = "freengy-client.db";

        internal static readonly string ConnectionString = 
            $"data source={ Path.Combine(DatabaseFolderName, SettingsDbFileName) };Foreign Keys=True";

        public const int PathMinLength = 3;
        public const int SettingsUnitNameMinLength = 2;
        public const int SettingsUnitNameMaxLength = 40;

        public static readonly string DefaultGamesFolderPath = Path.Combine(Environment.CurrentDirectory, "Games");
    }
}