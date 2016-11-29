// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.ModuleSettings 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Freengy.Settings.Constants;
    

    internal class FriendListSettingsUnit : SettingsUnitBase 
    {
        public virtual string FriendsFolderPath { get; set; }
        
        public override string Name { get; set; } = "FriendList Settings";


        protected internal override IDictionary<string, ICollection<Attribute>> ColumnsProperties { get; } =
            new Dictionary<string, ICollection<Attribute>>
            {
                { nameof(FriendListSettingsUnit.Name), new [] { new RequiredAttribute() } },
                {
                    nameof(FriendListSettingsUnit.FriendsFolderPath),
                    new Attribute[]
                    {
                        new RequiredAttribute(),
                        new StringLengthAttribute(SettingsConstants.PathMinLength)
                    }
                }
            };
    }
}