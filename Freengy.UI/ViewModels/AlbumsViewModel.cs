// Created by Laxale 04.05.2018
//
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

using Freengy.Base.Helpers.Commands;
using Freengy.UI.Views;
using Freengy.Base.ViewModels;
using Freengy.Base.Interfaces;
using Freengy.Common.Extensions;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.Networking.Interfaces;

using Catel.IoC;
using Freengy.Base.Messages;
using Freengy.Common.Helpers.Result;


namespace Freengy.UI.ViewModels 
{
    /// <summary>
    /// Вьюмодель для <see cref="AlbumsView"/>.
    /// </summary>
    internal class AlbumsViewModel : WaitableViewModel, IDisposable
    {
        private readonly IAlbumManager albumManager;
        private readonly ObservableCollection<AlbumViewModel> albumViewModels = new ObservableCollection<AlbumViewModel>();

        private bool isDisposed;
        private string newAlbumName;
        private bool isViewingAlbum;
        private bool isCreatingNewAlbum;
        private AlbumViewModel selectedAlbumViewModel;


        public AlbumsViewModel() 
        {
            albumManager = ServiceLocatorProperty.ResolveType<IAlbumManager>();

            AlbumViewModels = CollectionViewSource.GetDefaultView(albumViewModels);

            CommandViewAlbum = new MyCommand<AlbumViewModel>(ViewAlbumImpl);
            CommandAddAlbum = new MyCommand(AddAlbumImpl, () => !string.IsNullOrWhiteSpace(NewAlbumName));

            Mediator.SendMessage(new MessageInitializeModelRequest(this, "Loading albums"));
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
                CommandAddAlbum.RaiseCanExecuteChanged();

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

        /// <summary>
        /// Gets the collection of current albums.
        /// </summary>
        public ICollectionView AlbumViewModels { get; }


        public void Dispose() 
        {
            if (isDisposed) return;

            foreach (AlbumViewModel viewModel in albumViewModels)
            {
                viewModel.Dispose();
            }

            //not registered for now
            //Mediator.UnregisterRecipient(this);

            isDisposed = true;
        }

        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            Result<IEnumerable<Album>> myAlbumsResult = albumManager.GetMyAlbums().Result;

            if (myAlbumsResult.Success)
            {
                var viewModels = myAlbumsResult.Value.Select(album => new AlbumViewModel(album));
                albumViewModels.AddRange(viewModels);
            }
            else
            {
                ReportMessage(myAlbumsResult.Error.Message);
            }
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

            albumManager.SaveAlbum(newAlbum);
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