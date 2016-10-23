// Created by Laxale 23.10.2016
//
//


namespace Freengy.Base.Interfaces 
{
    /// <summary>
    /// Any piece of UI (module) must expose its root view name to allow shell register this module
    /// </summary>
    public interface IUiModule 
    {
        string ExportedViewName { get; }
    }
}