// Created by Laxale 24.10.2016
//
//


namespace Freengy.GamePlugin.Interfaces 
{
    using System;
    using System.Windows.Controls;

    using Freengy.Base.Interfaces;
    

    public interface IGamePlugin : IUiModule, INamedObject, IObjectWithId 
    {
        string GameIconSource { get; }
    }
}