// Created by Laxale 24.10.2016
//
//


namespace Freengy.GamePlugin.Constants 
{
    using System;
    using System.IO;


    internal static class StringConstants 
    {
        /// <summary>
        /// Name of a folder to hold game plugins in
        /// </summary>
        internal const string GamesFolderName = "Games";

        /// <summary>
        /// Path to a game plugins folder
        /// </summary>
        internal static readonly string GamesFolderPath = Path.Combine(Environment.CurrentDirectory, StringConstants.GamesFolderName);

        /// <summary>
        /// Key for a settings entry to hold plugin's main view full type name
        /// </summary>
        internal const string MainGameViewSettingsKey = "MainGameView";
    }
}