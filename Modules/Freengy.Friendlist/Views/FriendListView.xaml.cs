// Created by Laxale 23.10.2016
//
//

using Freengy.Base.Attributes;
using Freengy.FriendList.ViewModels;


namespace Freengy.FriendList.Views 
{
    [HasViewModel(typeof(FriendListViewModel))]
    public partial class FriendListView 
    {
        public FriendListView() 
        {
            InitializeComponent();
        }
    }
}