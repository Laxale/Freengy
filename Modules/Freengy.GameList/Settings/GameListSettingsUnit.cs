// Created by Laxale 23.11.2016
//
//


namespace Freengy.GameList.Settings 
{
    public sealed class GameListSettingsUnit 
        //: IGameListSettingsUnit 
    {
        #region Singleton

        private static GameListSettingsUnit instance;

        private GameListSettingsUnit()
        {

        }

        public static GameListSettingsUnit Instance =>
            GameListSettingsUnit.instance ?? (GameListSettingsUnit.instance = new GameListSettingsUnit());

        #endregion Singleton


        public string GamesFolderPath { get; set; }
        public string Name { get; } = "GameList Settings";
    }
}