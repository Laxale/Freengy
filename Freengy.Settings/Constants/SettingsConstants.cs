﻿// Created by Laxale 24.10.2016
//
//


namespace Freengy.Settings.Constants 
{
    using System.IO;


    public static class SettingsConstants 
    {
        public const string DatabaseFolderName = "db";
        public const string SettingsDbFileName = "Settings.sqlite";
        public static readonly string ConnectionString = 
            $"data source={ Path.Combine(DatabaseFolderName, SettingsDbFileName) };Foreign Keys=True";

        public const int PathMinLength = 3;
        public const int SettingsUnitNameMinLength = 2;
        public const int SettingsUnitNameMaxLength = 40;
    }
}