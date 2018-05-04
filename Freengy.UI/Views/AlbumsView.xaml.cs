// Created by Laxale 04.05.2018
//
//

using System;
using System.Windows;
using System.Windows.Controls;

using Freengy.Base.Views;
using Freengy.Base.Attributes;
using Freengy.UI.ViewModels;

using Catel.Messaging;


namespace Freengy.UI.Views 
{
    /// <summary>
    /// Вью коллекции альбомов.
    /// </summary>
    [HasViewModel(typeof(AlbumsViewModel))]
    public partial class AlbumsView : UserControl, IDisposable 
    {
        private bool isDisposed;
        private AlbumView childAlbumView;


        public AlbumsView() 
        {
            InitializeComponent();

            Unloaded += OnUnloaded;
        }

        ~AlbumsView() 
        {
            Dispose();
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() 
        {
            if (isDisposed) return;

            childAlbumView.Dispose();

            isDisposed = true;
        }


        private void AlbumView_OnLoaded(object sender, RoutedEventArgs e) 
        {
            childAlbumView = (AlbumView) sender;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs) 
        {
            Dispose();
        }
    }
}