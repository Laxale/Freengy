// Created by Laxale 27.10.2016
//
//


namespace Freengy.GamePlugin.Interfaces 
{
    /// <summary>
    /// Represents entity, wich is responsible for game loading. It also can be contacted by message
    /// </summary>
    public interface IGameDispatcher 
    {
        bool CanUnloadCurrentGame { get; }

        IGamePlugin CurrentRunningGame { get; }

        bool UnloadCurrentGame();

        bool LoadGame(IGamePlugin gamePlugin);

        bool CanLoadGame(IGamePlugin gamePlugin);
    }
}