// Created by Laxale 23.10.2016
//
//


namespace Freengy.Base.Interfaces 
{
    using System;


    /// <summary>
    /// Any piece of UI (module) must expose its root view type to allow shell register this module
    /// </summary>
    public interface IUiModule 
    {
        Type ExportedViewType { get; }
    }
}