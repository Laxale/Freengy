// Created by Laxale 24.10.2016
//
//


namespace Freengy.GamePlugin.Interfaces 
{
    using System.Threading.Tasks;
    using System.Collections.Generic;


    public interface IGameListProvider 
    {
        IEnumerable<IGamePlugin> GetInstalledGames();

        Task<IEnumerable<IGamePlugin>> GetInstalledGamesAsync();
    }
}