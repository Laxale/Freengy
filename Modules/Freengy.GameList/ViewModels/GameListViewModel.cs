// Created by Laxale 24.10.2016
//
//


namespace Freengy.GameList.ViewModels 
{
    using System;
    using System.Windows.Data;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Freengy.Base.ViewModels;
    using Freengy.GamePlugin.Interfaces;

    using Catel.IoC;


    public class GameListViewModel : WaitableViewModel
    {
        private readonly IGameListProvider gameListProvider;
        private readonly ObservableCollection<IGamePlugin> gameList = new ObservableCollection<IGamePlugin>();


        public GameListViewModel() : base(true)
        {
            this.gameListProvider = base.serviceLocator.ResolveType<IGameListProvider>();
        }

        protected override void SetupCommands() 
        {
            
        }


        public ICollectionView GameList { get; private set; }


        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            this.FillGameList();

            this.GameList = CollectionViewSource.GetDefaultView(this.gameList);
        }


        private void FillGameList() 
        {
            IEnumerable<IGamePlugin> installedGameList = this.gameListProvider.GetInstalledGames();

            foreach (var installedGame in installedGameList)
            {
                this.gameList.Add(installedGame);
            }
        }
    }
}