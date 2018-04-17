// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.ModuleSettings 
{
    internal class FriendListSettingsUnit : SettingsUnit 
    {
        public virtual string FriendsFolderPath { get; set; }
        

        public override string Name { get; } = "FriendList Settings";
    }
}