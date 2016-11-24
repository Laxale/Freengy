// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.ModuleSettings 
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Freengy.Settings.Constants;
    

    [Table(nameof(GameListSettingsUnit))]
    public sealed class GameListSettingsUnit : SettingsUnitBase 
    {
        #region Singleton

        private static GameListSettingsUnit instance;

        private GameListSettingsUnit()
        {

        }

        public static GameListSettingsUnit Instance =>
            GameListSettingsUnit.instance ?? (GameListSettingsUnit.instance = new GameListSettingsUnit());

        #endregion Singleton


        /// <summary>
        /// This is not meant to be used rigth now. Settings row is alone in a table
        /// </summary>
        public long Id { get; set; }

        [StringLength(1000, MinimumLength = 3)]
        public string GamesFolderPath { get; set; }

        [Required]
        [StringLength(Container.SettingsUnitNameMaxLength, MinimumLength = Container.SettingsUnitNameMinLength)]
        public string Name { get; set; } = "GameList Settings";
    }
}