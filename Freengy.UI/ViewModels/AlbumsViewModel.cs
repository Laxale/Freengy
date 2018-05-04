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
using Freengy.Common.Helpers;
using Freengy.Base.ViewModels;
using Freengy.Base.Interfaces;
using Freengy.Common.Extensions;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.Networking.Interfaces;
using Freengy.Base.Messages;
using Freengy.Common.Helpers.Result;

using Catel.IoC;


namespace Freengy.UI.ViewModels 
{
    /// <summary>
    /// Вьюмодель для <see cref="AlbumsView"/>.
    /// </summary>
    internal class AlbumsViewModel : WaitableViewModel, IDisposable
    {
        private readonly int searchDelayInMs = 300;
        private readonly IAlbumManager albumManager;
        private readonly DelayedEventInvoker delayedInvoker;
        private readonly ObservableCollection<AlbumViewModel> albumViewModels = new ObservableCollection<AlbumViewModel>();

        private bool isDisposed;
        private string albumName;
        private bool isViewingAlbum;
        private bool isCreatingNewAlbum;
        private AlbumViewModel selectedAlbumViewModel;


        public AlbumsViewModel() 
        {
            albumManager = ServiceLocatorProperty.ResolveType<IAlbumManager>();

            AlbumViewModels = CollectionViewSource.GetDefaultView(albumViewModels);

            delayedInvoker = new DelayedEventInvoker(searchDelayInMs);
            delayedInvoker.DelayedEvent += OnDelayedSearchEvent;
            AlbumViewModels.Filter = FilterAlbums;

            Mediator.SendMessage(new MessageInitializeModelRequest(this, "Loading albums"));
        }

        
        /// <summary>
        /// Command to create a new album.
        /// </summary>
        public MyCommand CommandAddAlbum { get; private set; }

        /// <summary>
        /// Command to view selected album.
        /// </summary>
        public MyCommand<AlbumViewModel> CommandViewAlbum { get; private set; }


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
        /// Gets or sets new album name to create or search.
        /// </summary>
        public string AlbumName 
        {
            get => albumName;

            set
            {
                if (albumName == value) return;

                albumName = value;
                delayedInvoker.RequestDelayedEvent();
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

            delayedInvoker.Dispose();

            foreach (AlbumViewModel viewModel in albumViewModels)
            {
                viewModel.Dispose();
            }

            //not registered for now
            //Mediator.UnregisterRecipient(this);

            isDisposed = true;
        }

        /// <summary>
        /// Сохранить изменения в изменённых альбомах.
        /// </summary>
        public void SaveChangedAlbums() 
        {
            var changedViewModels = albumViewModels.Where(viewModel => viewModel.IsChanged);

            foreach (AlbumViewModel changedViewModel in changedViewModels)
            {
                changedViewModel.SaveModel();
                albumManager.SaveAlbum(changedViewModel.PhotoAlbum);
            }
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

                GUIDispatcher.BeginInvokeOnGuiThread(() => albumViewModels.AddRange(viewModels));
            }
            else
            {
                ReportMessage(myAlbumsResult.Error.Message);
            }
        }

        /// <inheritdoc />
        protected override void SetupCommands() 
        {
            CommandViewAlbum = new MyCommand<AlbumViewModel>(ViewAlbumImpl);
            CommandAddAlbum = new MyCommand(AddAlbumImpl, () => !string.IsNullOrWhiteSpace(AlbumName));

            OnPropertyChanged(nameof(CommandAddAlbum));
            OnPropertyChanged(nameof(CommandViewAlbum));
        }


        private void OnDelayedSearchEvent() 
        {
            AlbumViewModels.Refresh();
        }

        private bool FilterAlbums(object albumObject) 
        {
            if (string.IsNullOrWhiteSpace(AlbumName))
            {
                return true;
            }

            bool isAcceptable = ((AlbumViewModel)albumObject)
                                .PhotoAlbum.Name.ToLowerInvariant().Contains(AlbumName.ToLowerInvariant());

            return isAcceptable;
        }

        private void AddAlbumImpl() 
        {
            if (albumViewModels.Any(viewModel => viewModel.PhotoAlbum.Name == AlbumName))
            {
                ReportMessage($"Album { AlbumName } already exists");
                return;
            }

            ClearInformation();

            AccountState myAccountState = ServiceLocatorProperty.ResolveType<ILoginController>().MyAccountState;

            var albumModel = new AlbumModel
            {
                CreationTime = DateTime.Now,
                Name = AlbumName,
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
                // сохраняем альбом при возвращении из вью содержимого альбома
                if (SelectedAlbumViewModel?.IsChanged ?? false)
                {
                    SelectedAlbumViewModel.SaveModel();
                    albumManager.SaveAlbum(SelectedAlbumViewModel.PhotoAlbum);
                }

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