// Created by Laxale 25.10.2016
//
//


namespace Freengy.SampleGame.Plugin 
{
    using System;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Freengy.Base.Interfaces;
    using Freengy.SampleGame.Views;
    using Freengy.GamePlugin.Interfaces;

    using Res = Freengy.GamePlugin.Resources;

    
    /// <summary>
    /// Sample/simple game plugin for testing
    /// </summary>
    public class SampleGamePlugin : IGamePlugin 
    {
        public Guid UniqueId { get; } = Guid.NewGuid();

        public string Name { get; } = Res.DefaultGameName;

        public Type ExportedViewType { get; } = typeof (SampleGameUi);

        public string GameIconSource { get; } = Res.DefaultGameIconSource;

        //public ImageSource GameIconSource { get; } = GetDefaultIconSource();

        public string DisplayedName { get; } = Res.DefaultDisplayedGameName;


        private static ImageSource GetDefaultIconSource()
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(Res.DefaultGameIconSource);
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}