// Created by Laxale 24.10.2016
//
//


namespace Freengy.Base.Interfaces 
{
    using System.Windows.Controls;


    /// <summary>
    /// Represents pluggable game
    /// </summary>
    public interface IGamePlugin : IUiModule, INamedObject, IObjectWithId 
    {
        Image GameIcon { get; }
    }
}