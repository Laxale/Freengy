// Created by Laxale 24.10.2016
//
//


namespace Freengy.GamePlugin.Interfaces 
{
    using System.Collections.Generic;


    public interface IGameListProvider 
    {
        IEnumerable<IGamePlugin> GetInstalledGames();
    }
}