// Created by Laxale 25.10.2016
//
//


namespace Freengy.SampleGame.Plugin 
{
    using System;
    
    using Freengy.SampleGame.Views;
    using Freengy.GamePlugin.Interfaces;
    
    using Res = Freengy.GamePlugin.Resources;


    public class SampleGamePlugin : IGamePlugin 
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; } = Res.DefaultGameName;

        public Type ExportedViewType { get; } = typeof (SampleGameUi);

        public string GameIconSource { get; } = Res.DefaultGameIconSource;

        public string DisplayedName { get; } = Res.DefaultDisplayedGameName;
    }
}