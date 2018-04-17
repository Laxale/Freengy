// Created by Laxale 24.10.2016
//
//

using System;

using Freengy.Base.Interfaces;


namespace Freengy.GamePlugin.Interfaces 
{
    public interface IGamePlugin : IUiModule, INamedObject, IObjectWithId 
    {
        string GameIconSource { get; }

        // if plugin is created in worker thread, ImageSource cannot be binded to ui element!
        // needs to be created in ui thread
        //ImageSource GameIconSource { get; }
    }
}