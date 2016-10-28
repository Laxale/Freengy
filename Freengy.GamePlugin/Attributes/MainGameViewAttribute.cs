// Created by Laxale 28.10.2016
//
//


namespace Freengy.GamePlugin.Attributes 
{
    using System;


    /// <summary>
    /// Must be put on a game plugin's main view class definition.
    /// <para>Metadata parser needs it to find assembly's main view type without loading this assembly</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MainGameViewAttribute : Attribute 
    {

    }
}