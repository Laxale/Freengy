// Created by Laxale 23.11.2016
//
//


namespace Freengy.GameList.Settings 
{
    using Freengy.Settings.Interfaces;


    public interface _IGameListSettingsUnit : IBaseSettingsUnit 
    {
        string GamesFolderPath { get; set; }
    }
}