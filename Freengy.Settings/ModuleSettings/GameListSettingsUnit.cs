// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.ModuleSettings 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Freengy.Settings.Constants;
    

    internal class GameListSettingsUnit : SettingsUnitBase 
    {
        public virtual string GamesFolderPath { get; set; }
        
        public override string Name { get; set; } = "GameList Settings";


        public override IDictionary<string, ICollection<Attribute>> ColumnsProperties { get; } =
            new Dictionary<string, ICollection<Attribute>>
            {
                { nameof(GameListSettingsUnit.Name), new [] { new RequiredAttribute() } },
                {
                    nameof(GameListSettingsUnit.GamesFolderPath),
                    new Attribute[]
                    {
                        new RequiredAttribute(),
                        new StringLengthAttribute(SettingsConstants.PathMinLength)
                    }
                }
            };
    }
}