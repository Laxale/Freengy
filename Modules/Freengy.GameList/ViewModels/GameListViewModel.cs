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


        public ICollectionView GameList { get; private set; }


        #region Override

        protected override void SetupCommands() 
        {

        }

        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            await this.FillGameList();

            this.GameList = CollectionViewSource.GetDefaultView(this.gameList);
        }

        #endregion Override
        

        private async Task FillGameList() 
        {
            IEnumerable<IGamePlugin> installedGameList = await this.gameListProvider.GetInstalledGamesAsync();

            foreach (var installedGame in installedGameList)
            {
                this.gameList.Add(installedGame);
            }
        }
    }
}