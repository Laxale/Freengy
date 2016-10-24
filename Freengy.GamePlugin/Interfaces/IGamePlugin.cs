// Created by Laxale 24.10.2016
//
//


namespace Freengy.GamePlugin.Interfaces 
{
    using System.Windows.Controls;

    using Freengy.Base.Interfaces;


    /// <summary>
    /// Represents pluggable game
    /// </summary>
    public interface IGamePlugin : IUiModule, INamedObject, IObjectWithId 
    {
        Image GameIcon { get; }
    }
}