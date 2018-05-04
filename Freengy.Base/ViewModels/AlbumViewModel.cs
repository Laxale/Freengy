// Created by Laxale 04.05.2018
//
//

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using Freengy.Base.Helpers.Commands;
using Freengy.Base.Messages;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;


namespace Freengy.Base.ViewModels 
{
    /// <summary>
    /// Вьюмодель для работы с <see cref="Album"/>.
    /// </summary>
    public class AlbumViewModel : WaitableViewModel, IDisposable 
    {
        private readonly ObservableCollection<ImageModel> imageModels = new ObservableCollection<ImageModel>();

        private bool isDisposed;


        public AlbumViewModel(Album album) 
        {
            PhotoAlbum = album;
            
            AddImageCommand = new MyCommand(() => AddImageImpl(null));

            ImageModels = CollectionViewSource.GetDefaultView(imageModels);

            album.Images.ForEach(imageModels.Add);

            Mediator.Register<MessageAddImageRequest>(this, OnAddImageRequest);
        }

        ~AlbumViewModel() 
        {
            Dispose();
        }


        /// <summary>
        /// Команда добавления изображения в данный альбом.
        /// </summary>
        public MyCommand AddImageCommand { get; }


        /// <summary>
        /// Возвращает ссылку на данный альбом изображений.
        /// </summary>
        public Album PhotoAlbum { get; }

        /// <summary>
        /// Gets the collection of album images.
        /// </summary>
        public ICollectionView ImageModels { get; }


        
        private void AddImageImpl(string imageUri) 
        {
            var imageModel = new ImageModel
            {
                RemoteUrl = imageUri
            };

            imageModels.Add(imageModel);
        }

        private void OnAddImageRequest(MessageAddImageRequest request) 
        {
            AddImageImpl(request.ImageUri);
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() 
        {
            if (isDisposed) return;

            Mediator.UnregisterRecipient(this);

            isDisposed = true;
        }
    }
}