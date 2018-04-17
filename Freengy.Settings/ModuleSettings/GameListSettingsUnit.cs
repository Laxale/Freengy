// Created by Laxale 23.11.2016
//
//

using Freengy.Settings.Constants;


namespace Freengy.Settings.ModuleSettings 
{
    public class GameListSettingsUnit : SettingsUnit 
    {
        public override string Name { get; } = "GameList Settings";

        public virtual string GamesFolderPath { get; set; } = SettingsConstants.DefaultGamesFolderPath;
    }
}