// Created by Laxale 04.05.2018
//
//

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using Freengy.Base.Helpers.Commands;
using Freengy.UI.Views;
using Freengy.Base.ViewModels;
using Freengy.Common.Extensions;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.Networking.Interfaces;

using Catel.IoC;


namespace Freengy.UI.ViewModels 
{
    /// <summary>
    /// Вьюмодель для <see cref="AlbumsView"/>.
    /// </summary>
    internal class AlbumsViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<AlbumViewModel> albumViewModels = new ObservableCollection<AlbumViewModel>();

        private string newAlbumName;
        private bool isViewingAlbum;
        private bool isCreatingNewAlbum;
        private AlbumViewModel selectedAlbumViewModel;


        public AlbumsViewModel() 
        {
            AlbumViewModels = CollectionViewSource.GetDefaultView(albumViewModels);

            CommandAddAlbum = new MyCommand(AddAlbumImpl);
            CommandViewAlbum = new MyCommand<AlbumViewModel>(ViewAlbumImpl);
        }


        /// <summary>
        /// Command to create a new album.
        /// </summary>
        public MyCommand CommandAddAlbum { get; }

        /// <summary>
        /// Command to view selected album.
        /// </summary>
        public MyCommand<AlbumViewModel> CommandViewAlbum { get; }


        /// <summary>
        /// Gets the collection of current albums.
        /// </summary>
        public ICollectionView AlbumViewModels { get; }


        /// <summary>
        /// Gets the flag - are we now creating a new album?
        /// </summary>
        public bool IsCreatingNewAlbum 
        {
            get => isCreatingNewAlbum;

            private set
            {
                if (isCreatingNewAlbum == value) return;

                isCreatingNewAlbum = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets new album name to create.
        /// </summary>
        public string NewAlbumName 
        {
            get => newAlbumName;

            set
            {
                if (newAlbumName == value) return;

                newAlbumName = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets currently selected for viewing album.
        /// </summary>
        public AlbumViewModel SelectedAlbumViewModel 
        {
            get => selectedAlbumViewModel;

            private set
            {
                if (selectedAlbumViewModel == value) return;

                selectedAlbumViewModel = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets flag - are we viewing album?
        /// </summary>
        public bool IsViewingAlbum 
        {
            get => isViewingAlbum;

            set
            {
                if (isViewingAlbum == value) return;

                isViewingAlbum = value;

                OnPropertyChanged();
            }
        }


        public void LoadAlbumsOf(Guid userId) 
        {

        }


        private void AddAlbumImpl() 
        {
            AccountState myAccountState = ServiceLocatorProperty.ResolveType<ILoginController>().MyAccountState;

            var albumModel = new AlbumModel
            {
                CreationTime = DateTime.Now,
                Name = NewAlbumName,
                OwnerAccountModel = myAccountState.Account.ToModel()
            };

            var newAlbum = new Album(albumModel);
            var newAlbumViewModel = new AlbumViewModel(newAlbum);

            albumViewModels.Add(newAlbumViewModel);
        }

        private void ViewAlbumImpl(AlbumViewModel albumViewModel) 
        {
            if (albumViewModel == null)
            {
                IsViewingAlbum = false;
                SelectedAlbumViewModel = null;
            }
            else
            {
                IsViewingAlbum = true;
                SelectedAlbumViewModel = albumViewModel;
            }
        }
    }
}