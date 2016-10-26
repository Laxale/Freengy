// Created by Laxale 26.10.2016
//
//


namespace Freengy.GamePlugin.DefaultImpl 
{
    using System;
    using System.Windows.Controls;

    using Freengy.GamePlugin.Interfaces;

    using Res = Freengy.GamePlugin.Resources;


    public abstract class GamePluginBase : IGamePlugin
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; } = Res.DefaultGameName;

        public Type ExportedViewType { get; protected set; } = typeof(ContentControl);

        public string DisplayedName { get; protected set; } = Res.DefaultDisplayedGameName;

        public Image GameIcon { get; protected set; } = new Image { Source = Res.DefaultGameIconSource };
    }
}