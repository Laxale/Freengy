// Created by Laxale 24.10.2016
//
//

using Freengy.Base.Attributes;
using Freengy.GameList.ViewModels;


namespace Freengy.GameList.Views 
{
    [HasViewModel(typeof(GameListViewModel))]
    public partial class GameListView 
    {
        public GameListView() 
        {
            InitializeComponent();
        }
    }
}