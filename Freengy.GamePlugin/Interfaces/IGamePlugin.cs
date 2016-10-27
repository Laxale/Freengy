// Created by Laxale 24.10.2016
//
//


namespace Freengy.GamePlugin.Interfaces 
{
    using System;
    using System.Windows.Media;
    using System.Windows.Controls;

    using Freengy.Base.Interfaces;
    

    public interface IGamePlugin : IUiModule, INamedObject, IObjectWithId 
    {
        string GameIconSource { get; }

        // if plugin is created in worker thread, ImageSource cannot be binded to ui element!
        // needs to be created in ui thread
        //ImageSource GameIconSource { get; }
    }
}