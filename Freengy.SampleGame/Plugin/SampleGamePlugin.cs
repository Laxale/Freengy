// Created by Laxale 25.10.2016
//
//


namespace Freengy.SampleGame.Plugin 
{
    using System;
    using System.Windows.Controls;

    using Freengy.SampleGame.Views;
    using Freengy.Base.Interfaces;
    using Freengy.GamePlugin.Interfaces;
    

    internal class SampleGamePlugin : IGamePlugin 
    {
        public Image GameIcon { get; } = null;

        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; } = "Sample game";

        public string DisplayedName { get; } = "Sample game displayed name";

        public Type ExportedViewType { get; } = typeof (SampleGameUi);
    }
}
