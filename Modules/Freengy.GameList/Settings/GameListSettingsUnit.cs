// Created by Laxale 23.11.2016
//
//


namespace Freengy.GameList.Settings 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Freengy.Settings.Constants;
    using Freengy.Settings.ModuleSettings;
    

    public class GameListSettingsUnit : SettingsUnitBase 
    {
        #region Singleton

        private static GameListSettingsUnit instance;

        private GameListSettingsUnit() 
        {

        }

        public static GameListSettingsUnit Instance =>
            GameListSettingsUnit.instance ?? (GameListSettingsUnit.instance = new GameListSettingsUnit());

        #endregion Singleton


        public virtual string GamesFolderPath { get; set; }
        
        public virtual string Name { get; set; } = "GameList Settings";


        public override IDictionary<string, ICollection<Attribute>> ColumnsProperties { get; } =
            new Dictionary<string, ICollection<Attribute>>
            {
                { nameof(GameListSettingsUnit.Name), new [] { new RequiredAttribute() } },
                {
                    nameof(GameListSettingsUnit.GamesFolderPath),
                    new Attribute[]
                    {
                        new RequiredAttribute(), new StringLengthAttribute(SettingsConstants.PathMinLength)
                    }
                }
            };
    }
}